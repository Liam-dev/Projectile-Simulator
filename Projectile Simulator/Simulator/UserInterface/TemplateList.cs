using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    public partial class TemplateList : UserControl
    {
        public event EventHandler ItemDoubleClicked;

        public string SelectedItem { get { return (string)listBox.SelectedItem; } }

        public TemplateList()
        {
            InitializeComponent();
        }

        private void TemplateList_Load(object sender, EventArgs e)
        {
            if (!(Site != null && Site.DesignMode))
            {
                string path = Directory.GetCurrentDirectory() + "/Content/Templates/";
                string[] names = Directory.GetFiles(path);

                List<string> files = new List<string>();

                foreach (string file in names)
                {
                    files.Add(Path.GetFileNameWithoutExtension(file));
                }

                listBox.DataSource = files;
            }
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            ItemDoubleClicked?.Invoke(sender, e);
        }
    }
}
