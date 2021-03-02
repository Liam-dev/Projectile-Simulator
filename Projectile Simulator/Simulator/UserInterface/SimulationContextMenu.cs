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
    public partial class SimulationContextMenu : UserControl
    {
        public event EventHandler ButtonClicked;

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
