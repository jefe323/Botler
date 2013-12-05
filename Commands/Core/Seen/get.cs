using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Botler.Commands.Core.Seen
{
    class get
    {
        static public void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "seen <nick>", Nick)); }
            else
            {
                if (Nick.ToLower() == args[1].ToLower()) { irc.SendMessage(SendType.Message, Channel, String.Format("You were last seen now saying \"{0}seen {1}\" you silly goose", Program.GlobalVar.bot_comm_char, args[1])); }
                else
                {
                    DateTime time = DateTime.Now;
                    bool listCheck = false;
                    bool found = false;
                    //first check seenList
                    List<person> response = Program.GlobalVar.seenList.FindAll(x => x.nick == args[1].ToLower());
                    foreach (person p in response)
                    {
                        if (response != null && (p.nick == args[1].ToLower() && p.channel == Channel))
                        {
                            //found
                            listCheck = true;
                            found = true;
                            TimeSpan elapsed = time.Subtract(DateTime.Parse(p.time));
                            irc.SendMessage(SendType.Message, Channel, String.Format("{0} was last seen here saying \"{1}\" ({2:%d} days, {3:%h} hours, {4:%m} minutes ago)", p.nick, p.message, elapsed, elapsed, elapsed));
                            break;
                        }
                    }
                    //then check seen table
                    if (listCheck == false)
                    {
                        MySqlCommand command = Program.GlobalVar.conn.CreateCommand();
                        command.CommandText = "SELECT nick,channel,time,message FROM seen WHERE nick='" + args[1].ToLower() + "' AND channel='" + Channel + "'";
                        try { Program.GlobalVar.conn.Open(); }
                        catch (Exception e) { Console.WriteLine(e.Message); }
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["channel"].ToString() == Channel && reader["nick"].ToString() == args[1].ToLower())
                            {
                                TimeSpan elapsed = time.Subtract(DateTime.Parse(reader["time"].ToString()));
                                irc.SendMessage(SendType.Message, Channel, String.Format("{0} was last seen here saying \"{1}\" ({2:%d} days, {3:%h} hours, {4:%m} minutes ago)", reader["nick"].ToString(), reader["message"].ToString(), elapsed, elapsed, elapsed));
                                found = true;
                            }
                        }
                        Program.GlobalVar.conn.Close();
                    }

                    if (found == false) { irc.SendMessage(SendType.Message, Channel, String.Format("It appears that nick hasn't talked in this channel yet")); }
                }
            }
        }
    }
}
