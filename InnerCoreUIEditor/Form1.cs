using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panelWorkspace.Click += PanelWorkspace_Click;
            panelWorkspace.ControlAdded += PanelWorkspace_ControlAdded;
            Global.counter = 0;
            Global.panelProperties = panelProperties;
            Global.panelWorkspace = panelWorkspace;
            Global.panelExplorer = panelExplorer;
        }

        private void PanelWorkspace_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control == null) return;
            Global.ReloadExporer();
        }

        private void PanelWorkspace_Click(object sender, EventArgs e)
        {
            InnerControl innerControl = Global.activeElement;
            if (innerControl != null)
            {
                innerControl.ClearPropPanel(panelProperties);
                Global.activeElement = null;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            /*openFileDialog1.ShowDialog();
            if (openFileDialog1.SafeFileName == "") return;*/
            string gui = File.ReadAllText("testParse.txt");
            JSONParser.Parse(gui);
        }
    }
}
