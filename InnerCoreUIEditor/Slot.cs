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
    public partial class Slot : InnerControl
    {
        private bool Visual { get; set; }
        private bool TransparentBg { get; set; }
        private Image ActiveImage { get; set; }
        public string Clicker { get; set; }
        public string ImageName { get; set; }

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
            _size.Size = new Size(51, 20);
            _size.Text = "Размер";
            propPanel.Controls.Add(_size);

            TextBox _sizeValue = new TextBox();
            _sizeValue.Location = new Point(52, elementY);
            _sizeValue.Size = new Size(151, 20);
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

            Label _transpBg = new Label();
            _transpBg.Location = new Point(0, elementY += 20);
            _transpBg.Size = new Size(102, 20);
            _transpBg.Text = "Прозрачный фон";
            propPanel.Controls.Add(_transpBg);

            CheckBox _transpBgCheck = new CheckBox();
            _transpBgCheck.Location = new Point(103, elementY);
            _transpBgCheck.Size = new Size(101, 20);
            _transpBgCheck.Checked = Visual;
            _transpBgCheck.CheckedChanged += (sender, e) => { TransparentBg = ((CheckBox)sender).Checked; };
            propPanel.Controls.Add(_transpBgCheck);

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
            AddRemoveButton(elementY, propPanel);
        }

        internal void Apply(string name, int x, int y, int size, bool visual, string imageName, string clicker)
        {
            if(name!="")elementName = name;
            Location = new Point(x, y);
            Size = new Size(size, size);
            Visual = visual;
            if(ImageName != imageName)
            {
                ImageName = imageName;
                string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + imageName + ".png";
                ApplyImage(path);                
            }
            Clicker = clicker;
        }

        private void ApplyImage(string path)
        {
            Bitmap bitmap = CreateBitmap(path);
            ActiveImage = ResizeImage(bitmap, new Size(Size.Width, Size.Height));
            pictureBoxSlot.Image = ActiveImage;
        }

        private Bitmap CreateBitmap(string path)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(path);
            }catch(ArgumentException)
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

            SizeF sizeF = new SizeF((float)size/GetWidth(), (float)size / GetWidth());
            Point oldLocation = Location;
            this.Scale(sizeF);
            Location = oldLocation;
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
            ApplyImage(openFileDialog1.FileName);
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

        internal override string MakeOutput()
        {
            string element = "\n\t";
            element += '\"' + elementName + "\": {";
            element += "type: \"slot\",";
            element += "x: " + Location.X + ',';
            element += "y: " + Location.Y + ',';
            element += "size: " + Width + ',';
            if(ImageName != "_default_slot_light.png") element += "bitmap: \"" + ImageName.Split('.')[0] + "\",";
            if(Visual) element += "visual: true,";
            if (TransparentBg) element += "isTransparentBackground: true,";
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
    }
}
