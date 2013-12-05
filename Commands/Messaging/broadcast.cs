using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Messaging
{
    class broadcast
    {
        //will probably have to lock this down, still working on best strategy to do so...
        public static void sendSpecific(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "send #<channel> <message>", Nick)); }
            else
            {
                if (args[1].StartsWith("#"))
                {
                    string msg = string.Empty;
                    foreach (string s in args)
                        msg += s + " ";
                    msg = msg.Substring(args[0].Length + args[1].Length + 2);
                    msg.TrimEnd(' ');
                    Console.WriteLine(Nick + " sent: \"" + msg + "\" to: " + args[1]);
                    irc.SendMessage(SendType.Message, args[1], msg);
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("Please input a valid channel name sir")); }
            }
        }

        public static void sendAll(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "broadcast <message>", Nick)); }
            else
            {
                string msg = string.Empty;
                foreach (string s in args)
                    msg += s + " ";
                msg = msg.Substring(args[0].Length + args[1].Length + 2);
                msg.TrimEnd(' ');

                //white text on red background
                if (args[1] == "emergency")
                {
                    msg = Utilities.TextFormatting.Bold(((char)3 + "0,4" + msg + (char)3 + "0,4"));
                }
                //will add more later

                MySqlCommand command = Program.GlobalVar.conn.CreateCommand();
                command.CommandText = "SELECT Channel FROM channels";
                try { Program.GlobalVar.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    irc.SendMessage(SendType.Message, reader["Channel"].ToString(), msg);
                }
                Program.GlobalVar.conn.Close();
            }
        }
    }
}
