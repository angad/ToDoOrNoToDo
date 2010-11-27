using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace Model
{
    public class Task : INotifyPropertyChanged
    {
        public const int DEFAULT_PRIORITY = 2;

        private int id;
        private string name;
        private string description;
        private bool star;
        private string deadline;
        private bool done;
        //string category;

        public Task(int id, string name, string desc = "", bool star = false, string deadline = "", bool done = false)
        {
            this.id = id;
            this.name = name;
            this.description = desc;
            this.star = star;
            this.deadline = deadline;
            this.done = done;
        }

        public Task(Task tsk)
        {
            this.id = tsk.getId();
            this.name = tsk.getName();
            this.description = tsk.getDesc();
            this.star = tsk.getStar();
            this.deadline = tsk.getDeadline();
            this.done = tsk.getDone();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Task)) return false;
            Task tsk = (Task)obj;
            if (tsk.id != this.id) return false;
            if (tsk.star != this.star) return false;
            if (!tsk.name.Equals(this.name)) return false;
            if (!tsk.description.Equals(this.description)) return false;
            if (!tsk.deadline.Equals(this.deadline)) return false;
            return true;
        }

        public override string ToString()
        {
            string str = "" + id + (star?"*":"") + "." + " "+ name;
            if (deadline!="") str += "\nDue: " + deadline;
            if (description != "") str += "\nDesc: " + description;
            return str;
        }

        #region normal setter and getter

        public Task(){}

        public int getId()
        {
            return id;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
            NotifyPropertyChanged("Name");
        }

        public string getDesc()
        {
            return description;
        }

        public void setDesc(string description)
        {
            this.description = description;
        }

        public bool getStar()
        {
            return star;
        }

        public void setStar(bool star)
        {
            this.star = star;
        }

        public string getDeadline()
        {
            return deadline;
        }

        public void setDeadline(string deadline)
        {
            this.deadline = deadline;
        }

        public bool getDone()
        {
            return done;
        }

        public void setDone(bool done)
        {
            this.done = done;
        }
        #endregion

        #region getter and setter for datagrid
        
        public int ID
        {
            get {
                return id;
            }
            set
            {
                // shouldn't have setter here
                
            }
        }
        public bool Star
        {
            get
            {
                return star;
            }
            set
            {
                //this is handled separatedly by another function already
                // MessageBox.Show(""+value);
                // NotifyPropertyGonnaChange();
                // star = value;
                // NotifyPropertyChanged(/*Star*/);
            }
        }

        public bool Done
        {
            get
            {
                return done;
            }
            set
            {
                //this is handled separatedly by another function already
                // MessageBox.Show(""+value);
                // NotifyPropertyGonnaChange();
                // star = value;
                // NotifyPropertyChanged(/*Star*/);
            }
        }


        public string Name {
            get {
                return name;
            }
            set {
                NotifyPropertyGonnaChange();
                this.name = value;
                NotifyPropertyChanged(/*"Name"*/);
            }
        }
        
        public string Desc
        {
            get
            {
                return description;
            }
            set
            {
                NotifyPropertyGonnaChange();
                this.description = value;
                NotifyPropertyChanged(/*"Desc"*/);
            }
        }
        public string Deadline
        {
            get
            {
                return deadline;
            }
            set
            {
                NotifyPropertyGonnaChange();
                this.deadline = value;
                NotifyPropertyChanged(/*"deadline"*/);
            }
        }
         
        #endregion
        
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                //MessageBox.Show("property changed");
            }
        }
        #endregion

        #region BeforeChange event #for backup
        public delegate void BeforeChangeEventHandler(object sender);
        public event BeforeChangeEventHandler BeforeChangeEvent;

        private void NotifyPropertyGonnaChange()
        {
            if (BeforeChangeEvent != null)
            {
                BeforeChangeEvent(this);
                //MessageBox.Show("property changed");
            }
        }
        #endregion
    }
}
