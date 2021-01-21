using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Projectile_Simulator
{
    public partial class StartPane : UserControl
    {
        public StartPane()
        {
            InitializeComponent();
        }

        private void StartPane_Load(object sender, EventArgs e)
        {

        }

        private void newButton_Click(object sender, EventArgs e)
        {
            new Thread(() => new Editor().ShowDialog()).Start();
            Application.ExitThread();
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
