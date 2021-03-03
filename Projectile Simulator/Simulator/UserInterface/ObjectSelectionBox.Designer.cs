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
            this.addTriggersButton = new System.Windows.Forms.Button();
            this.checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // addTriggersButton
            // 
            this.addTriggersButton.Location = new System.Drawing.Point(60, 130);
            this.addTriggersButton.Name = "addTriggersButton";
            this.addTriggersButton.Size = new System.Drawing.Size(100, 23);
            this.addTriggersButton.TabIndex = 2;
            this.addTriggersButton.Text = "Select objects";
            this.addTriggersButton.UseVisualStyleBackColor = true;
            this.addTriggersButton.Click += new System.EventHandler(this.addTriggersButton_Click);
            // 
            // checkedListBox
            // 
            this.checkedListBox.FormattingEnabled = true;
            this.checkedListBox.Location = new System.Drawing.Point(12, 12);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(210, 112);
            this.checkedListBox.TabIndex = 3;
            // 
            // ObjectSelectionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 161);
            this.Controls.Add(this.checkedListBox);
            this.Controls.Add(this.addTriggersButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObjectSelectionBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ObjectSelectionBox";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addTriggersButton;
        private System.Windows.Forms.CheckedListBox checkedListBox;
    }
}