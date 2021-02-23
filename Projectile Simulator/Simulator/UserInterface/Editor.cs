using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Simulator.Simulation;
using System.Threading.Tasks;

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

            Projectile projectile = new Projectile("redTempProjectile", Vector2.Zero, "ball", 5, 0.95f, 0.005f);
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
            Text = Filename + " - Projectile Simulator";

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            objectsToLoad = ObjectWriter.ReadJson<SimulationObject>(Filename);
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            // load objects
            foreach (var @object in objectsToLoad)
            {
                simulation.AddObject(@object);
            }

            // Cannon test
            simulation.cannon = (Cannon)objectsToLoad.Find(x => x is Cannon cannon);
            // register event
            simulation.cannon.Fired += simulation.CannonFired;

            inspector.SetDataSource(simulation.GetObjects());
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
                    break;

                case "openFile":
                    OpenFile();
                    break;

                case "saveFile":
                    Thread thread = new Thread(SaveFile);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    break;

                case "ball":
                    simulation.cannon.Fire();
                    break;

                case "newBox":
                    simulation.AddObject(new Box("box", new Vector2(-100, -100), "crate", 0.95f, new Vector2(64, 64)));
                    break;

                case "play":
                    simulation.Paused = false;
                    break;

                case "pause":
                    simulation.Paused = true;
                    break;
            }
        }

        // Run on close
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Filename == null)
            {
                string message = "Do you want to save your changes to the simulation\n\n" +
                             "If you click \"No\", your changes will be lost forever (a very long time!)";

                DialogResult result = MessageBox.Show(message, "Projectile Simulator", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                switch (result)
                {
                    case DialogResult.Yes:
                        e.Cancel = true;
                        Thread thread = new Thread(() =>
                        {
                            SaveFile();
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



        protected void ShowSaveFileDialogue()
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
            }
        }

        protected void SaveFile()
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
                    Text = Filename + " - Projectile Simulator";
                }
            }
        }

        protected void ShowOpenFileDialogue()
        {
            OpenFileDialog fileDialogue = new OpenFileDialog();
            fileDialogue.Title = "Open Simulation File";
            fileDialogue.DefaultExt = "sim";
            fileDialogue.Multiselect = false;

            if (fileDialogue.ShowDialog() == DialogResult.OK)
            {
                CloseEditor();
                new Thread(() => new Editor(fileDialogue.FileName).ShowDialog()).Start();
            }
        }

        protected void OpenFile()
        {
            Thread thread = new Thread(() =>
            {
                // Save current simulation
                Thread saveThread = new Thread(SaveFile);
                saveThread.SetApartmentState(ApartmentState.STA);
                saveThread.Start();

                // Load new simulation
                Thread loadThread = new Thread(ShowOpenFileDialogue);
                loadThread.SetApartmentState(ApartmentState.STA);
                loadThread.Start();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();    
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
    }
}
