using InnerCoreUIEditor.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    static class Global
    {
        public const int defaultHeight = 700;

        public static int X = 1000,
                        Y = defaultHeight;


        private static Panel _panelProperties;
        public static Panel panelProperties
        {
            get { return _panelProperties; }
            set { _panelProperties = value; }
        }

        private static Panel _panelWorkspace;
        public static Panel panelWorkspace
        {
            get { return _panelWorkspace; }
            set { _panelWorkspace = value; }
        }

        private static Panel _panelExplorer;
        public static Panel panelExplorer
        {
            get { return _panelExplorer; }
            set { _panelExplorer = value; }
        }

        private static InnerControl _activeElement;
        public static InnerControl activeElement
        {
            get { return _activeElement; }
            set { _activeElement = value; }
        }

        internal static void SetHeaderText(string text)
        {
            _innerHeader.SetText(text);
            _innerHeader.Visible = true;
        }

        private static int _counter;
        public static int counter
        {
            get { return _counter++; }
            set { _counter = value; }
        }

        private static bool _inventoryDrawed = false;
        public static bool inventoryDrawed
        {
            get { return _inventoryDrawed; }
            set { _inventoryDrawed = value; }
        }

        private static InnerHeader _innerHeader;
        public static InnerHeader innerHeader
        {
            get { return _innerHeader; }
            set { _innerHeader = value; }
        }

        private static string _BackgroundImageName;

        public static string BackgroundImageName
        {
            get { return _BackgroundImageName; }
            set { _BackgroundImageName = value; }
        }

        internal static void TurnToDefault(Type type, Image slotDefaultImage, string slotImageName)
        {
            //1
            foreach(Control c in _panelWorkspace.Controls)
            {
                if (c.GetType() == type)
                {
                    if(type == typeof(Slot))
                    {
                        Slot _c = (Slot)c;
                        _c.ToDefault();
                    } else
                    if(type == typeof(InvSlot))
                    {
                        InvSlot _c = (InvSlot)c;
                        _c.ToDefault();
                    }
                }
            }
        }

        internal static void TurnSlotsSelectionToDefault(Image selectionDefaultImage)
        {
            //2
            foreach (Control c in _panelWorkspace.Controls)
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

        internal static void TurnCloseButtonsToDefault(Image closeButtonDefaultImage, string closeButtonImageName, Image closeButton2DefaultImage, string closeButton2ImageName)
        {
            //3
            foreach (Control c in _panelWorkspace.Controls)
            {
                if (c.GetType() == typeof(CloseButton))
                {
                    CloseButton b = (CloseButton)c;
                    b.SetImage(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
                }
            }
        }

        internal static void SetGlobalColor(string bg_color)
        {
            bg_color = bg_color.Split('(')[1].Replace(")", "");
            string[] rgb = bg_color.Split(',');
            panelWorkspace.BackColor = Color.FromArgb(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
            ColorAllToPanelColor();
        }

        internal static void SetGlobalBackground(string bg_bitmap)
        {
            _BackgroundImageName = bg_bitmap;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + bg_bitmap + ".png";
            try
            {
                panelWorkspace.BackgroundImage = Bitmap.FromFile(path);
            } catch(Exception)
            {
                MessageBox.Show("Отсутствует файл " + path, "Изображение заднего фона");
            }
        }

        public static void ReloadExporer()
        {
            foreach(Control c in _panelExplorer.Controls)
            {
                c.Dispose();
            }
            _panelExplorer.Controls.Clear();
            foreach(Control _c in _panelWorkspace.Controls)
            {
                if (_c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.constant || c.hidden) continue;
                Label textBox = new Label();
                textBox.Location = new Point(0, panelExplorer.Controls.Count * 20);
                textBox.Size = new Size(panelExplorer.Width, 20);
                textBox.Text = c.elementName;
                textBox.Click += TextBox_Click; //Выбор объекта на рабочем столе4
                _panelExplorer.Controls.Add(textBox);
            }
        }

        private static void TextBox_Click(object sender, EventArgs e)
        {
            Label textBox = (Label)sender;
            foreach (Control _c in panelWorkspace.Controls)
            {
                if (_c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.elementName == textBox.Text)
                {
                    c.SelectControl();
                }
            }
        }

        internal static void AllMergeToBackground()
        {
            /*foreach(Control c in _panelWorkspace.Controls)
            {
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                InnerControl _c = (InnerControl)c;
                c.ToBackground();
            }*/
        }

        internal static void ColorAllToPanelColor()
        {
            foreach (Control c in panelWorkspace.Controls)
            {
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                InnerControl innerControl = (InnerControl)c;
                if (innerControl.hidden || innerControl.constant) continue;
                innerControl.ColorImagesToPanelColor();
            }
        }

        internal static void DrawInventorySlots()
        {
            for (int i = 0; i < 27; i++)
            {
                InvSlot invSlot = new InvSlot();
                invSlot.index = i + 9;
                invSlot.Location = new Point(i % 3 * invSlot.Width + _panelWorkspace.AutoScrollPosition.X, i/3 * invSlot.Width + _panelWorkspace.AutoScrollPosition.Y + (_innerHeader.Visible?40:0));
                invSlot.hidden = true;
                invSlot.elementName = "__invslot" + (i+9);
                _panelWorkspace.Controls.Add(invSlot);
            }
            inventoryDrawed = true;
        }

        internal static void SwitchInventorySlots()
        {
            _inventoryDrawed = !_inventoryDrawed;
            switch(_inventoryDrawed)
            {
                case true:
                    DrawInventorySlots();
                    break;

                case false:
                    RemoveInventorySlots();
                    break;

            }
            _panelWorkspace.Refresh();
        }

        internal static void SwitchHeader()
        {
            /*if(innerHeader.Visible)
            {
                foreach(Control c in _panelWorkspace.Controls)
                {
                    if (c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                    c.Location = new Point(c.Location.X, c.Location.Y - 80);
                }
            }
            else
            {
                foreach (Control c in _panelWorkspace.Controls)
                {
                    if (c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                    c.Location = new Point(c.Location.X, c.Location.Y - 80);
                }
            }*/
            //innerHeader.RefreshControl();
            _innerHeader.Visible = !_innerHeader.Visible;
            switch(_innerHeader.Visible)
            {
                case true:
                    if(inventoryDrawed)
                    {
                        for (int i = 0; i < _panelWorkspace.Controls.Count; i++)
                        {
                            Control c = _panelWorkspace.Controls[i];
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
                        for (int i = 0; i < _panelWorkspace.Controls.Count; i++)
                        {
                            Control c = _panelWorkspace.Controls[i];
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

        internal static void ChangeHeight(int height)
        {
            Y = height;
            foreach(Control c in _panelWorkspace.Controls)
            {
                if(c.GetType() == typeof(Label))
                {
                    c.Location = new Point(c.Location.X, height);
                    break;
                }
            }
            //_panelWorkspace.Height = height;
            _panelWorkspace.Refresh();
        }

        internal static void RemoveInventorySlots()
        {
            for (int i = 0; i < _panelWorkspace.Controls.Count; i++)
            {
                Control c = _panelWorkspace.Controls[i];
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                InnerControl _c = (InnerControl)c;
                if (_c.elementName.Contains("__invslot"))
                {
                    _panelWorkspace.Controls.RemoveAt(i);
                    i--;
                }
            }
            inventoryDrawed = false;
        }
    }
}
