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
    /// <summary>
    /// Control that allows the property editing of objects.
    /// </summary>
    public partial class Inspector : UserControl
    {
        // The current selected object in the inspector
        protected object selectedObject;

        /// <summary>
        /// Gets or sets the inspector's selected object.
        /// </summary>
        public object SelectedObject
        {
            get { return selectedObject; }

            set
            {
                if (value is Handle handle)
                {
                    propertyGrid.SelectedObject = handle.Parent;
                }
                else
                {
                    selectedObject = propertyGrid.SelectedObject = selectionBox.SelectedItem = value;
                } 
            }
        }           

        /// <summary>
        /// Occurs when the inspector's selected object is changed.
        /// </summary>
        public event EventHandler SelectedObjectChanged;

        public Inspector()
        {
            InitializeComponent();
        }

        private void Inspector_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets the data source for the inspector's selection box.
        /// </summary>
        /// <param name="source">List of objects to set as data source</param>
        public void SetDataSource(List<SimulationObject> source)
        {
            // if object deleted from simulation then clear from inspector
            if (!source.Contains((SimulationObject)SelectedObject))
            {
                SelectedObject = null;
            }

            // Remove handle objects
            source.RemoveAll(x => x is Handle);

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
