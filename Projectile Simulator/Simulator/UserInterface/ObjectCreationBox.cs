using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Simulator.Simulation;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Form used to name and create a new object.
    /// </summary>
    public partial class ObjectCreationBox : Form
    {
        private List<string> usedNames = new List<string>();

        /// <summary>
        /// Gets the name of the created object.
        /// </summary>
        public string ObjectName { get; private set; }

        /// <summary>
        /// Constructor for ObjectCreationBox.
        /// </summary>
        /// <param name="objects">Objects that have already been created.</param>
        public ObjectCreationBox(List<object> objects)
        {
            InitializeComponent();

            foreach (object @object in objects)
            {
                if (@object is SimulationObject simulationObject)
                {
                    usedNames.Add(simulationObject.Name);
                }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (textBox.TextLength == 0)
            {
                MessageBox.Show("No name was entered", "Invalid name!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (usedNames.Contains(textBox.Text))
            {
                MessageBox.Show("Name entered was not unique", "Invalid name!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                // Unique name
                ObjectName = textBox.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
