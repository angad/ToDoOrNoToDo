using System;
using System.Windows;
using System.Windows.Input;

using Controller;
using ATDSpace;
/**
 * Naming for understanding & clarity. We refactor later
 * This file is to contain the View in MVC
 */

namespace View
{
    /// <summary>
    /// View
    /// </summary>
    public partial class MainWindow : Window
    {        
        ControlCenter controller;
        //DataGridEvents dge;
        GUI gui;
        TextUI textui;
        UIAbstract currentui;

        // let's try to keep everything not part of the UI away from this file
        public MainWindow()
        {

            ATD atd = new ATD();
            atd.execute();
            
            InitializeComponent();
            
            new HotKeyHandler(this);
            new MinimizeEventHandler(this);

            controller = new ControlCenter(this);
            commandLine.Focus();
            //taskListDataGrid.FontFamily = new System.Windows.Media.FontFamily("/ToDoOrNoToDo;component/image/#Buxton Sketch");

            // installing UIs to the system
            gui = new GUI(GuiElement, manbox, taskListDataGrid, controller);
            textui = new TextUI(TextUIElement, terminal);

            controller.installUI(gui);
            controller.installUI(textui);
            DataContext = gui;
            taskListDataGrid.SelectedCellsChanged += new System.Windows.Controls.SelectedCellsChangedEventHandler(taskListDataGrid_SelectedCellsChanged);

            gui.enable();
            textui.disable();
            currentui = gui;
             
        }

        void taskListDataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            controller.showDesc(e.AddedCells[0].Item);
        }


        #region handlers related to update view

        public void onStatusChange(object newStatus)
        {
            status.Text = (string)(newStatus);
        }

        public void onViewChange(object sender)
        {
            string viewName = (string) sender;
            if (viewName == "TextUI")
            {
                gui.disable();
                textui.enable();
                currentui = textui;
            } else
                if (viewName == "GUI")
                {
                    textui.disable();
                    currentui = gui;
                    gui.enable();
                }
        }
        
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            gui.Delete_Click(sender, e);
        }                

        void OnStarValueChanged(object sender, RoutedEventArgs e)
        {
            gui.OnStarValueChanged(sender, e);
        }

        #endregion

        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }

        // something to prevent the app from resize?
        void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //terminal.Height = Application.Current.MainWindow.Height - 90;
        }

        // giving focus to commandLine when the window is activated
        void ActivatedHandler(object sender, EventArgs e)
        {
            commandLine.Focus();
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.Button b = (System.Windows.Controls.Button)sender;
            b.Opacity = 1;
            
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.Button b = (System.Windows.Controls.Button)sender;
            b.Opacity = 0;
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            gui.Done_Click(sender, e);
        }

     
       

        private void close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.Close();
        }

        private void minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            currentui.disable();
            //taskListDataGrid.Visibility = System.Windows.Visibility.Hidden;
            commandLine.Visibility = System.Windows.Visibility.Hidden;
            status.Visibility = System.Windows.Visibility.Hidden;
            cmdline.Visibility = System.Windows.Visibility.Hidden;
            about_pic.Visibility = System.Windows.Visibility.Visible;
            about_name.Visibility = System.Windows.Visibility.Visible;
            manbox.Visibility = System.Windows.Visibility.Hidden;
            
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            currentui.enable();
            //taskListDataGrid.Visibility = System.Windows.Visibility.Visible;
            commandLine.Visibility = System.Windows.Visibility.Visible;
            status.Visibility = System.Windows.Visibility.Visible;
            cmdline.Visibility = System.Windows.Visibility.Visible;
            about_pic.Visibility = System.Windows.Visibility.Hidden;
            //manbox.Visibility = System.Windows.Visibility.Hidden;
            about_name.Visibility = System.Windows.Visibility.Hidden;
            
            controller.showHome();
        }

        private void DoneTasks_Click(object sender, RoutedEventArgs e)
        {
            currentui.enable();
            //taskListDataGrid.Visibility = System.Windows.Visibility.Visible;
            commandLine.Visibility = System.Windows.Visibility.Visible;
            status.Visibility = System.Windows.Visibility.Visible;
            cmdline.Visibility = System.Windows.Visibility.Visible;
            about_pic.Visibility = System.Windows.Visibility.Hidden;
            //manbox.Visibility = System.Windows.Visibility.Hidden;
            about_name.Visibility = System.Windows.Visibility.Hidden;
            
            controller.showDone();
        }
        private void help_Click(object sender, RoutedEventArgs e)
        {
            currentui.enable();
            //taskListDataGrid.Visibility = System.Windows.Visibility.Visible;
            commandLine.Visibility = System.Windows.Visibility.Visible;
            status.Visibility = System.Windows.Visibility.Visible;
            cmdline.Visibility = System.Windows.Visibility.Visible;
            about_pic.Visibility = System.Windows.Visibility.Hidden;
            about_name.Visibility = System.Windows.Visibility.Hidden;
            
            controller.showMan();
        }

    }
}
