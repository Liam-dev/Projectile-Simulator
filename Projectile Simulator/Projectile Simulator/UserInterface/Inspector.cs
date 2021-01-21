using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Projectile_Simulator.Simulation;

namespace Projectile_Simulator.UserInterface
{
    public partial class Inspector : UserControl
    {
        public SimulationObject Object
        {
            set { propertyGrid.SelectedObject = value; }
        }

        public Inspector()
        {
            InitializeComponent();
        }

        private void Inspector_Load(object sender, EventArgs e)
        {
        }
    }
}
