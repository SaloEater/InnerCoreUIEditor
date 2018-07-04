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
        public static int X = 1000,
                        Y = 540;

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
        
        internal static int _counter;
        public static int counter
        {
            get { return _counter++; }
            set { _counter = value; }
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
                if (_c.GetType() == typeof(Label)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.constant || c.hidden) continue;
                TextBox textBox = new TextBox();
                textBox.Location = new Point(0, panelExplorer.Controls.Count * 20);
                textBox.Size = new Size(panelExplorer.Width, 20);
                textBox.Text = c.elementName;
                textBox.Click += TextBox_Click; //Выбор объекта на рабочем столе
                textBox.ReadOnly = true;
                _panelExplorer.Controls.Add(textBox);
            }
        }

        private static void TextBox_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            foreach (Control _c in panelWorkspace.Controls)
            {
                if (_c.GetType() == typeof(Label)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.elementName == textBox.Text)
                {
                    c.SelectControl();
                }
            }
        }

        internal static void DrawInventorySlots()
        {
            for (int i = 0; i < 27; i++)
            {
                InvSlot invSlot = new InvSlot();
                invSlot.index = i + 9;
                invSlot.Location = new Point((i % 3) * invSlot.Width, ((int)i/3) * invSlot.Width);
                invSlot.constant = true;
                invSlot.elementName = "__invslot" + i;
                panelWorkspace.Controls.Add(invSlot);
            }
        }

        internal static void RemoveInventorySlots()
        {
            for (int i = 0; i < panelWorkspace.Controls.Count; i++)
            {
                Control c = panelWorkspace.Controls[i];
                if (c.GetType() == typeof(Label)) continue;
                InnerControl _c = (InnerControl)c;
                if (_c.elementName.Contains("__invslot"))
                {
                    panelWorkspace.Controls.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
