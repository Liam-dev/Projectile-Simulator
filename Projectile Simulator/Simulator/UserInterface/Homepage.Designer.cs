namespace Simulator.UserInterface
{
    partial class Homepage
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
            this.title = new System.Windows.Forms.Label();
            this.startPane1 = new Simulator.UserInterface.StartPane();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.title.Location = new System.Drawing.Point(12, 19);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(327, 50);
            this.title.TabIndex = 0;
            this.title.Text = "Projectile Simulator";
            // 
            // startPane1
            // 
            this.startPane1.BackColor = System.Drawing.Color.Transparent;
            this.startPane1.Location = new System.Drawing.Point(12, 113);
            this.startPane1.Name = "startPane1";
            this.startPane1.Size = new System.Drawing.Size(330, 380);
            this.startPane1.TabIndex = 1;
            // 
            // Homepage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(824, 511);
            this.Controls.Add(this.startPane1);
            this.Controls.Add(this.title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Homepage";
            this.Text = "Projectile Simulator";
            this.Load += new System.EventHandler(this.Homepage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title;
        private StartPane startPane1;
    }
}