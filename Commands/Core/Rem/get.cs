using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Rem
{
    class get
    {
        static public void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            string trigger = string.Empty;
            string message = string.Empty;
            bool remCheck = false;
            bool global = false;
            bool infoCheck = false;

            //parse the proper trigger
            if (args[0].StartsWith("?"))
            {
                trigger = args[0].Remove(0, 1);
                infoCheck = true;
                Botler.Commands.Core.Rem.info.command(trigger, Channel, Nick, irc);
            }
            else if (args[0].StartsWith("+"))
            {
                trigger = args[0].Remove(0, 1);
                global = true;
            }
            else { trigger = args[0]; }


            if (infoCheck == false)
            {
                MySqlCommand command = Program.conn.CreateCommand();
                command.CommandText = "SELECT Trig,Channel,Message FROM rem where Trig='" + trigger + "'";
                try { Program.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //PARSE MYSQL STRING LITERALS!!!!!!
                    if (reader["trig"].ToString() == trigger)
                    {
                        if (global == true && reader["Channel"].ToString() == "global")
                        {
                            //use global version
                            remCheck = true;
                            message = reader["Message"].ToString();
                            break;
                        }
                        else if (global == false && reader["Channel"].ToString() == Channel.ToLower())
                        {
                            //use local version
                            remCheck = true;
                            message = reader["Message"].ToString();
                            break;
                        }
                        else if (global == false && reader["Channel"].ToString() == "global")
                        {
                            //will pull global version and keep it unless a local version is found
                            remCheck = true;
                            message = reader["Message"].ToString();
                        }
                    }
                }
                Program.conn.Close();
                #region output
                if (remCheck == true)
                {
                    if (args.Length == 1)
                    {
                        irc.SendMessage(SendType.Message, Channel, message);
                    }
                    else if (args.Length == 2)
                    {
                        try
                        {
                            irc.SendMessage(SendType.Message, Channel, String.Format(message, args[1]));
                        }
                        catch
                        {
                            irc.SendMessage(SendType.Message, Channel, "Invalid or missing parameters");
                        }
                    }
                    else if (args.Length == 3)
                    {
                        try
                        {
                            irc.SendMessage(SendType.Message, Channel, String.Format(message, args[1], args[2]));
                        }
                        catch
                        {
                            irc.SendMessage(SendType.Message, Channel, "Invalid or missing parameters");
                        }
                    }
                    else if (args.Length == 4)
                    {
                        try
                        {
                            irc.SendMessage(SendType.Message, Channel, String.Format(message, args[1], args[2], args[3]));
                        }
                        catch
                        {
                            irc.SendMessage(SendType.Message, Channel, "Invalid or missing parameters");
                        }
                    }
                #endregion
                }
            }
        }
    }
}
