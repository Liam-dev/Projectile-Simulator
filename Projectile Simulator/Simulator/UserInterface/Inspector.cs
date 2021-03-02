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
        protected object selectedObject;

        public object SelectedObject { get { return selectedObject; }  set { selectedObject = propertyGrid.SelectedObject = selectionBox.SelectedItem = value; } }

        public event EventHandler SelectedObjectChanged;

        public Inspector()
        {
            InitializeComponent();
        }

        private void Inspector_Load(object sender, EventArgs e)
        {

        }

        public void SetDataSource(List<SimulationObject> source)
        {
            // if object deleted from simulation then clear from inspector
            if (!source.Contains((SimulationObject)SelectedObject))
            {
                SelectedObject = null;
            }

            // Remove and re-add items to selection combo box
            selectionBox.Items.Clear();
            selectionBox.Items.AddRange(source.ToArray());
            selectionBox.DisplayMember = "Name";
        }

        private void selectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedObject = selectionBox.SelectedItem;
            SelectedObjectChanged?.Invoke(SelectedObject, e);
        }
    }
}
