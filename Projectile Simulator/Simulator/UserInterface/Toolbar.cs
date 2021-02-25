using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    public partial class Toolbar : UserControl
    {
        public event EventHandler ButtonClicked;

        public bool SimulationPaused
        {
            get
            {
                return playToolStripButton.Enabled;
            }
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
    }
}
