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
        public bool Visual { get; set; }
        public bool TransparentBg { get; set; }
        public Image ActiveImage { get; set; }
        public string Clicker { get; set; }
        public string ImageName { get; set; }

        private bool sizeTextChanged;
        private bool XTextChanged;
        private bool YTextChanged;

        public Slot()
        {
            InitializeComponent();
            Initialization();
        }

        public void Initialization()
        {
            ControlEditor.Init(pictureBoxSlot, this);
            ControlEditor.Init(pictureBoxSelection, this);
            pictureBoxSelection.SizeMode = PictureBoxSizeMode.StretchImage;
            ToDefault();
            pictureBoxSlot.Click += PictureBoxSlot_Click;
            Visual = false;
        }

        public override void ToDefault()
        {
            ActiveImage = Params.GetSlotImage(out string imageName);
            pictureBoxSlot.Image = new Bitmap(ActiveImage);
            ImageName = imageName;
            pictureBoxSelection.Image = new Bitmap(Params.GetSelectionImage(out imageName));
            ImageBlend.Blend(pictureBoxSlot.Image, pictureBoxSelection.Image);
            Refresh();
        }

        private void PictureBoxSlot_Click(object sender, EventArgs e)
        {
            SelectControl();
            pictureBoxSelection.Visible = true;
        }

        public override void FillPropPanel(Panel propPanel)
        {
            ClearPropPanel(propPanel);
            FillName(propPanel);
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

            Label _visual = new Label();
            _visual.Location = new Point(0, elementY += elementSpacing);
            _visual.Size = new Size(102, elementSpacing);
            _visual.Text = "Визуальный элемент";
            propPanel.Controls.Add(_visual);

            CheckBox _visualCheck = new CheckBox();
            _visualCheck.Location = new Point(103, elementY);
            _visualCheck.Size = new Size(101, elementSpacing);
            _visualCheck.Checked = Visual;
            _visualCheck.CheckedChanged += (sender, e) => { Visual = ((CheckBox)sender).Checked; };
            propPanel.Controls.Add(_visualCheck);

            Label _transpBg = new Label();
            _transpBg.Location = new Point(0, elementY += elementSpacing);
            _transpBg.Size = new Size(102, elementSpacing);
            _transpBg.Text = "Прозрачный фон";
            propPanel.Controls.Add(_transpBg);

            CheckBox _transpBgCheck = new CheckBox();
            _transpBgCheck.Location = new Point(103, elementY);
            _transpBgCheck.Size = new Size(101, elementSpacing);
            _transpBgCheck.Checked = TransparentBg;
            _transpBgCheck.CheckedChanged += (sender, e) => { TransparentBg = ((CheckBox)sender).Checked; };
            propPanel.Controls.Add(_transpBgCheck);

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

        internal void SetSelection(Image selectionDefaultImage)
        {
            pictureBoxSelection.Image = new Bitmap(selectionDefaultImage);
            ImageBlend.Blend(pictureBoxSlot.Image, pictureBoxSelection.Image);
            Refresh();
        }

        internal void Apply(string name, int x, int y, int size, bool visual, string imageName, string clicker)
        {
            if(name!="")elementName = name;
            Location = new Point(x, y);
            ResizeAll(new Size(size, size));
            Visual = visual;
            if(ImageName != imageName)
            {
                ImageName = imageName;
                string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + imageName + ".png";
                ApplyImage(path);                
            }
            Clicker = clicker;
        }

        public override void CountScale(char axis, int distance)
        {
            float scale = (float)distance / GetHeight();
            if (Location.X + scale + Global.panelWorkspace.AutoScrollPosition.X > Global.X || Location.Y + scale + Global.panelWorkspace.AutoScrollPosition.Y > Global.Y) return;
            Point oldLocation = Location;
            this.Scale(new SizeF(scale, scale));
            Location = oldLocation;
        }

        private void ApplyImage(string path)
        {
            Bitmap bitmap = CreateBitmap(path);
            ActiveImage = bitmap;
            pictureBoxSlot.Image = new Bitmap(ActiveImage);
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
                ToDefault();
                return (Bitmap)ActiveImage;
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

            SizeF sizeF = new SizeF((float)size/GetWidth(), (float)size / GetWidth());
            Point oldLocation = Location;
            Scale(sizeF);
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
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }

            ImageName = openFileDialog1.SafeFileName;
            ApplyImage(openFileDialog1.FileName);
            SetSelection(Params.GetSelectionImage(out string a));
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
            if(x < Global.panelWorkspace.AutoScrollPosition.X || x > Global.X - Size.Width)
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
            if( y < Global.panelWorkspace.AutoScrollPosition.Y || y > Global.Y - Size.Width)
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
            element += "x: " + (Location.X - Global.panelWorkspace.AutoScrollPosition.X) + ',';
            element += "y: " + (Location.Y - Global.panelWorkspace.AutoScrollPosition.Y) + ',';
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

        internal override void SelectControl()
        {
            base.SelectControl();
            pictureBoxSelection.Visible = true;
        }

        internal override void DeselectControl()
        {
            base.DeselectControl();
            pictureBoxSelection.Visible = false;
        }
    }
}
