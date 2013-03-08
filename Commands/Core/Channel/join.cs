using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Channel
{
    class join
    {
        static public void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "join #<channel>", Nick)); }
            else
            {
                //make sure entry is a valid channel
                if (args[1].StartsWith("#"))
                {
                    //see if the channel already exists
                    bool chanCheck = false;
                    MySqlCommand command = Program.conn.CreateCommand();
                    command.CommandText = "SELECT Channel FROM channels where Channel='" + args[1].ToLower() + "'";
                    try { Program.conn.Open(); }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["Channel"].ToString() == args[1].ToLower())
                        {
                            chanCheck = true;
                        }
                    }
                    Program.conn.Close();

                    if (chanCheck == true) { irc.SendMessage(SendType.Message, Channel, String.Format("I am already in {0} sir", args[1])); }
                    //channel is not in the database so add it
                    else
                    {
                        command.CommandText = "INSERT into channels (Channel,ChanOP,SuperUsers,Quiet,secret) values('" + args[1].ToLower() + "','" + Nick + "','','0','0')";
                        Program.conn.Open();
                        command.ExecuteNonQuery();
                        Program.conn.Close();

                        //Console message/info here
                        irc.SendMessage(SendType.Notice, Nick, String.Format("I am now joining {0} sir", args[1]));
                        irc.RfcJoin(args[1]);
                    }
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("Please input a valid channel name sir")); }
            }
        }
    }
}
