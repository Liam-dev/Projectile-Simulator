﻿using System;
using System.Windows.Forms;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Form that is displayed when application begins with no file.
    /// </summary>
    public partial class Homepage : Form
    {
        /// <summary>
        /// Constructor for Homepage.
        /// </summary>
        public Homepage()
        {
            InitializeComponent();
        }

        private void Homepage_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }
    }
}