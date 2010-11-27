using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace View
{
    class HotKeyHandler : IDisposable
    {

        #region Hotkey stuff
        public const int WM_HOTKEY = 0x0312;
        public const int VIRTUALKEYCODE_FOR_CAPS_LOCK = 0x14;
        public const int MOD_ALT = 0x0001;
        public const int MOD_CONTROL = 0x0002;
        public const int VK_SPACE = 0x0020;
        public const int MOD_NOREPEAT = 0x4000;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private readonly Window _mainWindow;

        WindowInteropHelper _host;
        #endregion

        public HotKeyHandler(Window mw)
        {
            // Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            //leave the _mainWindow here so that in case we want to move it to another class, it will be simple
            _mainWindow = mw;
            _host = new WindowInteropHelper(_mainWindow);

            SetupHotKey(_host.Handle);
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == WM_HOTKEY)
            {
                if (_mainWindow.WindowState == WindowState.Normal)
                {
                    _mainWindow.WindowState = WindowState.Minimized;
                }
                else
                {
                    _mainWindow.Show();
                    if (_mainWindow.WindowState == WindowState.Minimized)
                    {
                        _mainWindow.WindowState = WindowState.Normal;
                    }
                    _mainWindow.Activate();
                }
            }
        }

        private void SetupHotKey(IntPtr handle)
        {
            RegisterHotKey(handle, GetType().GetHashCode(), MOD_ALT | MOD_CONTROL | MOD_NOREPEAT, VK_SPACE);
        }

        public void Dispose()
        {
            UnregisterHotKey(_host.Handle, GetType().GetHashCode());
        }
    }
}
