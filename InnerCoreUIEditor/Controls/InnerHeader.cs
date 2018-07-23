using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor.Controls
{
    public partial class InnerHeader : InnerControl
    {

        CloseButton closeButton;

        public InnerHeader(ExplorerPainter explorerPainter, Params _params, InnerTabPage parentTabPage) : base(explorerPainter, _params, parentTabPage)
        {
            InitializeComponent();
            closeButton = new CloseButton(explorerPainter, _params, parentTabPage);
            closeButton.Location = new Point(Width - closeButton.Width, 0);
            closeButton.constant = true;
            closeButton.hidden = true;
            closeButton.Enabled = false;
            
            Controls.Add(closeButton);
            closeButton.BringToFront();
            richTextBox1.BackColor = Color.FromArgb(114, 106, 112);
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 20, (FontStyle)(richTextBox1.SelectionFont.Style));
            foreach(Control c in Controls)
            {
                c.Click += (sender, e) => { SelectControl(); };
            }
            //ImageBlend.ToPanelColor(pictureBox1.Image);                     
        }

        public override void FillPropPanel(Panel propPanel)
        {
            ClearPropPanel(propPanel);

            Label _global = new Label();
            _global.Location = new Point(0, elementY += elementSpacing);
            _global.Size = new Size(102, elementSpacing);
            _global.Text = "Кнопка закрытия";
            propPanel.Controls.Add(_global);

            CheckBox _globalCheck = new CheckBox();
            _globalCheck.Location = new Point(103, elementY);
            _globalCheck.Size = new Size(101, elementSpacing);
            _globalCheck.Checked = closeButton.Visible;
            _globalCheck.CheckedChanged += (sender, e) => { closeButton.Visible = ((CheckBox)sender).Checked; };
            propPanel.Controls.Add(_globalCheck);
        }

        public void SetText(string text)
        {
            richTextBox1.Text = text;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 20, (FontStyle)(richTextBox1.SelectionFont.Style));
        }

        internal void SetButtonVisibilty(bool visible)
        {
            closeButton.Visible = visible;
        }

        public string GetText()
        {
            return richTextBox1.Text;
        }

        public bool GetButtonVisibility()
        {
            return closeButton.Visible;
        }
    }
}
