using System;
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

        /// <summary>
        /// Constructor for Toolbar.
        /// </summary>
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

        /// <summary>
        /// Sets the button if the undo and redo button are enabled.
        /// </summary>
        /// <param name="undo">Is undo enabled.</param>
        /// <param name="redo">Is redo enabled.</param>
        public void SetUndoButtonState(bool undo, bool redo)
        {
            undoToolStripButton.Enabled = undo;
            redoToolStripButton.Enabled = redo;
        }
    }
}