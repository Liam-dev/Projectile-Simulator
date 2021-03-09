using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    public partial class ObjectSelectionBox : Form
    {
        public List<object> CheckedObjects { get; set; }

        public ObjectSelectionBox(List<object> objects, List<object> checkedObjects, string title, string buttonPrompt)
        {
            InitializeComponent();

            CheckedObjects = checkedObjects;

            checkedListBox.FormattingEnabled = true;
            checkedListBox.DisplayMember = "Name";

            // Adds already check objects
            checkedListBox.Items.AddRange(CheckedObjects.ToArray());

            // Checks already check objects
            for (int i = 0; i < CheckedObjects.Count; i++)
            {
                checkedListBox.SetItemChecked(i, true);
            }

            // Adds other objects
            checkedListBox.Items.AddRange(objects.Except(CheckedObjects).ToArray());

            Text = title;
            addTriggersButton.Text = buttonPrompt;
        }

        private void addTriggersButton_Click(object sender, EventArgs e)
        {
            object[] objects = new object[checkedListBox.CheckedItems.Count];
            checkedListBox.CheckedItems.CopyTo(objects, 0);

            CheckedObjects.Clear();
            CheckedObjects.AddRange(objects);
            DialogResult = DialogResult.OK;
        }
    }
}
