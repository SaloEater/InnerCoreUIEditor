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
    public partial class InnerControlAdder : UserControl
    {
        private static Type element { get; set; }

        public InnerControlAdder()
        {
            InitializeComponent();
        }

        /*public static void Init(Type innerControl, List<Control> hosts)
        {
            element = innerControl;
            foreach(Control c in hosts)
            {
                c.Click += (sender);
            }
        }

        private static void C_Click(object sender, EventArgs e)
        {
            
        }*/
    }
}
