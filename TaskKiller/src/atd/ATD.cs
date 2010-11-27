using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Drawing;

using Controller;
using Controller.ApiPackage;
using System.ComponentModel;
using Model;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using View;
using System.IO;

/*
 * ATD: 
 * Input file: contains mutilple test cases
 * <input>
 * <test>
 *   <expected>Path_To_Expected_Output_File</expected>
 *   <command>Command 1 to execute</command>
 *   <command>Command 2 to execute</command>
 *   ...
 * </test>
 * <test> ..other tests.. </test>
 * </input>
 * File output contain expected statuses of each given command and the list of task
 * <output>
 * <status> status_of_command 1 </status>
 * <status> status_of_command 2 </status>
 * ...
 * <task> 1st task in the expected task list </task>
 * <task> 2nd task in the expected task list </task>
 * </output>
 * structure of each task is like the format we see in the status message
 * ie:
 
 id*. name
 Due: deadline
 Desc: description
 
 * 
 */

namespace ATDSpace
{
    class ATD
    {

        TaskManager tm;
        API api;

        const string FILEPATH_IN = "C:\\Tasks\\input.xml";
        const string FILEPATH_OUT = "C:\\Tasks\\output.xml";
        const string FILEPATH_LOG = "C:\\Tasks\\ATD.log";
        //const string FILEPATH_TEST = "C:\\Tasks\\output.xml";

        XmlDocument xmlDoc;
        List<string> resultTasks, expectedTasks;
        List<string> resultStatuses, expectedStatuses;
        TextWriter tw;

        public ATD()
        {
            tm = new TaskManager();
            api = new API(tm);
            tm.removeAll();
            api.StatusChangeEvent += new API.StatusChangeHandler(onStatusChange);
            tm.TaskChangeEvent += new TaskManager.TaskChangeHandler(onTaskUpdate);
            
//            ReadInputTest(FILEPATH_IN);
        }

        private void onStatusChange(object newStatus)
        {
            resultStatuses.Add((string)newStatus);     
        }

        private void onTaskUpdate(object sender, int refresh)
        {
            // compare the task with the task
            resultTasks.Clear();
            List<Task> lst = (List<Task>)sender;
            for (int i = 0; i < lst.Count; i++)
            {
                resultTasks.Add(lst[i].ToString());
            }
        }

        /**
         * testFile = input of the test.
         * if it is not specified, run the default one
         */
        public void execute(string testFile = FILEPATH_IN)
        {
            // initialize
            tw = new StreamWriter(FILEPATH_LOG);
            // run test
            RunTest(testFile);
            // close file
            tw.Close();
        }

        /*
         * Parse the input file and run tests
         */
        void RunTest(string testFile = FILEPATH_IN)
        {
            if (!File.Exists(testFile))
            {
                tw.WriteLine("File input doesn't exists");
                return;
            }

            xmlDoc = new XmlDocument();
            xmlDoc.Load(testFile);
            XmlNodeList xmlNodeList_input = xmlDoc.ChildNodes;
            XmlNodeList xmlTests = xmlNodeList_input[1].ChildNodes;
            //MessageBox.Show("asdf");
            int test = 0;
            foreach (XmlNode x in xmlTests)
            {
                if (x.Name.ToString().Equals("test"))
                {
                    tw.WriteLine("============= Test " + test + " ===============");
                    test++;

                    executeCommands(x);
                    getExpectedOutput(x);

                    tw.Write("Check Task List.. ");
                    bool checkTask = checkList<string>(resultTasks, expectedTasks);
                    tw.WriteLine(checkTask ? "OK" : "Fail");

                    tw.Write("Check Status.. ");
                    bool checkStatus = checkList<string>(resultStatuses, expectedStatuses);
                    tw.WriteLine(checkStatus ? "OK" : "Fail");
                    
                    if (checkTask && checkStatus)
                        tw.WriteLine("OK");

                    //MessageBox.Show("Test " + test + ": " + (checkStatus && checkTask));
                    tm.removeAll();
                }
            }
        }

        /*
         * Executing given commands in the test case
         */
        void executeCommands(XmlNode x)
        {
            XmlNodeList xmlCommands = x.ChildNodes;

            tm.removeAll();
            resultTasks = new List<string>();
            resultStatuses = new List<string>();
            foreach (XmlNode cmd in xmlCommands)
            {
                if (cmd.Name.ToString().Equals("command"))
                {
                    try
                    {
                        api.execute(cmd.InnerText.ToString());
                    }
                    catch (Exception e)
                    {
                        tw.WriteLine("Error in executing command: " + cmd.InnerText.ToString() + " "+e.ToString());
                    }
                }
            }
        }

        /*
         * Extracting expected output and expected status for the given test data
         */
        void getExpectedOutput(XmlNode x)
        {
            XmlNodeList xnl = x.ChildNodes;
            string FILE_EXP = "";
            expectedStatuses = new List<string>();
            expectedTasks = new List<string>();
            // get the file containing output
            foreach (XmlNode exp in xnl)
            {
                if (exp.Name.ToString().Equals("expected"))
                {
                    FILE_EXP = exp.InnerText.ToString();
                }
            }

            if (FILE_EXP == "" || !File.Exists(FILE_EXP))
            {
                tw.WriteLine("Wrong output file: "+FILE_EXP);
                return;
            }

            //get the expected statuses of each command
            expectedStatuses.Clear();
            xmlDoc = new XmlDocument();
            xmlDoc.Load(FILE_EXP);
            XmlNodeList xmlExpected = xmlDoc.ChildNodes;
            XmlNodeList xmlStatuses = xmlExpected[1].ChildNodes;

            foreach (XmlNode item in xmlStatuses)
            {
                if (item.Name.ToString().Equals("status"))
                {
                    expectedStatuses.Add(item.InnerText.ToString());
                }
                if (item.Name.ToString().Equals("task"))
                {
                    expectedTasks.Add(item.InnerText.ToString());
                }
            }

        }
                
        /*
         * This function compare two lists
         * Even if the two lists are different in size, it will compare upto the last element of the shorter list
         */
        private bool checkList<T>(List<T> actual, List<T> expected)
        {
            bool check = true;
            if (actual.Count != expected.Count)
            {
                tw.WriteLine("\nERROR: Two lists have different size: " + actual.Count + " vs. " + expected.Count);
                check = false;
            }
            for(int i=0;i<Math.Min(actual.Count, expected.Count);i++){
                if (!actual[i].Equals(expected[i])){
                    if (check) tw.WriteLine();
                    check = false;
                    tw.WriteLine("ERROR: differ at item "+i+": "+actual[i].ToString()+" vs. "+expected[i].ToString());
                }
            }
            return check;
        }
    }            
}
