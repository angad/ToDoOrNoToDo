using System;
using System.Windows.Controls;
using Controller;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using Model;
using System.Windows.Data;

namespace View
{
    public class GUI: UIAbstract
    {
        private TextBlock manbox;
        private DataGrid taskListDataGrid;
        //private DataGridEvents dge;
        private Grid container;
        ControlCenter controller;
        public ICollectionView TaskListView { get; private set; }
        //public List<Task> TaskListView { get; set; }

        public GUI(Grid grd, TextBlock mb, DataGrid tl, ControlCenter cc)
        {
            container = grd;
            manbox = mb;
            taskListDataGrid = tl;
            controller = cc;
            TaskListView = CollectionViewSource.GetDefaultView(controller.ReturnTaskList());
        }

        /* Handling updating man
         */
        public override void manUpdate(object sender)
        {
            manbox.Visibility = System.Windows.Visibility.Visible;
            taskListDataGrid.Visibility = System.Windows.Visibility.Hidden;
            manbox.Text = (string)sender;
        }

        // called when the model (task manager) is changed
        // refresh = 1 when the view need to be manually refreshed
        public override void onTaskChange(object sender, int refresh)
        {
            taskListDataGrid.Visibility = System.Windows.Visibility.Visible;
            manbox.Visibility = System.Windows.Visibility.Hidden;
            if (refresh==1) // have to manually refresh the list
            {
                try
                {
                    TaskListView.Refresh();
                }
                catch (Exception e)
                {
                    // do nothing
                }
            }
        }

        public override void enable()
        {
            container.Visibility = Visibility.Visible;
            taskListDataGrid.Visibility = System.Windows.Visibility.Visible;
            manbox.Visibility = System.Windows.Visibility.Hidden;
        }

        public override void disable()
        {
            //MessageBox.Show("sdfsdf");
            container.Visibility = Visibility.Hidden;
            taskListDataGrid.Visibility = System.Windows.Visibility.Hidden;
            manbox.Visibility = System.Windows.Visibility.Hidden;
        }

        // handle action when delete is clicked
        public void Delete_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button b = (System.Windows.Controls.Button)sender;
            DataGridRow dgr = extensions.TryFindParent<DataGridRow>(b);
            Model.Task t = (Model.Task)dgr.Item;
            controller.removeTask(t.ID.ToString());
        }

        public void Done_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button b = (System.Windows.Controls.Button)sender;
            DataGridRow dgr = extensions.TryFindParent<DataGridRow>(b);
            Model.Task t = (Model.Task)dgr.Item;
            bool done = t.getDone();
            if (done == true) done = false;
            else done = true;
            controller.doneTask(t.ID.ToString(), done);
        }

        public void OnStarValueChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.DataGridCell dgc = (System.Windows.Controls.DataGridCell)sender;
            System.Windows.Controls.DataGridRow dgr = extensions.TryFindParent<System.Windows.Controls.DataGridRow>(dgc);
            int id = ((Model.Task)dgr.Item).getId();
            bool crrStar = ((Model.Task)dgr.Item).getStar();
            string newStar = dgc.Content.ToString().Substring(52);
            if (!newStar.Equals("" + crrStar))
            {
                controller.StarHandler(id, newStar);
            }
        }
    }
}
