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
            KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("");
            InnerControl innerControl = Global.activeElement;
            if (e.KeyData == Keys.Delete && innerControl != null)
            {
                panelWorkspace.Controls.Remove(innerControl);
                innerControl.Dispose();
                Global.ReloadExporer();
                Global.activeElement = null;
            }
        }

        private void PanelWorkspace_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control == null) return;
            Global.ReloadExporer();
            ((InnerControl)e.Control).FillPropPanel(panelProperties);
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
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName == "") return;
            string filename = saveFileDialog1.FileName;
            JSONParser.Save(filename);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.SafeFileName == "") return;
            string gui = File.ReadAllText(openFileDialog1.FileName);
            JSONParser.Parse(gui);
        }
    }
}
