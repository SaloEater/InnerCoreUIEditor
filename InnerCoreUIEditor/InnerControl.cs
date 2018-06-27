using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    public partial class InnerControl : UserControl
    {
        public string elementName { get; set; }

        public InnerControl()
        {
            InitializeComponent();
            elementName = this.GetType().ToString() + "_" + Global.counter;
        }

        public virtual void FillPropPanel(Panel propPanel)
        {
            int elementY = -20;

            Label _name = new Label();
            _name.Location = new Point(0, elementY += 20);
            _name.Size = new Size(102, 20);
            _name.Text = "Название";
            propPanel.Controls.Add(_name);

            TextBox _nameValue = new TextBox();
            _nameValue.Location = new Point(103, elementY);
            _nameValue.Size = new Size(100, 20);
            _nameValue.Text = elementName;
            _nameValue.LostFocus += _nameValue_LostFocus;
            _nameValue.KeyDown += _nameValue_KeyDown;
            propPanel.Controls.Add(_nameValue);
        }

        private void _nameValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _nameValue_LostFocus(sender, null);
            }
        }

        private void _nameValue_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string newName = textBox.Text;
            foreach(TextBox c in Global.panelExplorer.Controls)
            {
                if (c.Text.Equals(newName))
                {
                    textBox.Text = elementName;
                    return;
                }
            }
            elementName = newName;
            Global.ReloadExporer();
        }

        public virtual void ClearPropPanel(Panel propPanel)
        {
            foreach(Control c in propPanel.Controls)
            {
                c.Dispose();
            }
            propPanel.Controls.Clear();
        }

        public virtual void ResizeControl(int size)
        {
            if (size + Left < 0 || size + Left > Global.X || size + Top > Global.Y)return;
            if (Top < 0 && size - Top > Global.Y) return;
            Size = new Size(size, size);
            foreach(Control c in Controls)
            {
                c.Size = new Size(size, size);
            }
        }
    }
}
