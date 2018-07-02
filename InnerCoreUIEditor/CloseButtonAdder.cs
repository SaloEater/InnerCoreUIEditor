using System;
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
    public partial class CloseButtonAdder : UserControl
    {
        public CloseButtonAdder()
        {
            InitializeComponent();
            foreach(Control c in Controls)
            {
                c.Click += C_Click;
            }
        }

        private void C_Click(object sender, EventArgs e)
        {
            Control innerControl = (Control)sender;
            CloseButton element = new CloseButton();
            element.Location = new Point(0, Global.Y-element.Width);
            Global.panelWorkspace.Controls.Add(element);
            Global.panelWorkspace.Refresh();
        }
    }
}
