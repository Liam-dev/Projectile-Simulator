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

namespace Simulator
{
    /// <summary>
    /// Main editor form
    /// </summary>
    public partial class Editor : Form
    {
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

        // Run on close
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFile();
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
                    SaveFile();
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

        protected void SaveFile()
        {
            if (Filename == null)
            {
                Thread thread = new Thread(() =>
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
                        Text = Filename + " - Projectile Simulator";
                        ObjectWriter.WriteJson(Filename, simulation.GetObjects());
                    }
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            else if (File.Exists(Filename))
            {
                ObjectWriter.WriteJson(Filename, simulation.GetObjects());
            }
        }

        protected void OpenFile()
        {
            Thread thread = new Thread(() =>
            {
                OpenFileDialog fileDialogue = new OpenFileDialog();
                fileDialogue.Title = "Open Simulation File";
                fileDialogue.DefaultExt = "sim";
                fileDialogue.Multiselect = false;

                if (fileDialogue.ShowDialog() == DialogResult.OK)
                {
                    //Close();
                    new Thread(() => new Editor(fileDialogue.FileName).ShowDialog()).Start();
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
