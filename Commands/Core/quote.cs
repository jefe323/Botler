using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core
{
    class quote
    {
        //addquote <name> <message>
        static public void set(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length <= 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "addquote <nick> <message>", Nick)); }
            else
            {
                MySqlCommand command = Program.GlobalVar.conn.CreateCommand();

                //rebuild message
                string message = string.Empty;
                foreach (string s in args)
                    message += s + " ";
                message = message.Substring(args[0].Length + args[1].Length + 2);
                message = message.TrimEnd(' ');

                try
                {
                    Program.GlobalVar.conn.Open();
                    command.Connection = Program.GlobalVar.conn;
                    command.CommandText = "INSERT into quote VALUES(@nick,@message)";
                    command.Prepare();

                    command.Parameters.AddWithValue("@nick", args[1]);
                    command.Parameters.AddWithValue("@message", message);

                    command.ExecuteNonQuery();
                    Program.GlobalVar.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("I have added the quote sir, I hope it is very embarrassing"));
                }
                catch (Exception e) { Botler.Utilities.TextFormatting.ConsoleERROR(e.Message + "\n"); }
            }
        }

        //quote <nick>
        static public void get(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "quote <nick>", Nick)); }
            else
            {
                MySqlCommand command = Program.GlobalVar.conn.CreateCommand();

                //first get total number of quotes by nick
                command.CommandText = "SELECT COUNT(nick) FROM quote WHERE nick='" + args[1] + "'";
                try { Program.GlobalVar.conn.Open(); }
                catch (Exception e) { Botler.Utilities.TextFormatting.ConsoleERROR(e.Message + "\n"); }
                object result = command.ExecuteScalar();
                Program.GlobalVar.conn.Close();

                int max = Convert.ToInt32(result);
                Console.WriteLine(max);

                //select random number using result as max
                Random r = new Random();
                int x = r.Next(Convert.ToInt32(result));

                if (result != null && max != 0)
                {
                    command.CommandText = "SELECT nick,message FROM quote WHERE nick='" + args[1] + "' LIMIT " + x + ",1";
                    try { Program.GlobalVar.conn.Open(); }
                    catch (Exception e) { Botler.Utilities.TextFormatting.ConsoleERROR(e.Message + "\n"); }
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        irc.SendMessage(SendType.Message, Channel, String.Format("(1/{2})<{0}> {1}", reader["nick"].ToString(), reader["message"].ToString(), max));
                    }
                    Program.GlobalVar.conn.Close();
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("I don't seem to have any quotes for the nick sir")); }
            }
        }
    }
}
