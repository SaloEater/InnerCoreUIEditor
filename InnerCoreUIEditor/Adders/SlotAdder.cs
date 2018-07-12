﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    public partial class SlotAdder : UserControl
    {
        public SlotAdder()
        {
            InitializeComponent();
            foreach (Control c in Controls)
            {
                c.Click += C_Click;
            }
        }

        private void C_Click(object sender, EventArgs e)
        {
            Control innerControl = (Control)sender;
            Slot element = new Slot();
            element.Location = new Point(Global.X / 2 + Global.panelWorkspace.AutoScrollPosition.X, Global.Y / 2 + Global.panelWorkspace.AutoScrollPosition.Y);
            Global.panelWorkspace.Controls.Add(element);
            Global.panelWorkspace.Refresh();
        }
    }
}