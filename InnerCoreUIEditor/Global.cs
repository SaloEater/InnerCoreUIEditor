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
                        Y = 700;

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
            foreach(InnerControl c in _panelWorkspace.Controls)
            {
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
            foreach (InnerControl c in panelWorkspace.Controls)
            {
                if (c.elementName == textBox.Text)
                {
                    Global.activeElement = c;
                    c.FillPropPanel(panelProperties);
                }
            }
        }
    }
}
