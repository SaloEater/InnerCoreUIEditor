using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    static class ExplorerPainter
    {
        public static void Color(string elementName)
        {
            foreach(Label t in Global.panelExplorer.Controls)
            {
                if(t.Text == elementName)
                {
                    t.BackColor = System.Drawing.Color.Gray;
                    break;
                }
            }
        }

        public static void Uncolor(string elementName)
        {
            foreach (Label t in Global.panelExplorer.Controls)
            {
                if (t.Text == elementName)
                {
                    t.BackColor = System.Drawing.Color.White;
                    break;
                }
            }
        }
    }
}
