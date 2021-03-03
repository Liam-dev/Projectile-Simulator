using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Simulator.Simulation;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Control that contains context menu for Simulation control.
    /// </summary>
    public partial class SimulationContextMenu : UserControl
    {
        /// <summary>
        /// Occurs when button in context menu is clicked.
        /// </summary>
        public event EventHandler ButtonClicked;

        /// <summary>
        /// Sets the items that are displayed in the context menu depending on the object that is selected.
        /// </summary>
        public object SelectedObject
        {
            set { SetMenuItems((SimulationObject)value); }
        }

        public SimulationContextMenu()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            ButtonClicked?.Invoke(sender, e);
        }

        // Determines the items that should be displayed in the context menu
        private void SetMenuItems(SimulationObject value)
        {
            contextMenuStrip.Items.Clear();

            // Cannon
            if (value is Cannon)
            {
                contextMenuStrip.Items.AddRange(new ToolStripItem[] {
                fireCannonToolStripMenuItem,
                toolStripSeparator2 });
            }

            // Simulation Object
            contextMenuStrip.Items.AddRange(new ToolStripItem[] {
            cutToolStripMenuItem,
            copyToolStripMenuItem,
            pasteToolStripMenuItem,
            toolStripSeparator1,
            deleteToolStripMenuItem });

            // Stopwatch

        }
    }
}
