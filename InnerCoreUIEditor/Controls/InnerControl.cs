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
using InnerCoreUIEditor.Controls;

namespace InnerCoreUIEditor
{
    public partial class InnerControl : UserControl
    {
        public Params _params;
        public ExplorerPainter explorerPainter;

        public string elementName { get; set; }

        public int elementY, elementSpacing = 20;

        public InnerTabPage parentTabPage;

        //Editor's vars

        public bool XTextChanged;
        public bool YTextChanged;
        public bool scaleTextChanged;
        public float scale { get; set; }
        public Size originSize { get; set; }

        //public bool propPanelCleared;

        public bool constant;
        public bool hidden;

        public string clicker;

        public InnerControl(ExplorerPainter explorerPainter, Params _params, InnerTabPage parentTabPage)
        {
            InitializeComponent();
            this.parentTabPage = parentTabPage;
            elementName = this.GetType().ToString() + "_" + DateTime.Now  + "_" + DateTime.Now.Millisecond;
            clicker = "";
            elementY = 0;
            constant = false;
            hidden = false;
            //propPanelCleared = false;
            Disposed += InnerControl_Disposed;
            this.explorerPainter = explorerPainter;
            this._params = _params;
        }

        public virtual void ToDefault()
        {
            throw new NotImplementedException();
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
            foreach(Control c in parentTabPage.GetPropertiesPanel().Controls)
            {
                c.Dispose();
            }
            parentTabPage.GetPropertiesPanel().Controls.Clear();
        }

        public void ClearPropPanel(Panel propPanel)
        {
            foreach (Control c in propPanel.Controls)
            {
                c.Dispose();
            }
            propPanel.Controls.Clear();
            propPanel.Refresh();
            //propPanelCleared = true;
            elementY = 0;
        }

        public virtual void ToBackground()
        {
            throw new NotImplementedException();
        }

        public virtual void FillPropPanel(Panel propPanel)
        {
            /*if (!propPanelCleared)
            {
                ClearPropPanel(propPanel);
                FillName(propPanel);
            }*/
            if(!constant)AddRemoveButton(propPanel);
            propPanel.Refresh();
            //Console.WriteLine("Drawed");
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

            Label _visible = new Label();
            _visible.Location = new Point(0, elementY += elementSpacing);
            _visible.Size = new Size(102, elementSpacing);
            _visible.Text = "Видимость";
            propPanel.Controls.Add(_visible);

            CheckBox _globalCheck = new CheckBox();
            _globalCheck.Location = new Point(103, elementY);
            _globalCheck.Size = new Size(101, elementSpacing);
            _globalCheck.Checked = Visible;
            _globalCheck.CheckedChanged += (sender, e) => { Visible = ((CheckBox)sender).Checked; };
            propPanel.Controls.Add(_globalCheck);

            elementY += elementSpacing;
        }

        private void _nameValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _nameValue_LostFocus(sender, null);
                e.SuppressKeyPress = true;
            }
        }

        public void _coordsXValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _coordsXValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        public void _coordsXValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            if (!XTextChanged) return;
            XTextChanged = false;
            TextBox textBox = (TextBox)sender;
            int x;
            if (!int.TryParse(textBox.Text, out x))
            {
                textBox.Text = Left.ToString();
                return;
            }
            x += parentTabPage.GetDesktopPanel().AutoScrollPosition.X;
            if (x < parentTabPage.GetDesktopPanel().AutoScrollPosition.X || x > parentTabPage.MaxX() - Size.Width)
            {
                textBox.Text = Left.ToString();
                return;
            }
            if (x != Location.X)
            {
                Location = new Point(x, Location.Y);
                parentTabPage.GetDesktopPanel().Refresh();
            }
        }

        public void _coordsYValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _coordsYValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        public void _coordsYValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            if (!YTextChanged) return;
            YTextChanged = false;
            TextBox textBox = (TextBox)sender;
            int y;
            if (!int.TryParse(textBox.Text, out y))
            {
                textBox.Text = Top.ToString();
                return;
            }
            y += parentTabPage.GetDesktopPanel().AutoScrollPosition.Y;
            if (y < parentTabPage.GetDesktopPanel().AutoScrollPosition.Y || y > parentTabPage.MaxY() - Size.Width)
            {
                textBox.Text = Top.ToString();
                return;
            }
            if (y != Location.Y)
            {
                Location = new Point(Location.X, y);
                parentTabPage.GetDesktopPanel().Refresh();
            }
        }

        public void _sizeValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _sizeValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        //Это float, а нужно оверрайдить int
        public virtual void _sizeValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            if (!scaleTextChanged) return;
            scaleTextChanged = false;
            TextBox textBox = (TextBox)sender;
            float scale;
            if (!float.TryParse(textBox.Text, out scale))
            {
                textBox.Text = scale.ToString();
                return;
            }

            if (scale * originSize.Height > parentTabPage.MaxX() || scale * originSize.Width > parentTabPage.MaxY())
            {
                textBox.Text = scale.ToString();
                return;
            }

            if (scale == this.scale) return;
            ChangeScale(scale);
        }

        public virtual void ChangeScale(float scale)
        {
            throw new NotImplementedException();
        }

        internal void Remove()
        {
            RemoveButton_Click(null, null);
        }

        internal virtual void SelectControl()
        {
            if (parentTabPage.activeElement != null) parentTabPage.activeElement.DeselectControl();
            parentTabPage.activeElement = this;
            //propPanelCleared = false;
            elementY = 0;
            //parentTabPage.GetDesktopPanel().ScrollControlIntoView(this);
            //Сделать фокусировку панели на элементе
            explorerPainter.Color(elementName);
            parentTabPage.activeElement.FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        internal virtual void DeselectControl()
        {
            parentTabPage.activeElement = null;
            //propPanelCleared = false;
            ClearPropPanel(parentTabPage.GetPropertiesPanel());
            explorerPainter.Uncolor(elementName);
        }

        private void _nameValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            TextBox textBox = (TextBox)sender;
            string newName = textBox.Text;
            foreach(Label c in parentTabPage.GetExplorerPanel().Controls)
            {
                if (c.Text.Equals(newName))
                {
                    textBox.Text = elementName;
                    return;
                }
            }
            elementName = newName;
            parentTabPage.ReloadExporer();
            SelectControl();
        }

        public virtual void ResizeControl(char longestSide, int distance)
        {
            switch (longestSide)
            {
                case 'x':
                    {
                        if (distance < 0) distance = (int)GetWidth() + distance;
                        CountScale('x', distance);
                        break;
                    }

                case 'y':
                    {
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

        public virtual void ColorImagesToPanelColor()
        {
            //throw new NotImplementedException();
        }

        private void ToFrontButton_Click(object sender, EventArgs e)
        {
            BringToFront();
            parentTabPage.GetDesktopPanel().Refresh();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach(Control _c in parentTabPage.GetDesktopPanel().Controls)
            {
                if (_c.GetType()== typeof(Label) || _c.GetType() == typeof(InnerHeader)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.elementName == elementName)
                {   
                    parentTabPage.GetDesktopPanel().Controls.Remove(_c);
                    break;
                }
            }
            parentTabPage.ReloadExporer();
            this.Dispose();
        }

        internal virtual string MakeOutput()
        {
            MessageBox.Show(this.GetType() + " не содержит выходной функции");
            return "";
        }
    }
}
