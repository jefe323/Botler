using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core
{
    class blacklist
    {
        //blacklist <nick> <host>
        static public void set(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 3) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "blacklist <nick> <host>", Nick)); }
            else
            {
                MySqlCommand command = Program.conn.CreateCommand();

                try
                {
                    Program.conn.Open();
                    command.Connection = Program.conn;
                    command.CommandText = "INSERT into blacklist VALUES(@nick,@host)";
                    command.Prepare();

                    command.Parameters.AddWithValue("@nick", args[1]);
                    command.Parameters.AddWithValue("@host", args[2]);

                    command.ExecuteNonQuery();
                    Program.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("I have added {0}@{1} to the blacklist sir, we don't want naughty people using me", args[1], args[2]));
                }
                catch (Exception e) { Botler.Utilities.TextFormatting.ConsoleERROR(e.Message + "\n"); }
            }
        }
    }
}
