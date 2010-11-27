using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml;
using System.IO;


namespace Controller.ApiPackage
{
    class API
    {
        /* Event signature*/

        #region event handler for status change
        public delegate void StatusChangeHandler(object sender);
        public event StatusChangeHandler StatusChangeEvent;
        public void sendUpdateStatus(string newStatus)
        {
            if (StatusChangeEvent != null)
            {
                StatusChangeEvent(newStatus);
            }
        }
        #endregion
        #region event handler for man update
        public delegate void ManContentHandler(object sender);
        public event ManContentHandler ManContentEvent;
        public void manUpdate(string man)
        {
            if (ManContentEvent != null)
            {
                ManContentEvent(man);
            }
        }
        #endregion
        #region event handler for change view
        public delegate void ViewChangeHandler(object sender);
        public event ViewChangeHandler ViewChangeEvent;
        public void sendViewChange(string viewName)
        {
            if (ViewChangeEvent!= null)
            {
                ViewChangeEvent(viewName);
            }
        }
        #endregion

        TaskManager tm;
        //View.OutputHandler oh = new View.OutputHandler();

        public API(TaskManager tmRef)
        {
            this.tm = tmRef;
        }

        public void execute(String cmdStr)
        {
            //parsing the command
            CommandParser cmd = new CommandParser(cmdStr);
            if (cmd.isInvalidCommand())
            {
                sendUpdateStatus("Invalid command: '" + cmdStr + "'");
                return;
            }

            //based on the command code to execute correct functions
            //sendUpdateStatus("Executing the command");
            //tm.broadcastChange();

            int cmdCode = cmd.getCommandCode();
            switch (cmdCode)
            {
                case (int)CommandParser.CmdCode.Add:
                    addTask(cmd.getMainParam(),
                            cmd.getOptionalParam("-t"),
                            cmd.getOptionalParam("-s"),
                            cmd.getOptionalParam("-d"));
                    break;

                case (int)CommandParser.CmdCode.Remove:
                    removeTask(cmd.getMainParam());
                    break;

                case (int)CommandParser.CmdCode.Edit:
                    if (cmd.noFlag())
                    {
                        sendUpdateStatus("Edit must have a flag");
                        break;
                    }
                    editTask(cmd.getMainParam(), "name", cmd.getOptionalParam("-n"));
                    editTask(cmd.getMainParam(), "deadline", cmd.getOptionalParam("-t"));
                    editTask(cmd.getMainParam(), "star", cmd.getOptionalParam("-s"));
                    editTask(cmd.getMainParam(), "description", cmd.getOptionalParam("-d"));
                    break;

                case (int)CommandParser.CmdCode.Ls:
                    displayTask();
                    break;

                case (int)CommandParser.CmdCode.Man:
                    manUpdate(getManString(cmd.getMainParam()));
                    sendUpdateStatus("Executing MAN");
                    break;

                // hmm i think the exit command shouldn't be handled by api
                // because it's closing the application, which is out of the scope of the api
                // ex: what happend if it's a textUI thing that doesn't have Application..Exit like WPF
                // i add it as a special handling in commandLine_keyup
                case (int)CommandParser.CmdCode.Exit:
                    sendUpdateStatus("Executing Exit");
                    break;

                case (int)CommandParser.CmdCode.TextView:
                    sendViewChange("TextUI");
                    tm.refreshDisplay();
                    break;

                case (int)CommandParser.CmdCode.GUIView:
                    sendViewChange("GUI");
                    tm.refreshDisplay();
                    break;

                case (int)CommandParser.CmdCode.Undo:
                    if (tm.revert() == 0)
                    {
                        sendUpdateStatus("Revert successfully");
                    }
                    else
                    {
                        sendUpdateStatus("At the last change");
                    }
                    break;
                case (int)CommandParser.CmdCode.Done:
                    if (cmd.getMainParam() == "")
                        tm.listDoneTasks();
                    else
                        doneTask(cmd.getMainParam(), true);
                    break;
                case (int)CommandParser.CmdCode.Star:
                    if (cmd.getMainParam() == "")
                        tm.listStarTasks();
                    else 
                        editTask(cmd.getMainParam(), "star", "True");
                    break;
                case (int)CommandParser.CmdCode.Show:
                    showTask(cmd.getMainParam());
                    break;
            }
        }

