using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Control that contains buttons that allow Editor to be opened in different ways.
    /// </summary>
    public partial class StartPane : UserControl
    {
        /// <summary>
        /// Constructor for StartPane.
        /// </summary>
        public StartPane()
        {
            InitializeComponent();
        }

        private void StartPane_Load(object sender, EventArgs e)
        {
        }

        // When new button is clicked, load the selected template simulation into Editor
        private void newButton_Click(object sender, EventArgs e)
        {
            if (templateList.SelectedItem != null)
            {
                // Open new simulation from selected template in a new Editor
                string templateName = templateList.SelectedItem;
                string path = "Content/Templates/" + templateName + ".sim";
                new Thread(() => new Editor(path, true).ShowDialog()).Start();
            }
            else
            {
                // No template selected
                new Thread(() => new Editor().ShowDialog()).Start();
            }

            Application.ExitThread();
        }

        // When load button is clicked, open file dialogue to choose simulation to load into Editor
        private void loadButton_Click(object sender, EventArgs e)
        {
            // Open OpenFileDialog for simulation files
            OpenFileDialog fileDialogue = new OpenFileDialog
            {
                Title = "Open Simulation File",
                DefaultExt = "sim",
                Multiselect = false,
                CheckFileExists = true,
                Filter = "Simulation files (*.sim)|*.sim"
            };

            if (fileDialogue.ShowDialog(this) == DialogResult.OK)
            {
                new Thread(() => new Editor(fileDialogue.FileName, false).ShowDialog()).Start();
                Application.ExitThread();
            }
        }

        private void preferencesButton_Click(object sender, EventArgs e)
        {
            EditorPreferences preferences;

            // Read preferences
            if (File.Exists(Editor.PreferencesPath))
            {
                preferences = FileSaver.ReadJson<EditorPreferences>(Editor.PreferencesPath);
            }
            else
            {
                preferences = new EditorPreferences() { AutoName = false, ShowTrajectories = true };
            }

            // Open preferences editor
            new PreferencesEditor(preferences, false).ShowDialog(this);
        }
    }
}