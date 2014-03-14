using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Rem
{
    class toggle
    {
        //restrict to bot OP or Chan OP
        static public void lck(string[] args, string Channel, string Nick, IrcClient irc)
        {
            bool remCheck = false;
            bool global = false;
            int newLCK = 0;
            bool lckType = false;
            string destination = string.Empty;
            string trigger = string.Empty;

            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "remlock <trigger>", Nick)); }

            else
            {
                if (args[1].StartsWith("+"))
                {
                    trigger = args[1].Remove(0, 1);
                    global = true;
                }
                else { trigger = args[1]; }

                MySqlCommand command = Program.GlobalVar.conn.CreateCommand();
                command.CommandText = "SELECT Trig,Channel,lck FROM rem where Trig='" + trigger + "'";
                try { Program.GlobalVar.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == Channel.ToLower())
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 0)
                        {
                            newLCK = 1;
                            lckType = true;
                        }
                        else
                        {
                            newLCK = 0;
                            lckType = false;
                        }
                        destination = Channel.ToLower();
                        break;
                    }
                    else if (global == true && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 0)
                        {
                            newLCK = 1;
                            lckType = true;
                        }
                        else
                        {
                            newLCK = 0;
                            lckType = false;
                        }
                        destination = "global";
                        break;
                    }
                    else if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 0)
                        {
                            newLCK = 1;
                            lckType = true;
                        }
                        else
                        {
                            newLCK = 0;
                            lckType = false;
                        }
                        destination = "global";
                    }
                }
                Program.GlobalVar.conn.Close();
                if (remCheck == true)
                {
                    command.CommandText = "UPDATE rem SET lck=" + newLCK + " WHERE Trig='" + trigger + "' and Channel='" + destination + "'";
                    Program.GlobalVar.conn.Open();
                    command.ExecuteNonQuery();
                    Program.GlobalVar.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} now has a lock status of {1}", trigger, lckType));
                    Console.WriteLine("{0} has set {1}'s lock status to {2}", Nick, trigger, lckType);
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have anything stored for {0} sir", trigger)); }
            }
        }

        //restrict to bot OP or Chan OP
        static public void global(string[] args, string Channel, string Nick, IrcClient irc)
        {
            bool remCheck = false;
            bool global = false;
            string newChannel = string.Empty;
            string destination = string.Empty;
            string trigger = string.Empty;

            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "global <trigger>", Nick)); }

            else
            {
                if (args[1].StartsWith("+"))
                {
                    trigger = args[1].Remove(0, 1);
                    global = true;
                }
                else { trigger = args[1]; }

                MySqlCommand command = Program.GlobalVar.conn.CreateCommand();
                command.CommandText = "SELECT Trig,Channel FROM rem where Trig='" + trigger + "'";
                try { Program.GlobalVar.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == Channel.ToLower())
                    {
                        remCheck = true;
                        newChannel = "global";
                        destination = Channel.ToLower();
                        break;
                    }
                    else if (global == true && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        newChannel = Channel.ToLower();
                        destination = "global";
                        break;
                    }
                    else if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        newChannel = Channel.ToLower();
                        destination = "global";
                    }
                }
                Program.GlobalVar.conn.Close();
                if (remCheck == true)
                {
                    command.CommandText = "UPDATE rem SET Channel='" + newChannel + "' WHERE Trig='" + trigger + "' and Channel='" + destination + "'";
                    Program.GlobalVar.conn.Open();
                    command.ExecuteNonQuery();
                    Program.GlobalVar.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} is now set to {1}", trigger, newChannel));
                    Console.WriteLine("{0} has set {1} to channel \"{2}\"", Nick, trigger, newChannel);
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have anything stored for {0} sir", trigger)); }
            }
        }
    }
}
