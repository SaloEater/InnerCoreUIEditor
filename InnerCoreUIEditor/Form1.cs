using InnerCoreUIEditor.Controls;
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
            Params.Initialization();
            InitializeComponent();
            panelWorkspace.Click += PanelWorkspace_Click;
            panelWorkspace.ControlAdded += PanelWorkspace_ControlAdded;
            panelWorkspace.BackColor = Color.FromArgb(114, 106, 112);
            label1.Location = new Point(Global.X, Global.Y);
            //panelWorkspace.Size = new Size(Global.X, Global.Y);
            Global.counter = 0;
            Global.panelProperties = panelProperties;
            Global.panelWorkspace = panelWorkspace;
            Global.panelExplorer = panelExplorer;
            Global.innerHeader = innerHeader1;
            KeyDown += Form1_KeyDown;
        }

        private void SetupParams()
        {
            Params.AllToDefault();
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
            if (e.Control == null || e.Control.GetType() == typeof(Label) || e.Control.GetType() == typeof(InnerHeader)) return;
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
            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (saveFileDialog1.FileName == "") return;
            string filename = saveFileDialog1.FileName;
            JSONParser.Save(filename);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Javascript files (*.js)|*.js|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName == "") return;
            string gui = File.ReadAllText(openFileDialog1.FileName);
            ClearWorkscreen();
            panelWorkspace.Refresh();
            Global.ReloadExporer();
            Params.AllToDefault();
            JSONParser.Parse(gui);
            TryToGuessDrawOrder();
        }

        private void ClearWorkscreen()
        {
            for (int i = 0; i < panelWorkspace.Controls.Count; i++)
            {
                Control _c = panelWorkspace.Controls[i];
                if (_c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.constant) continue;
                c.Remove();
                i--;
            }
        }

        private void TryToGuessDrawOrder()
        {
            foreach (Control c in panelWorkspace.Controls)
            {
                if (c.GetType() != typeof(InnerBitmap) ) continue;
                c.BringToFront();
            }

            foreach (Control c in panelWorkspace.Controls)
            {
                if (c.GetType() != typeof(InnerImage) && c.GetType() != typeof(Scale)) continue;
                c.BringToFront();
            }

            foreach (Control c in panelWorkspace.Controls)
            {
                if (c.GetType() != typeof(Button) && c.GetType() != typeof(CloseButton)) continue;
                c.BringToFront();
            }

            foreach (Control c in panelWorkspace.Controls)
            {
                if (c.GetType() != typeof(Slot) && c.GetType() != typeof(InvSlot)) continue;
                c.BringToFront();
            }

            foreach (Control c in panelWorkspace.Controls)
            {
                if (c.GetType() != typeof(InnerText)) continue;
                c.BringToFront();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Global.activeElement != null) Global.activeElement.Select();
        }

        private void инвентарьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Global.SwitchInventorySlots();
        }

        private void заголовокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.SwitchHeader();
        }

        private void цветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelWorkspace.BackgroundImage = null;
            colorDialog1.ShowDialog();
            panelWorkspace.BackColor = colorDialog1.Color;
            Global.ColorAllToPanelColor();
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

        private void изображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDefaultFileDialog();
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            Global.BackgroundImageName = openFileDialog1.SafeFileName;
            panelWorkspace.BackgroundImage = image;
            Global.AllMergeToBackground();
        }

        private void слотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDefaultFileDialog();
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            Params.SetSlotImage(image, openFileDialog1.SafeFileName);
        }

        private void слотИнвентаряToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDefaultFileDialog();
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            Params.SetInvSlotImage(image, openFileDialog1.SafeFileName);
        }

        private void рамкаСлотаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDefaultFileDialog();
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            Params.SetSelectionImage(image, openFileDialog1.SafeFileName);
        }

        private void кнопкаЗакрытияВыклToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDefaultFileDialog();
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            Params.SetCloseButtonImage(image, openFileDialog1.SafeFileName);
        }

        private void кнопкаЗакрытияВклToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDefaultFileDialog();
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            Params.SetCloseButton2Image(image, openFileDialog1.SafeFileName);
        }

        private void очиститьЭкранToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearWorkscreen();
            panelWorkspace.BackgroundImage = null;
            panelWorkspace.BackColor = Color.FromArgb(114, 106, 112);
            innerHeader1.SetText("");
            innerHeader1.Visible = false;
            Global.ChangeHeight(Global.defaultHeight);
        }

        private void поУмолчаниюСлотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Params.SlotToDefault();
        }

        private void поУмолчаниюСлотИнвентаряToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Params.InvSlotToDefault();
        }

        private void поУмолчаниюВыделениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Params.SelectionToDefault();
        }

        private void поУмолчаниюЗакрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Params.CloseButtonToDefault();
        }

        private void поУмолчаниюЗакрыть2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Params.CloseButton2ToDefault();
        }

        private void cброситьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Params.AllToDefault();
        }
    }
}
