using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InnerCoreUIEditor.Controls;
using System.Globalization;

namespace InnerCoreUIEditor
{
    public partial class InnerTabPage : UserControl
    {
        Params _params;
        ExplorerPainter explorerPainter;
        public InnerHeader innerHeader;

        Label label;

        public int defaultHeight = 700,
                defaultWidth = 1000;

        JSONParser jSONParser;

        bool changed = false;
        public float globalScale = 1;

        public InnerTabPage()
        {
            InitializeComponent();
            panelDesktop.BackColor = Color.FromArgb(114, 106, 112);
            _params = new Params(this);
            explorerPainter = new ExplorerPainter(tabPageExplorer);            

            jSONParser = new JSONParser(_params, explorerPainter, this);

            label = new Label();
            label.Text = "";
            label.Location = new Point(defaultWidth, defaultHeight);
            label.Size = new Size(0, 0);
            panelDesktop.Controls.Add(label);
            //Console.WriteLine("{0}, {1}", panelDesktop.Width, panelDesktop.Height);

            innerHeader = new InnerHeader(explorerPainter, _params, this);
            innerHeader.Location = new Point(0, 0);
            innerHeader.Visible = false;
            panelDesktop.Controls.Add(innerHeader);

            panelDesktop.ControlAdded += PanelDesktop_ControlAdded;
            panelDesktop.Click += PanelDesktop_Click;

            toolStripTextBoxHeight.Text = MaxY().ToString();
            toolStripTextBoxHeight.LostFocus += ToolStripTextBoxHeight_LostFocus;
            toolStripTextBoxHeight.KeyDown += ToolStripTextBoxHeight_KeyDown;

            toolStripTextBoxScale.Text = "1";
            toolStripTextBoxScale.LostFocus += ToolStripTextBoxScale_LostFocus;
            toolStripTextBoxScale.KeyDown += ToolStripTextBoxScale_KeyDown;
        }

        private void ToolStripTextBoxScale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ToolStripTextBoxScale_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void ToolStripTextBoxScale_LostFocus(object sender, EventArgs e)
        {
            ToolStripTextBox textBox = (ToolStripTextBox)sender;
            float scale;
            if (!float.TryParse(textBox.Text, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out scale))
            {
                textBox.Text = globalScale.ToString();
                return;
            }
            ChangeGlobalScale(scale);
        }

        private void ToolStripTextBoxHeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ToolStripTextBoxHeight_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void ToolStripTextBoxHeight_LostFocus(object sender, EventArgs e)
        {
            ToolStripTextBox textBox = (ToolStripTextBox)sender;
            int y;
            if (!int.TryParse(textBox.Text, out y))
            {
                textBox.Text = MaxY().ToString();
                return;
            }
            ChangeHeight(y);
        }

        private void ClearWorkscreen()
        {
            for (int i = 0; i < panelDesktop.Controls.Count; i++)
            {
                Control _c = panelDesktop.Controls[i];
                if (_c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.constant) continue;
                c.Remove();
                i--;
            }
        }

        internal Panel GetPropertiesPanel()
        {
            return panelProperties;
        }

        public int MaxX()
        {
            return label.Left;
        }

        public int MaxY()
        {
            return label.Top;
        }

