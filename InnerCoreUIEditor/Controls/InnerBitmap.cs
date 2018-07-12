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
    public partial class InnerBitmap : InnerControl
    {
        public Image activeImage { get; set; }
        public string imageName { get; set; }
        public float scale { get; set; }
        public Size originSize { get; set; }
        public Point oldLocation { get; set; }

        private bool scaleTextChanged;
        private bool XTextChanged;
        private bool YTextChanged;

        public InnerBitmap()
        {
            InitializeComponent();
            Initialization();           
        }

        public void Initialization()
        {
            pictureBox1.Click += PictureBox_Click;
            ControlEditor.Init(pictureBox1, this);

            scale = 1;
            activeImage = Resources._selection;
            imageName = "_selection.png";
            ApplyImage(activeImage);
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            SelectControl();
        }

        private void ApplyImage(Image activeImage)
        {
            originSize = activeImage.Size;
            ChangeControlSize(originSize);
            ColorImagesToPanelColor();
            ChangeScale(scale);
            Console.Write(Size);
        }

        private void ChangeControlSize(Size originSize)
        {
            pictureBox1.Size = originSize;
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

        public override void ColorImagesToPanelColor()
        {
            pictureBox1.Image = ImageBlend.MergeWithPanel(new Bitmap(activeImage), new Point(Location.X+Global.panelWorkspace.AutoScrollPosition.X, Location.Y + Global.panelWorkspace.AutoScrollPosition.Y));
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

            Label _activeImage = new Label();
            _activeImage.Location = new Point(0, elementY += elementSpacing);
            _activeImage.Size = new Size(102, elementSpacing);
            _activeImage.Text = "Изображение";
            propPanel.Controls.Add(_activeImage);

            TextBox _activeImagePath = new TextBox();
            _activeImagePath.Location = new Point(103, elementY);
            _activeImagePath.Size = new Size(100, elementSpacing);
            _activeImagePath.Text = imageName;
            _activeImagePath.LostFocus += new EventHandler(_imagePicPath_LostFocus);
            _activeImagePath.ReadOnly = true;

            Button openFileDialog = new Button();
            openFileDialog.Size = new Size(_activeImagePath.Size.Height / 5 * 4, _activeImagePath.Size.Height / 5 * 4);
            openFileDialog.Location = new Point(_activeImagePath.Size.Width - _activeImagePath.Size.Height, 0);
            openFileDialog.Click += new EventHandler(openFileDialog_Click);
            _activeImagePath.Controls.Add(openFileDialog);

            propPanel.Controls.Add(_activeImagePath);            

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

        internal override void SelectControl()
        {
            base.SelectControl();
        }  
        
        internal void Apply(int x, int y, float scale, string imageName)
        {
            Location = new Point(x, y);
            ChangeScale(scale);

            this.imageName = imageName;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + imageName + ".png";
            ApplyImage(path);
        }

        private void ApplyImage(string path)
        {
            activeImage = CreateBitmap(path, out bool success);
            if(!success)
            {
                activeImage = Resources._selection;
                imageName = "_selection.png";
            }
            ApplyImage(activeImage);
        }

        private Bitmap CreateBitmap(string path, out  bool success)
        {
            Bitmap bitmap;
            try
            {
                bitmap = (Bitmap) Bitmap.FromFile(path);
            }catch(Exception)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                success = false;
                return Resources._selection;
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
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }

            imageName = openFileDialog1.SafeFileName;
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

        internal override string MakeOutput()
        {
            string element = "\n\t";
            element += '{';
            element += "type: \"bitmap\",";
            element += "x: " + (Location.X - Global.panelWorkspace.AutoScrollPosition.X) + ',';
            element += "y: " + (Location.Y - Global.panelWorkspace.AutoScrollPosition.Y) + ',';
            element += "scale: " + scale.ToString().Replace(',', '.') + ',';
            element += "bitmap: \"" + imageName.Split('.')[0] + "\",";
            element += '}';
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
