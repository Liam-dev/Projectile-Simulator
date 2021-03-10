using System;
using System.Drawing;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Control that is used in Homepage form as large buttons.
    /// </summary>
    public partial class LargeMenuButton : UserControl
    {
        /// <summary>
        /// Occurs when the button is pressed
        /// </summary>
        public event EventHandler Pressed;

        /// <summary>
        /// Gets or sets the heading string that is displayed on the button.
        /// </summary>
        public string Heading
        {
            get { return heading.Text; }
            set { heading.Text = value; }
        }

        /// <summary>
        /// Gets or sets the subheading string that is displayed on the button.
        /// </summary>
        public string Subheading
        {
            get { return subheading.Text; }
            set { subheading.Text = value; }
        }

        /// <summary>
        /// Gets or sets the icon that is displayed on the button.
        /// </summary>
        public Image Icon
        {
            get { return icon.Image; }
            set { icon.Image = value; }
        }

        /// <summary>
        /// Constructor for LargeMenuButton.
        /// </summary>
        public LargeMenuButton()
        {
            InitializeComponent();
        }

        // When button is clicked anywhere on the control
        private void OnClick(object sender, EventArgs e)
        {
            Pressed?.Invoke(this, e);
        }
    }
}