        private void TryToGuessDrawOrder()
        {
            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() != typeof(InnerBitmap)) continue;
                c.BringToFront();
            }

            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() != typeof(InnerImage) && c.GetType() != typeof(Scale)) continue;
                c.BringToFront();
            }

            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() != typeof(Button) && c.GetType() != typeof(CloseButton)) continue;
                c.BringToFront();
            }

            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() != typeof(Slot) && c.GetType() != typeof(InvSlot)) continue;
                c.BringToFront();
            }

            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() != typeof(InnerText)) continue;
                c.BringToFront();
            }
        }

        internal void AddElement(InnerControl copy)
        {
            panelDesktop.Controls.Add(copy);
        }

        internal void Parse(string filename, string gui)
        {
            jSONParser.Parse(gui);
            ((TabPage)Parent).Text = filename;
            TryToGuessDrawOrder();
        }

        internal void Save(string filename)
        {
            changed = false;
            jSONParser.Save(filename);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (activeElement != null) activeElement.Select();
        }

        internal Panel GetDesktopPanel()
        {
            return panelDesktop;
        }

        private void OpenDefaultFileDialog(out bool canceled)
        {
            canceled = false;
            openFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog1.FileName = "";            
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel)
            {
                canceled = true;
                return;
            }
            if (openFileDialog1.SafeFileName == "")
            {
                canceled = true;
                return;
            }
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                canceled = true;
                return;
            }
        }

        internal Panel GetExplorerPanel()
        {
            return tabPageExplorer;
        }

        internal void ShowRemoveWarning(out bool cancelled)
        {
            if (changed)
            {
                DialogResult res = MessageBox.Show("Вы не сохранили вашу работу. \nПродолжить?", "Закрытие окна", MessageBoxButtons.OKCancel);
                if (res == DialogResult.Cancel)
                    cancelled = true;
                else
                    cancelled = false;
            }
            else
            {
                cancelled = false;
            }
        }

        private void инвентарьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchInventorySlots();
        }

        private void заголовокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchHeader();
        }

        private void поУмолчаниюЗакрыть2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _params.CloseButton2ToDefault();
        }

        private void цветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            стандартныеВозможностиToolStripMenuItem.DropDown.Close();
            panelDesktop.BackgroundImage = null;
            colorDialog1.ShowDialog();
            panelDesktop.BackColor = colorDialog1.Color;
            ColorAllToPanelColor();
        }

        private void изображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            стандартныеВозможностиToolStripMenuItem.DropDown.Close();
            OpenDefaultFileDialog(out bool canceled);
            if (canceled) return;
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            BackgroundImageName = openFileDialog1.SafeFileName;
            panelDesktop.BackgroundImage = image;
            AllMergeToBackground();
        }

        private void слотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            параметрыToolStripMenuItem.DropDown.Close();
            OpenDefaultFileDialog(out bool canceled);
            if (canceled) return;
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            _params.SetSlotImage(image, openFileDialog1.SafeFileName);
        }

        private void слотИнвентаряToolStripMenuItem_Click(object sender, EventArgs e)
        {
            параметрыToolStripMenuItem.DropDown.Close();
            OpenDefaultFileDialog(out bool canceled);
            if (canceled) return;
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            _params.SetInvSlotImage(image, openFileDialog1.SafeFileName);
        }

        private void рамкаСлотаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            параметрыToolStripMenuItem.DropDown.Close();
            OpenDefaultFileDialog(out bool canceled);
            if (canceled) return;
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            _params.SetSelectionImage(image, openFileDialog1.SafeFileName);
        }

        private void кнопкаЗакрытияВыклToolStripMenuItem_Click(object sender, EventArgs e)
        {
            параметрыToolStripMenuItem.DropDown.Close();
            OpenDefaultFileDialog(out bool canceled);
            if (canceled) return;
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            _params.SetCloseButtonImage(image, openFileDialog1.SafeFileName);
        }

        private void кнопкаЗакрытияВклToolStripMenuItem_Click(object sender, EventArgs e)
        {
            параметрыToolStripMenuItem.DropDown.Close();
            OpenDefaultFileDialog(out bool canceled);
            if (canceled) return;
            Image image = Bitmap.FromFile(openFileDialog1.FileName);
            _params.SetCloseButton2Image(image, openFileDialog1.SafeFileName);
        }

        private void очиститьЭкранToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearWorkscreen();
            panelDesktop.BackgroundImage = null;
            panelDesktop.BackColor = Color.FromArgb(114, 106, 112);
            innerHeader.SetText("");
            innerHeader.Visible = false;
            ChangeHeight(label.Top);
        }

        private void поУмолчаниюСлотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _params.SlotToDefault();
        }

        private void поУмолчаниюСлотИнвентаряToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _params.InvSlotToDefault();
        }

        private void поУмолчаниюВыделениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _params.SelectionToDefault();
        }

        private void поУмолчаниюЗакрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _params.CloseButtonToDefault();
        }

        private void cброситьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _params.AllToDefault();
        }

        private void PanelDesktop_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control == null || e.Control.GetType() == typeof(Label) || e.Control.GetType() == typeof(InnerHeader)) return;
            ReloadExporer();
            ((InnerControl)e.Control).SelectControl();
        }

        private void PanelDesktop_Click(object sender, EventArgs e)
        {
            InnerControl innerControl = activeElement;
            if (innerControl != null)
            {
                innerControl.DeselectControl();
            }
        }

        private InnerControl _activeElement;
        public InnerControl activeElement
        {
            get { return _activeElement; }
            set { _activeElement = value; }
        }

        internal void SetHeaderText(string text)
        {
            innerHeader.SetText(text);
            innerHeader.Visible = true;
        }

        private int _counter;
        public int counter
        {
            get { return _counter++; }
            set { _counter = value; }
        }

        private bool _inventoryDrawed = false;
        public bool inventoryDrawed
        {
            get { return _inventoryDrawed; }
            set { _inventoryDrawed = value; }
        }

        private string _BackgroundImageName;

        public string BackgroundImageName
        {
            get { return _BackgroundImageName; }
            set { _BackgroundImageName = value; }
        }

        internal void TurnToDefault(Type type, Image slotDefaultImage, string slotImageName)
        {
            //1
            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() == type)
                {
                    if (type == typeof(Slot))
                    {
                        Slot _c = (Slot)c;
                        _c.ToDefault();
                    }
                    else
                    if (type == typeof(InvSlot))
                    {
                        InvSlot _c = (InvSlot)c;
                        _c.ToDefault();
                    }
                }
            }
        }

        internal void DeselectActiveControl()
        {
            if (_activeElement == null) return;
            _activeElement.DeselectControl();
        }

        internal void TurnSlotsSelectionToDefault(Image selectionDefaultImage)
        {
            //2
            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() == typeof(Slot))
                {
                    Slot _c = (Slot)c;
                    _c.SetSelection(selectionDefaultImage);
                }
                else
                if (c.GetType() == typeof(InvSlot))
                {
                    InvSlot _c = (InvSlot)c;
                    _c.ToDefault();
                    _c.SetSelection(selectionDefaultImage);
                }
            }
        }

        internal void TurnCloseButtonsToDefault(Image closeButtonDefaultImage, string closeButtonImageName, Image closeButton2DefaultImage, string closeButton2ImageName)
        {
            //3
            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() == typeof(CloseButton))
                {
                    CloseButton b = (CloseButton)c;
                    b.SetImage(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
                }
            }
        }

        internal void SetGlobalColor(string bg_color)
        {
            bg_color = bg_color.Split('(')[1].Replace(")", "");
            string[] rgb = bg_color.Split(',');
            panelDesktop.BackColor = Color.FromArgb(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
            ColorAllToPanelColor();
        }

        internal void SetGlobalBackground(string bg_bitmap)
        {
            _BackgroundImageName = bg_bitmap;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + bg_bitmap + ".png";
            try
            {
                panelDesktop.BackgroundImage = Bitmap.FromFile(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутствует файл " + path, "Изображение заднего фона");
            }
        }

        public void ReloadExporer()
        {
            foreach (Control c in tabPageExplorer.Controls)
            {
                c.Dispose();
            }
            tabPageExplorer.Controls.Clear();
            foreach (Control _c in panelDesktop.Controls)
            {
                if (_c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.hidden) continue;
                Label textBox = new Label();
                textBox.Location = new Point(15, tabPageExplorer.Controls.Count * 20);
                textBox.Text = c.elementName;
                textBox.Click += TextBox_Click; //Выбор объекта на рабочем столе
                textBox.AutoSize = true;
                textBox.BackColor = tabPageExplorer.BackColor;
                tabPageExplorer.Controls.Add(textBox);
            }
            changed = true;
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            Label textBox = (Label)sender;
            foreach (Control _c in panelDesktop.Controls)
            {
                if (_c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.elementName == textBox.Text)
                {
                    c.SelectControl();
                }
            }
        }

        internal void AllMergeToBackground()
        {
            /*foreach(Control c in panelDesktop.Controls)
            {
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                InnerControl _c = (InnerControl)c;
                c.ToBackground();
            }*/
        }

        internal void ColorAllToPanelColor()
        {
            foreach (Control c in panelDesktop.Controls)
            {
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                InnerControl innerControl = (InnerControl)c;
                if (innerControl.hidden || innerControl.constant) continue;
                innerControl.ColorImagesToPanelColor();
            }
        }

        internal void DrawInventorySlots()
        {
            for (int i = 0; i < 27; i++)
            {
                InvSlot invSlot = new InvSlot(explorerPainter, _params, this);
                invSlot.index = i + 9;
                invSlot.Location = new Point(i % 3 * invSlot.Width + panelDesktop.AutoScrollPosition.X, i / 3 * invSlot.Width + panelDesktop.AutoScrollPosition.Y + (innerHeader.Visible ? 40 : 0));
                invSlot.constant = true;
                invSlot.hidden = true;
                invSlot.elementName = "__invslot" + (i + 9);
                panelDesktop.Controls.Add(invSlot);
            }
            inventoryDrawed = true;
        }

        internal void SwitchInventorySlots()
        {
            _inventoryDrawed = !_inventoryDrawed;
            switch (_inventoryDrawed)
            {
                case true:
                    DrawInventorySlots();
                    break;

                case false:
                    RemoveInventorySlots();
                    break;

            }
            panelDesktop.Refresh();
        }

        internal void SwitchHeader()
        {
            /*if(innerHeader.Visible)
            {
                foreach(Control c in panelDesktop.Controls)
                {
                    if (c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                    c.Location = new Point(c.Location.X, c.Location.Y - 80);
                }
            }
            else
            {
                foreach (Control c in panelDesktop.Controls)
                {
                    if (c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                    c.Location = new Point(c.Location.X, c.Location.Y - 80);
                }
            }*/
            //innerHeader.RefreshControl();
            innerHeader.Visible = !innerHeader.Visible;
            switch (innerHeader.Visible)
            {
                case true:
                    if (inventoryDrawed)
                    {
                        for (int i = 0; i < panelDesktop.Controls.Count; i++)
                        {
                            Control c = panelDesktop.Controls[i];
                            if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                            InnerControl _c = (InnerControl)c;
                            if (_c.elementName.Contains("__invslot"))
                            {
                                _c.Location = new Point(_c.Location.X, _c.Location.Y + 40);
                            }
                        }
                    }
                    break;

                case false:
                    if (inventoryDrawed)
                    {
                        for (int i = 0; i < panelDesktop.Controls.Count; i++)
                        {
                            Control c = panelDesktop.Controls[i];
                            if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                            InnerControl _c = (InnerControl)c;
                            if (_c.elementName.Contains("__invslot"))
                            {
                                _c.Location = new Point(_c.Location.X, _c.Location.Y - 40);
                            }
                        }
                    }
                    break;
            }
        }

        internal void ChangeHeight(int height)
        {
            label.Location  = new Point(label.Left, height);
            toolStripTextBoxHeight.Text = height.ToString();
        }

        internal void RemoveInventorySlots()
        {
            for (int i = 0; i < panelDesktop.Controls.Count; i++)
            {
                Control c = panelDesktop.Controls[i];
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                InnerControl _c = (InnerControl)c;
                if (_c.elementName.Contains("__invslot"))
                {
                    panelDesktop.Controls[i].Dispose();
                    i--;
                }
            }
            inventoryDrawed = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Slot control = new Slot(explorerPainter, _params, this);
            control.Location = new Point(panelDesktop.Width / 2, panelDesktop.Height / 2);
            panelDesktop.Controls.Add(control);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InvSlot control = new InvSlot(explorerPainter, _params, this);
            control.Location = new Point(panelDesktop.Width / 2, panelDesktop.Height / 2);
            panelDesktop.Controls.Add(control);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InnerButton control = new InnerButton(explorerPainter, _params, this);
            control.Location = new Point(panelDesktop.Width / 2, panelDesktop.Height / 2);
            panelDesktop.Controls.Add(control);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CloseButton control = new CloseButton(explorerPainter, _params, this);
            control.Location = new Point(panelDesktop.Width -control.Width, 0);
            panelDesktop.Controls.Add(control);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Scale control = new Scale(explorerPainter, _params, this);
            control.Location = new Point(panelDesktop.Width / 2, panelDesktop.Height / 2);
            panelDesktop.Controls.Add(control);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            InnerText control = new InnerText(explorerPainter, _params, this);
            control.Location = new Point(panelDesktop.Width / 2, panelDesktop.Height / 2);
            panelDesktop.Controls.Add(control);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            InnerImage control = new InnerImage(explorerPainter, _params, this);
            control.Location = new Point(panelDesktop.Width / 2, panelDesktop.Height / 2);
            panelDesktop.Controls.Add(control);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            InnerBitmap control = new InnerBitmap(explorerPainter, _params, this);
            control.Location = new Point(panelDesktop.Width / 2, panelDesktop.Height / 2);
            panelDesktop.Controls.Add(control);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelDesktop.BackgroundImage = null;
        }

        private void сброситьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelDesktop.BackColor = Color.FromArgb(114, 106, 112);
        }

        private void сбросить1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeGlobalScale(1);
        }

        private void ChangeGlobalScale(float v)
        {
            globalScale = v;
        }

        private void сбросить700ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeHeight(defaultHeight);
        }
    }
}
