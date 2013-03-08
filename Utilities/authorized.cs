using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Utilities
{
    class authorized
    {
        static public bool check(string Channel, string Nick, string type)
        {
            string returnValue = "";
            if (Nick == Program.bot_op) { returnValue = "BotOP"; }
            else if (Nick == getChanOP(Channel)) { returnValue = "ChanOP"; }
            if (returnValue == type) { return true; }
            else if (returnValue == "BotOP" && type == "ChanOP") { return true; }
            else { return false; }
        }

        static public string getChanOP(string Channel)
        {
            string op = "";
            MySqlCommand command = Program.conn.CreateCommand();
            command.CommandText = "SELECT Channel,ChanOP FROM channels WHERE Channel='" + Channel.ToLower() + "'";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Channel"].ToString() == Channel.ToLower())
                {
                    op = reader["ChanOP"].ToString();
                }
            }
            Program.conn.Close();
            return op;
        }
    }
}
