using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InnerCoreUIEditor.Properties;

namespace InnerCoreUIEditor
{
    public partial class InnerControl : UserControl
    {
        public string elementName { get; set; }

        public int elementY, elementSpacing = 20;

        public bool propPanelCleared;

        public bool constant;
        public bool hidden;

        public InnerControl()
        {
            InitializeComponent();
            elementName = this.GetType().ToString() + "_" + Global.counter;
            elementY = 0;
            constant = false;
            hidden = false;
            propPanelCleared = false;
            Disposed += InnerControl_Disposed;
        }

        public void ResizeAll(Size size)
        {
            foreach (Control c in Controls)
            {
                c.Size = size;
            }
            Size = size;
        }

        private void InnerControl_Disposed(object sender, EventArgs e)
        {
            foreach(Control c in Global.panelProperties.Controls)
            {
                c.Dispose();
            }
            Global.panelProperties.Controls.Clear();
        }

        public void ClearPropPanel(Panel propPanel)
        {
            if(!propPanelCleared)
            {
                foreach (Control c in propPanel.Controls)
                {
                    c.Dispose();
                }
                propPanel.Controls.Clear();
                propPanel.Refresh();
                propPanelCleared = true;
            }
        }

        public virtual void FillPropPanel(Panel propPanel)
        {
            if (!propPanelCleared)
            {
                ClearPropPanel(propPanel);
                FillName(propPanel);
            }
            AddRemoveButton(propPanel);
        }

        public void FillName(Panel propPanel)
        {
            Label _name = new Label();
            _name.Location = new Point(0, elementY);
            _name.Size = new Size(60, elementSpacing);
            _name.Text = "Название";
            propPanel.Controls.Add(_name);

            TextBox _nameValue = new TextBox();
            _nameValue.Location = new Point(61, elementY);
            _nameValue.Size = new Size(142, elementSpacing);
            _nameValue.Text = elementName;
            _nameValue.LostFocus += _nameValue_LostFocus;
            _nameValue.KeyDown += _nameValue_KeyDown;
            propPanel.Controls.Add(_nameValue);
            elementY += elementSpacing;
        }

        private void _nameValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _nameValue_LostFocus(sender, null);
            }
        }

        internal void Remove()
        {
            RemoveButton_Click(null, null);
        }

        internal virtual void SelectControl()
        {
            if (Global.activeElement != null) Global.activeElement.DeselectControl();
            Global.activeElement = this;
            propPanelCleared = false;
            elementY = 0;
            //Сделать фокусировку панели на элементе
            Global.activeElement.FillPropPanel(Global.panelProperties);
            ExplorerPainter.Color(elementName);
        }

        internal void DeselectControl()
        {
            Global.activeElement = null;
            propPanelCleared = false;
            ClearPropPanel(Global.panelProperties);
            ExplorerPainter.Uncolor(elementName);
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

        public virtual void ResizeControl(char longestSide, int distance)
        {
            switch (longestSide)
            {
                case 'x':
                    {
                        Console.WriteLine('x');
                        if (distance < 0) distance = (int)GetWidth() + distance;
                        CountScale('x', distance);
                        break;
                    }

                case 'y':
                    {
                        Console.WriteLine('y');
                        if (distance < 0) distance = (int)GetHeight() + distance;
                        CountScale('y', distance);
                        break;
                    }
            }
        }

        public virtual void CountScale(char axis, int distance)
        {
            throw new NotImplementedException();
        }

        public virtual float GetWidth()
        {
            throw new NotImplementedException();
        }

        public virtual float GetHeight()
        {
            throw new NotImplementedException();
        }

        public void AddRemoveButton(Panel propPanel)
        {
            Button removeButton = new Button();
            removeButton.Location = new Point(0, elementY+=elementSpacing);
            removeButton.Size = new Size(102, elementSpacing);
            removeButton.Text = "Удалить";
            removeButton.TextAlign = ContentAlignment.MiddleCenter;
            removeButton.Click += RemoveButton_Click;
            propPanel.Controls.Add(removeButton);

            Button toFrontButton = new Button();
            toFrontButton.BackgroundImageLayout = ImageLayout.Stretch;
            toFrontButton.Image = Resources.button_to_front;
            toFrontButton.Size = toFrontButton.Image.Size;
            toFrontButton.Location = new Point(103, elementY);
            toFrontButton.Click += ToFrontButton_Click;
            propPanel.Controls.Add(toFrontButton);
           
        }

        private void ToFrontButton_Click(object sender, EventArgs e)
        {
            BringToFront();
            Global.panelWorkspace.Refresh();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach(Control _c in Global.panelWorkspace.Controls)
            {
                if (_c.GetType() == typeof(Label)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.elementName == elementName)
                {   
                    Global.panelWorkspace.Controls.Remove(_c);
                    break;
                }
            }
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
