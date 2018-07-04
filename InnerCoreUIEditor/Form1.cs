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

        private bool inventoryDrawed;

        public Form1()
        {
            InitializeComponent();
            inventoryDrawed = false;
            panelWorkspace.Click += PanelWorkspace_Click;
            panelWorkspace.ControlAdded += PanelWorkspace_ControlAdded;
            panelWorkspace.Size = new Size(Global.X, Global.Y);
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
                innerControl.DeselectControl();
                panelWorkspace.Controls.Remove(innerControl);
                innerControl.Dispose();
                Global.ReloadExporer();
                Global.activeElement = null;
            }
        }

        private void PanelWorkspace_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control == null || e.Control.GetType() == typeof(Label)) return;
            Global.ReloadExporer();
            ((InnerControl)e.Control).SelectControl();
        }

        private void PanelWorkspace_Click(object sender, EventArgs e)
        {
            InnerControl innerControl = Global.activeElement;
            if (innerControl != null)
            {
                innerControl.DeselectControl();
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
            for(int i = 0; i < panelWorkspace.Controls.Count; i++)
            {
                Control _c = panelWorkspace.Controls[i];
                if (_c.GetType() == typeof(Label)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.constant) continue;
                c.Remove();
                i--;
            }
            panelWorkspace.Refresh();
            Global.ReloadExporer();
            JSONParser.Parse(gui);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Global.activeElement != null) Global.activeElement.Select();
        }

        private void инвентарьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
