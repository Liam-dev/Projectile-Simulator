using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Projectile_Simulator
{
    public partial class LargeMenuButton : UserControl
    {       
        public event EventHandler Pressed; 

        public string Heading
        {
            get { return heading.Text; }
            set { heading.Text = value; }
        }
        public string Subheading
        {
            get { return subheading.Text; }
            set { subheading.Text = value; }
        }

        public Image Icon
        {
            get { return icon.Image; }
            set { icon.Image = value; }
        }

        public LargeMenuButton()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, EventArgs e)
        {
            OnPressed(EventArgs.Empty);
        }

        protected virtual void OnPressed(EventArgs e)
        {
            Pressed?.Invoke(this, e);
        }
    }
}
