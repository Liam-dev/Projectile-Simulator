using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Projectile_Simulator.UserInterface
{
    public partial class Toolbar : UserControl
    {
        public event EventHandler BallButtonClicked;
        public event EventHandler ZoomInButtonClicked;
        public event EventHandler ZoomOutButtonClicked;

        public Toolbar()
        {
            InitializeComponent();
        }

        private void ballToolStripButton_Click(object sender, EventArgs e)
        {
            BallButtonClicked?.Invoke(this, e);
        }

        private void zoomInToolStripButton_Click(object sender, EventArgs e)
        {
            ZoomInButtonClicked?.Invoke(this, e);
        }

        private void zoomOutToolStripButton_Click(object sender, EventArgs e)
        {
            ZoomOutButtonClicked?.Invoke(this, e);
        }

    }
}
