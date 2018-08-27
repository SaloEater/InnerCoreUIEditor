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

        public InvSlot(InnerTabPage parentTabPage) : base(parentTabPage)
        {
            InitializeComponent();
            Initialization();
        }

        public void Initialization()
        {
            ControlEditor.Init(pictureBoxSlot, this, parentTabPage);
            ControlEditor.Init(pictureBoxSelection, this, parentTabPage);
            pictureBoxSelection.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSlot.Dock = DockStyle.Fill;
            ToDefault();
            pictureBoxSlot.Click += PictureBoxSlot_Click;
            index = 0;
        }

        internal override InnerControl MakeCopy()
        {
            if (constant || hidden) throw new ArgumentOutOfRangeException();
            InvSlot control = new InvSlot(parentTabPage);
            control.Location = Location;
            control.Size = Size;
            control.Visible = Visible;
            control.scale = scale;
            control.originSize = originSize;
            control.originSize = originSize;

            control.ActiveImage = ActiveImage;
            control.ImageName = ImageName;
            control.index = index;
            control.ApplyImage();

            return control;
        }

        private void ApplyImage()
        {
            pictureBoxSlot.Image = ActiveImage;
        }

        public override void ToDefault()
        {
            ActiveImage = _params.GetInvSlotImage(out string imageName);
            pictureBoxSlot.Image = new Bitmap(ActiveImage);
            ImageName = imageName;
            pictureBoxSelection.Image = new Bitmap(_params.GetSelectionImage(out imageName));
            ImageBlend.Blend(pictureBoxSlot.Image, pictureBoxSelection.Image);
            Refresh();
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
            _sizeValue.TextChanged += _sizeValue_TextChanged;
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

            base.FillPropPanel(propPanel);
        }

        private void _sizeValue_TextChanged(object sender, EventArgs e)
        {
            if (constant) return;
            sizeTextChanged = true;
        }

        internal void SetSelection(Image selectionDefaultImage)
        {
            pictureBoxSelection.Image = new Bitmap(selectionDefaultImage);
            ImageBlend.Blend(pictureBoxSlot.Image, pictureBoxSelection.Image);
            Refresh();
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
            element += '\"' + elementName + "\": \n\t{";
            element += "\n\t\ttype: \"invSlot\",";
            element += "\n\t\tx: " + (Location.X - parentTabPage.GetDesktopPanel().AutoScrollPosition.X) + ',';
            element += "\n\t\ty: " + (Location.Y - parentTabPage.GetDesktopPanel().AutoScrollPosition.Y) + ',';
            element += "\n\t\tsize: " + Width + ',';
            element += "\n\t\tindex: " + index + ',';
            if (ImageName != _params.invSlotImageName) element += "\n\t\tbitmap: \"" + ImageName.Split('.')[0] + "\",";
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

            SizeF sizeF = new SizeF((float)size / this.GetWidth(), (float)size / GetWidth());
            parentTabPage.SaveUndoAction(new ActionStack(this, 2, size));
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
            openFileDialog1.Filter = "PNG (*.png)|*.png|All files (*.*)|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.Cancel) return;
            if (openFileDialog1.SafeFileName == "") return;
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
    }
}
