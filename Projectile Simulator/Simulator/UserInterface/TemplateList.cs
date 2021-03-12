using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Control the lists simulation templates.
    /// </summary>
    public partial class TemplateList : UserControl
    {
        // Directory containing template
        private string templateDirectory;

        /// <summary>
        /// Occurs when an item in the list is double clicked.
        /// </summary>
        public event EventHandler ItemDoubleClicked;

        /// <summary>
        /// Gets the selected item in the list.
        /// </summary>
        public string SelectedItem { get { return (string)listBox.SelectedItem; } }

        /// <summary>
        /// Constructor for TemplateList.
        /// </summary>
        public TemplateList()
        {
            InitializeComponent();
        }

        private void TemplateList_Load(object sender, EventArgs e)
        {
            List<string> files = new List<string>()
            {
                "Blank simulation",
                "Collision example",
                "Detection example",
                "Simple cannon"
            };

            listBox.DataSource = files;
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