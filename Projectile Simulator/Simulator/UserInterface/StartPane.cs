using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Simulator.UserInterface
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
            if (templateList.SelectedItem != null)
            {
                string templateName = templateList.SelectedItem;
                string path = "Content/Templates/" + templateName + ".sim";
                new Thread(() => new Editor(path, true).ShowDialog()).Start();
            }
            else
            {
                new Thread(() => new Editor().ShowDialog()).Start();
            }
            Application.ExitThread();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialogue = new OpenFileDialog();
            fileDialogue.Title = "Open Simulation File";
            fileDialogue.DefaultExt = "sim";
            fileDialogue.Multiselect = false;
            fileDialogue.CheckFileExists = true;
            fileDialogue.Filter = "Simulation files (*.sim)|*.sim";

            if (fileDialogue.ShowDialog() == DialogResult.OK)
            {
                new Thread(() => new Editor(fileDialogue.FileName, false).ShowDialog()).Start();
                Application.ExitThread();
            }        
        }

        private void preferencesButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Preferences opening");
        }
    }
}
