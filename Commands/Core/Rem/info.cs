using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Rem
{
    class info
    {
        static public void command(string trigger, string Channel, string Nick, IrcClient irc)
        {
            //first make sure the trigger exists
            bool remCheck = false;
            bool global = false;
            bool lck = false;
            MySqlCommand command = Program.GlobalVar.conn.CreateCommand();
            command.CommandText = "SELECT Trig,Channel,Nick,Time,lck FROM rem where Trig='" + trigger + "'";
            try { Program.GlobalVar.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Trig"].ToString() == trigger && (reader["Channel"].ToString() == Channel.ToLower() || reader["channel"].ToString() == "global"))
                {
                    remCheck = true;
                    if (reader["Channel"].ToString() == "global")
                        global = true;
                    if (Convert.ToInt32(reader["lck"]) == 1)
                        lck = true;
                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} was set by {1} at {2} (G={3}/L={4})", trigger, reader["Nick"].ToString(), reader["Time"].ToString(), global, lck));
                    global = false;
                    lck = false;
                }
            }
            Program.GlobalVar.conn.Close();
            //get info
            if (remCheck == false) { irc.SendMessage(SendType.Notice, Nick, String.Format("I don't have anything stored for {0} sir", trigger)); }
        }
    }
}
