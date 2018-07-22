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


        //Global hotkeys
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        private IntPtr thisWindow;
        private Hotkey hotkey;

        //Global hotkeys

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

            tabControl1.Click += TabControl1_Click;
            KeyDown += Form1_KeyDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            thisWindow = FindWindow(null, "Form1");
            hotkey = new Hotkey(thisWindow);
            hotkey.RegisterControlHotKey(1, Keys.O);
            hotkey.RegisterControlHotKey(2, Keys.N);
            hotkey.RegisterControlHotKey(3, Keys.S);
            hotkey.RegisterControlHotKey(4, Keys.W);
            hotkey.RegisterHotKey(5, Keys.Delete);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            hotkey.UnRegisterHotKeys();
        }

        protected override void WndProc(ref Message keyPressed)
        {
            if (keyPressed.Msg == 0x0312)
            {
                IntPtr hotkeyID = keyPressed.WParam;
                int hk = (int)hotkeyID;
                //if(tabControl1.SelectedIndex>=0) InnerTabPage innerTabPage = (InnerTabPage)tabControl1.SelectedTab.Controls[0];
                switch(hk)
                {
                    case 1:
                        toolStripMenuItem2_Click(null, null);
                        break;

                    case 2:
                        новоеГпиToolStripMenuItem_Click(null, null);
                        break;

                    case 3:
                        toolStripMenuItem1_Click(null, null);
                        break;

                    case 4:
                        удалитьToolStripMenuItem_Click(null, null);
                        break;

                    case 5:
                        DeleteActiveElement();
                        break;
                }
            }
            base.WndProc(ref keyPressed);
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
            /*InnerControl innerControl = Global.activeElement;
            if (e.KeyData == Keys.Delete && innerControl != null)
            {
                innerControl.DeselectControl();
                Global.workspace.Controls.Remove(innerControl);
                innerControl.Dispose();
                Global.ReloadExporer();
                Global.activeElement = null;
            }*/
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
                tabControl1.SelectedTab.Dispose();
            }
        }
    }
}
