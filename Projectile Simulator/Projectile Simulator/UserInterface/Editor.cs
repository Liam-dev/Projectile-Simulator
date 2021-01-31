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
        List<SimulationObject> loadedObjects;

        public Editor()
        {
            InitializeComponent();

            loadedObjects = new List<SimulationObject>();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;
            
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            foreach (var @object in loadedObjects)
            {
                simulation.AddObject(@object);
            }

            // Test objects for mouse zoom testing
            simulation.AddObject(new SimulationObject(new Vector2(100, 100), simulation.Editor.Content.Load<Texture2D>("crate")));
            simulation.AddObject(new SimulationObject(new Vector2(100, 400), simulation.Editor.Content.Load<Texture2D>("crate")));
            simulation.AddObject(new SimulationObject(new Vector2(400, 100), simulation.Editor.Content.Load<Texture2D>("crate")));
            simulation.AddObject(new SimulationObject(new Vector2(400, 400), simulation.Editor.Content.Load<Texture2D>("crate")));
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

            loadedObjects = ObjectSerializer.ReadFromJson<List<SimulationObject>>(filename);
        }
        
        // Run on close
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            ObjectSerializer.WriteToJson("C:/Users/Liam/Desktop/ObjectData/data.sim", simulation.GetObjects());
        }

        private void toolbar_BallButtonClicked(object sender, EventArgs e)
        {
            simulation.AddObject(new Projectile(new Vector2(20, 350), new Vector2(300, 0), simulation.Editor.Content.Load<Texture2D>("ball"), 5));
            simulation.AddObject(new Projectile(new Vector2(500, 350), new Vector2(-100, 10), simulation.Editor.Content.Load<Texture2D>("ball"), 5));
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
