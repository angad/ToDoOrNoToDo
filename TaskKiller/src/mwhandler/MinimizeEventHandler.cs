using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace View
{
    class MinimizeEventHandler
    {

        private Window _mainWindow;
        System.Windows.Forms.NotifyIcon myNotifyIcon;

        public MinimizeEventHandler(Window mw)
        {
            _mainWindow = mw;
            #region minimize to tray stuff
            myNotifyIcon = new System.Windows.Forms.NotifyIcon();
            myNotifyIcon.BalloonTipText = "The application has been minimised. Click the tray icon to show";
            myNotifyIcon.BalloonTipTitle = "ToDoOrNotToDo";
            myNotifyIcon.Text = "ToDoOrNotToDo";
            myNotifyIcon.Icon = new System.Drawing.Icon("icon.ico");
            myNotifyIcon.Click += new EventHandler(myNotifyIconClick);
            #endregion
            //_mainWindow.MouseLeftButtonDown += DragWindow;
            _mainWindow.StateChanged += new EventHandler(MainWindowStateChangeHandler);
        }


        public void myNotifyIconClick(object sender, EventArgs e)
        {
            myNotifyIcon.Visible = false;
            _mainWindow.Show();
            if (_mainWindow.WindowState == WindowState.Minimized)
            {
                _mainWindow.WindowState = WindowState.Normal;
            }
        }

        void MainWindowStateChangeHandler(object sender, EventArgs e)
        {
            if (_mainWindow.WindowState == WindowState.Minimized)
            {
                MainWindowMinimizeToTray(sender, e);
            }
        }

        void MainWindowMinimizeToTray(object sender, EventArgs e)
        {
            _mainWindow.Hide();
            myNotifyIcon.Visible = true;
            myNotifyIcon.ShowBalloonTip(2000);
        }
    }
}
