using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller.ApiPackage
{    
        class CommandParser
        {
            // index of this 3 arrays {CmdCode, CmdText, FlagText} must be linked 
            // eg: Add = 0, and first element in CmdText is text for command Add

            public enum CmdCode { Invalid = -1, Add, Remove, Edit, Ls, Man, Exit, TextView, GUIView, Undo, Done, Star, Show };
            public static string[][] CmdText = new string[][] {
                new string[] {"add"},
                new string[] {"remove", "rm", "del", "delete"},
                new string[] {"edit"},
                new string[] {"ls", "list"},
                new string[] {"man", "help", "manual"},
                new string[] {"exit", "quit"},
                new string[] {"imageek","imawesome"},
                new string[] {"iwantgui"},
                new string[] {"revert", "undo"},
                new string[] {"done"},
                new string[] {"star"},
                new string[] {"show"}
            };
            public static string[][] FlagText = new string[][]{
                new string[] {"-d", "-t", "-p", "-s"}, // ADD
                new string[] {},
                new string[] {"-n", "-d", "-t", "-p", "-s"}, //EDIT
            };

            public static string DEFAULT_PARAM_VALUE = "";
            public enum ErrorCode { NoError = -1, InvalidCommand, InvalidFlag, DoubleFlag, FlagNoValue };

            //-------------------------------------------------------------------------------------------//

            private string cmdString;
            private ErrorCode errorCode = ErrorCode.NoError;
            private int cmdCode;
            private Dictionary<String, String> opt_param;
            public  string main_parameter = "";

            public CommandParser(string command)
            {
                opt_param = new Dictionary<string, string>();

                //store the original command;
                cmdString = command;

                // add faked flag to avoid handling special cases in parsing
                command += " -";

                string commandName = command.Substring(0, command.IndexOf(" "));
                cmdCode = getCommandCode(commandName);

                // wrong command
                if (cmdCode == (int)CmdCode.Invalid)
                {
                    //errorCode = ErrorCode.InvalidCommand;
                    //return;
                    // implicit add: if user don't specify any command, it's considered as add command
                    cmdCode = (int)CmdCode.Add;
                    command = "add " + command;
                }

                // getting the remaining part
                command = command.Substring(command.IndexOf(" "));

                // the main param will be from the space to the first " -" (first flag)
                if (command.IndexOf(" -") != 0)
                {
                    main_parameter = command.Substring(0, command.IndexOf(" -")).Trim();
                }

                // parse the flags
                parseFlags(command.Substring(command.IndexOf(" -") + 1));
            }

            private void parseFlags(string cmd)
            {
                if (cmd.Length <= 2) return;
                int firstSpace = cmd.IndexOf(" ");

                string flag = cmd.Substring(0, firstSpace);

                if (!hasFlag(cmdCode, flag))
                {
                    errorCode = ErrorCode.InvalidFlag;
                    return;
                }

                string remaining = cmd.Substring(firstSpace);

                int nextFlag = remaining.IndexOf(" -");
                string value = "";

                if (nextFlag > 0)
                    value = remaining.Substring(1, nextFlag - 1);
                else
                {
                    if (cmdCode == (int)CmdCode.Add && flag == "-s")
                    {
                        value = "1";
                    } else 
                    errorCode = ErrorCode.FlagNoValue;
                }

                if (!opt_param.ContainsKey(flag))
                    opt_param.Add(flag, value.Trim());
                else { 
                    errorCode = ErrorCode.DoubleFlag;
                    return;
                }

                //parse the remaining part
                remaining = remaining.Substring(nextFlag + 1);
                parseFlags(remaining);
            }

            public int getCommandCode(string cmdText)
            {
                int nCmd = CmdText.Length;
                for (int i = 0; i < nCmd; i++)
                {
                    string[] availText = CmdText[i];
                    for (int j = 0; j < availText.Length; j++)
                    {
                        if (cmdText.Equals(availText[j]))
                        {
                            return i;
                        }
                    }
                }
                return (int)CmdCode.Invalid;
            }

            private bool hasFlag(int cmdCode, string flag)
            {
                int nCmd = FlagText.Length;
                if (cmdCode < 0 || cmdCode >= nCmd) return false;
                int nFlag = FlagText[cmdCode].Length;
                for (int i = 0; i < nFlag; i++)
                {
                    if (flag.Equals(FlagText[cmdCode][i]))
                    {
                        return true;
                    }
                }
                return false;
            }

            public string getCommandString()
            {
                return cmdString;
            }

            public int getCommandCode()
            {
                return cmdCode;
            }

            public string getMainParam()
            {
                return main_parameter;
            }

            public string getOptionalParam(string flag)
            {
                //return optional_parameter[flag]; if does not exist, return an empty string
                if (opt_param.ContainsKey(flag))
                {
                    return opt_param[flag];
                }
                else
                    // in case not found, return empty string
                    return DEFAULT_PARAM_VALUE;
            }

            public ErrorCode getErrorCode()
            {
                return errorCode;
            }

            public bool isInvalidCommand()
            {
                return errorCode != ErrorCode.NoError;
            }

            public bool noFlag()
            {
                return opt_param.Count == 0;
            }
        }   
}
