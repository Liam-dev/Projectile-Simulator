namespace Simulator.UserInterface
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
            Simulator.Simulation.Camera camera1 = new Simulator.Simulation.Camera();
            this.simulation = new Simulator.UserInterface.Simulation();
            this.toolbar = new Simulator.UserInterface.Toolbar();
            this.inspector = new Simulator.UserInterface.Inspector();
            this.simulationContextMenu = new Simulator.UserInterface.SimulationContextMenu();
            this.SuspendLayout();
            // 
            // simulation
            // 
            this.simulation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            camera1.MaxZoomLevel = 8;
            camera1.MinZoomLevel = -20;
            camera1.OldZoom = 1F;
            camera1.Zoom = 1F;
            camera1.ZoomMultiplier = 1.1F;
            this.simulation.Camera = camera1;
            this.simulation.Location = new System.Drawing.Point(5, 52);
            this.simulation.MouseHoverUpdatesOnly = true;
            this.simulation.Name = "simulation";
            this.simulation.Paused = false;
            this.simulation.Scale = 100F;
            this.simulation.Size = new System.Drawing.Size(880, 540);
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
            this.toolbar.SimulationPaused = true;
            this.toolbar.Size = new System.Drawing.Size(1228, 50);
            this.toolbar.TabIndex = 4;
            this.toolbar.ButtonClicked += new System.EventHandler(this.toolbar_ButtonClicked);
            // 
            // inspector
            // 
            this.inspector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inspector.Location = new System.Drawing.Point(891, 51);
            this.inspector.Name = "inspector";
            this.inspector.SelectedObject = null;
            this.inspector.Size = new System.Drawing.Size(330, 542);
            this.inspector.TabIndex = 5;
            // 
            // simulationContextMenu
            // 
            this.simulationContextMenu.Location = new System.Drawing.Point(0, 0);
            this.simulationContextMenu.Margin = new System.Windows.Forms.Padding(0);
            this.simulationContextMenu.Name = "simulationContextMenu";
            this.simulationContextMenu.Size = new System.Drawing.Size(0, 0);
            this.simulationContextMenu.TabIndex = 6;
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 597);
            this.Controls.Add(this.simulationContextMenu);
            this.Controls.Add(this.inspector);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.simulation);
            this.MinimumSize = new System.Drawing.Size(640, 360);
            this.Name = "Editor";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Editor_FormClosing);
            this.Load += new System.EventHandler(this.Editor_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Simulator.UserInterface.Simulation simulation;
        private Simulator.UserInterface.Toolbar toolbar;
        private UserInterface.Inspector inspector;
        private SimulationContextMenu simulationContextMenu;
    }
}