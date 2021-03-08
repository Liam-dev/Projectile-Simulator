using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    public partial class PreferencesEditor : Form
    {
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

        public SimulationState SimulationSettings { get { return (SimulationState)propertyGrid.SelectedObject; } }

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
            // Update preferences
            //Preferences = new EditorPreferences() { AutoName = autoNameCheckBox.Checked, ShowTrajectories = trajectoryCheckBox.Checked };

            // Save preferences to file
            FileSaver.WriteJson(Editor.preferencesPath, Preferences);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
