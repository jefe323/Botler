using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core
{
    class tell
    {
        static public void set(string[] args, string Channel, string Nick, IrcClient irc)
        {
            DateTime tellTime = DateTime.Now;
            string message = string.Empty;

            if (args.Length == 1 || args.Length == 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "tell <nick> <message>", Nick)); }
            else
            {
                foreach (string s in args)
                    message += s + " ";
                message = message.Substring(args[0].Length + args[1].Length + 2);
                message = message.TrimEnd(' ');

                MySqlCommand command = Program.GlobalVar.conn.CreateCommand();

                Program.GlobalVar.conn.Open();
                command.Connection = Program.GlobalVar.conn;
                command.CommandText = "INSERT into tell VALUES(@to,@from,@message,@time)";
                command.Prepare();

                command.Parameters.AddWithValue("@to", args[1].ToLower());
                command.Parameters.AddWithValue("@from", Nick);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@time", tellTime.ToString());

                command.ExecuteNonQuery();
                Program.GlobalVar.conn.Close();

                try { Program.GlobalVar.tellList.Add(args[1].ToLower()); }
                catch { }

                Console.WriteLine("{0} has left a message for {1}", Nick, args[1]);
                irc.SendMessage(SendType.Message, Channel, String.Format("I will pass that along sir"));
            }
        }

        static public void get(string[] args, string Channel, string Nick, IrcClient irc)
        {
            DateTime time = DateTime.Now;
            string message = string.Empty;
            bool tellCheck = false;
            
            MySqlCommand command = Program.GlobalVar.conn.CreateCommand();
            command.CommandText = "SELECT Nick_To,Nick_From,Message,Time FROM tell where Nick_To='" + Nick.ToLower() + "'";
            try { Program.GlobalVar.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Nick_To"].ToString() == Nick.ToLower())
                {
                    tellCheck = true;
                    //Compose output
                    TimeSpan elapsed = time.Subtract(DateTime.Parse(reader["Time"].ToString()));
                    message = String.Format("({0:%d} days, {1:%h} hours, {2:%m} minutes) Sent from {3} -- {4}", elapsed, elapsed, elapsed, reader["Nick_From"].ToString(), reader["Message"].ToString());
                    //send output
                    irc.SendMessage(SendType.Message, Nick, message);

                    try { Program.GlobalVar.tellList.Remove(Nick); }
                    catch { }
                }
            }
            Program.GlobalVar.conn.Close();

            command.CommandText = "DELETE FROM tell WHERE Nick_To='" + Nick.ToLower() + "'";
            Program.GlobalVar.conn.Open();
            command.ExecuteNonQuery();
            Program.GlobalVar.conn.Close();

            if (tellCheck == false) { irc.SendMessage(SendType.Message, Nick, String.Format("I don't have any messages for you sir")); }
        }
    }
}
