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

namespace Projectile_Simulator
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            simulation.MouseHoverUpdatesOnly = false;
        }

        /*
        public Editor(string filename)
        {
            
        }
        */

        private void toolbar_BallButtonClicked(object sender, EventArgs e)
        {
            simulation.AddObject(new Projectile(new Vector2(50, 50), simulation.Editor.Content.Load<Texture2D>("ball"), 20));
        }
    }
}
