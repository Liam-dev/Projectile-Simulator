using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Control that contains a menu bar and toolbar
    /// </summary>
    public partial class Toolbar : UserControl
    {
        /// <summary>
        /// Occurs when a button in the Toolbar is clicked.
        /// </summary>
        public event EventHandler ButtonClicked;

        /// <summary>
        /// Sets the status of the play and pause buttons in the toolbar.
        /// </summary>
        public bool SimulationPaused
        {
            set
            {
                playToolStripButton.Enabled = value;
                pauseToolStripButton.Enabled = !value;
            }
        }

        public Toolbar()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            ButtonClicked?.Invoke(sender, e);
        }

        public void Simulation_Paused(object sender, EventArgs e)
        {
            SimulationPaused = true;
        }

        public void Simulation_UnPaused(object sender, EventArgs e)
        {
            SimulationPaused = false;
        }

        public void SetUndoButtonState(bool undo, bool redo)
        {
            undoToolStripButton.Enabled = undo;
            redoToolStripButton.Enabled = redo;
        }
    }
}
