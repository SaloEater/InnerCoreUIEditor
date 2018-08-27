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
using System.Globalization;

namespace InnerCoreUIEditor
{
    public partial class InnerImage : InnerControl
    {
        public Image activeImage { get; set; }
        public string imageName { get; set; }
        public Point oldLocation { get; set; }

        public Image overlayImage;
        public string overlayImageName;
        public float overlayScale;
        public Size overlayOriginSize { get; set; }
        public bool overlayEnabled { get; set; }

        private bool offsetXTextChanged;
        private bool offsetYTextChanged;
        private bool overlayScaleTextChanged;

        public InnerImage(InnerTabPage parentTabPage) : base(parentTabPage)
        {
            InitializeComponent();
            Initialization();           
        }

        internal override void ApplyChanges(int type, object value)
        {
            if (type < 4) base.ApplyChanges(type, value);
            switch (type)
            {
                case 4:
                    overlayEnabled = (Boolean)value;
                    pictureBox2.Visible = overlayEnabled;
                    break;

                case 5:
                    pictureBox2.Scale(new SizeF((float)value, (float)value));
                    break;

                case 6:
                    pictureBox2.Location = (Point)value;
                    break;
            }
            FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        internal override ActionStack MakeSnapshot(int type)
        {
            /*
             * 4 - overlayEnabled
             * 5 - overlayScale
             * 6 - overlayLocation
             * 
             */
            if (type < 4) return base.MakeSnapshot(type);
            ActionStack action = null;
            switch (type)
            {
                case 4:
                    action = new ActionStack(this, 4, overlayEnabled);
                    break;

                case 5:
                    action = new ActionStack(this, 5, overlayScale);
                    break;

                case 6:
                    action = new ActionStack(this, 6, pictureBox2.Location);
                    break;
            }
            return action;
        }

        public void Initialization()
        {
            pictureBox1.Click += PictureBox_Click;
            pictureBox2.Click += PictureBox_Click;
            pictureBox2.VisibleChanged += PictureBox2_VisibleChanged;
            ControlEditor.Init(pictureBox1, this, parentTabPage);
            ControlEditor.Init(pictureBox2, this, parentTabPage);

            overlayImageName = "";
            overlayImage = Resources._default_slot_empty;
            overlayImageName = "_default_slot_empty";
            overlayScale = 1;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            ApplyOverlayImage();
            activeImage = Resources._selection;
            imageName = "_selection.png";

            ApplyImage(activeImage);
            scale = parentTabPage.globalScale;
            ChangeScale(scale);
        }

        internal override InnerControl MakeCopy()
        {
            if (constant || hidden) throw new ArgumentOutOfRangeException();
            InnerImage control = new InnerImage(parentTabPage);
            control.Location = Location;
            control.Size = Size;
            control.Visible = Visible;
            control.scale = scale;
            control.originSize = originSize;
            control.oldLocation = oldLocation;

            control.activeImage = activeImage;
            control.imageName = imageName;
            control.overlayImage = overlayImage;
            control.overlayImageName = overlayImageName;
            control.overlayScale = overlayScale;
            control.overlayOriginSize = overlayOriginSize;
            control.overlayEnabled = overlayEnabled;
            control.ApplyImage(control.activeImage);

            return control;
        }

        private void PictureBox2_VisibleChanged(object sender, EventArgs e)
        {
            if (overlayEnabled)
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
            ChangeControlSize(originSize);
            pictureBox1.Image = activeImage;
            pictureBox2.Image = overlayImage;
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
            pictureBox1.Image = ImageBlend.ResizeImage(pictureBox1.Image, Width, Height);
            pictureBox2.Size = overlayImage.Size;
            if (overlayEnabled)
            {
                overlayScale *= scale;
                pictureBox2.Scale(new SizeF(overlayScale, overlayScale));
                pictureBox2.Image = ImageBlend.ResizeImage(pictureBox2.Image, pictureBox2.Width, pictureBox2.Height);
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

            Label _overlayEnabled = new Label();
            _overlayEnabled.Location = new Point(0, elementY += elementSpacing);
            _overlayEnabled.Size = new Size(151, elementSpacing);
            _overlayEnabled.Text = "Дополнительный слой";
            propPanel.Controls.Add(_overlayEnabled);

            CheckBox _ovEnCheck = new CheckBox();
            _ovEnCheck.Location = new Point(152, elementY);
            _ovEnCheck.Size = new Size(51, elementSpacing);
            _ovEnCheck.Checked = overlayEnabled;
            _ovEnCheck.CheckedChanged += (sender, e) => { overlayEnabled = ((CheckBox)sender).Checked; pictureBox2.Visible = overlayEnabled; };
            propPanel.Controls.Add(_ovEnCheck);

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



            Label _clicker = new Label();
            _clicker.Location = new Point(0, elementY += elementSpacing);
            _clicker.Size = new Size(102, elementSpacing);
            _clicker.Text = "Клик по объекту";
            propPanel.Controls.Add(_clicker);

            Button clickerForm = new Button();
            clickerForm.Location = new Point(103, elementY);
            clickerForm.Size = new Size(elementSpacing, elementSpacing);
            clickerForm.Click += ClickerForm_Click;
            clickerForm.BackgroundImage = Resources.expandIcon;
            clickerForm.BackgroundImageLayout = ImageLayout.Stretch;
            propPanel.Controls.Add(clickerForm);
            base.FillPropPanel(propPanel);
        }

        private void ClickerForm_Click(object sender, EventArgs e)
        {
            ClickerInput clickerInput = new ClickerInput(parentTabPage);
            clickerInput.Location = MousePosition;
            clickerInput.Show();
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
            if (!float.TryParse(textBox.Text, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out scale))
            {
                textBox.Text = overlayScale.ToString();
                return;
            }
            overlayScale = scale;
            pictureBox2.Scale(new SizeF(overlayScale, overlayScale));
            if (overlayEnabled) ImageBlend.Blend(pictureBox1.Image, pictureBox2.Image);
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
        }

        internal void Apply(string name, int x, int y, float scale, string imageName, string overlayImageName, Point overlayOffset, float overlayScale, string clicker)
        {
            if (name != "") elementName = name;
            Location = new Point(x, y);
            ChangeScale(scale);

            this.imageName = imageName;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + imageName + ".png";
            ApplyImage(path);

            this.overlayImageName = overlayImageName;
            path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + overlayImageName + ".png";
            this.overlayScale = overlayScale;
            try
            {
                overlayImage = Bitmap.FromFile(path);
            } catch(Exception)
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
            ImageBlend.Blend(pictureBox1.Image, overlayImage);
            this.clicker = clicker;
        }

        internal void Apply(string name, int x, int y, float scale, string imageName, string clicker)
        {
            if(name!="")elementName = name;
            Location = new Point(x, y);
            ChangeScale(scale);

            this.imageName = imageName;
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + imageName + ".png";
            ApplyImage(path);

            this.clicker = clicker;
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
            }catch(ArgumentException)
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
            string element = "\n\t";
            element += '\"' + elementName + "\": \n\t{";
            element += "\n\t\ttype: \"image\",";
            element += "\n\t\tx: " + (Location.X - parentTabPage.GetDesktopPanel().AutoScrollPosition.X) + ',';
            element += "\n\t\ty: " + (Location.Y - parentTabPage.GetDesktopPanel().AutoScrollPosition.Y) + ',';
            element += "\n\t\tscale: " + scale.ToString().Replace(',', '.') + ',';
            element += "\n\t\tbitmap: \"" + imageName.Split('.')[0] + "\",";
            if(overlayEnabled)
            {
                element += "\n\t\toverlay: \"" + overlayImageName.Split('.')[0] + "\",";
                element += "\n\t\toverlay_scale: " + overlayScale.ToString().Replace(',', '.') + ',';
                element += "\n\t\toverlayOffset: { x:" + pictureBox2.Left + " , y: " + pictureBox2.Top + "},";
            }
            if (clicker != "") element += "\n\t\tclicker: " + clicker + ',';
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
