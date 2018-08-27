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
using System.IO;

namespace InnerCoreUIEditor
{
    public partial class InnerText : InnerControl
    {
        bool WValueChanged;
        bool HValueChanged;

        public InnerText(InnerTabPage parentTabPage) : base(parentTabPage)
        {
            InitializeComponent();
            Initialization();
        }

        internal override void ApplyChanges(int type, object value)
        {
            /*
            Типы изменений:
            4 - размер
             */
            switch (type)
            {
                case 4:
                    Size = (Size)value;
                    break;
            }
            FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        internal override ActionStack MakeSnapshot(int type)
        {
            ActionStack action = null;
            switch (type)
            {
                case 4:
                    //позиция
                    action = new ActionStack(this, 4, Size);
                    break;
            }
            return action;
        }

        public void Initialization()
        {
            ControlEditor.Init(richTextBox1, this, parentTabPage);
            richTextBox1.Click += (sender, e) => SelectControl();
        }

        internal override InnerControl MakeCopy()
        {
            if (constant || hidden) throw new ArgumentOutOfRangeException();
            InnerText control = new InnerText(parentTabPage);
            control.Location = Location;
            control.Size = Size;
            control.Visible = Visible;
            control.scale = scale;
            control.originSize = originSize;

            control.richTextBox1.Text = richTextBox1.Text;

            return control;
        }

        public override void FillPropPanel(Panel propPanel)
        {
            ClearPropPanel(propPanel);
            FillName(propPanel);

            Label _coords = new Label();
            _coords.Location = new Point(0, elementY += elementSpacing);
            _coords.Size = new Size(204, elementSpacing);
            _coords.Text = "Координаты";
            propPanel.Controls.Add(_coords);

            Label _coordsX = new Label();
            _coordsX.Location = new Point(0, elementY += elementSpacing);
            _coordsX.Size = new Size(51, elementSpacing);
            _coordsX.Text = "X";
            propPanel.Controls.Add(_coordsX);

            TextBox _coordsXValue = new TextBox();
            _coordsXValue.Location = new Point(52, elementY);
            _coordsXValue.Size = new Size(151, elementSpacing);
            _coordsXValue.Text = (Location.X - parentTabPage.GetDesktopPanel().AutoScrollPosition.X).ToString();
            _coordsXValue.LostFocus += new EventHandler(_coordsXValue_LostFocus);
            _coordsXValue.KeyDown += _coordsXValue_KeyDown;
            _coordsXValue.TextChanged += (sender, e) => { XTextChanged = true; };
            propPanel.Controls.Add(_coordsXValue);

            Label _coordsY = new Label();
            _coordsY.Location = new Point(0, elementY += elementSpacing);
            _coordsY.Size = new Size(51, elementSpacing);
            _coordsY.Text = "Y";
            propPanel.Controls.Add(_coordsY);

            TextBox _coordsYValue = new TextBox();
            _coordsYValue.Location = new Point(52, elementY);
            _coordsYValue.Size = new Size(151, elementSpacing);
            _coordsYValue.Text = (Location.Y - parentTabPage.GetDesktopPanel().AutoScrollPosition.Y).ToString();
            _coordsYValue.LostFocus += new EventHandler(_coordsYValue_LostFocus);
            _coordsYValue.KeyDown += _coordsYValue_KeyDown;
            _coordsYValue.TextChanged += (sender, e) => { YTextChanged = true; };
            propPanel.Controls.Add(_coordsYValue);

            Label _size = new Label();
            _size.Location = new Point(0, elementY += elementSpacing);
            _size.Size = new Size(204, elementSpacing);
            _size.Text = "Размер";
            propPanel.Controls.Add(_size);

            Label _width = new Label();
            _width.Location = new Point(0, elementY += elementSpacing);
            _width.Size = new Size(51, elementSpacing);
            _width.Text = "Ширина";
            propPanel.Controls.Add(_width);

            TextBox _widthValue = new TextBox();
            _widthValue.Location = new Point(52, elementY);
            _widthValue.Size = new Size(151, elementSpacing);
            _widthValue.Text = Width.ToString();
            _widthValue.LostFocus += _widthValue_LostFocus;
            _widthValue.KeyDown += _widthValue_KeyDown;
            propPanel.Controls.Add(_widthValue);

            Label _height = new Label();
            _height.Location = new Point(0, elementY += elementSpacing);
            _height.Size = new Size(51, elementSpacing);
            _height.Text = "Высота";
            propPanel.Controls.Add(_height);

            TextBox _heightValue = new TextBox();
            _heightValue.Location = new Point(52, elementY);
            _heightValue.Size = new Size(151, elementSpacing);
            _heightValue.Text = Height.ToString();
            _heightValue.LostFocus += _heightValue_LostFocus;
            _heightValue.KeyDown += _heightValue_KeyDown;
            propPanel.Controls.Add(_heightValue);

            /*Label _text = new Label();
            _text.Location = new Point(0, elementY += elementSpacing);
            _text.Size = new Size(51, elementSpacing);
            _text.Text = "Текст";
            propPanel.Controls.Add(_text);

            TextBox _textValue = new TextBox();
            _textValue.Location = new Point(52, elementY);
            _textValue.Size = new Size(151, elementSpacing);
            _textValue.Text = richTextBox1.Text;
            _textValue.LostFocus += _textValue_LostFocus;
            _textValue.KeyDown += _textValue_KeyDown;
            propPanel.Controls.Add(_textValue);*/

            /* Придумать как сделать окно для вставки функции кликера
            Label _clicker = new Label();
            _clicker.Location = new Point(0, elementY += elementSpacing);
            _clicker.Size = new Size(102, elementSpacing);
            _clicker.Text = "Кликер";
            propPanel.Controls.Add(_clicker);

            TextBox _imagePicPath = new TextBox();
            _imagePicPath.Location = new Point(103, elementY += elementSpacing);
            _imagePicPath.Size = new Size(102, elementSpacing);
            _imagePicPath.Text = ImageName;
            _imagePicPath.GotFocus += new EventHandler(_imagePicPath_GotFocus);
            _imagePicPath.ReadOnly = true;
            propPanel.Controls.Add(_imagePicPath);*/
            base.FillPropPanel(propPanel);
        }

        private void _textValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            TextBox textBox = (TextBox)sender;
            richTextBox1.Text = textBox.Text;
        }

