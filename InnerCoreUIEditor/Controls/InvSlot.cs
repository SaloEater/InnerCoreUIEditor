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
    public partial class InvSlot : InnerControl
    {
        public int index { get; set; }
        public Image ActiveImage { get; set; }
        public string ImageName { get; set; }

        private bool sizeTextChanged;
        private bool XTextChanged;
        private bool YTextChanged;

        public InvSlot()
        {
            InitializeComponent();
            Initialization();
        }

        public void Initialization()
        {
            ControlEditor.Init(pictureBoxSlot, this);
            ActiveImage = ResizeImage(Resources._default_slot_light, new Size(Size.Width, Size.Height));
            pictureBoxSlot.Image = new Bitmap(ActiveImage);
            pictureBoxSlot.Click += PictureBoxSlot_Click;
            ImageName = "_default_slot_light.png";
            index = 0;
        }

        public override void FillPropPanel(Panel propPanel)
        {
            ClearPropPanel(propPanel);

            FillName(propPanel);

            Label _index = new Label();
            _index.Location = new Point(0, elementY);
            _index.Size = new Size(51, elementSpacing);
            _index.Text = "Номер слота";
            propPanel.Controls.Add(_index);

            TextBox _indexValue = new TextBox();
            _indexValue.Location = new Point(52, elementY);
            _indexValue.Size = new Size(151, elementSpacing);
            _indexValue.Text = index.ToString();
            _indexValue.LostFocus += _indexValue_LostFocus;
            _indexValue.KeyDown += _indexValue_KeyDown;
            propPanel.Controls.Add(_indexValue);
            elementY += elementSpacing;

            Label _size = new Label();
            _size.Location = new Point(0, elementY);
            _size.Size = new Size(51, elementSpacing);
            _size.Text = "Размер";
            propPanel.Controls.Add(_size);

            TextBox _sizeValue = new TextBox();
            _sizeValue.Location = new Point(52, elementY);
            _sizeValue.Size = new Size(151, elementSpacing);
            _sizeValue.Text = Size.Height.ToString();
            _sizeValue.LostFocus += _sizeValue_LostFocus;
            _sizeValue.KeyDown += _sizeValue_KeyDown;
            _sizeValue.TextChanged += (sender, e) => { sizeTextChanged = true; };
            propPanel.Controls.Add(_sizeValue);

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
            _coordsXValue.Text = (Location.X - Global.panelWorkspace.AutoScrollPosition.X).ToString();
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
            _coordsYValue.Text = (Location.Y - Global.panelWorkspace.AutoScrollPosition.Y).ToString();
            _coordsYValue.LostFocus += new EventHandler(_coordsYValue_LostFocus);
            _coordsYValue.KeyDown += _coordsYValue_KeyDown;
            _coordsYValue.TextChanged += (sender, e) => { YTextChanged = true; };
            propPanel.Controls.Add(_coordsYValue);

            Label _image = new Label();
            _image.Location = new Point(0, elementY += elementSpacing);
            _image.Size = new Size(102, elementSpacing);
            _image.Text = "Изображение";
            propPanel.Controls.Add(_image);

            TextBox _imagePicPath = new TextBox();
            _imagePicPath.Location = new Point(103, elementY);
            _imagePicPath.Size = new Size(100, elementSpacing);
            _imagePicPath.Text = ImageName;
            _imagePicPath.LostFocus += new EventHandler(_imagePicPath_LostFocus);
            _imagePicPath.ReadOnly = true;

            Button openFileDialog = new Button();
            openFileDialog.Size = new Size(_imagePicPath.Size.Height / 5 * 4, _imagePicPath.Size.Height / 5 * 4);
            openFileDialog.Location = new Point(_imagePicPath.Size.Width - _imagePicPath.Size.Height, 0);
            openFileDialog.Click += new EventHandler(openFileDialog_Click);
            _imagePicPath.Controls.Add(openFileDialog);

            propPanel.Controls.Add(_imagePicPath);

            base.FillPropPanel(propPanel);
        }

        private void _indexValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _indexValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _indexValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            TextBox textBox = (TextBox)sender;
            int index;
            if (!int.TryParse(textBox.Text, out index))
            {
                textBox.Text = this.index.ToString();
                return;
            }

            if (index < 0 || index > 35)
            {
                textBox.Text = index.ToString();
                return;
            }

            this.index = index;
        }

        internal void Apply(string name, int x, int y, int size, string imageName, int index)
        {
            if (name != "") elementName = name;
            Location = new Point(x, y);
            ResizeAll(new Size(size, size));
            if (ImageName != imageName)
            {
                ImageName = imageName;
                string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + imageName + ".png";
                ApplyImage(path);
            }
            if (index > 0 && index < 36) this.index = index;
        }

        internal override string MakeOutput()
        {
            string element = "\n\t";
            element += '\"' + elementName + "\": {";
            element += "type: \"invSlot\",";
            element += "x: " + (Location.X - Global.panelWorkspace.AutoScrollPosition.X) + ',';
            element += "y: " + (Location.Y - Global.panelWorkspace.AutoScrollPosition.Y) + ',';
            element += "size: " + Width + ',';
            element += "index: " + index + ',';
            if (ImageName != "_default_slot_light.png") element += "bitmap: \"" + ImageName.Split('.')[0] + "\",";
            element += "}";
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

        public override void CountScale(char axis, int distance)
        {
            switch (axis)
            {
                case 'x':
                    {
                        float scale = (float)distance / GetWidth();
                        Point oldLocation = Location;
                        this.Scale(new SizeF(scale, scale));
                        Location = oldLocation;
                        break;
                    }

                case 'y':
                    {
                        float scale = (float)distance / GetHeight();
                        Point oldLocation = Location;
                        this.Scale(new SizeF(scale, scale));
                        Location = oldLocation;
                        break;
                    }
            }
        }

        private void PictureBoxSlot_Click(object sender, EventArgs e)
        {
            SelectControl();
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

        private void ApplyImage(string path)
        {
            Bitmap bitmap = CreateBitmap(path);
            ActiveImage = ResizeImage(bitmap, new Size(Size.Width, Size.Height));
            pictureBoxSlot.Image = new Bitmap(ActiveImage);
        }

        private Bitmap CreateBitmap(string path)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(path);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                return Resources._default_slot_light;
            }
            for (int _x = 0; _x < bitmap.Width; _x++)
            {
                for (int _y = 0; _y < bitmap.Height; _y++)
                {
                    Color pixelColor = bitmap.GetPixel(_x, _y);
                    Color newColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmap.SetPixel(_x, _y, newColor);
                }
            }
            return bitmap;
        }

        private void _coordsXValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _coordsXValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _coordsYValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _coordsYValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _sizeValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _sizeValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _sizeValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            if (!sizeTextChanged) return;
            sizeTextChanged = false;
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

            SizeF sizeF = new SizeF((float)size / this.GetWidth(), (float)size / GetWidth());
            Point oldLocation = Location;
            this.Scale(sizeF);
            Location = oldLocation;
        }

        private void _imagePicPath_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            /*TextBox textBox = (TextBox)sender;
            if(textBox.Controls.Count!=0)textBox.Controls[0].Dispose();
            textBox.Controls.Clear();*/
        }

        private void openFileDialog_Click(object sender, EventArgs e)
        {
            if (constant) return;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }

            ImageName = openFileDialog1.SafeFileName;
            ApplyImage(openFileDialog1.FileName);
            FillPropPanel(Global.panelProperties);
        }

        private void _coordsXValue_LostFocus(object sender, EventArgs e)
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
            x += Global.panelWorkspace.AutoScrollPosition.X;
            if (x < Global.panelWorkspace.AutoScrollPosition.X || x > Global.X - Size.Width)
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
            y += Global.panelWorkspace.AutoScrollPosition.Y;
            if (y < Global.panelWorkspace.AutoScrollPosition.Y || y > Global.Y - Size.Width)
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
