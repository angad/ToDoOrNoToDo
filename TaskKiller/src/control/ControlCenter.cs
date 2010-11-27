using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using View;
using Model;
using Controller.ApiPackage;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace Controller
{
    public class ControlCenter
    {
        TaskManager tm;

        CommandLineHanlder eh;
        
        API api;
        MainWindow mw;
        FileManager fm;
        const string FILEPATH = "C:\\Tasks\\tasks.xml"; 
        
        public ControlCenter(MainWindow mw)
        {
            this.mw = mw;
            tm = new TaskManager();
            api = new API(tm);
            eh = new CommandLineHanlder(api);

            //set up file manager
            fm = new FileManager(tm);
            fm.loadFile(FILEPATH);
            tm.TaskChangeEvent += new TaskManager.TaskChangeHandler(fm.write);

            //setting up all the events
            api.StatusChangeEvent += new API.StatusChangeHandler(mw.onStatusChange);
            api.ViewChangeEvent += new API.ViewChangeHandler(mw.onViewChange);

            // Set up key_event handler for windows components
            // handle the event 'a command is entered'
            mw.commandLine.KeyUp += new KeyEventHandler(eh.KeyUp);
            mw.commandLine.KeyDown += new KeyEventHandler(eh.KeyDown);
            mw.commandLine.MouseLeftButtonDown += new MouseButtonEventHandler(eh.MouseLeftButtonDown);
            mw.commandLine.LostFocus += new RoutedEventHandler(eh.LostFocus);
            mw.commandLine.Text = "Enter Command";
        }

        public void installUI(UIAbstract ui)
        {
            api.ManContentEvent += new API.ManContentHandler(ui.manUpdate);
            tm.TaskChangeEvent += new TaskManager.TaskChangeHandler(ui.onTaskChange);
            tm.refreshDisplay();
        }

        public void StarHandler(int id, string star)
        {
            api.editTask(id.ToString(), "star", star);
        }
        public List<Task> ReturnTaskList()
        {
            return tm.getViewList();
        }
        public void removeTask(string id)
        {
            api.removeTask(id);
        }
        public void doneTask(string id, bool done)
        {
            api.doneTask(id, done);
        }

        internal void showDone()
        {
            tm.listDoneTasks();
        }

        internal void showHome()
        {
            tm.listDoingTasks();
        }

        internal void showDesc(object obj)
        {
            Task t = (Task)obj;
            api.displayDesc(t);
        }

        internal void showMan()
        {
            api.manUpdate(api.getManString("man"));
        }
    }
}
