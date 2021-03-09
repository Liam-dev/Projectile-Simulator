namespace Simulator.UserInterface
{
    partial class PreferencesEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.preferncesLabel = new System.Windows.Forms.Label();
            this.preferencesPanel = new System.Windows.Forms.Panel();
            this.autoNameCheckBox = new System.Windows.Forms.CheckBox();
            this.trajectoryCheckBox = new System.Windows.Forms.CheckBox();
            this.propertiesPanel = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.button = new System.Windows.Forms.Button();
            this.preferencesPanel.SuspendLayout();
            this.propertiesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // preferncesLabel
            // 
            this.preferncesLabel.AutoSize = true;
            this.preferncesLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.preferncesLabel.Location = new System.Drawing.Point(10, 10);
            this.preferncesLabel.Name = "preferncesLabel";
            this.preferncesLabel.Size = new System.Drawing.Size(146, 21);
            this.preferncesLabel.TabIndex = 0;
            this.preferncesLabel.Text = "Editor preferences";
            // 
            // preferencesPanel
            // 
            this.preferencesPanel.Controls.Add(this.autoNameCheckBox);
            this.preferencesPanel.Controls.Add(this.trajectoryCheckBox);
            this.preferencesPanel.Controls.Add(this.preferncesLabel);
            this.preferencesPanel.Location = new System.Drawing.Point(12, 12);
            this.preferencesPanel.Name = "preferencesPanel";
            this.preferencesPanel.Size = new System.Drawing.Size(339, 268);
            this.preferencesPanel.TabIndex = 1;
            // 
            // autoNameCheckBox
            // 
            this.autoNameCheckBox.AutoSize = true;
            this.autoNameCheckBox.Location = new System.Drawing.Point(19, 92);
            this.autoNameCheckBox.Margin = new System.Windows.Forms.Padding(10);
            this.autoNameCheckBox.Name = "autoNameCheckBox";
            this.autoNameCheckBox.Size = new System.Drawing.Size(258, 19);
            this.autoNameCheckBox.TabIndex = 1;
            this.autoNameCheckBox.Text = "Automatically name new simulation objects";
            this.autoNameCheckBox.UseVisualStyleBackColor = true;
            // 
            // trajectoryCheckBox
            // 
            this.trajectoryCheckBox.AutoSize = true;
            this.trajectoryCheckBox.Location = new System.Drawing.Point(19, 53);
            this.trajectoryCheckBox.Margin = new System.Windows.Forms.Padding(10);
            this.trajectoryCheckBox.Name = "trajectoryCheckBox";
            this.trajectoryCheckBox.Size = new System.Drawing.Size(168, 19);
            this.trajectoryCheckBox.TabIndex = 1;
            this.trajectoryCheckBox.Text = "Show projectile trajectories";
            this.trajectoryCheckBox.UseVisualStyleBackColor = true;
            // 
            // propertiesPanel
            // 
            this.propertiesPanel.Controls.Add(this.propertyGrid);
            this.propertiesPanel.Controls.Add(this.propertiesLabel);
            this.propertiesPanel.Location = new System.Drawing.Point(357, 12);
            this.propertiesPanel.Name = "propertiesPanel";
            this.propertiesPanel.Size = new System.Drawing.Size(355, 337);
            this.propertiesPanel.TabIndex = 2;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(11, 46);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(331, 280);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.AutoSize = true;
            this.propertiesLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.propertiesLabel.Location = new System.Drawing.Point(11, 10);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(168, 21);
            this.propertiesLabel.TabIndex = 0;
            this.propertiesLabel.Text = "Simulation properties";
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(121, 305);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(111, 23);
            this.button.TabIndex = 3;
            this.button.Text = "Save changes";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // PreferencesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 361);
            this.Controls.Add(this.button);
            this.Controls.Add(this.propertiesPanel);
            this.Controls.Add(this.preferencesPanel);
            this.Name = "PreferencesEditor";
            this.Text = "Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreferencesBox_FormClosing);
            this.preferencesPanel.ResumeLayout(false);
            this.preferencesPanel.PerformLayout();
            this.propertiesPanel.ResumeLayout(false);
            this.propertiesPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label preferncesLabel;
        private System.Windows.Forms.Panel preferencesPanel;
        private System.Windows.Forms.CheckBox autoNameCheckBox;
        private System.Windows.Forms.CheckBox trajectoryCheckBox;
        private System.Windows.Forms.Panel propertiesPanel;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button button;
    }
}