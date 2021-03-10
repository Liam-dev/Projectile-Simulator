using Simulator.Simulation;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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

        // When accept button is clicked
        private void addButton_Click(object sender, EventArgs e)
        {
            // Check if entered text is valid

            if (textBox.TextLength == 0)
            {
                // Nothing entered
                MessageBox.Show("No name was entered", "Invalid name!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (usedNames.Contains(textBox.Text))
            {
                // Name was not unique
                MessageBox.Show("Name entered was not unique", "Invalid name!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                // Unique name entered
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