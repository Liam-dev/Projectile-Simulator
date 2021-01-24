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
            var m = new SimulationObject(new Vector2(8, 1.6f), simulation.Editor.Content.Load<Texture2D>("character"));
            simulation.AddObject(m);
            inspector.Object = m;
        }

        /*
        public Editor(string filename)
        {
            
        }
        */

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ObjectSerializer.WriteToJson("C:/Users/Liam/Desktop/ObjectData/data.sim", simulation.GetObjects());
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
            simulation.camera.Zoom *= 1.5f;
        }

        private void toolbar_ZoomOutButtonClicked(object sender, EventArgs e)
        {
            //simulation.Scale /= 1.5f;
            simulation.camera.Zoom /= 1.5f;
        }
    }
}
