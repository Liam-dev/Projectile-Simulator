using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Projectile_Simulator
{
    public partial class StartPane : UserControl
    {
        public StartPane()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            Editor editor = new Editor();
            editor.Show();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileLoader = new OpenFileDialog();
            fileLoader.ShowDialog();
        }

        private void preferencesButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Preferences opening");
        }
    }
}
