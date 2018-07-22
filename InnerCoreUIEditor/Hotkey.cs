using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    class Hotkey
    {
        private List<int> ids;

        /// <summary>
        /// Typical hotkey assigngments
        /// </summary>
        public enum fsModifiers
        {
            Alt = 0x0001,
            Control = 0x0002,
            Shift = 0x0004, // Changes!
            Window = 0x0008,
        }

        private IntPtr _hWnd;

        public Hotkey(IntPtr hWnd)
        {
            this._hWnd = hWnd;
            ids = new List<int>();
        }

        public void RegisterControlHotKey(int id, Keys keys)
        {
            RegisterHotKey(_hWnd, id, (uint)fsModifiers.Control, (uint)keys);
            ids.Add(id);
        }

        public void RegisterHotKey(int id, Keys keys)
        {
            RegisterHotKey(_hWnd, id, (uint)0, (uint)keys);
            ids.Add(id);
        }

        public void UnRegisterHotKeys()
        {
            foreach(int id in ids)
                UnregisterHotKey(_hWnd, id);
        }

        #region WindowsAPI
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion

    }
}
