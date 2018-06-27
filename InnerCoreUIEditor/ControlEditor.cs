using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    class ControlEditor
    {
        private static bool moving,
                    scaling; //Активное действие

        private static bool bottomEdge,
                    upperEdge,
                    leftEdge,
                    rightEdge; //Положение курсора

        private static Point cursorOrigin; //Положение курсора в момент нажатия

        private static float rightbotd;

        public static void Init(Control origin)
        {
            Init(origin, origin);
        }

        public static void Init(Control origin, Control target)
        {
            rightbotd = 0;
            moving = false;
            scaling = false;
            bottomEdge = false;
            upperEdge = false;
            leftEdge = false;
            rightEdge = false;
            cursorOrigin = Point.Empty;

            origin.MouseDown += Control_MouseDown; // Активировать изменение объекта
            origin.MouseUp += Control_MouseUp; // Прекратить изменение объекта
            origin.MouseMove += (sender, e) => MoveControl(target, e); // Само изменение рассчитывается здесь
        }

        private static void Control_MouseDown(object sender, MouseEventArgs e)
        {
            cursorOrigin = e.Location;
            if (bottomEdge || upperEdge || leftEdge || bottomEdge)
            {
                scaling = true;
            }
            else
            {
                moving = true;
                ((Control)sender).Cursor = Cursors.Hand;
            }
        }

        private static void Control_MouseUp(object sender, MouseEventArgs e)
        {
            cursorOrigin = Point.Empty;
            moving = false;
            scaling = false;
            UpdateCursor((Control)sender);
        }



        private static void MoveControl(object container, MouseEventArgs e)
        {
            InnerControl obj = (InnerControl)container;
            if (!moving && !scaling)
            {
                UpdateMousePosition(obj, e.Location);
                UpdateCursor(obj);
            }
            else if (moving)
            {
                int x = (e.X - cursorOrigin.X) + obj.Left;
                int y = (e.Y - cursorOrigin.Y) + obj.Top;
                if (x - Global.panelWorkspace.AutoScrollPosition.X > Global.X || x - Global.panelWorkspace.AutoScrollPosition.X < 0) x = obj.Left;
                if (y - Global.panelWorkspace.AutoScrollPosition.Y > Global.Y || y - Global.panelWorkspace.AutoScrollPosition.Y < 0) y = obj.Top;
                obj.Location = new Point(x, y);
            }
            else if (scaling)
            {
                if (leftEdge)
                {
                    if (upperEdge)
                    {
                        int x = obj.Location.X - cursorOrigin.X + e.X;
                        if (x < 0 || x > Global.X - obj.Size.Width)return;
                        int y = obj.Location.Y - cursorOrigin.X + e.X;
                        if (y < 0 || y > Global.Y - obj.Size.Width)return;
                        obj.Location = new Point(x, y);
                        obj.ResizeControl(obj.Width + cursorOrigin.X - e.X);
                    }
                    else if (bottomEdge)
                    {
                        int x = obj.Location.X - cursorOrigin.X + e.X;
                        if (x < 0 || x > Global.X - obj.Size.Width) return;
                        int y = obj.Location.Y;
                        obj.Location = new Point(x, y);
                        obj.ResizeControl(obj.Width + cursorOrigin.X - e.X);
                    }
                    else
                    {
                        //Не нужен
                    }
                }
                else if (rightEdge)
                {
                    if (upperEdge)
                    {
                        int x = obj.Location.X;
                        int y = obj.Location.Y - cursorOrigin.Y + e.Y;
                        if (y < 0 || y > Global.Y - obj.Size.Width) return;
                        obj.Location = new Point(x, y);
                        obj.ResizeControl(obj.Height + cursorOrigin.Y - e.Y);
                    }
                    else if (bottomEdge)
                    {
                        if(rightbotd!= e.X - cursorOrigin.X) obj.ResizeControl(obj.Width + e.X - cursorOrigin.X);
                        rightbotd = e.X - cursorOrigin.X;
                    }
                    else
                    {
                        //Не нужен
                    }
                }
                else if (upperEdge || bottomEdge)
                {
                    //Не нужен
                }
                else
                {

                }
            }
        }

        private static void UpdateMousePosition(Control sender, Point location)
        {
            bottomEdge = Math.Abs(location.Y-sender.Height) <= 4;
            upperEdge = Math.Abs(location.Y) <= 4;
            leftEdge = Math.Abs(location.X) <= 4;
            rightEdge = Math.Abs(location.X - sender.Width) <= 4;
        }

        private static void UpdateCursor(Control sender)
        {
            if(leftEdge)
            {
                if(upperEdge)
                {
                    sender.Cursor = Cursors.SizeNWSE;
                } else if(bottomEdge)
                {
                    sender.Cursor = Cursors.SizeNESW;
                } else
                {
                    sender.Cursor = Cursors.SizeWE;
                }
            } else if(rightEdge)
            {
                if (upperEdge)
                {
                    sender.Cursor = Cursors.SizeNESW;
                }
                else if (bottomEdge)
                {
                    sender.Cursor = Cursors.SizeNWSE;
                }
                else
                {
                    sender.Cursor = Cursors.SizeWE;
                }
            } else if(upperEdge || bottomEdge)
            {
                sender.Cursor = Cursors.SizeNS;
            } else
            {
                sender.Cursor = Cursors.Default;
            }
        }
    }
}
