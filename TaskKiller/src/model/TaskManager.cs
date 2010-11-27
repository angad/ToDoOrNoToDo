using System.Collections.Generic;
using System.Linq;
/**
 * The model, which contains the data & stuff.
 * It will notify the view when there needs a change in UI
 */

namespace Model
{

    class TaskManager
    {
        // containing all tasks
        List<Task> TaskList = new List<Task>();

        // containing tasks for display purpose
        List<Task> ViewList = new List<Task>();
        
        // backup list of tasks
        List<Task> backupList = null;

        // currently showing done task or not done task
        bool currentView = false;

        int maxTaskID = 0;

        #region event handler
        public delegate void TaskChangeHandler(object sender, int refresh);
        public event TaskChangeHandler TaskChangeEvent;

        /* To be called when data is updated:
         * When a task is added / edited / deleted
         * It will trigger the DataChangeEvent
         */
        public void broadcastChange(object sender = null, int refresh = 1)
        {
            // MessageBox.Show(TaskChangeEvent.ToString());
            if (TaskChangeEvent != null)
            {
                TaskChangeEvent(ViewList, refresh);
            }
        }
        #endregion

        public TaskManager() { }

        #region redirecting commands
        public void add(string taskname, string deadline, bool star, string description, bool done)
        {
            // always backup before making change
            backup();
            
            // adding task
            maxTaskID++;
            Task newTask = new Task(maxTaskID, taskname, description, star, deadline, done);
            newTask.PropertyChanged += taskChange;
            newTask.BeforeChangeEvent += backup;

            TaskList.Add( newTask );

            // always add new task to view
            if (done == currentView)
            {
               ViewList.Add(newTask);
            }

            broadcastChange();
        }
        public int remove(int taskId)
        {
            // checking is done in API
            backup();
            Task result = returnTaskAtID(taskId);
            TaskList.Remove(result);
            if (ViewList.Exists(result.Equals))
            {
                ViewList.Remove(result);
            }
            broadcastChange();
            return 0;
        }

        public int editName(int taskId, string name)
        {
            backup();
            Task result = returnTaskAtID(taskId);
            result.setName(name);
            broadcastChange();
            return 0;
        }

        public int editDescription(int taskId, string description)
        {
            backup();
            Task result = returnTaskAtID(taskId);
            result.setDesc(description);
            broadcastChange();
            return 0;
        }

        public int editStar(int taskId, bool star)
        {
            backup();
            Task result = returnTaskAtID(taskId);
            result.setStar(star);
            broadcastChange();
            return 0;
        }

        public int editDone(int taskId, bool done)
        {
            backup();
            Task result = returnTaskAtID(taskId);
            result.setDone(done);
            if (currentView != done)
            {
                ViewList.Remove(result);
            }
            else
            {
                ViewList.Add(result);
            }
            broadcastChange();
            return 0;
        }

        public int editDeadline(int taskId, string deadline)
        {
            backup();
            Task result = returnTaskAtID(taskId);
            result.setDeadline(deadline);
            broadcastChange();
            return 0;
        }

        #endregion

        #region handling task change events from task
        public void taskChange(object sender, object arg)
        {
            broadcastChange(null, 0);
        }
        #endregion

        #region backup and revert
        public void backup(object sender = null)
        {
            backupList = new List<Task>();
            for (int i = 0; i < TaskList.Count; i++)
            {
                backupList.Add(new Task(TaskList[i]));
            }
        }
        public int revert()
        {
            if (backupList == null)
            {
                return -1;
            }
            else
            {
                //TaskList = backupList;
                TaskList.Clear();
                for (int i = 0; i < backupList.Count; i++)
                {
                    TaskList.Add(backupList[i]);
                }
                backupList = null;
                changeViewList(false);
                broadcastChange();
            }
            return 0;
        }
        #endregion

        private void changeViewList(bool doing = false)
        {
            currentView = doing;
            ViewList.Clear();
            for (int i = 0; i < TaskList.Count; i++)
            {
                if (TaskList[i].getDone() == doing)
                    ViewList.Add(TaskList[i]);
            }
            broadcastChange();
        }

        public void listDoneTasks()
        {
            changeViewList(true);            
        }
        public void listDoingTasks()
        {
            changeViewList(false);
        }
        public void listStarTasks()
        {
            ViewList.Clear();
            for (int i = 0; i < TaskList.Count; i++)
            {
                if (TaskList[i].getStar())
                    ViewList.Add(TaskList[i]);
            }
            broadcastChange();
        }

        public int TaskListCount()
        {
            return TaskList.Count;
        }

        public void removeAll()
        {
            TaskList.Clear();
        }

        public void refreshDisplay()
        {
            broadcastChange();
        }

        public bool isEmpty()
        {
            if (TaskList.Count == 0) return true;
            return false;
        }

        public Task returnTaskAtID(int taskId)
        {
            for (int i = 0; i < TaskList.Count; i++)
            {
                if (TaskList.ElementAt(i).getId() == taskId)
                {
                    return TaskList.ElementAt(i);
                }
            }

            return null;
        }
        public bool hasTaskId(int taskId)
        {
            return (returnTaskAtID(taskId) != null);
        }

        public List<Task> getViewList()
        {
            return ViewList;
        }
        public List<Task> getAllTasks()
        {
            return TaskList;
        }

        public string getDesc(int id)
        {
            Task t = returnTaskAtID(id);
            return t.getDesc();
        }

    }
}
