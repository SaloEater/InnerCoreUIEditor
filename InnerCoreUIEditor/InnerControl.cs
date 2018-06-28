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
            Disposed += InnerControl_Disposed;
        }

        private void InnerControl_Disposed(object sender, EventArgs e)
        {
            foreach(Control c in Global.panelProperties.Controls)
            {
                c.Dispose();
            }
            Global.panelProperties.Controls.Clear();
        }

        public virtual void FillPropPanel(Panel propPanel)
        {
            int elementY = -20;

            Label _name = new Label();
            _name.Location = new Point(0, elementY += 20);
            _name.Size = new Size(60, 20);
            _name.Text = "Название";
            propPanel.Controls.Add(_name);

            TextBox _nameValue = new TextBox();
            _nameValue.Location = new Point(61, elementY);
            _nameValue.Size = new Size(142, 20);
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

        public virtual void ResizeControl(char longestSide, int distance)
        {
            switch (longestSide)
            {
                case 'x':
                    {
                        if (distance < 0) distance = (int)GetWidth() + distance;
                        float scale = (float)distance / GetWidth();
                        Point oldLocation = Location;
                        this.Scale(new SizeF(scale, scale));
                        Location = oldLocation;
                        break;
                    }

                case 'y':
                    {
                        if (distance < 0) distance = (int)GetHeight() + distance;
                        float scale = (float)distance / GetHeight();
                        Point oldLocation = Location;
                        this.Scale(new SizeF(scale, scale));
                        Location = oldLocation;
                        break;
                    }
            }
        }

        public virtual float GetWidth()
        {
            throw new NotImplementedException();
        }

        public virtual float GetHeight()
        {
            throw new NotImplementedException();
        }

        public void AddRemoveButton(int elementY, Panel propPanel)
        {
            Button removeButton = new Button();
            removeButton.Location = new Point(0, elementY+=20);
            removeButton.Size = new Size(102, 20);
            removeButton.Text = "Удалить";
            removeButton.TextAlign = ContentAlignment.MiddleCenter;
            removeButton.Click += RemoveButton_Click;
            propPanel.Controls.Add(removeButton);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            Global.panelWorkspace.Controls.Remove(this);
            Global.ReloadExporer();
            this.Dispose();
        }

        internal virtual string MakeOutput()
        {
            MessageBox.Show(this.GetType() + " не содержит выходной функции");
            return "";
        }
    }
}
