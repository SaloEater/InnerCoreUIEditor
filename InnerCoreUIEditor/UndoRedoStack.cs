using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnerCoreUIEditor
{
    public class UndoRedoStack
    {
        Stack<ActionStack> undoStack,
            redoStack;

        public UndoRedoStack()
        {
            undoStack = new Stack<ActionStack>();
            redoStack = new Stack<ActionStack>();
        }

        public void UndoPush(ActionStack action)
        {
            undoStack.Push(action);
        }

        public void UndoPop()
        {
            if (undoStack.Count == 0) return;
            ActionStack action = undoStack.Pop();
            try
            {
                RedoPush(MakeSnapshot(action));
                action.ApplyAction();
            }
            catch(ArgumentException)
            {
                undoStack.Push(action);
            }
        }

        private ActionStack MakeSnapshot(ActionStack action)
        {
            Type type = action.c.GetType();
            if (type == typeof(CloseButton) || type == typeof(InnerBitmap) 
                || type == typeof(InnerButton) || type == typeof(InnerImage) 
                || type == typeof(InvSlot) || type == typeof(Scale) 
                || type == typeof(Slot) || type == typeof(InnerText) || type == typeof(InnerControl))
            {
                return ((InnerControl)action.c).MakeSnapshot(action.type);
            }
            else if (action.c.GetType() == typeof(InnerTabPage))
            {
                return ((InnerTabPage)action.c).MakeSnapshot(action.type);
            }
            throw new ArgumentException();
        }

        public void RedoPush(ActionStack action)
        {
            redoStack.Push(action);
        }

        public void RedoPop()
        {
            if (redoStack.Count == 0) return;
            ActionStack action = redoStack.Pop();
            try
            {
                UndoPush(MakeSnapshot(action));
                action.ApplyAction();
            }
            catch (ArgumentException)
            {
                redoStack.Push(action);
            }
        }

    }
}
