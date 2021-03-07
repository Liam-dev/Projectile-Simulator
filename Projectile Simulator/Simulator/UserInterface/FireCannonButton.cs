using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Simulator.Simulation;

namespace Simulator.UserInterface
{
    public partial class FireCannonButton : UserControl
    {
        public event EventHandler Clicked;

        public Cannon Cannon { get; set; }

        public Vector2 DrawOffset { get; set; }

        public FireCannonButton()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, new EventArgs());
        }
    }
}
