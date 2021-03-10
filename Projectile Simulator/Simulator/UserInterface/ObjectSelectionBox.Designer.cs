namespace Simulator.UserInterface
{
    partial class ObjectSelectionBox
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
            this.updateObjectsButton = new System.Windows.Forms.Button();
            this.checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // updateObjectsButton
            // 
            this.updateObjectsButton.Location = new System.Drawing.Point(89, 176);
            this.updateObjectsButton.Name = "updateObjectsButton";
            this.updateObjectsButton.Size = new System.Drawing.Size(100, 23);
            this.updateObjectsButton.TabIndex = 2;
            this.updateObjectsButton.Text = "Select objects";
            this.updateObjectsButton.UseVisualStyleBackColor = true;
            this.updateObjectsButton.Click += new System.EventHandler(this.updateObjectsButton_Click);
            // 
            // checkedListBox
            // 
            this.checkedListBox.CheckOnClick = true;
            this.checkedListBox.FormattingEnabled = true;
            this.checkedListBox.Location = new System.Drawing.Point(12, 30);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(260, 130);
            this.checkedListBox.TabIndex = 3;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(51, 15);
            this.label.TabIndex = 4;
            this.label.Text = "Triggers:";
            // 
            // ObjectSelectionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 211);
            this.Controls.Add(this.label);
            this.Controls.Add(this.checkedListBox);
            this.Controls.Add(this.updateObjectsButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ObjectSelectionBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ObjectSelectionBox";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button updateObjectsButton;
        private System.Windows.Forms.CheckedListBox checkedListBox;
        private System.Windows.Forms.Label label;
    }
}