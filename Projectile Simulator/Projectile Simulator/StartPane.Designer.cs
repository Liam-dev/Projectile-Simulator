namespace Projectile_Simulator
{
    partial class StartPane
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartPane));
            this.title = new System.Windows.Forms.Label();
            this.newButton = new Projectile_Simulator.LargeMenuButton();
            this.loadButton = new Projectile_Simulator.LargeMenuButton();
            this.preferencesButton = new Projectile_Simulator.LargeMenuButton();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.title.Location = new System.Drawing.Point(9, 10);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(129, 32);
            this.title.TabIndex = 0;
            this.title.Text = "Get started";
            // 
            // newButton
            // 
            this.newButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.newButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newButton.Heading = "Create a new simulation";
            this.newButton.Icon = ((System.Drawing.Image)(resources.GetObject("newButton.Icon")));
            this.newButton.Location = new System.Drawing.Point(19, 67);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(287, 76);
            this.newButton.Subheading = "Open a blank simulation or choose from a template to get started";
            this.newButton.TabIndex = 1;
            this.newButton.Pressed += new System.EventHandler(this.newButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.loadButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.loadButton.Heading = "Load existing simulation";
            this.loadButton.Icon = ((System.Drawing.Image)(resources.GetObject("loadButton.Icon")));
            this.loadButton.Location = new System.Drawing.Point(19, 172);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(287, 76);
            this.loadButton.Subheading = "Open a local simulation file from your computer";
            this.loadButton.TabIndex = 2;
            this.loadButton.Pressed += new System.EventHandler(this.loadButton_Click);
            // 
            // preferencesButton
            // 
            this.preferencesButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.preferencesButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.preferencesButton.Heading = "Edit preferences";
            this.preferencesButton.Icon = ((System.Drawing.Image)(resources.GetObject("preferencesButton.Icon")));
            this.preferencesButton.Location = new System.Drawing.Point(19, 275);
            this.preferencesButton.Name = "preferencesButton";
            this.preferencesButton.Size = new System.Drawing.Size(287, 76);
            this.preferencesButton.Subheading = "Change options without opening a simulation";
            this.preferencesButton.TabIndex = 3;
            this.preferencesButton.Pressed += new System.EventHandler(this.preferencesButton_Click);
            // 
            // StartPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.preferencesButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.title);
            this.Name = "StartPane";
            this.Size = new System.Drawing.Size(330, 380);
            this.Load += new System.EventHandler(this.StartPane_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title;
        private LargeMenuButton newButton;
        private LargeMenuButton loadButton;
        private LargeMenuButton preferencesButton;
    }
}
