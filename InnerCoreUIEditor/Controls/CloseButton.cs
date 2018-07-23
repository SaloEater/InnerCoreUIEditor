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

        public Image activeImage { get; set; }
        public Image activeImage2 { get; set; }
        public string Clicker { get; set; }
        public string activeImageName { get; set; }
        public string activeImage2Name { get; set; }
        public Point oldLocation { get; set; }
        private bool global { get; set; }

        private int TimerSec;

        public CloseButton(ExplorerPainter explorerPainter, Params _params, InnerTabPage parentTabPage) : base(explorerPainter, _params, parentTabPage)
        {
            InitializeComponent();
            Initialization();
        }

        public void Initialization()
        {
            ControlEditor.Init(pictureBox1, this, parentTabPage);
            pictureBox1.Dock = DockStyle.Fill;
            activeImage = _params.GetCloseButtonImage(out string image);
            activeImageName = image;
            ApplyImage(activeImage);
            pictureBox1.Click += button_Click;
            activeImage2 = _params.GetCloseButton2Image(out image);
            activeImage2Name = image;
            scale = parentTabPage.globalScale;
            ChangeScale(scale);
            BringToFront();
        }

        internal override InnerControl MakeCopy()
        {
            if (constant || hidden) throw new ArgumentOutOfRangeException();
            CloseButton control = new CloseButton(explorerPainter, _params, parentTabPage);
            control.Location = Location;
            control.Size = Size;
            control.Visible = Visible;
            control.scale = scale;
            control.originSize = originSize;
            control.oldLocation = oldLocation;
            control.originSize = originSize;

            control.activeImage = activeImage;
            control.activeImage2 = activeImage2;
            control.Clicker = Clicker;
            control.activeImageName = activeImageName;
            control.activeImage2Name = activeImage2Name;
            control.global = global;
            control.ApplyImage(control.activeImage);

            return control;
        }

        private void ApplyImage(Image activeImage)
        {
            originSize = activeImage.Size;
            ChangeControlSize(originSize);
            pictureBox1.Image = new Bitmap(activeImage);
            ChangeScale(scale);
            Console.Write(Size);
        }

        private void ChangeControlSize(Size originSize)
        {
            foreach (Control c in Controls)
            {
                c.Size = originSize;
            }
            Size = originSize;
        }

        public override void ChangeScale(float scale)
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
            pictureBox.Image = new Bitmap(activeImage2);
            TimerSec = 0;
            timer1.Start();
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

            Label _activeImage = new Label();
            _activeImage.Location = new Point(0, elementY += elementSpacing);
            _activeImage.Size = new Size(102, elementSpacing);
            _activeImage.Text = "Не нажатая";
            propPanel.Controls.Add(_activeImage);

            TextBox _activeImagePath = new TextBox();
            _activeImagePath.Location = new Point(103, elementY);
            _activeImagePath.Size = new Size(100, elementSpacing);
            _activeImagePath.Text = activeImageName;
            _activeImagePath.LostFocus += new EventHandler(_imagePicPath_LostFocus);
            _activeImagePath.ReadOnly = true;

            Button openFileDialog = new Button();
            openFileDialog.Size = new Size(_activeImagePath.Size.Height / 5 * 4, _activeImagePath.Size.Height / 5 * 4);
            openFileDialog.Location = new Point(_activeImagePath.Size.Width - _activeImagePath.Size.Height, 0);
            openFileDialog.Click += new EventHandler(openFileDialog_Click);
            _activeImagePath.Controls.Add(openFileDialog);

            propPanel.Controls.Add(_activeImagePath);

            Label _image = new Label();
            _image.Location = new Point(0, elementY += elementSpacing);
            _image.Size = new Size(102, elementSpacing);
            _image.Text = "Нажатая";
            propPanel.Controls.Add(_image);

            TextBox _imagePicPath = new TextBox();
            _imagePicPath.Location = new Point(103, elementY);
            _imagePicPath.Size = new Size(100, elementSpacing);
            _imagePicPath.Text = activeImage2Name;
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

        internal void SetImage(Image closeButtonDefaultImage, string closeButtonImageName, Image closeButton2DefaultImage, string closeButton2ImageName)
        {
            activeImage = closeButtonDefaultImage;
            pictureBox1.Image = activeImage;
            activeImageName = closeButtonImageName;

            activeImage2 = closeButton2DefaultImage;
            activeImage2Name = closeButton2ImageName;

            Refresh();
        }

        internal void Apply(string name, int x, int y, float scale, string activeImage2Name, string activeImageName, string clicker)
        {
            if(name!="")elementName = name;
            Location = new Point(x, y);
            ChangeScale(scale);

            this.activeImageName = activeImageName;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + activeImageName + ".png";
            ApplyImage(path);

            this.activeImage2Name = activeImage2Name;
            path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + activeImage2Name + ".png";
            activeImage2 = CreateBitmap(path, out bool success);
            if (!success)
            {
                activeImage2 = _params.GetCloseButton2Image(out string _name);
                activeImage2Name = _name;
            }

            Clicker = clicker;
        }

        private void ApplyImage(string path)
        {
            activeImage = CreateBitmap(path, out bool success);
            if(!success)
            {
                activeImage = _params.GetCloseButtonImage(out string name);
                activeImageName = name;
            }
            ApplyImage(activeImage);
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
                return null;
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
            openFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }

            activeImageName = openFileDialog1.SafeFileName;
            ApplyImage(openFileDialog1.FileName);
            FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        private void OpenFileDialog2_Click(object sender, EventArgs e)
        {
            if (constant) return;
            openFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }

            activeImage2Name = openFileDialog1.SafeFileName;
            activeImage2 = Image.FromFile(openFileDialog1.FileName);
            FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        internal override string MakeOutput()
        {
            string element = "\n\t";
            element += '\"' + elementName + "\": \n\t{";
            element += "\n\t\ttype: \"closeButton\",";
            element += "\n\t\tx: " + (Location.X - parentTabPage.GetDesktopPanel().AutoScrollPosition.X) + ',';
            element += "\n\t\ty: " + (Location.Y - parentTabPage.GetDesktopPanel().AutoScrollPosition.Y) + ',';
            element += "\n\t\tscale: " + scale.ToString().Replace(',', '.') + ',';
            if(activeImageName!=_params.closeButtonImageName)element += "\n\t\tbitmap: \"" + activeImageName.Split('.')[0] + "\",";
            if (activeImage2Name != _params.closeButton2ImageName) element += "\n\t\tbitmap2: \"" + activeImage2Name.Split('.')[0] + "\",";
            if (global) element += "\n\t\tglobal: true,";
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimerSec++;
            if(TimerSec==1)
            {
                pictureBox1.Image = new Bitmap(activeImage);
                timer1.Stop();
            }
        }
    }
}
