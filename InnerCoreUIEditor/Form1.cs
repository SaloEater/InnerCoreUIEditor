using InnerCoreUIEditor.Controls;
using InnerCoreUIEditor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

            /*workspace.Click += workspace_Click;
            workspace.ControlAdded += workspace_ControlAdded;
            workspace.BackColor = Color.FromArgb(114, 106, 112);
            label1.Location = new Point(Global.X, Global.Y);
            //workspace.Size = new Size(Global.X, Global.Y);
            Global.panelProperties = panelProperties;
            Global.workspace = workspace;
            Global.panelExplorer = panelExplorer;*/

            KeyPreview = true;
            tabControl1.Click += TabControl1_Click;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.ShiftKey)
            {
                if (tabControl1.SelectedIndex >= 0) ((InnerTabPage)tabControl1.SelectedTab.Controls[0]).aligment = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void DeleteActiveElement()
        {
            if (tabControl1.SelectedIndex < 0) return;
            InnerTabPage innerTabPage = (InnerTabPage)tabControl1.SelectedTab.Controls[0];
            if(innerTabPage.activeElement != null)
            {
                innerTabPage.activeElement.Remove();
            }
        }

        private void TabControl1_Click(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            MouseEventArgs mouseEventArgs = (MouseEventArgs)e;
            if (mouseEventArgs.Button == MouseButtons.Right)
            {// iterate through all the tab pages
                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    // get their rectangle area and check if it contains the mouse cursor
                    Rectangle r = tabControl.GetTabRect(i);
                    if (r.Contains(mouseEventArgs.Location))
                    {
                        tabControl.SelectedIndex = i;
                        contextMenuStrip1.Show(MousePosition);
                    }
                }
            }
            //Закрытие вкладок
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Control)
            {
                switch(e.KeyCode)
                {
                    case Keys.O:
                        toolStripMenuItem2_Click(null, null);
                        break;

                    case Keys.N:
                        новоеГпиToolStripMenuItem_Click(null, null);
                        break;

                    case Keys.S:
                        toolStripMenuItem1_Click(null, null);
                        break;

                    case Keys.W:
                        удалитьToolStripMenuItem_Click(null, null);
                        break;

                    case Keys.D:
                        CloneActiveElement();
                        break;
                    
                }
            }
            else if(e.KeyCode == Keys.Delete)
            {
                DeleteActiveElement();
            } else if(e.Shift)
            {
                if(tabControl1.SelectedIndex >= 0)((InnerTabPage)tabControl1.SelectedTab.Controls[0]).aligment = true;
            }
        }

        private void CloneActiveElement()
        {
            if (tabControl1.SelectedIndex < 0) return;
            InnerTabPage innerTabPage = (InnerTabPage)tabControl1.SelectedTab.Controls[0];
            if (innerTabPage.activeElement == null) return;
            try
            {
                InnerControl copy = innerTabPage.activeElement.MakeCopy();
                innerTabPage.AddElement(copy);
                copy.BringToFront();
            } catch (ArgumentOutOfRangeException e)
            {

            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex < 0) return;
            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (saveFileDialog1.FileName == "") return;
            string filename = saveFileDialog1.FileName;
            //Сохранить активную вкладку    
            ((InnerTabPage)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0]).Save(filename);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Javascript files (*.js)|*.js|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName == "") return;
            string gui = File.ReadAllText(openFileDialog1.FileName);
            новоеГпиToolStripMenuItem_Click(null, null);
            ((InnerTabPage)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0]).Parse(openFileDialog1.SafeFileName, gui);
        }

        private void OpenDefaultFileDialog()
        {
            openFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog1.FileName = "";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }
        }

        private TabPage MakeNewTabPage()
        {
            InnerTabPage innerTabPage = new InnerTabPage();
            innerTabPage.Dock = DockStyle.Fill;

            TabPage tabPage = new TabPage();
            tabPage.Controls.Add(innerTabPage);
            tabPage.Text = "Безымянный";

            return tabPage;
        }

        private void новоеГпиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Add(MakeNewTabPage());
            tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((InnerTabPage)tabControl1.SelectedTab.Controls[0]).ShowRemoveWarning(out bool cancelled);
            if (!cancelled)
            {
                int prevIndex = tabControl1.SelectedIndex;
                tabControl1.SelectedTab.Dispose();
                if (tabControl1.TabPages.Count == 0) return;
                tabControl1.SelectedIndex = prevIndex < tabControl1.TabPages.Count ? prevIndex : tabControl1.TabPages.Count - 1;
            }
        }
    }
}
