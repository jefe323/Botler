using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Channel
{
    class part
    {
        static public void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length > 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "part [#channel]", Nick)); }
            else
            {
                string partChan = Channel;
                bool chanCheck = false;
                if (args.Length == 2) { partChan = args[1]; }

                //make sure it is a valid channel
                if (partChan.StartsWith("#"))
                {
                    //make sure channel is in the database
                    MySqlCommand command = Program.GlobalVar.conn.CreateCommand();
                    command.CommandText = "SELECT Channel FROM channels WHERE Channel='" + partChan.ToLower() + "'";
                    try { Program.GlobalVar.conn.Open(); }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["Channel"].ToString() == partChan.ToLower())
                        {
                            chanCheck = true;
                        }
                    }
                    Program.GlobalVar.conn.Close();
                    if (chanCheck == false) { irc.SendMessage(SendType.Message, Channel, String.Format("I'm not in that channel sir")); }
                    else
                    {
                        command.CommandText = "DELETE FROM channels WHERE Channel='" + partChan.ToLower() + "'";
                        Program.GlobalVar.conn.Open();
                        command.ExecuteNonQuery();
                        Program.GlobalVar.conn.Close();

                        //Console message/info here
                        irc.SendMessage(SendType.Notice, Nick, String.Format("I am now leaving {0} sir", partChan));
                        irc.RfcPart(partChan);
                    }
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("Please input a valid channel name sir")); }
            }
        }
    }
}
