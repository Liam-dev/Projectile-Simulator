﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;

namespace Projectile_Simulator
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            new UITypeEditor();
        }

        public Editor(string filename)
        {

        }
    }
}
