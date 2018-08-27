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
        public string ImageName { get; set; }

        private bool sizeTextChanged;

        public Slot(InnerTabPage parentTabPage) : base(parentTabPage)
        {
            InitializeComponent();
            Initialization();
        }

        public void Initialization()
        {
            ControlEditor.Init(pictureBoxSlot, this, parentTabPage);
            ControlEditor.Init(pictureBoxSelection, this, parentTabPage);
            pictureBoxSelection.SizeMode = PictureBoxSizeMode.StretchImage;
            ToDefault();
            pictureBoxSlot.Click += PictureBoxSlot_Click;
            Visual = false;
        }

        internal override void ApplyChanges(int type, object value)
        {
            if (type < 4) base.ApplyChanges(type, value);
            switch (type)
            {
                case 4:
                    Visual = (Boolean)value;
                    break;

                case 5:
                    TransparentBg = (Boolean)value;
                    break;
            }
            FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        internal override ActionStack MakeSnapshot(int type)
        {
            /*
             * 4 - visual
             * 5 - transparent
             */
            if (type < 4) return base.MakeSnapshot(type);
            ActionStack action = null;
            switch (type)
            {
                case 4:
                    action = new ActionStack(this, 4, Visual);
                    break;

                case 5:
                    action = new ActionStack(this, 5, TransparentBg);
                    break;
            }
            return action;
        }

        internal override InnerControl MakeCopy()
        {
            if (constant || hidden) throw new ArgumentOutOfRangeException();
            Slot control = new Slot(parentTabPage);
            control.Location = Location;
            control.Size = Size;
            control.Visible = Visible;
            control.scale = scale;
            control.originSize = originSize;
            control.originSize = originSize;

            control.ActiveImage = ActiveImage;
            control.ImageName = ImageName;
            control.Visual = Visual;
            control.TransparentBg = TransparentBg;
            control.ApplyImage();

            return control;
        }

        private void ApplyImage()
        {
            pictureBoxSlot.Image = ActiveImage;
        }

        public override void ToDefault()
        {
            ActiveImage = _params.GetSlotImage(out string imageName);
            pictureBoxSlot.Image = new Bitmap(ActiveImage);
            ImageName = imageName;
            pictureBoxSelection.Image = new Bitmap(_params.GetSelectionImage(out imageName));
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
            this.clicker = clicker;
        }

        public override void CountScale(char axis, int distance)
        {
            float scale = (float)distance / GetHeight();
            if (Location.X + scale + parentTabPage.GetDesktopPanel().AutoScrollPosition.X > parentTabPage.MaxX() || Location.Y + scale + parentTabPage.GetDesktopPanel().AutoScrollPosition.Y > parentTabPage.MaxY()) return;
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

        public override void _sizeValue_LostFocus(object sender, EventArgs e)
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

            if (size + Left < 0 || size + Left > parentTabPage.MaxX() || size + Top > parentTabPage.MaxY())
            {
                textBox.Text = Width.ToString();
                return;
            }

            if (textBox.Text == Size.Height.ToString()) return;

            SizeF sizeF = new SizeF((float)size/GetWidth(), (float)size / GetWidth());
            parentTabPage.SaveUndoAction(new ActionStack(this, 2, sizeF));
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
            openFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName.Split('.')[1] != "png")
            {
                MessageBox.Show("Нужно выбрать *.png файл");
                return;
            }

            ImageName = openFileDialog1.SafeFileName;
            ApplyImage(openFileDialog1.FileName);
            SetSelection(_params.GetSelectionImage(out string a));
            FillPropPanel(parentTabPage.GetPropertiesPanel());
        }

        internal override string MakeOutput()
        {
            string element = "\n\t";
            element += '\"' + elementName + "\": \n\t{";
            element += "\n\t\ttype: \"slot\",";
            element += "\n\t\tx: " + (Location.X - parentTabPage.GetDesktopPanel().AutoScrollPosition.X) + ',';
            element += "\n\t\ty: " + (Location.Y - parentTabPage.GetDesktopPanel().AutoScrollPosition.Y) + ',';
            element += "\n\t\tsize: " + Width + ',';
            if(ImageName != _params.slotImageName) element += "\n\t\tbitmap: \"" + ImageName.Split('.')[0] + "\",";
            if(Visual) element += "\n\t\tvisual: true,";
            if (TransparentBg) element += "\n\t\tisTransparentBackground: true,";
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
