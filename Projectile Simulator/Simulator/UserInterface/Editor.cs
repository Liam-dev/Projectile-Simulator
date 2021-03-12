using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Simulator.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

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
        private SimulationState loadedState;

        // Object that is currently saved to clipboard
        private SimulationObject clipboardObject;

        // Object to save undo redo changes
        private UndoRedoStack<SimulationState> undoRedoStack = new UndoRedoStack<SimulationState>();

        // Editor preferences
        private EditorPreferences preferences;

        /// <summary>
        /// Path to save the Editor preferences file to.
        /// </summary>
        public static string PreferencesPath = "preferences.json";

        /// <summary>
        /// Gets the filename of the file in the editor.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Parameterless constructor for Editor to open with no file.
        /// </summary>
        public Editor()
        {
            InitializeComponent();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            WindowState = FormWindowState.Maximized;

            // Load default simulation state
            loadedState = new SimulationState();

            // Load preferences
            ReadPreferences();
        }

        /// <summary>
        /// Constructor for Editor to open with a file.
        /// </summary>
        /// <param name="filename">Name of file to load into editor.</param>
        /// <param name="isTemplate">Whether the file is to be used as template.</param>
        /// <param name="state">JSON text of simulation state to deserialize.</param>
        public Editor(string filename, bool isTemplate = false, string state = default)
        {
            InitializeComponent();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            WindowState = FormWindowState.Maximized;

            // Load simulation state from file or template
            if (isTemplate)
            {
                loadedState = FileSaver.DeserializeJson<SimulationState>(state);
            }
            else
            {
                loadedState = FileSaver.ReadJson<SimulationState>(filename);
            }
            

            // Initialise undo redo stack
            undoRedoStack.AddState(loadedState);

            ReadPreferences();      

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
            simulation.ObjectMoved += Simulation_ObjectMoved;

            toolbar.SetUndoButtonState(false, false);

            simulation.LoadState(loadedState, true);
        }

        // Reads user preferences from preferences file
        private void ReadPreferences()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/" + PreferencesPath;
            if (File.Exists(path))
            {
                preferences = FileSaver.ReadJson<EditorPreferences>(path);
            }
            else
            {
                preferences = new EditorPreferences() { AutoName = false, ShowTrajectories = true };
            }

            Trajectory.Visible = preferences.ShowTrajectories;
        }

        private void Simulation_ObjectMoved(object sender, EventArgs e)
        {
            PerformedAction();
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

            // Get action to perform and perform it
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

                case "preferences":
                    OpenPreferences();
                    break;

                case "undo":
                    simulation.LoadState(undoRedoStack.Undo());
                    toolbar.SetUndoButtonState(undoRedoStack.CanUndo(), undoRedoStack.CanRedo());
                    break;

                case "redo":
                    simulation.LoadState(undoRedoStack.Redo());
                    toolbar.SetUndoButtonState(undoRedoStack.CanUndo(), undoRedoStack.CanRedo());
                    break;

                case "cut":
                    clipboardObject = (SimulationObject)simulation.SelectedObject;
                    simulation.RemoveObject(clipboardObject);
                    inspector.SetDataSource(simulation.GetObjectsToDisplay());
                    PerformedAction();
                    break;

                case "copy":
                    clipboardObject = (SimulationObject)simulation.SelectedObject;
                    break;

                case "paste":
                    // Create copy of object
                    JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All };
                    string data = JsonConvert.SerializeObject(clipboardObject, Formatting.Indented, settings);
                    SimulationObject @object = JsonConvert.DeserializeObject<SimulationObject>(data, settings);

                    // Place object at screen centre
                    @object.Position = simulation.Camera.GetSimulationPostion(simulation.MousePosition);

                    // Get name for object
                    CreateNewObject(@object);
                    PerformedAction();
                    break;

                case "deleteObject":
                    SimulationObject selectedObject = (SimulationObject)simulation.SelectedObject;
                    simulation.RemoveObject(selectedObject);
                    inspector.SetDataSource(simulation.GetObjectsToDisplay());
                    PerformedAction();
                    break;

                case "fireCannon":
                    if (simulation.SelectedObject is Cannon cannon)
                    {
                        cannon.Fire();
                    }
                    break;

                case "newBox":
                    CreateNewObject(new Box("box", simulation.ScreenCentre, "crate", 0.95f, new Vector2(64, 64)));
                    PerformedAction();
                    break;

                case "newWall":
                    CreateNewObject(new Wall("wall", simulation.ScreenCentre, Microsoft.Xna.Framework.Color.SaddleBrown, 0.95f, new Vector2(25, 300)));
                    break;

                case "newCannon":
                    Projectile projectile = new Projectile("redTempProjectile", Vector2.Zero, "ball", 5, 0.9f, 16, 0.005f);
                    CreateNewObject(new Cannon("cannon", simulation.ScreenCentre, "cannon", projectile));
                    PerformedAction();
                    break;

                case "newTapeMeasure":

                    TapeMeasure tapeMeasure = new TapeMeasure("tape measure", simulation.ScreenCentre, simulation.ScreenCentre + new Vector2(100, 0), 8, "line", "Arial");
                    bool created = CreateNewObject(tapeMeasure);
                    if (created)
                    {
                        simulation.AddObject(tapeMeasure.Start);
                        simulation.AddObject(tapeMeasure.End);
                    }
                    PerformedAction();
                    break;

                case "newStopwatch":
                    CreateNewObject(new Stopwatch("stopwatch", simulation.ScreenCentre, "stopwatch", "SevenSegment"));
                    PerformedAction();
                    break;

                case "newDetector":
                    CreateNewObject(new Detector("detector", simulation.ScreenCentre, "detector", 150));
                    PerformedAction();
                    break;

                case "play":
                    simulation.Paused = false;
                    toolbar.SimulationPaused = false;
                    PerformedAction();
                    break;

                case "pause":
                    simulation.Paused = true;
                    toolbar.SimulationPaused = true;
                    PerformedAction();
                    break;

                case "addStartTrigger":
                    AddTriggerToStopwatch(simulation.SelectedObject, Stopwatch.StopwatchInput.Start);
                    PerformedAction();
                    break;

                case "addStopTrigger":
                    AddTriggerToStopwatch(simulation.SelectedObject, Stopwatch.StopwatchInput.Stop);
                    PerformedAction();
                    break;

                case "zoomIn":
                    simulation.Camera.ZoomIn(simulation.Camera.GetSimulationPostion(0.25f * new Vector2(Width, Height)));
                    break;

                case "zoomOut":
                    simulation.Camera.ZoomOut(simulation.Camera.GetSimulationPostion(0.25f * new Vector2(Width, Height)));
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
                // Show unsaved file warning of close

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
            else
            {
                SaveFile();
            }
        }

        // Saves simulation state to undo stack
        private void PerformedAction()
        {
            undoRedoStack.AddState(simulation.GetState());
            toolbar.SetUndoButtonState(undoRedoStack.CanUndo(), undoRedoStack.CanRedo());
        }

        // Opens preferences editor to edit Editor preferences and simulation settings
        private void OpenPreferences()
        {
            // Open preferences editor form
            PreferencesEditor preferencesEditor = new PreferencesEditor(preferences, true, simulation.GetState());
            preferencesEditor.ShowDialog(this);

            // Update preferences
            preferences = preferencesEditor.Preferences;
            Trajectory.Visible = preferences.ShowTrajectories;

            // Update simulation settings
            simulation.LoadSettings(preferencesEditor.SimulationSettings);
        }

        // Attempts to add a simulation object into the simulation
        // Returns true if successful, and false if not
        private bool CreateNewObject(SimulationObject @object)
        {
            // Check if new object should be automatically named
            if (preferences.AutoName)
            {
                // Get list of already used names
                List<string> usedNames = new List<string>();
                foreach (SimulationObject existingObject in simulation.GetObjects())
                {
                    usedNames.Add(existingObject.Name);
                }

                // Continue to attempt to generate new unique object name until it is unique
                string name = @object.GetType().Name;
                int i = 1;
                while (usedNames.Contains(name))
                {
                    // Generate new name
                    name = @object.GetType().Name + i.ToString();
                    i++;
                }

                // Apply unique name
                @object.Name = name;
            }
            else
            {
                // Open object creation form to get user to enter name
                ObjectCreationBox objectCreationBox = new ObjectCreationBox(simulation.GetObjectsToSave());

                if (objectCreationBox.ShowDialog(this) == DialogResult.OK)
                {
                    // Apply unique name
                    @object.Name = objectCreationBox.ObjectName;
                }
                else
                {
                    return false;
                }
            }

            // Add object to simulation
            simulation.AddObject(@object);
            inspector.SelectedObject = @object;
            return true;
        }

        // Closes Editor and opens new blank editor in new thread (does not save current file)
        private void NewFile()
        {
            // Open blank simulation
            CloseEditor();
            new Thread(() => new Editor().ShowDialog()).Start();
        }

        // Open a open file dialogue in new thread (does not save current file)
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
        private void Save()
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
        private void SaveFilePerformAction(Action action)
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
        private DialogResult ShowUnsavedFileMessage()
        {
            // Get filename to display in message
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
        // Returns true if file saved, and false if not
        private bool ShowSaveFileDialogue()
        {
            SaveFileDialog fileDialogue = new SaveFileDialog
            {
                Title = "Save Simulation File",
                DefaultExt = "sim",
                AddExtension = true,
                CheckPathExists = true,
                Filter = "Simulation files (*.sim)|*.sim|All files (*.*)|*.*"
            };

            // If file is saved by user, then write simulation state to file
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
        private void ShowOpenFileDialogue()
        {
            OpenFileDialog fileDialogue = new OpenFileDialog
            {
                Title = "Open Simulation File",
                DefaultExt = "sim",
                Multiselect = false,
                CheckFileExists = true,
                Filter = "Simulation files (*.sim)|*.sim"
            };

            if (fileDialogue.ShowDialog() == DialogResult.OK)
            {
                // Open file in new editor
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
        private void Screenshot()
        {
            Thread saveThread = new Thread(() =>
            {
                // Get the rendered simulation
                RenderTarget2D screenshot = simulation.GetDrawCapture();

                SaveFileDialog fileDialogue = new SaveFileDialog
                {
                    Title = "Save Screenshot",
                    DefaultExt = "png",
                    AddExtension = true,
                    CheckPathExists = true,
                    Filter = "Portable Graphics Format (*.png)|*.png|All files (*.*)|*.*"
                };

                if (fileDialogue.ShowDialog() == DialogResult.OK)
                {
                    // Save simulation render as png to the specified file
                    FileStream fileStream = new FileStream(fileDialogue.FileName, FileMode.Create);
                    screenshot.SaveAsPng(fileStream, screenshot.Width, screenshot.Height);
                    fileStream.Close();
                }
            });

            saveThread.SetApartmentState(ApartmentState.STA);
            saveThread.Start();
        }

        // Adds a trigger to the selected stopwatch
        private void AddTriggerToStopwatch(ISelectable selectable, Stopwatch.StopwatchInput input)
        {
            if (selectable is Stopwatch stopwatch)
            {
                List<object> availableTriggers = new List<object>();
                List<object> currentTriggers = new List<object>();

                // Get available triggers to add (any trigger in simulation that isn't on a different input of the selected stopwatch)
                foreach (SimulationObject simulationObject in simulation.GetObjects())
                {
                    if (simulationObject is ITrigger trigger)
                    {
                        // Check if trigger is already assigned to the selected stopwatch
                        if (stopwatch.TriggerDictionary.ContainsKey(trigger))
                        {
                            // Check is trigger is on the selected input
                            if (stopwatch.TriggerDictionary[trigger] == input)
                            {
                                // If yes, then make trigger available
                                availableTriggers.Add(trigger);
                            }

                            // If trigger is already on the other input, then do not add trigger to stopwatch
                        }
                        else
                        {
                            // If trigger is not assigned already to the stopwatch, then make it available
                            availableTriggers.Add(trigger);
                        }
                    }
                }

                // Get current triggers for the stopwatch on the specified input
                foreach (var trigger in stopwatch.TriggerDictionary)
                {
                    if (trigger.Value == input)
                    {
                        currentTriggers.Add(trigger.Key);
                    }
                }

                string title = "Select " + input.ToString().ToLower() + " triggers for " + stopwatch.Name;

                ObjectSelectionBox objectSelectionBox = new ObjectSelectionBox(availableTriggers, currentTriggers, title, "Update triggers");

                // Show trigger selection box
                if (objectSelectionBox.ShowDialog(this) == DialogResult.OK)
                {
                    // Add new triggers
                    foreach (ITrigger trigger in objectSelectionBox.CheckedObjects)
                    {
                        stopwatch.AddTrigger(trigger, input);
                    }

                    // Remove old triggers
                    foreach (ITrigger trigger in availableTriggers.Except(objectSelectionBox.CheckedObjects))
                    {
                        stopwatch.RemoveTrigger(trigger);
                    }
                }
            }
        }

        // Opens URL of web page in default browser
        private void OpenWebPage(string url)
        {
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = url
            };

            // Open page
            System.Diagnostics.Process.Start(processStartInfo);
        }
    }
}