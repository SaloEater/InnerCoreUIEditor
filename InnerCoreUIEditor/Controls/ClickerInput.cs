using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    public partial class ClickerInput : Form
    {
        public ClickerInput()
        {
            InitializeComponent();
            richTextBox1.Text = Global.activeElement.clicker;
            FormClosed += ClickerInput_FormClosed;
            Resize += ClickerInput_Resize;
        }

        private void ClickerInput_Resize(object sender, EventArgs e)
        {
            
        }

        private void ClickerInput_FormClosed(object sender, FormClosedEventArgs e)
        {
            Global.activeElement.clicker = richTextBox1.Text;
        }
    }
}
