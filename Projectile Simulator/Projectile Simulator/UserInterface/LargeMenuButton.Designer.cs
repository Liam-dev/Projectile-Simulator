namespace Projectile_Simulator.UserInterface
{
    partial class LargeMenuButton
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
            this.heading = new System.Windows.Forms.Label();
            this.subheading = new System.Windows.Forms.Label();
            this.icon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // heading
            // 
            this.heading.AutoSize = true;
            this.heading.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.heading.Location = new System.Drawing.Point(52, 2);
            this.heading.MinimumSize = new System.Drawing.Size(220, 0);
            this.heading.Name = "heading";
            this.heading.Size = new System.Drawing.Size(220, 30);
            this.heading.TabIndex = 0;
            this.heading.Text = "Heading";
            this.heading.Click += new System.EventHandler(this.OnClick);
            // 
            // subheading
            // 
            this.subheading.AutoSize = true;
            this.subheading.Font = new System.Drawing.Font("Yu Gothic UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.subheading.Location = new System.Drawing.Point(56, 32);
            this.subheading.MaximumSize = new System.Drawing.Size(220, 35);
            this.subheading.MinimumSize = new System.Drawing.Size(220, 35);
            this.subheading.Name = "subheading";
            this.subheading.Size = new System.Drawing.Size(220, 35);
            this.subheading.TabIndex = 1;
            this.subheading.Text = "Subheading";
            this.subheading.Click += new System.EventHandler(this.OnClick);
            // 
            // icon
            // 
            this.icon.Location = new System.Drawing.Point(10, 15);
            this.icon.Margin = new System.Windows.Forms.Padding(0);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(32, 32);
            this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icon.TabIndex = 2;
            this.icon.TabStop = false;
            this.icon.Click += new System.EventHandler(this.OnClick);
            // 
            // LargeMenuButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.icon);
            this.Controls.Add(this.subheading);
            this.Controls.Add(this.heading);
            this.Name = "LargeMenuButton";
            this.Size = new System.Drawing.Size(300, 76);
            this.Click += new System.EventHandler(this.OnClick);
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label heading;
        private System.Windows.Forms.Label subheading;
        private System.Windows.Forms.PictureBox icon;
    }
}
