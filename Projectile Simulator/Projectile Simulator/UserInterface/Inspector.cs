using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Projectile_Simulator.UserInterface
{
    public partial class Inspector : UserControl
    {
        Vector2 stuff;

        public Inspector()
        {
            InitializeComponent();
            stuff = new Vector2();
            propertyGrid.SelectedObject = stuff;
        }

        private void Inspector_Load(object sender, EventArgs e)
        {

        }

        private void propertyGrid_Click(object sender, EventArgs e)
        {

        }
    }
}
