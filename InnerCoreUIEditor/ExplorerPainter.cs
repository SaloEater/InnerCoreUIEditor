using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    public class ExplorerPainter
    {
        private Panel panelExplorer;

        public ExplorerPainter(Panel panelExplorer)
        {
            this.panelExplorer = panelExplorer;
        }

        public void Color(string elementName)
        {
            foreach(Label t in panelExplorer.Controls)
            {
                if(t.Text == elementName)
                {
                    t.BackColor = System.Drawing.Color.LightBlue;
                    break;
                }
            }
        }

        public void Uncolor(string elementName)
        {
            foreach (Label t in panelExplorer.Controls)
            {
                if (t.Text == elementName)
                {
                    t.BackColor = panelExplorer.BackColor;
                    break;
                }
            }
        }
    }
}
