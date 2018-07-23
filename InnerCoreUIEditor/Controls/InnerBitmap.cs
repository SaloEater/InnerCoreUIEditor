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
        public Point oldLocation { get; set; }

        public InnerBitmap(ExplorerPainter explorerPainter, Params _params, InnerTabPage parentTabPage) : base(explorerPainter, _params, parentTabPage)
        {
            InitializeComponent();
            Initialization();           
        }

        public void Initialization()
        {
            pictureBox1.Click += PictureBox_Click;
            ControlEditor.Init(pictureBox1, this, parentTabPage);
            pictureBox1.Dock = DockStyle.Fill;
            activeImage = Resources._selection;
            imageName = "_selection.png";
            ApplyImage(activeImage);
            scale = parentTabPage.globalScale;
            ChangeScale(scale);
        }

        internal override InnerControl MakeCopy()
        {
            if (constant || hidden) throw new ArgumentOutOfRangeException();
            InnerBitmap control = new InnerBitmap(explorerPainter, _params, parentTabPage);
            control.oldLocation = oldLocation;
            control.Location = Location;
            control.Size = Size;
            control.Visible = Visible;
            control.scale = scale;
            control.originSize = originSize;

            control.activeImage = activeImage;
            control.imageName = imageName;
            control.ApplyImage(control.activeImage);

            return control;
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            SelectControl();
        }

        private void ApplyImage(Image activeImage)
        {
            originSize = activeImage.Size;
            ChangeControlSize(originSize);
            pictureBox1.Image = activeImage;
            ColorImagesToPanelColor();
            ChangeScale(scale);
            elementName = imageName;
            parentTabPage.ReloadExporer();
            SelectControl();
            Console.Write(Size);
        }

        private void ChangeControlSize(Size originSize)
        {
            pictureBox1.Size = originSize;
            Size = originSize;
        }

        public override void ChangeScale(float scale)
        {
            this.scale = scale;
            oldLocation = Location;
            ChangeControlSize(originSize);
            Scale(new SizeF(scale, scale));
            pictureBox1.Image = ImageBlend.ResizeImage(pictureBox1.Image, Width, Height);
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
            pictureBox1.Image = ImageBlend.MergeWithPanel(parentTabPage.GetDesktopPanel(), new Bitmap(activeImage), new Point(Location.X+parentTabPage.GetDesktopPanel().AutoScrollPosition.X, Location.Y + parentTabPage.GetDesktopPanel().AutoScrollPosition.Y));
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

            imageName = openFileDialog1.SafeFileName;
            ApplyImage(openFileDialog1.FileName);
            FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        internal override string MakeOutput()
        {
            string element = "\n\t{";
            element += "\n\t\ttype: \"bitmap\",";
            element += "\n\t\tx: " + (Location.X - parentTabPage.GetDesktopPanel().AutoScrollPosition.X) + ',';
            element += "\n\t\ty: " + (Location.Y - parentTabPage.GetDesktopPanel().AutoScrollPosition.Y) + ',';
            element += "\n\t\tscale: " + scale.ToString().Replace(',', '.') + ',';
            element += "\n\t\tbitmap: \"" + imageName.Split('.')[0] + "\",";
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
