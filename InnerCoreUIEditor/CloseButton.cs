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
    public partial class CloseButton : InnerControl
    {
        public Image UnpressedImage { get; set; }
        public Image PressedImage { get; set; }
        public string Clicker { get; set; }
        public string UnpressedImageName { get; set; }
        public string PressedImageName { get; set; }
        public float scale { get; set; }
        public Size originSize { get; set; }
        public Point oldLocation { get; set; }
        private bool global { get; set; }

        private int TimerSec;

        private bool scaleTextChanged;
        private bool XTextChanged;
        private bool YTextChanged;

        public CloseButton()
        {
            InitializeComponent();
            Initialization();           
        }

        public void Initialization()
        {
            scale = 1;
            ControlEditor.Init(pictureBox1, this);
            UnpressedImage = Resources.close_button_up;
            UnpressedImageName = "close_button_up.png";
            ApplyImage(UnpressedImage);
            pictureBox1.Click += button_Click;
            PressedImage = Resources.close_button_down;
            PressedImageName = "close_button_down.png";
        }

        private void ApplyImage(Image unpressedImage)
        {
            originSize = unpressedImage.Size;
            ChangeControlSize(originSize);
            pictureBox1.Image = unpressedImage;
            ChangeScale(scale);
            Console.Write(Size);
        }

        private void ChangeControlSize(Size originSize)
        {
            foreach(Control c in Controls)
            {
                c.Size = originSize;
            }
            Size = originSize;
        }

        private void ChangeScale(float scale)
        {
            this.scale = scale;
            oldLocation = Location;
            ChangeControlSize(originSize);
            Scale(new SizeF(scale, scale));
            Location = oldLocation;
        }

        public override void CountScale(char axis, int distance)
        {
            switch (axis)
            {
                case 'x':
                    {
                        scale = (float)distance / (float)originSize.Width;
                        ChangeScale(scale);
                        break;
                    }

                case 'y':
                    {
                        scale = (float)distance / (float)originSize.Height;
                        ChangeScale(scale);
                        break;
                    }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            SelectControl();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.Image = PressedImage;
            TimerSec = 0;
            timer1.Start();
        }

        public override void FillPropPanel(Panel propPanel)
        {
            if(!propPanelCleared)
            {
                ClearPropPanel(propPanel);
                FillName(propPanel);
            }
            Label _size = new Label();
            _size.Location = new Point(0, elementY);
            _size.Size = new Size(51, elementSpacing);
            _size.Text = "Размер";
            propPanel.Controls.Add(_size);

            TextBox _sizeValue = new TextBox();
            _sizeValue.Location = new Point(52, elementY);
            _sizeValue.Size = new Size(151, elementSpacing);
            _sizeValue.Text = scale.ToString();
            _sizeValue.LostFocus += _sizeValue_LostFocus;
            _sizeValue.KeyDown += _sizeValue_KeyDown;
            _sizeValue.TextChanged += (sender, e) => { scaleTextChanged = true; };
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

            Label _unpressedImage = new Label();
            _unpressedImage.Location = new Point(0, elementY += elementSpacing);
            _unpressedImage.Size = new Size(102, elementSpacing);
            _unpressedImage.Text = "Не нажатая";
            propPanel.Controls.Add(_unpressedImage);

            TextBox _unpressedImagePath = new TextBox();
            _unpressedImagePath.Location = new Point(103, elementY);
            _unpressedImagePath.Size = new Size(100, elementSpacing);
            _unpressedImagePath.Text = UnpressedImageName;
            _unpressedImagePath.LostFocus += new EventHandler(_imagePicPath_LostFocus);
            _unpressedImagePath.ReadOnly = true;

            Button openFileDialog = new Button();
            openFileDialog.Size = new Size(_unpressedImagePath.Size.Height / 5 * 4, _unpressedImagePath.Size.Height / 5 * 4);
            openFileDialog.Location = new Point(_unpressedImagePath.Size.Width - _unpressedImagePath.Size.Height, 0);
            openFileDialog.Click += new EventHandler(openFileDialog_Click);
            _unpressedImagePath.Controls.Add(openFileDialog);

            propPanel.Controls.Add(_unpressedImagePath);

            Label _image = new Label();
            _image.Location = new Point(0, elementY += elementSpacing);
            _image.Size = new Size(102, elementSpacing);
            _image.Text = "Нажатая";
            propPanel.Controls.Add(_image);

            TextBox _imagePicPath = new TextBox();
            _imagePicPath.Location = new Point(103, elementY);
            _imagePicPath.Size = new Size(100, elementSpacing);
            _imagePicPath.Text = PressedImageName;
            _imagePicPath.LostFocus += new EventHandler(_imagePicPath_LostFocus);
            _imagePicPath.ReadOnly = true;

            Button openFileDialog2 = new Button();
            openFileDialog2.Size = new Size(_imagePicPath.Size.Height / 5 * 4, _imagePicPath.Size.Height / 5 * 4);
            openFileDialog2.Location = new Point(_imagePicPath.Size.Width - _imagePicPath.Size.Height, 0);
            openFileDialog2.Click += OpenFileDialog2_Click;
            _imagePicPath.Controls.Add(openFileDialog2);

            propPanel.Controls.Add(_imagePicPath);

            Label _global = new Label();
            _global.Location = new Point(0, elementY += elementSpacing);
            _global.Size = new Size(102, elementSpacing);
            _global.Text = "Глобальная";
            propPanel.Controls.Add(_global);

            CheckBox _globalCheck = new CheckBox();
            _globalCheck.Location = new Point(103, elementY);
            _globalCheck.Size = new Size(101, elementSpacing);
            _globalCheck.Checked = global;
            _globalCheck.CheckedChanged += (sender, e) => { global = ((CheckBox)sender).Checked; };
            propPanel.Controls.Add(_globalCheck);

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

        internal void Apply(string name, int x, int y, float scale, string pressedImageName, string unpressedImageName, string clicker)
        {
            if(name!="")elementName = name;
            Location = new Point(x, y);
            ChangeScale(scale);

            UnpressedImageName = unpressedImageName;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + unpressedImageName + ".png";
            ApplyImage(path);

            PressedImageName = pressedImageName;
            path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + pressedImageName + ".png";
            PressedImage = CreateBitmap(path, out bool success);
            if (!success)
            {
                PressedImage = Resources.close_button_down;
                PressedImageName = "close_button_down.png";
            }

            Clicker = clicker;
        }

        private void ApplyImage(string path)
        {
            UnpressedImage = CreateBitmap(path, out bool success);
            if(!success)
            {
                UnpressedImage = Resources.close_button_up;
                UnpressedImageName = "close_button_up.png";
            }
            ApplyImage(UnpressedImage);
        }

        private Bitmap CreateBitmap(string path, out  bool success)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(path);
            }catch(ArgumentException)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                success = false;
                return Resources.default_frame_6;
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
            success = true;
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
            if (!scaleTextChanged) return;
            scaleTextChanged = false;
            TextBox textBox = (TextBox)sender;
            float scale;
            if (!float.TryParse(textBox.Text, out scale))
            {
                textBox.Text = scale.ToString();
                return;
            }

            if (scale*originSize.Height > Global.X || scale*originSize.Width > Global.Y)
            {
                textBox.Text = scale.ToString();
                return;
            }

            if (scale == this.scale) return;
            ChangeScale(scale);  
        }

        private void _imagePicPath_LostFocus(object sender, EventArgs e)
        {
            /*TextBox textBox = (TextBox)sender;
            if(textBox.Controls.Count!=0)textBox.Controls[0].Dispose();
            textBox.Controls.Clear();*/
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

            UnpressedImageName = openFileDialog1.SafeFileName;
            ApplyImage(openFileDialog1.FileName);
            FillPropPanel(Global.panelProperties);
        }

        private void OpenFileDialog2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }

            PressedImageName = openFileDialog1.SafeFileName;
            PressedImage = Image.FromFile(openFileDialog1.FileName);
            FillPropPanel(Global.panelProperties);
        }

        private void _coordsXValue_LostFocus(object sender, EventArgs e)
        {
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

        internal override string MakeOutput()
        {
            string element = "\n\t";
            element += '\"' + elementName + "\": {";
            element += "type: \"closeButton\",";
            element += "x: " + (Location.X - Global.panelWorkspace.AutoScrollPosition.X) + ',';
            element += "y: " + (Location.Y - Global.panelWorkspace.AutoScrollPosition.Y) + ',';
            element += "scale: " + scale.ToString().Replace(',', '.') + ',';
            element += "bitmap: \"" + UnpressedImageName.Split('.')[0] + "\",";
            element += "bitmap2: \"" + PressedImageName.Split('.')[0] + "\",";
            element += "global: " + global + ',';
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimerSec++;
            if(TimerSec==1)
            {
                pictureBox1.Image = UnpressedImage;
                timer1.Stop();
            }
        }
    }
}