        public void showTask(string id)
        {
            int taskId = getId(id);
            if (taskId == -1)
            {
                sendUpdateStatus("Invalid taskId");
                return;
            }

            string taskInfo = tm.returnTaskAtID(taskId).ToString();
            sendUpdateStatus(taskInfo);
        }

        public string getManString(string command)
        {
            // testing purpose       
            //return "yummy text";

            if (command == "") command = "man";

            CommandParser cmd2 = new CommandParser(command);
            string cmdString = CommandParser.CmdText[(int)cmd2.getCommandCode()][0];

            const string FILEPATH = "man.xml";
            if (!File.Exists(FILEPATH))
            {
                return "Documentation file missing! \n Use 'add' command to add a task, 'rm' \nto delete and 'ls' to list tasks.";
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FILEPATH);
            XmlNodeList xmlNodeList_man = xmlDoc.ChildNodes;
            XmlNodeList xmlNodeList_commands = xmlNodeList_man[0].ChildNodes;

            string man = "";

            foreach (XmlNode x in xmlNodeList_commands)
            {
                    if (x.Name.ToString().Equals(cmdString))
                    {
                        man = x.InnerText.ToString();
                    }
            }
            return man;             
        }

        // changing the addTask, removeTask.. like this so that they won't be dependent on CommandParser
        // and can be used by mouseEventHandlers too
        public void addTask(string name, string deadline, string star, string description)
        {
            bool st = false;
            //sendUpdateStatus("adding");
            if (name == "")
            {
                sendUpdateStatus("Invalid name");
                return;
            }

            if (star == "")
            {
                st = false;
            }
            else st = true;

            tm.add(name, deadline, st, description, false);
            sendUpdateStatus("Successful add");
        }

        // structure of editTask is like this so that it can be used by the dataGrid too
        public void editTask(string id_str, string key, string value)
        {
            int taskId = getId(id_str);
            if (taskId == -1)
            {
                sendUpdateStatus("Invalid taskId");
                return;
            }
            if (value.Length == 0)
            {
                //sendUpdateStatus("Invalid value");
                return;
            }
            sendUpdateStatus("Successful edit");
            if (key.Equals("name")) tm.editName(taskId, value);
            if (key.Equals("description")) tm.editDescription(taskId, value);
            if (key.Equals("deadline")) tm.editDeadline(taskId, value);
            if (key.Equals("star"))
            {
                bool a;
                if (value.Equals("1") || value.Equals("True")) a = true;
                else a = false;
                tm.editStar(taskId, a);
            }
            if (key.Equals("done"))
            {
                bool a;
                if (value.Equals("1") || value.Equals("True")) a = true;
                else a = false;
                tm.editDone(taskId, a);
            }
        }

        public void removeTask(string id_str)
        {
            int taskId = getId(id_str);
            if (taskId == -1)
            {
                sendUpdateStatus("Invalid taskId");
                return;
            }

            tm.remove(taskId);
            sendUpdateStatus("Successful remove");
        }

        public void displayTask()
        {
            tm.listDoingTasks();
        }

        private int getId(string id_str)
        {
            int taskId;
            try
            {
                taskId = int.Parse(id_str);
            }
            catch (Exception e)
            {
                return -1;
            }
            if (!tm.hasTaskId(taskId))
            {
                return -1;
            }
            return taskId;
        }

        public void doneTask(string id, bool done)
        {
            int taskId = getId(id);
            if (taskId == -1)
            {
                sendUpdateStatus("Invalid taskId");
                return;
            }

            tm.editDone(taskId, done);
            sendUpdateStatus("Successful Done");
        }

        internal void displayDesc(Task t)
        {
            sendUpdateStatus(t.getDesc());
        }
    }
}
