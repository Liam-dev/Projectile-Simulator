using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Simulator.Simulation;
using System.Text.Json;
using Newtonsoft.Json;
using Simulator.Converters;
using System.Linq;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Main application form for editing simulations.
    /// </summary>
    public partial class Editor : Form
    {
        // Delegate used for thread-safe calling
        private delegate void SafeCallDelegate();

        // List of object to load into simulation when form is loaded
        private List<object> objectsToLoad;

        // Object that is currently saved to clipboard
        private SimulationObject clipboardObject;

        /// <summary>
        /// Gets the filename of the file in the editor.
        /// </summary>
        public string Filename { get; protected set; }

        public Editor()
        {
            InitializeComponent();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            WindowState = FormWindowState.Maximized;

            objectsToLoad = new List<object>();

            /*
            // TEMPORARY
            // Test objects for mouse zoom testing
            objectsToLoad.Add(new Box("box", new Vector2(0, -64), "crate", 0.95f, new Vector2(64, 64)));
            objectsToLoad.Add(new Box("box", new Vector2(0, 0), "crate", 0.95f, new Vector2(64, 64)));
            objectsToLoad.Add(new Box("box", new Vector2(100, 400), "crate", 0.95f, new Vector2(64, 64)));
            objectsToLoad.Add(new Box("box", new Vector2(400, 100), "crate", 0.95f, new Vector2(64, 64)));
            objectsToLoad.Add(new Box("box", new Vector2(400, 400), "crate", 0.95f, new Vector2(64, 64)));

            objectsToLoad.Add(new Wall("wall", new Vector2(800, 100), Microsoft.Xna.Framework.Color.SaddleBrown, 0.95f, new Vector2(20, 500)));
            objectsToLoad.Add(new Wall("floor", new Vector2(320, 600), Microsoft.Xna.Framework.Color.ForestGreen, 0.95f, new Vector2(500, 20)));

            Projectile projectile = new Projectile("redTempProjectile", Vector2.Zero, "ball", 5, 0.9f, 16, 0.005f);
            objectsToLoad.Add(new Cannon("cannon", new Vector2(0, 600), "cannon", projectile));

            objectsToLoad.Add(new TapeMeasure("tape measure", new Vector2(64, -64), new Vector2(0, 64), 8, "line", "Arial"));
            */
        }

        /// <summary>
        /// Open Editor with  file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="isTemplate"></param>
        public Editor(string filename, bool isTemplate = false)
        {
            InitializeComponent();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            WindowState = FormWindowState.Maximized;

            // Load simulation state from file
            objectsToLoad = new List<object>();
            SimulationState loadedState = FileSaver.ReadJson(filename);
            objectsToLoad = loadedState.Objects;
            simulation.LoadState(loadedState);

            if (!isTemplate)
            { 
                Filename = filename;
                ChangeFormTitle();
            }
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            inspector.SelectedObjectChanged += Inspector_SelectedObjectChanged;

            // Right click context menu for simulation
            simulationContextMenu.ButtonClicked += toolbar_ButtonClicked;
            simulation.ContextMenuStrip = simulationContextMenu.contextMenuStrip;
            simulationContextMenu.contextMenuStrip.Opening += ContextMenuStrip_Opening;

            simulation.ObjectAdded += Simulation_ObjectAdded;
            simulation.SelectedObjectChanged += Simulation_SelectedObjectChanged;
            simulation.SimulationPaused += toolbar.Simulation_Paused;
            simulation.SimulationResumed += toolbar.Simulation_UnPaused;

            // Load objects into simulation
            foreach (object @object in objectsToLoad)
            {
                if (@object is SimulationObject simulationObject)
                {
                    simulation.AddObject(simulationObject);
                }
                else if (@object is Camera camera)
                {
                    // Load camera
                    simulation.Camera = camera;
                }
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            simulationContextMenu.SelectedObject = simulation.ContextMenuOpening();
        }

        private void Inspector_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (sender is ISelectable selectable)
                simulation.SelectObject(selectable);
        }

        private void Simulation_ObjectAdded(object sender, EventArgs e)
        {
            inspector.SetDataSource(simulation.GetObjectsToDisplay());
        }

        private void Simulation_SelectedObjectChanged(object sender, EventArgs e)
        {
            inspector.SelectedObject = sender;
        }

        // When a button is clicked in the toolbar or context menu
        private void toolbar_ButtonClicked(object sender, EventArgs e)
        {
            // Get tag of button
            string tag = string.Empty;

            if (sender is ToolStripButton button)
            {
                if (button.Tag is string text)
                {
                    tag = text;
                }      
            }
            else if (sender is ToolStripMenuItem item)
            {
                if (item.Tag is string text)
                {
                    tag = text;
                }
            }

            // Get action to perform
            switch (tag)
            {
                case "newFile":
                    SaveFilePerformAction(NewFile);
                    break;

                case "openFile":
                    SaveFilePerformAction(OpenFile);
                    break;

                case "saveFile":
                    SaveFile();
                    break;

                case "saveAs":
                    SaveAs();
                    break;

                case "exit":
                    CloseEditor();
                    break;

                case "screenshot":
                    Screenshot();
                    break;

                case "cut":
                    clipboardObject = (SimulationObject)simulation.SelectedObject;
                    simulation.RemoveObject(clipboardObject);
                    inspector.SetDataSource(simulation.GetObjectsToDisplay());
                    break;

                case "copy":
                    clipboardObject = (SimulationObject)simulation.SelectedObject;
                    break;

                case "paste":
                    /*
                    JsonSerializerOptions options = new JsonSerializerOptions() { Converters = { new Vector2JsonConverter() } };
                    SimulationObject @object = (SimulationObject)JsonSerializer.Deserialize(JsonSerializer.Serialize(clipboardObject, clipboardObject.GetType(), options), clipboardObject.GetType(), options);
                    */
                    JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All };
                    string data = JsonConvert.SerializeObject(clipboardObject, Formatting.Indented, settings);
                    SimulationObject @object = JsonConvert.DeserializeObject<SimulationObject>(data, settings);
                    @object.Position = simulation.Camera.GetSimulationPostion(simulation.MousePosition);
                    simulation.AddObject(@object);
                    break;

                case "ball":
                    simulation.FireAllCannons();
                    break;

                case "deleteObject":
                    SimulationObject selectedObject = (SimulationObject)simulation.SelectedObject;
                    simulation.RemoveObject(selectedObject);
                    inspector.SetDataSource(simulation.GetObjectsToDisplay());
                    break;

                case "fireCannon":
                    if (simulation.SelectedObject is Cannon cannon)
                    {
                        cannon.Fire();
                    }
                    break;

                case "newBox":
                    simulation.AddObject(new Box("box", simulation.ScreenCentre, "crate", 0.95f, new Vector2(64, 64)));
                    break;

                case "newWall":

                    break;

                case "newCannon":
                    Projectile projectile = new Projectile("redTempProjectile", Vector2.Zero, "ball", 5, 0.9f, 16, 0.005f);
                    simulation.AddObject(new Cannon("cannon", simulation.ScreenCentre, "cannon", projectile));
                    break;

                case "newTapeMeasure":
                    TapeMeasure tapeMeasure = new TapeMeasure("tape measure", simulation.ScreenCentre, simulation.ScreenCentre + new Vector2(100, 0), 8, "line", "Arial");
                    simulation.AddObject(tapeMeasure);
                    simulation.AddObject(tapeMeasure.Start);
                    simulation.AddObject(tapeMeasure.End);
                    break;

                case "newStopwatch":
                    simulation.AddObject(new Stopwatch("stopwatch", simulation.ScreenCentre, "stopwatch", "SevenSegment"));
                    break;

                case "newDetector":
                    simulation.AddObject(new Detector("detector", simulation.ScreenCentre, "detector", 150));
                    break;

                case "play":
                    simulation.Paused = false;
                    toolbar.SimulationPaused = false;
                    break;

                case "pause":
                    simulation.Paused = true;
                    toolbar.SimulationPaused = true;
                    break;

                case "addStartTrigger":
                    AddTriggerToStopwatch(simulation.SelectedObject, Stopwatch.StopwatchInput.Start);
                    break;

                case "addStopTrigger":
                    AddTriggerToStopwatch(simulation.SelectedObject, Stopwatch.StopwatchInput.Stop);
                    break;

                case "about":
                    OpenWebPage("https://github.com/Liam-dev/Projectile-Simulator");
                    break;
            }
        }

        // Runs on closing of editor form
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Filename == null)
            {
                // Unsaved file warning

                switch (ShowUnsavedFileMessage())
                {
                    // Save file before closing
                    case DialogResult.Yes:
                        e.Cancel = true;
                        Thread thread = new Thread(() =>
                        {
                            Save();
                            CloseEditor();
                        });

                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                        break;

                    // Close without saving
                    case DialogResult.No:       
                        break;

                    // Cancel
                    case DialogResult.Cancel:        
                        e.Cancel = true;
                        break;
                }
            }               
        }

        // Closes Editor and opens new blank editor in new thread (does not save current file)
        private void NewFile()
        {
            // Open blank simulation
            CloseEditor();
            new Thread(() => new Editor().ShowDialog()).Start();
        }

        // Open open file dialogue in new thread (does not save current file)
        private void OpenFile()
        {
            // Open simulation
            Thread thread = new Thread(ShowOpenFileDialogue);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        // Saves current simulation to file in new thread
        private void SaveFile()
        {
            Thread thread = new Thread(Save);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        // Saves current simulation to new file in new thread
        private void SaveAs()
        {
            Thread thread = new Thread(() =>
            {
                ShowSaveFileDialogue();
                ChangeFormTitle();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();    
        }

        // Writes simulation to file if already saved or shows dialogue for unnamed files
        protected void Save()
        {
            if (Filename == null)
            {
                ShowSaveFileDialogue();
                ChangeFormTitle();
            }
            else if (File.Exists(Filename))
            {
                FileSaver.WriteJson(Filename, simulation.GetState());
            }
        }

        // Saves file before performing another action
        protected void SaveFilePerformAction(Action action)
        {
            // Unsaved file warning message

            switch (ShowUnsavedFileMessage())
            {
                case DialogResult.Yes:

                    // Save current simulation
                    Thread saveThread = new Thread(() =>
                    {
                        // Save
                        if (Filename == null || Filename == string.Empty)
                        {
                            if (ShowSaveFileDialogue())
                            {
                                ChangeFormTitle();

                                // Perform action
                                action();
                            }
                        }
                        else if (File.Exists(Filename))
                        {
                            FileSaver.WriteJson(Filename, simulation.GetState());

                            // Perform action
                            action();
                        }

                    });

                    saveThread.SetApartmentState(ApartmentState.STA);
                    saveThread.Start();
                    break;

                case DialogResult.No:
                    // Set dummy name to stop warning message
                    Filename = string.Empty;

                    // Perform action
                    action();
                    break;

                default:
                    break;
            }
        }

        // Thread-safe changing of Editor title
        private void ChangeFormTitle()
        {
            if (InvokeRequired)
            {
                Invoke(new SafeCallDelegate(ChangeFormTitle));
            }
            else
            {
                if (Filename != null)
                {
                    Text = Path.GetFileName(Filename) + " - Projectile Simulator";
                }
            }
        }

        // Thread-safe closing of Editor
        private void CloseEditor()
        {
            if (InvokeRequired)
            {
                Invoke(new SafeCallDelegate(CloseEditor));
            }
            else
            {
                Close();
            }
        }

        // Shows message warning box about unsaved file
        protected DialogResult ShowUnsavedFileMessage()
        {
            string name = "Untitled Simulation";
            
            if (Filename != null && Filename != string.Empty)
            {
                name = Path.GetFileName(Filename);
            }
            string message = "Do you want to save your changes to \"" + name + "\" \n\n" +
                             "If you click \"No\", your changes will be lost forever! (a long time!)";

            return MessageBox.Show(message, "Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }

        // Shows save file dialogue and saves simulation to file
        protected bool ShowSaveFileDialogue()
        {
            SaveFileDialog fileDialogue = new SaveFileDialog();
            fileDialogue.Title = "Save Simulation File";
            fileDialogue.DefaultExt = "sim";
            fileDialogue.AddExtension = true;
            fileDialogue.CheckPathExists = true;
            fileDialogue.Filter = "Simulation files (*.sim)|*.sim|All files (*.*)|*.*";

            if (fileDialogue.ShowDialog() == DialogResult.OK)
            {
                Filename = fileDialogue.FileName;
                FileSaver.WriteJson(Filename, simulation.GetState());
                return true;
            }
            else
            {
                return false;
            }
        }

        // Shows open file dialogue and opens selected file in new thread and closes current Editor 
        protected void ShowOpenFileDialogue()
        {
            OpenFileDialog fileDialogue = new OpenFileDialog();
            fileDialogue.Title = "Open Simulation File";
            fileDialogue.DefaultExt = "sim";
            fileDialogue.Multiselect = false;
            fileDialogue.CheckFileExists = true;
            fileDialogue.Filter = "Simulation files (*.sim)|*.sim";

            if (fileDialogue.ShowDialog() == DialogResult.OK)
            {
                CloseEditor();
                new Thread(() => new Editor(fileDialogue.FileName, false).ShowDialog()).Start();
            }
            else
            {
                // Allows saving after cancelled load
                Filename = null;
            }
        }

        // Takes a screenshot of the simulation window and saves it as a png image to a specified file
        protected void Screenshot()
        {
            Thread saveThread = new Thread(() =>
            {
                RenderTarget2D screenshot = simulation.GetDrawCapture();

                SaveFileDialog fileDialogue = new SaveFileDialog();
                fileDialogue.Title = "Save Screenshot";
                fileDialogue.DefaultExt = "png";
                fileDialogue.AddExtension = true;
                fileDialogue.CheckPathExists = true;
                fileDialogue.Filter = "Portable Graphics Format (*.png)|*.png|All files (*.*)|*.*";

                if (fileDialogue.ShowDialog() == DialogResult.OK)
                {
                    FileStream fileStream = new FileStream(fileDialogue.FileName, FileMode.Create);
                    screenshot.SaveAsPng(fileStream, screenshot.Width, screenshot.Height);
                    fileStream.Close();
                }
            });

            saveThread.SetApartmentState(ApartmentState.STA);
            saveThread.Start();
        }

        protected void AddTriggerToStopwatch(ISelectable selectable, Stopwatch.StopwatchInput input)
        {
            if (selectable is Stopwatch stopwatch)
            {
                List<object> allTriggers = new List<object>();
                foreach (SimulationObject simulationObject in simulation.GetObjects())
                {
                    if (simulationObject is ITrigger)
                    {
                        allTriggers.Add(simulationObject);
                    }
                }

                List<object> currentTriggers = new List<object>();
                foreach (var trigger in stopwatch.Triggers)
                {
                    if (trigger.Item2 == input)
                    {
                        currentTriggers.Add(trigger.Item1);
                    }
                }

                string title = "Select " + input.ToString().ToLower() +" triggers for " + stopwatch.Name;

               ObjectSelectionBox objectSelectionBox = new ObjectSelectionBox(allTriggers, currentTriggers, title, "Update triggers");

                if (objectSelectionBox.ShowDialog(this) == DialogResult.OK)
                {
                    // Add new triggers
                    foreach (ITrigger trigger in objectSelectionBox.CheckedObjects)
                    {
                        stopwatch.AddTrigger(trigger, input);
                    }

                    // Remove old triggers
                    foreach (ITrigger trigger in allTriggers.Except(objectSelectionBox.CheckedObjects))
                    {
                        stopwatch.RemoveTrigger(trigger, input);
                    }
                }
            }   
        }

        // Opens URL of web page in default browser 
        protected void OpenWebPage(string url)
        {
            var processStartInfo = new System.Diagnostics.ProcessStartInfo();
            processStartInfo.UseShellExecute = true;
            processStartInfo.FileName = url;
            System.Diagnostics.Process.Start(processStartInfo);
        }
    }
}
