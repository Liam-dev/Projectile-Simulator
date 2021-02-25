using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Simulator.Simulation;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace Simulator
{
    /// <summary>
    /// Main editor form
    /// </summary>
    public partial class Editor : Form
    {
        public delegate void SafeCallDelegate();

        public string Filename { get; protected set; }

        private List<SimulationObject> objectsToLoad;

        public Editor()
        {
            InitializeComponent();

            objectsToLoad = new List<SimulationObject>();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            // Test objects for mouse zoom testing

            objectsToLoad.Add(new Box("box", new Vector2(0, -64), "crate", 0.95f, new Vector2(64, 64)));
            objectsToLoad.Add(new Box("box", new Vector2(0, 0), "crate", 0.95f, new Vector2(64, 64)));
            objectsToLoad.Add(new Box("box", new Vector2(100, 400), "crate", 0.95f, new Vector2(64, 64)));
            objectsToLoad.Add(new Box("box", new Vector2(400, 100), "crate", 0.95f, new Vector2(64, 64)));
            objectsToLoad.Add(new Box("box", new Vector2(400, 400), "crate", 0.95f, new Vector2(64, 64)));

            objectsToLoad.Add(new Box("box", new Vector2(800, 100), "wall", 0.95f, new Vector2(20, 500)));
            objectsToLoad.Add(new Box("box", new Vector2(320, 600), "wall", 0.95f, new Vector2(500, 20)));

            Projectile projectile = new Projectile("redTempProjectile", Vector2.Zero, "ball", 5, 0.9f, 0.005f);
            objectsToLoad.Add(new Cannon("cannon", new Vector2(0, 600), "cannon", projectile));

            objectsToLoad.Add(new TapeMeasure("tape measure", new Vector2(64, -64), new Vector2(0, 64), 8, "line"/*, simulation.Editor.Content.Load<SpriteFont>("Label")*/));
        }     

        /// <summary>
        /// Open Editor with  file
        /// </summary>
        /// <param name="filename"></param>
        public Editor(string filename)
        {
            InitializeComponent();

            Filename = filename;
            ChangeFormTitle();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            objectsToLoad = ObjectWriter.ReadJson<SimulationObject>(Filename);
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            simulation.SelectedObjectChanged += Simulation_SelectedObjectChanged;
            simulation.SimulationPaused += toolbar.Simulation_Paused;
            simulation.SimulationUnPaused += toolbar.Simulation_UnPaused;

            simulation.Paused = false;

            // load objects
            foreach (var @object in objectsToLoad)
            {
                simulation.AddObject(@object);

                // Cannon test
                if (@object is Cannon cannon)
                {
                    cannon.Fired += simulation.CannonFired;
                }
            }

            inspector.SetDataSource(simulation.GetObjects());
        }

        private void Simulation_SelectedObjectChanged(object sender, EventArgs e)
        {
            inspector.Object = (SimulationObject)sender;
        }

        private void toolbar_ButtonClicked(object sender, EventArgs e)
        {
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

            switch (tag)
            {
                case "newFile":
                    simulation.Paused = true;
                    SaveFilePerformAction(NewFile);
                    break;

                case "openFile":
                    simulation.Paused = true;
                    SaveFilePerformAction(OpenFile);
                    break;

                case "saveFile":
                    simulation.Paused = true;
                    SaveFile();
                    break;

                case "saveAs":
                    simulation.Paused = true;
                    SaveAs();
                    break;

                case "exit":
                    simulation.Paused = true;
                    CloseEditor();
                    break;

                case "screenshot":
                    simulation.Paused = true;
                    Screenshot();
                    break;

                case "ball":
                    simulation.FireAllCannons();
                    break;

                case "newBox":
                    simulation.AddObject(new Box("box", simulation.ScreenCentre, "crate", 0.95f, new Vector2(64, 64)));
                    break;

                case "play":
                    simulation.Paused = false;
                    toolbar.SimulationPaused = false;
                    break;

                case "pause":
                    simulation.Paused = true;
                    toolbar.SimulationPaused = true;
                    break;

                case "about":
                    OpenWebPage("https://github.com/Liam-dev/Projectile-Simulator");
                    break;
            }
        }

        // Run on close
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            simulation.Paused = true;

            if (Filename == null)
            {
                switch (ShowUnsavedFileMessage())
                {
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

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }               
        }

        private void NewFile()
        {
            // Open blank simulation
            CloseEditor();
            new Thread(() => new Editor().ShowDialog()).Start();
        }

        private void OpenFile()
        {
            // Open new simulation
            Thread thread = new Thread(ShowOpenFileDialogue);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void SaveFile()
        {
            Thread thread = new Thread(Save);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

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

        protected void Save()
        {
            if (Filename == null)
            {
                ShowSaveFileDialogue();
                ChangeFormTitle();
            }
            else if (File.Exists(Filename))
            {
                ObjectWriter.WriteJson(Filename, simulation.GetObjects());
            }
        }

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
                                action();
                            }
                        }
                        else if (File.Exists(Filename))
                        {
                            ObjectWriter.WriteJson(Filename, simulation.GetObjects());
                            action();
                        }
                    });

                    saveThread.SetApartmentState(ApartmentState.STA);
                    saveThread.Start();
                    break;

                case DialogResult.No:
                    // Set dummy name to stop warning message
                    Filename = string.Empty;
                    action();
                    break;

                default:
                    break;
            }
        }

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
                ObjectWriter.WriteJson(Filename, simulation.GetObjects());
                return true;
            }
            else
            {
                return false;
            }
        }

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
                new Thread(() => new Editor(fileDialogue.FileName).ShowDialog()).Start();
            }
            else
            {
                // Allows saving after cancelled load
                Filename = null;
            }
        }
        protected void OpenWebPage(string url)
        {
            var processStartInfo = new System.Diagnostics.ProcessStartInfo();
            processStartInfo.UseShellExecute = true;
            processStartInfo.FileName = url;
            System.Diagnostics.Process.Start(processStartInfo);
        }
    }
}
