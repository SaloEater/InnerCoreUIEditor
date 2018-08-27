using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    public class ActionStack
    {
        public Control c;
        public int type;
        public object value;

        public ActionStack(Control c, int type, object value)
        {
            this.c = c;
            this.type = type;
            this.value = value;
        }

        public void ApplyAction()
        {
            Type _type = c.GetType();
            if (c == null) return;
            if (_type == typeof(CloseButton) || _type == typeof(InnerBitmap)
                || _type == typeof(InnerButton) || _type == typeof(InnerImage)
                || _type == typeof(InvSlot) || _type == typeof(Scale)
                || _type == typeof(Slot) || _type == typeof(InnerText) || _type == typeof(InnerControl))
            {
                ((InnerControl)c).ApplyChanges(type, value);

            }
            else if(c.GetType() == typeof(InnerTabPage))
            {
                ((InnerTabPage)c).ApplyChanges(type, value);
            }
        }

    }
}
