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
    public partial class Slot : InnerControl
    {
        private bool Visual { get; set; }
        private Image ActiveImage { get; set; }
        private string Clicker { get; set; }
        private string ImageName { get; set; }

        public Slot()
        {
            InitializeComponent();
            ControlEditor.Init(pictureBoxSlot, this);
            ActiveImage = ResizeImage(Resources._default_slot_light, new Size(Size.Width, Size.Height));
            pictureBoxSlot.Image = ActiveImage;
            pictureBoxSlot.Click += PictureBoxSlot_Click;

            ImageName = "_default_slot_light.png";
            Visual = false;
        }

        private void PictureBoxSlot_Click(object sender, EventArgs e)
        {
            Global.activeElement = this;
            Global.activeElement.FillPropPanel(Global.panelProperties);
        }

        private Image ResizeImage(Image image, Size size)
        {
            Image newImage = new Bitmap(size.Width, size.Height);
            using (Graphics gfx = Graphics.FromImage(newImage))
            {
                gfx.DrawImage(image, new Rectangle(Point.Empty, size));
            }            
            return newImage;
        }

        public override void FillPropPanel(Panel propPanel)
        {
            propPanel.Visible = true;
            foreach(Control c in propPanel.Controls)
            {
                c.Dispose();
            }
            propPanel.Controls.Clear();
            base.FillPropPanel(propPanel);

            int elementY = 0;

            Label _size = new Label();
            _size.Location = new Point(0, elementY += 20);
            _size.Size = new Size(102, 20);
            _size.Text = "Размер";
            propPanel.Controls.Add(_size);

            TextBox _sizeValue = new TextBox();
            _sizeValue.Location = new Point(103, elementY);
            _sizeValue.Size = new Size(100, 20);
            _sizeValue.Text = Size.Height.ToString();
            _sizeValue.LostFocus += _sizeValue_LostFocus;
            _sizeValue.KeyDown += _sizeValue_KeyDown;
            propPanel.Controls.Add(_sizeValue);

            Label _coords = new Label();
            _coords.Location = new Point(0, elementY += 20);
            _coords.Size = new Size(204, 20);
            _coords.Text = "Координаты";
            propPanel.Controls.Add(_coords);

            Label _coordsX = new Label();
            _coordsX.Location = new Point(0, elementY += 20);
            _coordsX.Size = new Size(51, 20);
            _coordsX.Text = "X";
            propPanel.Controls.Add(_coordsX);

            TextBox _coordsXValue = new TextBox();
            _coordsXValue.Location = new Point(52, elementY);
            _coordsXValue.Size = new Size(151, 20);
            _coordsXValue.Text = (Location.X - Global.panelWorkspace.AutoScrollPosition.X).ToString();
            _coordsXValue.LostFocus += new EventHandler(_coordsXValue_LostFocus);
            _coordsXValue.KeyDown += _coordsXValue_KeyDown;
            propPanel.Controls.Add(_coordsXValue);

            Label _coordsY = new Label();
            _coordsY.Location = new Point(0, elementY += 20);
            _coordsY.Size = new Size(51, 20);
            _coordsY.Text = "Y";
            propPanel.Controls.Add(_coordsY);

            TextBox _coordsYValue = new TextBox();
            _coordsYValue.Location = new Point(52, elementY);
            _coordsYValue.Size = new Size(151, 20);
            _coordsYValue.Text = (Location.Y - Global.panelWorkspace.AutoScrollPosition.Y).ToString();
            _coordsYValue.LostFocus += new EventHandler(_coordsYValue_LostFocus);
            _coordsYValue.KeyDown += _coordsYValue_KeyDown;
            propPanel.Controls.Add(_coordsYValue);      

            Label _image = new Label();
            _image.Location = new Point(0, elementY += 20);
            _image.Size = new Size(102, 20);
            _image.Text = "Изображение";
            propPanel.Controls.Add(_image);

            TextBox _imagePicPath = new TextBox();
            _imagePicPath.Location = new Point(103, elementY);
            _imagePicPath.Size = new Size(100, 20);
            _imagePicPath.Text = ImageName;
            _imagePicPath.LostFocus += new EventHandler(_imagePicPath_LostFocus);
            _imagePicPath.ReadOnly = true;

            Button openFileDialog = new Button();
            openFileDialog.Size = new Size(_imagePicPath.Size.Height / 5 * 4, _imagePicPath.Size.Height / 5 * 4);
            openFileDialog.Location = new Point(_imagePicPath.Size.Width - _imagePicPath.Size.Height, 0);
            openFileDialog.Click += new EventHandler(openFileDialog_Click);
            _imagePicPath.Controls.Add(openFileDialog);

            propPanel.Controls.Add(_imagePicPath);

            Label _visual = new Label();
            _visual.Location = new Point(0, elementY += 20);
            _visual.Size = new Size(102, 20);
            _visual.Text = "Визуальный элемент";
            propPanel.Controls.Add(_visual);

            CheckBox _visualCheck = new CheckBox();
            _visualCheck.Location = new Point(103, elementY);
            _visualCheck.Size = new Size(101, 20);
            _visualCheck.Checked = Visual;
            _visualCheck.CheckedChanged += (sender, e) => { Visual = ((CheckBox)sender).Checked; };
            propPanel.Controls.Add(_visualCheck);

            /* Придумать как сделать окно для вставки функции кликера
            Label _clicker = new Label();
            _clicker.Location = new Point(0, elementY += 20);
            _clicker.Size = new Size(102, 20);
            _clicker.Text = "Кликер";
            propPanel.Controls.Add(_clicker);

            TextBox _imagePicPath = new TextBox();
            _imagePicPath.Location = new Point(103, elementY += 20);
            _imagePicPath.Size = new Size(102, 20);
            _imagePicPath.Text = ImageName;
            _imagePicPath.GotFocus += new EventHandler(_imagePicPath_GotFocus);
            _imagePicPath.ReadOnly = true;
            propPanel.Controls.Add(_imagePicPath);*/
        }

        private void _coordsXValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _coordsXValue_LostFocus(sender, null);
            }
        }

        private void _coordsYValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _coordsYValue_LostFocus(sender, null);
            }
        }

        private void _sizeValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _sizeValue_LostFocus(sender, null);
            }
        }

        private void _sizeValue_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int size;
            if (!int.TryParse(textBox.Text, out size))
            {
                textBox.Text = Width.ToString();
                return;
            }

            if (size + Left < 0 || size + Left > Global.X || size + Top > Global.Y)
            {
                textBox.Text = Width.ToString();
                return;
            }

            if (textBox.Text == Size.Height.ToString()) return;

            ResizeControl(size);
            Refresh();
            Global.panelWorkspace.Refresh();
            //пуш
        }

        private void _imagePicPath_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if(textBox.Controls.Count!=0)textBox.Controls[0].Dispose();
            textBox.Controls.Clear();
        }

        private void openFileDialog_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }

            ImageName = openFileDialog1.SafeFileName;
            ActiveImage = ResizeImage(Image.FromFile(openFileDialog1.FileName), new Size(Size.Width, Size.Height));
            pictureBoxSlot.Image = ActiveImage;
            pictureBoxSlot.Refresh();
            
        }

        private void _coordsXValue_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int x;
            if (!int.TryParse(textBox.Text, out x))
            {
                textBox.Text = Left.ToString();
                return;
            }
            x -= Global.panelWorkspace.AutoScrollPosition.X;
            if(x < 0 || x > Global.X - Size.Width)
            {
                textBox.Text = Left.ToString();
                return;
            }
            if (x != Location.X)
            {
                Location = new Point(x, Location.Y);
                Global.panelWorkspace.Refresh();
            }
        }

        private void _coordsYValue_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int y;
            if (!int.TryParse(textBox.Text, out y))
            {
                textBox.Text = Top.ToString();
                return;
            }
            y -= Global.panelWorkspace.AutoScrollPosition.Y;
            if( y < 0 || y > Global.Y - Size.Width)
            {
                textBox.Text = Top.ToString();
                return;
            }
            if (y != Location.Y)
            {
                Location = new Point(Location.X, y);
                Global.panelWorkspace.Refresh();
            }
        }

    }
}
