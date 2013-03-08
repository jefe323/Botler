using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Channel
{
    class chanop
    {
        static public void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length > 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "chanop [#channel]", Nick)); }
            else
            {
                string opChan = Channel;
                bool chanCheck = false;
                if (args.Length == 2) { opChan = args[1]; }

                if (opChan.StartsWith("#"))
                {
                    MySqlCommand command = Program.conn.CreateCommand();
                    command.CommandText = "SELECT Channel,ChanOP FROM channels WHERE Channel='" + opChan.ToLower() + "'";
                    try { Program.conn.Open(); }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["Channel"].ToString() == opChan.ToLower())
                        {
                            irc.SendMessage(SendType.Message, Channel, String.Format("The Channel Operator for {0} is {1}", opChan, reader["ChanOP"].ToString()));
                            chanCheck = true;
                        }
                    }
                    Program.conn.Close();
                    if (chanCheck == false) { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have data for that channel sir")); }
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("Please input a valid channel name sir")); }
            }
        }
    }
}
