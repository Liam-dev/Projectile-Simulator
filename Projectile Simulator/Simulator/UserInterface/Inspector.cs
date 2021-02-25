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
    public partial class Inspector : UserControl
    {
        protected SimulationObject selectedObject;

        public SimulationObject Object { get { return selectedObject; }  set { propertyGrid.SelectedObject = selectedObject = value; } }

        public Inspector()
        {
            InitializeComponent();
        }

        private void Inspector_Load(object sender, EventArgs e)
        {

        }

        public void SetDataSource(List<SimulationObject> source)
        {
            selectionBox.DataSource = source;
            selectionBox.DisplayMember = "Name";
        }

        private void selectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object = (SimulationObject)selectionBox.SelectedItem;
        }
    }
}
