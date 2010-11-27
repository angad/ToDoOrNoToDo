using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Windows.Controls;
using System.Windows;

namespace View
{
    public class TextUI : UIAbstract
    {

        public override void manUpdate(object sender)
        {
            terminal.Document.Blocks.Clear();
            //terminal.AppendText((string)sender);
        }
        public override void onTaskChange(object sender, int state)
        {
            List<Task> tl = (List<Task>)sender;
            if (tl.Count == 0) display_empty();
            else display_ls(tl);
        }

        public override void enable()
        {
            grid.Visibility = System.Windows.Visibility.Visible;
        }

        public override void disable()
        {
            grid.Visibility = System.Windows.Visibility.Hidden;
        }

        private static string HEADLINE = "────┬───────────────────┬───────\n" +
                                         " ID │        Name       │  Due  \n" +
                                         "────┼───────────────────┼───────\n\n";

        private RichTextBox terminal;
        private List<Task> TaskList = new List<Task>();
        private Grid grid;

        public TextUI(Grid grd, RichTextBox tmn)
        {
            terminal = tmn;
            grid = grd;
        }

        //Output obj = new Output();        

        private void reset()
        {
            terminal.AppendText(HEADLINE);
        }

        public void display_empty()
        {
            terminal.Document.Blocks.Clear();
            terminal.AppendText("There is no task at the momment. Try add to add a task or man for more help");
        }

        public void display_ls(object output)
        {
            terminal.Document.Blocks.Clear();
            reset();

            string space_id = " ";
            string name;
            string deadline;
            string desc;

            TaskList = (List<Task>)output;
            foreach (Task tsk in TaskList)
            {
                if (tsk.getId().ToString().Length == 1) space_id = "  "; else if (tsk.getId().ToString().Length == 2) space_id = " "; else space_id = "";

                name = compute_space(tsk.getName(), 19);
                deadline = compute_space(tsk.getDeadline(), 6);

                terminal.AppendText("" + space_id + tsk.getId() + "" + (tsk.getStar() ? "*" : " ") + "│" + name + "│" + deadline + "\n");
            }
        }

        private string compute_space(string str, int len)
        {
            int length = str.Length;
            string ret_str = str;
            if (len >= length)
            {
                for (int i = 0; i < (len - length); i++)
                {
                    ret_str += " ";
                }
            }
            else ret_str = str.Substring(0, len - 2) + "..";
            return ret_str;
        }

    }
}
