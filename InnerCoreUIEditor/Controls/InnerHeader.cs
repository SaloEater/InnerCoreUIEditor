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
    public partial class InnerHeader : UserControl
    {
        public InnerHeader()
        {
            InitializeComponent();
            richTextBox1.BackColor = Color.FromArgb(114, 106, 112);
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 20, (FontStyle)(richTextBox1.SelectionFont.Style));
            //ImageBlend.ToPanelColor(pictureBox1.Image);         
            
        }

        public void SetText(string text)
        {
            richTextBox1.Text = text;
        }

        public string GetText()
        {
            return richTextBox1.Text;
        }
    }
}
