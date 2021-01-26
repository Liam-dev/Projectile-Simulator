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
        public Editor()
        {
            InitializeComponent();

            // Re-enables updates for simulation (causes performance issues in designer)
            simulation.MouseHoverUpdatesOnly = false;
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            
            simulation.AddObject(new SimulationObject(new Vector2(100, 100), simulation.Editor.Content.Load<Texture2D>("character")));
            simulation.AddObject(new SimulationObject(new Vector2(100, 400), simulation.Editor.Content.Load<Texture2D>("character")));
            simulation.AddObject(new SimulationObject(new Vector2(400, 100), simulation.Editor.Content.Load<Texture2D>("character")));
            simulation.AddObject(new SimulationObject(new Vector2(400, 400), simulation.Editor.Content.Load<Texture2D>("character")));
            
            //simulation.AddObject(new SimulationObject(new Vector2(0, 0), simulation.Editor.Content.Load<Texture2D>("map2")));
            
        }


        public Editor(string filename)
        {
            simulation.MouseHoverUpdatesOnly = false;
            //List<SimulationObject> loadedObjects = ObjectSerializer.ReadFromJson<SimulationObject>(filename)
            //simulation.AddObject();

        }
        
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            ObjectSerializer.WriteToJson("C:/Users/Liam/Desktop/ObjectData/data.sim", simulation.GetObjects());
        }

        private void toolbar_BallButtonClicked(object sender, EventArgs e)
        {
            var p = new Projectile(new Vector2(2, 9), simulation.Editor.Content.Load<Texture2D>("ball"), 20);
            simulation.AddObject(p);
            //inspector.Object = p;
        }

        private void toolbar_ZoomInButtonClicked(object sender, EventArgs e)
        {
            //simulation.Scale *= 1.5f;
            //simulation.camera.ZoomIn();
        }

        private void toolbar_ZoomOutButtonClicked(object sender, EventArgs e)
        {
            //simulation.Scale /= 1.5f;
            //simulation.camera.ZoomOut();
        }
    }
}
