using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectile_Simulator.Simulation;

namespace Projectile_Simulator
{
    /// <summary>
    /// Main editor form
    /// </summary>
    public partial class Editor : Form
    {
        private List<SimulationObject> objectsToLoad;

        public Editor()
        {
            InitializeComponent();

            objectsToLoad = new List<SimulationObject>();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            // Test objects for mouse zoom testing
            
            objectsToLoad.Add(new Box(new Vector2(100, 100), "crate", new Vector2(64, 64)));
            objectsToLoad.Add(new Box(new Vector2(100, 400), "crate", new Vector2(64, 64)));
            objectsToLoad.Add(new Box(new Vector2(400, 100), "crate", new Vector2(64, 64)));
            objectsToLoad.Add(new Box(new Vector2(400, 400), "crate", new Vector2(64, 64)));

            objectsToLoad.Add(new Box(new Vector2(800, 100), "wall", new Vector2(20, 500)));
            objectsToLoad.Add(new Box(new Vector2(320, 600), "wall", new Vector2(500, 20)));
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            // load objects
            foreach (var @object in objectsToLoad)
            {
                simulation.AddObject(@object);
            }
        }

        /// <summary>
        /// Open Editor with  file
        /// </summary>
        /// <param name="filename"></param>
        public Editor(string filename)
        {
            InitializeComponent();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;

            objectsToLoad = ObjectWriter.ReadJson<SimulationObject>(filename);
        }
        
        // Run on close
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            ObjectWriter.WriteJson("C:/Users/Liam/Desktop/ObjectData/data.sim", simulation.GetObjects());
        }



        private void toolbar_BallButtonClicked(object sender, EventArgs e)
        {
            simulation.AddObject(new Projectile(simulation.cannon.Position + new Vector2(40, 40), new Vector2(800, -800), "ball", 5));
            //simulation.AddObject(new Projectile(new Vector2(40, 200), new Vector2(200, 60), simulation.Editor.Content.Load<Texture2D>("ball"), 5));
            //simulation.AddObject(new Projectile(new Vector2(400, 220), new Vector2(0, 0), simulation.Editor.Content.Load<Texture2D>("ball"), 5));
        }

        private void toolbar_ZoomInButtonClicked(object sender, EventArgs e)
        {
            /*
            simulation.camera.ZoomIn();
            simulation.camera.Update(Vector2.Zero);
            */
        }

        private void toolbar_ZoomOutButtonClicked(object sender, EventArgs e)
        {
            /*
            simulation.camera.ZoomOut();
            simulation.camera.Update(Vector2.Zero);
            */
        }
    }
}
