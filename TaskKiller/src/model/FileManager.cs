using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows;


namespace Model
{
    class FileManager
    {
        TaskManager tm;

        const string FILEPATH = "C:\\Tasks\\tasks.xml";
        const string BACKUP = "C:\\Tasks\\tasks_backup.xml";
        public FileManager(TaskManager tm)
        {
            this.tm = tm;
        }

        public Boolean parseXML(string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNodeList xmlNodeList_killer = xmlDoc.ChildNodes;
                XmlNodeList xmlNodeList_tasks = xmlNodeList_killer[1].ChildNodes;

                string id, name, desc, category, deadline, star, done;
                id = name = desc = category = deadline = star = done = "";

                foreach (XmlNode x in xmlNodeList_tasks)
                    if (x.Name == "task")
                {
                    XmlNodeList xmlNodeList_task = x.ChildNodes;

                    foreach (XmlNode task in xmlNodeList_task)
                    {
                        if (task.Name.ToString().Equals("id"))
                        {
                            id = task.InnerText.ToString();
                        }
                        if (task.Name.ToString().Equals("name"))
                        {
                            name = task.InnerText.ToString();
                        }
                        if (task.Name.ToString().Equals("desc"))
                        {
                            desc = task.InnerText.ToString();
                        }
                        if (task.Name.ToString().Equals("deadline"))
                        {
                            deadline = task.InnerText.ToString();
                        }
                        if (task.Name.ToString().Equals("star"))
                        {
                            star = task.InnerText.ToString();
                        }
                        if (task.Name.ToString().Equals("category"))
                        {
                            category = task.InnerText.ToString();
                        }
                        if (task.Name.ToString().Equals("done"))
                        {
                            done = task.InnerText.ToString();
                        }
                    }
                    //tsk = new Task(Convert.ToInt32(id), name, desc, Convert.ToBoolean(star), deadline, Convert.ToBoolean(done));
//                    MessageBox.Show(name+" "+star + " " + done);
                    tm.add(name, deadline, Convert.ToBoolean(star), desc, Convert.ToBoolean(done));
                }
            }
            catch (Exception e)
            {
                throw e;
                return false;
            }
            return true;
        }

        public void loadFile(string filePath)
        {
            //IF FILE DOES
            if (!File.Exists(filePath))
            {
                if (!File.Exists(BACKUP))
                {
                    write(new List<Task>());
                }
                else
                {
                    loadFile(BACKUP);
                    return;
                }
            }

            try
            {
                parseXML(filePath);
            }

            catch (Exception e)
            {
                if (filePath.Equals(FILEPATH))
                {
                    System.Windows.MessageBox.Show("Tasks XML File Corrupted. Reverting to a backup of saved tasks.");
                    loadFile(BACKUP);
                    return;
                }
                else
                {
                    System.Windows.MessageBox.Show("Tasks XML File corrupted. Even the backup is corrupted you idiot! Making a new file for you");
                    System.IO.File.Delete(filePath);
                    System.IO.File.Delete(BACKUP);
                    loadFile(filePath);
                    return;
                }

            }
        }

        public void write(object sender, int state=0)
        {
            //asks taskmanager for the list and then writes to file
            string dir = "C:\\Tasks";

            //Create directory if does not exist
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            string filePath = "C:\\Tasks\\tasks.xml";

            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "", "");
            dec.Encoding = "UTF-8";
            xmlDoc.AppendChild(dec);

            List<Task> TaskList = tm.getAllTasks();// (List < Task > ) sender;

            XmlElement killer = xmlDoc.CreateElement("killer");
            xmlDoc.AppendChild(killer);

            foreach (Task t in TaskList)
            {
                XmlElement task = xmlDoc.CreateElement("task");
                killer.AppendChild(task);

                XmlElement id = xmlDoc.CreateElement("id");
                task.AppendChild(id);
                id.InnerText = t.getId().ToString();

                XmlElement name = xmlDoc.CreateElement("name");
                task.AppendChild(name);
                name.InnerText = t.getName();

                XmlElement desc = xmlDoc.CreateElement("desc");
                task.AppendChild(desc);
                desc.InnerText = t.getDesc();

                XmlElement deadline = xmlDoc.CreateElement("deadline");
                task.AppendChild(deadline);
                deadline.InnerText = t.getDeadline();

                XmlElement star = xmlDoc.CreateElement("star");
                task.AppendChild(star);
                star.InnerText = t.getStar().ToString();
                
                XmlElement done = xmlDoc.CreateElement("done");
                task.AppendChild(done);
                done.InnerText = t.getDone().ToString();
            }

            xmlDoc.Save(filePath);
            xmlDoc.Save(BACKUP);
        }
    }
}
