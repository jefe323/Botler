using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Rem
{
    class set
    {
        static public void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1 || args.Length == 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "remember <trigger> <message>", Nick)); }
            else
            {
                //see if rem already exists 
                bool remCheck = false;
                MySqlCommand command = Program.GlobalVar.conn.CreateCommand();
                command.CommandText = "SELECT Trig,Channel FROM rem where Trig='" + args[1] + "'";
                try { Program.GlobalVar.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Trig"].ToString() == args[1] && reader["Channel"].ToString() == Channel.ToLower())
                        remCheck = true;
                }
                Program.GlobalVar.conn.Close();

                if (remCheck == true) { irc.SendMessage(SendType.Message, Channel, String.Format("I already have something saved for {0} sir", args[1])); }
                //if not there, add it
                else
                {
                    DateTime time = DateTime.Now;
                    string message = string.Empty;
                    foreach (string s in args)
                        message += s + " ";
                    message = message.Substring(args[0].Length + args[1].Length + 2);
                    message = message.TrimEnd(' ').TrimStart(' ');

                    Program.GlobalVar.conn.Open();
                    command.Connection = Program.GlobalVar.conn;
                    command.CommandText = "INSERT into rem VALUES(@trig,@message,@nick,@channel,@time,@lck)";
                    command.Prepare();

                    command.Parameters.AddWithValue("@trig", args[1]);
                    command.Parameters.AddWithValue("@message", message);
                    command.Parameters.AddWithValue("@nick", Nick);
                    command.Parameters.AddWithValue("@channel", Channel.ToLower());
                    command.Parameters.AddWithValue("@time", time.ToString());
                    command.Parameters.AddWithValue("@lck", 0);

                    command.ExecuteNonQuery();
                    Program.GlobalVar.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} was added sir", args[1]));
                    Console.WriteLine(String.Format("{0} added ({1} -> {2}) from channel \"{3}\"", Nick, args[1], message, Channel));
                }
            }
        }
    }
}
