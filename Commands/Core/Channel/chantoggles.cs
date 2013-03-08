using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core.Channel
{
    class chantoggles
    {
        static public void setSecret(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length > 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "setSecret [#channel]", Nick)); }
            else
            {
                string secChan = Channel;
                bool chanCheck = false;
                int secret = 0;
                if (args.Length == 2) { secChan = args[1]; }

                if (secChan.StartsWith("#"))
                {
                    MySqlCommand command = Program.conn.CreateCommand();
                    command.CommandText = "SELECT Channel,secret FROM channels WHERE Channel='" + secChan.ToLower() + "'";
                    try { Program.conn.Open(); }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["Channel"].ToString() == secChan.ToLower())
                        {
                            if (Convert.ToInt32(reader["secret"]) == 0) { secret = 1; }
                            else { secret = 2; }
                            chanCheck = true;
                        }
                    }
                    Program.conn.Close();
                    if (chanCheck == false) { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have data for that channel sir")); }
                    if (secret == 1)
                    {
                        command.CommandText = "UPDATE channels SET secret=1 WHERE Channel='" + secChan.ToLower() + "'";
                        Program.conn.Open();
                        command.ExecuteNonQuery();
                        Program.conn.Close();

                        irc.SendMessage(SendType.Notice, Nick, String.Format("Secret has been set on {0} sir", secChan));
                    }
                    else if (secret == 2)
                    {
                        command.CommandText = "UPDATE channels SET secret=0 WHERE Channel='" + secChan.ToLower() + "'";
                        Program.conn.Open();
                        command.ExecuteNonQuery();
                        Program.conn.Close();

                        irc.SendMessage(SendType.Notice, Nick, String.Format("Secret has been removed on {0} sir", secChan));
                    }
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("Please input a valid channel name sir")); }
            }
        }

        static public void getSecret(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "getSecret", Nick)); }
            else
            {
                MySqlCommand command = Program.conn.CreateCommand();
                command.CommandText = "SELECT Channel,secret FROM channels WHERE Channel='" + Channel.ToLower() + "'";
                try { Program.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Channel"].ToString() == Channel.ToLower())
                    {
                        if (Convert.ToInt32(reader["secret"]) == 0) { irc.SendMessage(SendType.Message, Channel, String.Format("{0} is currently set to Not Secret", Channel)); }
                        else { irc.SendMessage(SendType.Message, Channel, String.Format("{0} is currently set to Secret", Channel)); }
                    }
                }
                Program.conn.Close();
            }
        }

        static public void setQuiet(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length > 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "setQuiet [#channel]", Nick)); }
            else
            {
                string quietChan = Channel;
                bool chanCheck = false;
                int quiet = 0;
                if (args.Length == 2) { quietChan = args[1]; }

                if (quietChan.StartsWith("#"))
                {
                    MySqlCommand command = Program.conn.CreateCommand();
                    command.CommandText = "SELECT Channel,Quiet FROM channels WHERE Channel='" + quietChan.ToLower() + "'";
                    try { Program.conn.Open(); }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["Channel"].ToString() == quietChan.ToLower())
                        {
                            if (Convert.ToInt32(reader["Quiet"]) == 0) { quiet = 1; }
                            else { quiet = 2; }
                            chanCheck = true;
                        }
                    }
                    Program.conn.Close();
                    if (chanCheck == false) { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have data for that channel sir")); }
                    if (quiet == 1)
                    {
                        command.CommandText = "UPDATE channels SET Quiet=1 WHERE Channel='" + quietChan.ToLower() + "'";
                        Program.conn.Open();
                        command.ExecuteNonQuery();
                        Program.conn.Close();

                        irc.SendMessage(SendType.Notice, Nick, String.Format("Quiet has been set on {0} sir", quietChan));
                    }
                    else if (quiet == 2)
                    {
                        command.CommandText = "UPDATE channels SET Quiet=0 WHERE Channel='" + quietChan.ToLower() + "'";
                        Program.conn.Open();
                        command.ExecuteNonQuery();
                        Program.conn.Close();

                        irc.SendMessage(SendType.Notice, Nick, String.Format("Quiet has been removed on {0} sir", quietChan));
                    }
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("Please input a valid channel name sir")); }
            }
        }

        static public void getQuiet(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length > 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "getQuiet [#channel]", Nick)); }
            else
            {
                string quietChan = Channel;
                bool chanCheck = false;
                if (args.Length == 2) { quietChan = args[1]; }

                if (quietChan.StartsWith("#"))
                {
                    MySqlCommand command = Program.conn.CreateCommand();
                    command.CommandText = "SELECT Channel,Quiet FROM channels WHERE Channel='" + quietChan.ToLower() + "'";
                    try { Program.conn.Open(); }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["Channel"].ToString() == quietChan.ToLower())
                        {
                            if (Convert.ToInt32(reader["Quiet"]) == 0) { irc.SendMessage(SendType.Notice, Nick, String.Format("{0} is currently set to Not Quiet", Channel)); }
                            else { irc.SendMessage(SendType.Notice, Nick, String.Format("{0} is currently set to Quiet", Channel)); }
                            chanCheck = true;
                        }
                    }
                    Program.conn.Close();
                    if (chanCheck == false) { irc.SendMessage(SendType.Notice, Nick, String.Format("I don't have data on that channel sir")); }
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("Please input a valid channel name sir")); }
            }
        }
    }
}
