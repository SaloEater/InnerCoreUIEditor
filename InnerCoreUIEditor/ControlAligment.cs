using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    class ControlAligment
    {
        private List<int> horizontalBounds;
        private List<int> verticalBounds;

        Panel parent;
        Graphics g;
        int error = 3;
        List<Pen> pens;

        public ControlAligment(Control parent)
        {
            this.parent = (Panel)parent;
            g = parent.CreateGraphics();
        }

        public void Init(Control target)
        {
            pens = new List<Pen>();
            parent.Refresh();
            horizontalBounds = new List<int>();
            verticalBounds = new List<int>();
            foreach (Control c in parent.Controls)
            {
                if (c == target) continue;
                horizontalBounds.Add(c.Left);
                horizontalBounds.Add(c.Left+c.Width);

                verticalBounds.Add(c.Top);
                verticalBounds.Add(c.Top + c.Height);
            }
        }

        public void TryToMove(Control obj, Point newLocation)
        {
            foreach (Pen pen in pens) pen.Dispose();
            pens = new List<Pen>();
            int x = newLocation.X;
            int y = newLocation.Y;

            foreach(int nX in horizontalBounds)
            {
                if(Math.Abs(nX-x)<=error)
                {
                    x = nX;
                    /*Pen pen = new Pen(Color.Black, 1);
                    g.DrawLine(pen, nX, y/2-parent.AutoScrollPosition.X, nX, y*3/2 - parent.AutoScrollPosition.X);
                    pens.Add(pen);*/
                } else 
                if(Math.Abs(nX - x - obj.Width) <= error)
                {
                    x = nX - obj.Width;
                    /*Pen pen = new Pen(Color.Black, 1);
                    g.DrawLine(pen, nX, y / 2 - parent.AutoScrollPosition.X, nX, y * 3 / 2 - parent.AutoScrollPosition.X);
                    pens.Add(pen);*/
                }
            }

            foreach (int nY in verticalBounds)
            {
                if (Math.Abs(nY - y) <= error )
                {
                    y = nY;
                    /*Pen pen = new Pen(Color.Black, 1);
                    g.DrawLine(pen, x / 2 - parent.AutoScrollPosition.Y, nY, x * 3 / 2 - parent.AutoScrollPosition.Y, nY);
                    pens.Add(pen);*/
                }
                else
                if (Math.Abs(nY - y - obj.Height) <= error)
                {
                    y = nY - obj.Height;
                    /*Pen pen = new Pen(Color.Black, 1);
                    g.DrawLine(pen, x / 2 - parent.AutoScrollPosition.Y, nY, x * 3 / 2 - parent.AutoScrollPosition.Y, nY);
                    pens.Add(pen);*/
                }
            }

            obj.Location = new Point(x,y);
        }
    }
}
