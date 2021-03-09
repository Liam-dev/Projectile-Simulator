using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    /// <summary>
    /// A Form used to select and check objects from a list.
    /// </summary>
    public partial class ObjectSelectionBox : Form
    {
        /// <summary>
        /// Gets or sets the objects which are checked.
        /// </summary>
        public List<object> CheckedObjects { get; set; }

        /// <summary>
        /// Constructor for ObjectSelectionBox.
        /// </summary>
        /// <param name="objects">List of all available objects to select from.</param>
        /// <param name="checkedObjects">List of objects which are already checked.</param>
        /// <param name="title">Title of the form.</param>
        /// <param name="buttonPrompt">Prompt to display on the button.</param>
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