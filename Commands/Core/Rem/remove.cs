using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Rem
{
    class remove
    {
        static public void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "forget <trigger>", Nick)); }
            else
            {
                bool remCheck = false;
                bool global = false;
                bool lckCheck = false;
                bool perm = true;
                string chan = string.Empty;
                string trigger = string.Empty;

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
                        if (Convert.ToInt32(reader["lck"]) == 1) { lckCheck = true; }
                        chan = Channel.ToLower();
                        break;
                    }
                    else if (global == true && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 1) { lckCheck = true; }
                        chan = "global";
                        break;
                    }
                    else if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 1) { lckCheck = true; }
                        chan = "global";
                    }
                }
                Program.GlobalVar.conn.Close();

                if (lckCheck == true)
                {
                    if (Botler.Utilities.authorized.check(Channel, Nick, "ChanOP") != true) { perm = false; }
                }

                if (remCheck == true && perm == true)
                {
                    command.CommandText = "DELETE FROM rem WHERE Trig='" + trigger + "' and Channel='" + chan + "'";
                    Program.GlobalVar.conn.Open();
                    command.ExecuteNonQuery();
                    Program.GlobalVar.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} was removed sir", args[1]));
                    Console.WriteLine(String.Format("{0} removed {1} from channel \"{2}\"", Nick, args[1], chan));
                }
                else if (perm == false) { irc.SendMessage(SendType.Message, Channel, String.Format("{0} is locked and you don't have permission to remove it sir", trigger)); }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have anything stored for {0} sir", trigger)); }
            }
        }
    }
}
