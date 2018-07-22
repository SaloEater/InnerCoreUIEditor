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
    public partial class Scale : InnerControl
    {
        public Image activeImage { get; set; }
        public string Clicker { get; set; }
        public string imageName { get; set; }
        public Point oldLocation { get; set; }

        private Image scaledActiveImage;

        private bool invert;
        private int chosenItem;

        //Добавить привязку к заднему фону

        public Image overlayImage;
        public string overlayImageName;
        public float overlayScale;
        public Size overlayOriginSize { get; set; }
        public bool overlayEnabled { get; set; }

        private bool offsetXTextChanged;
        private bool offsetYTextChanged;
        private bool overlayScaleTextChanged;

        public Scale(ExplorerPainter explorerPainter, Params _params, InnerTabPage parentTabPage) : base(explorerPainter, _params, parentTabPage)
        {
            InitializeComponent();
            Initialization();           
        }

        public void Initialization()
        {
            pictureBox1.Click += PictureBox_Click;
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            pictureBox2.Click += PictureBox_Click;
            pictureBox2.VisibleChanged += PictureBox2_VisibleChanged;
            ControlEditor.Init(pictureBox1, this, parentTabPage);
            ControlEditor.Init(pictureBox2, this, parentTabPage);
            chosenItem = 0;

            overlayImageName = "";
            overlayImage = Resources._default_slot_empty;
            overlayImageName = "_default_slot_empty";
            overlayScale = 1;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            ApplyOverlayImage();

            scale = 1;
            activeImage = Resources._selection;
            imageName = "_selection.png";
            ApplyImage(activeImage);
        }

        private void PictureBox2_VisibleChanged(object sender, EventArgs e)
        {
            if(overlayEnabled)
            {
                ImageBlend.Blend(pictureBox1.Image, pictureBox2.Image);
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            SelectControl();
        }

        private void ApplyImage(Image activeImage)
        {
            originSize = activeImage.Size;
            //ChangeControlSize(originSize);
            ColorImagesToPanelColor();
            ChangeScale(scale);
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
            scaledActiveImage = ImageBlend.ResizeImage(pictureBox1.Image, (int)(originSize.Width * scale), (int)(originSize.Height * scale));
            pictureBox1.Image = scaledActiveImage;
            pictureBox2.Size = overlayImage.Size;
            if (overlayEnabled)
            {
                overlayScale *= scale;
                pictureBox2.Scale(new SizeF(overlayScale, overlayScale));
            }
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
            //pictureBox1.Image = new Bitmap(activeImage);
            pictureBox1.Image = ImageBlend.MergeWithPanel(parentTabPage.GetDesktopPanel(), new Bitmap(activeImage), new Point(Location.X + parentTabPage.GetDesktopPanel().AutoScrollPosition.X, Location.Y + parentTabPage.GetDesktopPanel().AutoScrollPosition.Y));
            if (overlayEnabled)
            {
                pictureBox2.Image = new Bitmap(overlayImage);
                ImageBlend.Blend(pictureBox1.Image, pictureBox2.Image);
            }
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

            Label _originImage = new Label();
            _originImage.Location = new Point(0, elementY += elementSpacing);
            _originImage.Size = new Size(102, elementSpacing);
            _originImage.Text = "Маска";
            propPanel.Controls.Add(_originImage);

            Button openFileDialogOriginImage = new Button();
            openFileDialogOriginImage.Location = new Point(103, elementY);
            openFileDialogOriginImage.Size = new Size(elementSpacing, elementSpacing);
            openFileDialogOriginImage.Click += OpenFileDialogOriginImage_Click;
            propPanel.Controls.Add(openFileDialogOriginImage);

            Label _invert = new Label();
            _invert.Location = new Point(0, elementY += elementSpacing);
            _invert.Size = new Size(102, elementSpacing);
            _invert.Text = "Инверсия";
            propPanel.Controls.Add(_invert);

            CheckBox _invertCheck = new CheckBox();
            _invertCheck.Location = new Point(103, elementY);
            _invertCheck.Size = new Size(101, elementSpacing);
            _invertCheck.Checked = invert;
            _invertCheck.CheckedChanged += (sender, e) => { invert = ((CheckBox)sender).Checked; };
            propPanel.Controls.Add(_invertCheck);

            Label _side = new Label();
            _side.Location = new Point(0, elementY += elementSpacing);
            _side.Size = new Size(102, elementSpacing);
            _side.Text = "Сторона";
            propPanel.Controls.Add(_side);

            ComboBox comboBox = new ComboBox();
            comboBox.Location = new Point(103, elementY);
            comboBox.Size = new Size(100, elementSpacing);
            comboBox.Items.Add("Направо");
            comboBox.Items.Add("Вверх");
            comboBox.Items.Add("Налево");
            comboBox.Items.Add("Вниз");
            comboBox.Text = comboBox.Items[chosenItem].ToString();
            comboBox.TextChanged += ComboBox_TextChanged;
            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            propPanel.Controls.Add(comboBox);

            Label _slider = new Label();
            _slider.Location = new Point(0, elementY += elementSpacing);
            _slider.Size = new Size(203, elementSpacing);
            _slider.Text = "Демонстрация";
            propPanel.Controls.Add(_slider);

            TrackBar trackBar = new TrackBar();
            trackBar.Location = new Point(0, elementY+= elementSpacing);
            trackBar.Size = new Size(203, 2*elementSpacing);
            trackBar.Maximum = 100;
            trackBar.Minimum = 1;
            trackBar.Value = 100;
            trackBar.ValueChanged += TrackBar_ValueChanged;
            trackBar.LostFocus += (sender, e) => { ((TrackBar)sender).Value = 100; pictureBox1.Location = new Point(0, 0); };
            propPanel.Controls.Add(trackBar);

            Label _overlayEnabled = new Label();
            _overlayEnabled.Location = new Point(0, elementY += 3*elementSpacing);
            _overlayEnabled.Size = new Size(151, elementSpacing);
            _overlayEnabled.Text = "Дополнительный слой";
            propPanel.Controls.Add(_overlayEnabled);

            CheckBox _visualCheck = new CheckBox();
            _visualCheck.Location = new Point(152, elementY);
            _visualCheck.Size = new Size(51, elementSpacing);
            _visualCheck.Checked = overlayEnabled;
            _visualCheck.CheckedChanged += (sender, e) => { overlayEnabled = ((CheckBox)sender).Checked; pictureBox2.Visible = overlayEnabled; };
            propPanel.Controls.Add(_visualCheck);

            Label _offset = new Label();
            _offset.Location = new Point(0, elementY += elementSpacing);
            _offset.Size = new Size(204, elementSpacing);
            _offset.Text = "Сдвиг";
            propPanel.Controls.Add(_offset);

            Label _offsetX = new Label();
            _offsetX.Location = new Point(0, elementY += elementSpacing);
            _offsetX.Size = new Size(51, elementSpacing);
            _offsetX.Text = "X";
            propPanel.Controls.Add(_offsetX);

            TextBox _offsetXValue = new TextBox();
            _offsetXValue.Location = new Point(52, elementY);
            _offsetXValue.Size = new Size(151, elementSpacing);
            _offsetXValue.Text = pictureBox2.Location.X.ToString();
            _offsetXValue.LostFocus += new EventHandler(_offsetXValue_LostFocus);
            _offsetXValue.KeyDown += _offsetXValue_KeyDown;
            _offsetXValue.TextChanged += (sender, e) => { offsetXTextChanged = true; };
            propPanel.Controls.Add(_offsetXValue);

            Label _offsetY = new Label();
            _offsetY.Location = new Point(0, elementY += elementSpacing);
            _offsetY.Size = new Size(51, elementSpacing);
            _offsetY.Text = "Y";
            propPanel.Controls.Add(_offsetY);

            TextBox _offsetYValue = new TextBox();
            _offsetYValue.Location = new Point(52, elementY);
            _offsetYValue.Size = new Size(151, elementSpacing);
            _offsetYValue.Text = pictureBox2.Location.Y.ToString();
            _offsetYValue.LostFocus += new EventHandler(_offsetYValue_LostFocus);
            _offsetYValue.KeyDown += _offsetYValue_KeyDown;
            _offsetYValue.TextChanged += (sender, e) => { offsetYTextChanged = true; };
            propPanel.Controls.Add(_offsetYValue);

            Label _overlayImage = new Label();
            _overlayImage.Location = new Point(0, elementY += elementSpacing);
            _overlayImage.Size = new Size(102, elementSpacing);
            _overlayImage.Text = "Изображение";
            propPanel.Controls.Add(_overlayImage);

            TextBox _overlayImagePath = new TextBox();
            _overlayImagePath.Location = new Point(103, elementY);
            _overlayImagePath.Size = new Size(100, elementSpacing);
            _overlayImagePath.Text = overlayImageName;
            _overlayImagePath.LostFocus += new EventHandler(__overlayImagePath_LostFocus);
            _overlayImagePath.ReadOnly = true;

            Button openFileDialog2 = new Button();
            openFileDialog2.Size = new Size(_overlayImagePath.Size.Height / 5 * 4, _overlayImagePath.Size.Height / 5 * 4);
            openFileDialog2.Location = new Point(_overlayImagePath.Size.Width - _overlayImagePath.Size.Height, 0);
            openFileDialog2.Click += new EventHandler(openFileDialog2_Click);
            _overlayImagePath.Controls.Add(openFileDialog2);

            propPanel.Controls.Add(_overlayImagePath);

            Label _overlaySize = new Label();
            _overlaySize.Location = new Point(0, elementY += elementSpacing);
            _overlaySize.Size = new Size(51, elementSpacing);
            _overlaySize.Text = "Размер";
            propPanel.Controls.Add(_overlaySize);

            TextBox _overlaySizeValue = new TextBox();
            _overlaySizeValue.Location = new Point(52, elementY);
            _overlaySizeValue.Size = new Size(151, elementSpacing);
            _overlaySizeValue.Text = overlayScale.ToString();
            _overlaySizeValue.LostFocus += _overlaySizeValue_LostFocus;
            _overlaySizeValue.KeyDown += _overlaySizeValue_KeyDown;
            _overlaySizeValue.TextChanged += (sender, e) => { overlayScaleTextChanged = true; };
            propPanel.Controls.Add(_overlaySizeValue);

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

        private void OpenFileDialogOriginImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }
            Bitmap image = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
            ApplyMask(image);
            Refresh();
        }

        public void ApplyMask(Bitmap image)
        {
            ImageBlend.MergeWithPanel(parentTabPage.GetDesktopPanel(), new Bitmap(image), new Point(Location.X + parentTabPage.GetDesktopPanel().AutoScrollPosition.X, Location.Y + parentTabPage.GetDesktopPanel().AutoScrollPosition.Y));
            //ImageBlend.Blend(image, pictureBox1.Image);
            BackgroundImage = image;
        }

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar trackBar = (TrackBar)sender;
            int value = trackBar.Value;
            if (!invert)
            {
                switch (chosenItem)
                {
                    case 0:
                        pictureBox1.Width = pictureBox1.Image.Size.Width * value / 100;
                        break;

                    case 1:
                        pictureBox1.Top = Height - Height * value / 100;
                        pictureBox1.Image = ImageBlend.CropVertical(scaledActiveImage, value);
                        break;

                    case 2:
                        pictureBox1.Left = Width - Width * value / 100;
                        pictureBox1.Image = ImageBlend.CropHorizontal(scaledActiveImage, value);

                        break;

                    case 3:
                        pictureBox1.Height = pictureBox1.Image.Size.Height * value / 100;
                        break;
                }
            } else
            {
                switch (chosenItem)
                {
                    case 0:
                        pictureBox1.Location = new Point(-Width* (100 - value) / 100, pictureBox1.Location.Y);
                        break;

                    case 1:
                        pictureBox1.Location = new Point(pictureBox1.Location.X, Height * (100 - value) / 100);
                        break;

                    case 2:
                        pictureBox1.Location = new Point(Width * (100-value) / 100, pictureBox1.Location.Y);

                        break;

                    case 3:
                        pictureBox1.Location = new Point(pictureBox1.Location.X, -Height * (100-value)   / 100);
                        break;
                }
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.Text = (string)comboBox.Items[chosenItem];
            
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {   
            ComboBox comboBox = (ComboBox)sender;
            if(comboBox.SelectedIndex!=-1)
                chosenItem = comboBox.SelectedIndex;
            comboBox.Text = (string)comboBox.Items[chosenItem];
            
        }

        internal override void SelectControl()
        {
            base.SelectControl();
        }

        private void _overlaySizeValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            if (!overlayEnabled) return;
            if (!overlayScaleTextChanged) return;
            overlayScaleTextChanged = false;
            TextBox textBox = (TextBox)sender;
            float scale;
            if (!float.TryParse(textBox.Text, out scale))
            {
                textBox.Text = overlayScale.ToString();
                return;
            }
            overlayScale = scale;
            pictureBox2.Scale(new SizeF(overlayScale, overlayScale));
            if(overlayEnabled) ImageBlend.Blend(pictureBox1.Image, pictureBox2.Image);
        }

        private void _overlaySizeValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (!overlayEnabled) return;
            if (e.KeyCode == Keys.Enter)
            {
                _overlaySizeValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _offsetYValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (!overlayEnabled) return;
            if (e.KeyCode == Keys.Enter)
            {
                _offsetYValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _offsetYValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            if (!overlayEnabled) return;
            if (!offsetYTextChanged) return;
            offsetYTextChanged = false;
            TextBox textBox = (TextBox)sender;
            int y;
            if (!int.TryParse(textBox.Text, out y))
            {
                textBox.Text = pictureBox2.Top.ToString();
                return;
            }
            pictureBox2.Location = new Point(pictureBox2.Location.X, y);
        }

        private void _offsetXValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (!overlayEnabled) return;
            if (e.KeyCode == Keys.Enter)
            {
                _offsetXValue_LostFocus(sender, null);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void _offsetXValue_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            if (!overlayEnabled) return;
            if (!offsetXTextChanged) return;
            offsetXTextChanged = false;
            TextBox textBox = (TextBox)sender;
            int x;
            if (!int.TryParse(textBox.Text, out x))
            {
                textBox.Text = pictureBox2.Left.ToString();
                return;
            }
            pictureBox2.Location = new Point(x, pictureBox2.Location.Y);
        }

        private void __overlayImagePath_LostFocus(object sender, EventArgs e)
        {
            if (constant) return;
            if (!overlayEnabled) return;

        }

        private void openFileDialog2_Click(object sender, EventArgs e)
        {
            if (constant) return;
            if (!overlayEnabled) return;
            openFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName == "") return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }
            overlayImageName = openFileDialog1.SafeFileName;
            overlayImage = Bitmap.FromFile(openFileDialog1.FileName);
            ApplyOverlayImage();
            FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        private void ApplyOverlayImage()
        {
            pictureBox2.Size = overlayImage.Size;
            pictureBox2.Image = new Bitmap(overlayImage);
            pictureBox2.Scale(new SizeF(overlayScale, overlayScale));
            ImageBlend.Blend(pictureBox1.Image, pictureBox2.Image);
            this.Refresh();
        }

        internal void Apply(string name, int x, int y, float scale, string imageName, int side, bool invert, string overlayImageName, Point overlayOffset, float overlayScale, string clicker)
        {
            if (name != "") elementName = name;
            Location = new Point(x, y);
            ChangeScale(scale);

            this.imageName = imageName;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + imageName + ".png";
            ApplyImage(path);
            pictureBox1.Image = ImageBlend.MergeWithPanel(parentTabPage.GetDesktopPanel(), new Bitmap(pictureBox1.Image), new Point(Location.X + parentTabPage.GetDesktopPanel().AutoScrollPosition.X, Location.Y + parentTabPage.GetDesktopPanel().AutoScrollPosition.Y));

            this.overlayImageName = overlayImageName;
            path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + overlayImageName + ".png";
            this.overlayScale = overlayScale;
            try
            {
                overlayImage = Bitmap.FromFile(path);
            } catch(ArgumentException)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                overlayImage = Resources._default_slot_empty;
                overlayImageName = "_default_slot_empty.png";
            }
            overlayEnabled = true;
            pictureBox2.Visible = true;
            pictureBox2.Size = overlayImage.Size;
            pictureBox2.Image = new Bitmap(overlayImage);
            pictureBox2.Scale(new SizeF(overlayScale, overlayScale));
            pictureBox2.Location = overlayOffset;
            ImageBlend.Blend(pictureBox1.Image, pictureBox2.Image);
            chosenItem = side;
            this.invert = invert;
            Clicker = clicker;
        }

        internal void Apply(string name, int x, int y, float scale, string imageName, int side, bool invert, string clicker)
        {
            if(name!="")elementName = name;
            Location = new Point(x, y);
            ChangeScale(scale);

            this.imageName = imageName;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + imageName + ".png";
            ApplyImage(path);
            chosenItem = side;
            this.invert = invert;

            Clicker = clicker;
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
            pictureBox1.Image = ImageBlend.MergeWithPanel(parentTabPage.GetDesktopPanel(), new Bitmap(pictureBox1.Image), new Point(Location.X + parentTabPage.GetDesktopPanel().AutoScrollPosition.X, Location.Y + parentTabPage.GetDesktopPanel().AutoScrollPosition.Y));
        }

        internal override string MakeOutput()
        {
            string element = "\n\t";
            element += '\"' + elementName + "\": \n\t{";
            element += "\n\t\ttype: \"scale\",";
            element += "\n\t\tx: " + (Location.X - parentTabPage.GetDesktopPanel().AutoScrollPosition.X) + ',';
            element += "\n\t\ty: " + (Location.Y - parentTabPage.GetDesktopPanel().AutoScrollPosition.Y) + ',';
            element += "\n\t\tscale: " + scale.ToString().Replace(',', '.') + ',';
            element += "\n\t\tbitmap: \"" + imageName.Split('.')[0] + "\",";
            element += "\n\t\tinvert: " + invert.ToString().ToLower() + ',';
            element += "\n\t\tdirection: " + chosenItem + ',';
            if (overlayEnabled)
            {
                element += "\n\t\toverlay: \"" + overlayImageName.Split('.')[0] + "\",";
                element += "\n\t\toverlay_scale: " + overlayScale.ToString().Replace(',', '.') + ',';
                element += "\n\t\toverlayOffset: { x:" + pictureBox2.Left + " , y: " + pictureBox2.Top + "},";
            }

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
