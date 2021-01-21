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

        public Toolbar()
        {
            InitializeComponent();
        }

        private void ballToolStripButton_Click(object sender, EventArgs e)
        {
            OnBallButtonClicked(e);
        }

        protected virtual void OnBallButtonClicked(EventArgs e)
        {
            BallButtonClicked?.Invoke(this, e);
        }
    }
}
