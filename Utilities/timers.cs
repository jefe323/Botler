using System;
using System.Timers;
using MySql.Data.MySqlClient;

namespace Botler.Utilities
{
    class timers
    {
        internal static void Begin()
        {
            //run every 5 minutes
            var seenTimer = new Timer { Interval = 300000, Enabled = true };
            seenTimer.Elapsed += new ElapsedEventHandler(seenTimer_Elapsed);
        }

        static void seenTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (Commands.Core.Seen.person p in Program.seenList)
            {
                //see if p.nick is already in the database
                bool check = false;
                MySqlCommand command = Program.conn.CreateCommand();
                command.CommandText = "SELECT nick,channel FROM seen where nick='" + p.nick + "' AND channel ='" + p.channel + "'";
                try { Program.conn.Open(); }
                catch (Exception d) { Console.WriteLine(d.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["nick"].ToString() == p.nick && reader["channel"].ToString() == p.channel)
                    {
                        check = true;
                    }
                }
                Program.conn.Close();
                //if is is, then update it
                if (check == true)
                {
                    Program.conn.Open();
                    command.Connection = Program.conn;
                    command.CommandText = "UPDATE seen SET time=@time, message=@message WHERE nick='" + p.nick + "' AND channel='" + p.channel + "'";
                    command.Prepare();

                    command.Parameters.AddWithValue("@time", p.time);
                    command.Parameters.AddWithValue("@message", p.message);

                    command.ExecuteNonQuery();
                    Program.conn.Close();
                }
                //if not, then add it
                else
                {
                    Program.conn.Open();
                    command.Connection = Program.conn;
                    command.CommandText = "INSERT into seen VALUES(@nick, @channel, @time, @message)";
                    command.Prepare();

                    command.Parameters.AddWithValue("@nick", p.nick);
                    command.Parameters.AddWithValue("@channel", p.channel);
                    command.Parameters.AddWithValue("@time", p.time);
                    command.Parameters.AddWithValue("@message", p.message);

                    
                    command.ExecuteNonQuery();
                    Program.conn.Close();
                }
            }
            Program.seenList.Clear();
        }
    }
}
