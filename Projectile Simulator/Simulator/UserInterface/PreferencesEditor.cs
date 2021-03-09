using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    /// <summary>
    /// A Form used to edit Editor preferences and simulation settings.
    /// </summary>
    public partial class PreferencesEditor : Form
    {
        /// <summary>
        /// Gets or sets the Editor preferences.
        /// </summary>
        public EditorPreferences Preferences
        {
            get
            {
                return new EditorPreferences()
                {
                    AutoName = autoNameCheckBox.Checked,
                    ShowTrajectories = trajectoryCheckBox.Checked
                };
            }
            set
            {
                autoNameCheckBox.Checked = value.AutoName;
                trajectoryCheckBox.Checked = value.ShowTrajectories;
            }
        }

        /// <summary>
        /// Gets the SimulationState settings.
        /// </summary>
        public SimulationState SimulationSettings { get { return (SimulationState)propertyGrid.SelectedObject; } }

        /// <summary>
        /// Constructor for the PreferencesEditor.
        /// </summary>
        /// <param name="preferences">The preferences for the Editor.</param>
        /// <param name="showSimulationProperties">Should the form show properties about the simulation.</param>
        /// <param name="simulationState">The state of the simulation.</param>
        public PreferencesEditor(EditorPreferences preferences, bool showSimulationProperties, SimulationState simulationState = default)
        {
            InitializeComponent();

            Preferences = preferences;

            propertiesPanel.Visible = showSimulationProperties;

            if (showSimulationProperties)
            {
                propertyGrid.SelectedObject = simulationState;
            }
        }

        private void PreferencesBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save preferences to file
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/" + Editor.PreferencesPath;
            FileSaver.WriteJson(path, Preferences);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
