using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using Controller.ApiPackage;
using System.Windows;
/**
 * The Controller, which contain listener to Events in View
 * Modify both View and Model
 */

namespace Controller
{
    class CommandLineHanlder
    {
        API api;
        
        public CommandLineHanlder(API apiRef)
        {
            api = apiRef;
        }


        public void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Clear();
            tb.Focus();
        }

        public void GotFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Equals(""))
            {
                tb.Text = "Enter Command";
            }
        }

        public void LostFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Equals(""))
            {
                tb.Text = "Enter Command";
            }
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Enter Command")
            {
                tb.Text = "";
            }
        }

        // triggered when user releases key in commandLine. handle if it's a command
        public void KeyUp(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox) sender;
            
            if (tb.Text.Equals(""))
            {
                tb.Text = "Enter Command";
            }

            if (e.Key.ToString().Equals("Return"))
            {
                string command = tb.Text.ToString();
                tb.Clear();

                //disable the commandline for awhile
                tb.KeyUp -= new KeyEventHandler(KeyUp);
                
                if (command.Equals("exit") || command.Equals("quit"))
                {
                    Application.Current.MainWindow.Close();
                }

                // pass the cmd to logic
                if (!command.Equals("Enter Command"))
                api.execute(command);

                // attach the event handler once we finish the command
                tb.KeyUp += new KeyEventHandler(KeyUp);
            }
        }
    }
}
