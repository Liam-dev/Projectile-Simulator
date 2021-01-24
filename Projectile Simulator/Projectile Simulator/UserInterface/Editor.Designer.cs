﻿namespace Projectile_Simulator
{
    partial class Editor
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
            this.simulation = new Projectile_Simulator.UserInterface.Simulation();
            this.toolbar = new Projectile_Simulator.UserInterface.Toolbar();
            this.inspector = new Projectile_Simulator.UserInterface.Inspector();
            this.SuspendLayout();
            // 
            // simulation
            // 
            this.simulation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.simulation.Location = new System.Drawing.Point(5, 52);
            this.simulation.MouseHoverUpdatesOnly = true;
            this.simulation.Name = "simulation";
            this.simulation.Size = new System.Drawing.Size(960, 540);
            this.simulation.TabIndex = 0;
            this.simulation.Text = "Simulation";
            // 
            // toolbar
            // 
            this.toolbar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Margin = new System.Windows.Forms.Padding(0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(1228, 50);
            this.toolbar.TabIndex = 4;
            this.toolbar.BallButtonClicked += new System.EventHandler(this.toolbar_BallButtonClicked);
            this.toolbar.ZoomInButtonClicked += new System.EventHandler(this.toolbar_ZoomInButtonClicked);
            this.toolbar.ZoomOutButtonClicked += new System.EventHandler(this.toolbar_ZoomOutButtonClicked);
            // 
            // inspector
            // 
            this.inspector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inspector.Location = new System.Drawing.Point(971, 51);
            this.inspector.Name = "inspector";
            this.inspector.Size = new System.Drawing.Size(250, 542);
            this.inspector.TabIndex = 5;
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 597);
            this.Controls.Add(this.inspector);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.simulation);
            this.MinimumSize = new System.Drawing.Size(640, 360);
            this.Name = "Editor";
            this.Text = "Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Editor_FormClosing);
            this.Load += new System.EventHandler(this.Editor_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Projectile_Simulator.UserInterface.Simulation simulation;
        private Projectile_Simulator.UserInterface.Toolbar toolbar;
        private UserInterface.Inspector inspector;
    }
}