        private void _textValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _textValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _heightValue_KeyDown(object sender, KeyEventArgs e)
        {
            HValueChanged = true;
            if (e.KeyCode == Keys.Enter)
            {
                _heightValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _heightValue_LostFocus(object sender, EventArgs e)
        {
            if (constant || !HValueChanged) return;
            TextBox textBox = (TextBox)sender;
            int height;
            if (!int.TryParse(textBox.Text, out height))
            {
                textBox.Text = Height.ToString();
                return;
            }
            if (height < 0 || height > parentTabPage.MaxY() - Top)
            {
                textBox.Text = Height.ToString();
                return;
            }
            ResizeAll(new Size(richTextBox1.Width, height));
        }

        private void _widthValue_LostFocus(object sender, EventArgs e)
        {
            if (constant || !WValueChanged) return;
            TextBox textBox = (TextBox)sender;
            int width;
            if (!int.TryParse(textBox.Text, out width))
            {
                textBox.Text = Width.ToString();
                return;
            }
            if (width < 0 || width > parentTabPage.MaxX() - Left)
            {
                textBox.Text = Width.ToString();
                return;
            }
            ResizeAll(new Size(width, richTextBox1.Height));
        }

        private void _widthValue_KeyDown(object sender, KeyEventArgs e)
        {
            WValueChanged = true;
            if (e.KeyCode == Keys.Enter)
            {
                _widthValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        internal void Apply(string name, int x, int y, int width, int height, string text)
        {
            if(name!="")elementName = name;
            Location = new Point(x, y);
            ResizeAll(new Size(width, height));
            richTextBox1.Text = text;
        }

        internal override string MakeOutput()
        {
            string element = "\n\t";
            element += '\"' + elementName + "\": \n\t{";
            element += "\n\t\ttype: \"text\",";
            element += "\n\t\tx: " + (Location.X - parentTabPage.GetDesktopPanel().AutoScrollPosition.X) + ',';
            element += "\n\t\ty: " + (Location.Y - parentTabPage.GetDesktopPanel().AutoScrollPosition.Y) + ',';
            element += "\n\t\twidth: " + Width + ',';
            element += "\n\t\theight: " + Height + ',';
            element += "\n\t\ttext: " + richTextBox1.Text + ',';
            element += "\n\t}";
            return element;
        }

        public override float GetWidth()
        {
            return (float)Width;
        }

        public override float GetHeight()
        {
            return (float)Height;
        }
    }
}
