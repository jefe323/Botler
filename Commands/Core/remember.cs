using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler.Commands.Core
{
    class remember
    {
        static public void set(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1 || args.Length == 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "remember <trigger> <message>", Nick)); }
            else
            {
                //see if rem already exists 
                bool remCheck = false;
                MySqlCommand command = Program.conn.CreateCommand();                                        
                command.CommandText = "SELECT Trig,Channel FROM rem where Trig='" + args[1] + "'";
                try { Program.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Trig"].ToString() == args[1] && reader["Channel"].ToString() == Channel.ToLower())
                        remCheck = true;
                }
                Program.conn.Close();
                
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

                    command.CommandText = "INSERT into rem (Trig,Message,Nick,Channel,Time) values('"+ args[1] + "','" + message + "','" + Nick + "','" + Channel.ToLower() + "','" + time.ToString() + "')";
                    Program.conn.Open();
                    command.ExecuteNonQuery();
                    Program.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} was added sir", args[1]));
                    Console.WriteLine(String.Format("{0} added ({1} -> {2}) from channel \"{3}\"", Nick, args[1], message, Channel));
                }
            }
        }

        static public void get(string[] args, string Channel, string Nick, IrcClient irc)
        {
            string trigger = string.Empty;
            string message = string.Empty;
            bool remCheck = false;
            bool global = false;
            bool infoCheck = false;
            
            //parse the proper trigger
            if (args[0].StartsWith("?")) 
            { 
                trigger = args[0].Remove(0, 1);
                infoCheck = true;
                info(trigger, Channel, Nick, irc);
            }
            else if (args[0].StartsWith("+")) 
            { 
                trigger = args[0].Remove(0, 1);
                global = true;
            }
            else { trigger = args[0]; }

            
            if (infoCheck == false)
            {
                MySqlCommand command = Program.conn.CreateCommand();
                command.CommandText = "SELECT Trig,Channel,Message FROM rem where Trig='" + trigger + "'";
                try { Program.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //PARSE MYSQL STRING LITERALS!!!!!!
                    if (reader["trig"].ToString() == trigger)
                    {
                        if (global == true && reader["Channel"].ToString() == "global")
                        {
                            //use global version
                            remCheck = true;
                            message = reader["Message"].ToString();
                            break;
                        }
                        else if (global == false && reader["Channel"].ToString() == Channel.ToLower())
                        {
                            //use local version
                            remCheck = true;
                            message = reader["Message"].ToString();
                            break;
                        }
                        else if (global == false && reader["Channel"].ToString() == "global")
                        {
                            //will pull global version and keep it unless a local version is found
                            remCheck = true;
                            message = reader["Message"].ToString();
                        }
                    }
                }
                Program.conn.Close();
                #region output
                if (remCheck == true)
                {
                    if (args.Length == 1)
                    {
                        irc.SendMessage(SendType.Message, Channel, message);
                    }
                    else if (args.Length == 2)
                    {
                        try
                        {
                            irc.SendMessage(SendType.Message, Channel, String.Format(message, args[1]));
                        }
                        catch
                        {
                            irc.SendMessage(SendType.Message, Channel, "Invalid or missing parameters");
                        }
                    }
                    else if (args.Length == 3)
                    {
                        try
                        {
                            irc.SendMessage(SendType.Message, Channel, String.Format(message, args[1], args[2]));
                        }
                        catch
                        {
                            irc.SendMessage(SendType.Message, Channel, "Invalid or missing parameters");
                        }
                    }
                    else if (args.Length == 4)
                    {
                        try
                        {
                            irc.SendMessage(SendType.Message, Channel, String.Format(message, args[1], args[2], args[3]));
                        }
                        catch
                        {
                            irc.SendMessage(SendType.Message, Channel, "Invalid or missing parameters");
                        }
                    }
                #endregion
                }
            }
        }       

        static public void remove(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "forget <trigger>", Nick)); }
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

                MySqlCommand command = Program.conn.CreateCommand();
                command.CommandText = "SELECT Trig,Channel,lck FROM rem where Trig='" + trigger + "'";
                try { Program.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == Channel.ToLower())
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 1) { lckCheck = true ; }
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
                Program.conn.Close();

                if (lckCheck == true)
                {
                    if (authorized(Channel, Nick, "ChanOP") != true) { perm = false; }
                }

                if (remCheck == true && perm == true)
                {
                    command.CommandText = "DELETE FROM rem WHERE Trig='" + trigger + "' and Channel='" + chan + "'";
                    Program.conn.Open();
                    command.ExecuteNonQuery();
                    Program.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} was removed sir", args[1]));
                    Console.WriteLine(String.Format("{0} removed {1} from channel \"{2}\"", Nick, args[1], chan));
                }
                else if (perm == false) { irc.SendMessage(SendType.Message, Channel, String.Format("{0} is locked and you don't have permission to remove it sir", trigger)); }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have anything stored for {0} sir", trigger)); }
            }
        }

        //restrict to bot OP or Chan OP
        static public void lck(string[] args, string Channel, string Nick, IrcClient irc)
        {
            bool remCheck = false;
            bool global = false;
            int newLCK = 0;
            bool lckType = false;
            string destination = string.Empty;
            string trigger = string.Empty;

            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "remlock <trigger>", Nick)); }

            else
            {
                if (args[1].StartsWith("+"))
                {
                    trigger = args[1].Remove(0, 1);
                    global = true;
                }
                else { trigger = args[1]; }

                MySqlCommand command = Program.conn.CreateCommand();
                command.CommandText = "SELECT Trig,Channel,lck FROM rem where Trig='" + trigger + "'";
                try { Program.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == Channel.ToLower())
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 0)
                        {
                            newLCK = 1;
                            lckType = true;
                        }
                        else
                        {
                            newLCK = 0;
                            lckType = false;
                        }
                        destination = Channel.ToLower();
                        break;
                    }
                    else if (global == true && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 0)
                        {
                            newLCK = 1;
                            lckType = true;
                        }
                        else
                        {
                            newLCK = 0;
                            lckType = false;
                        }
                        destination = "global";
                        break;
                    }
                    else if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        if (Convert.ToInt32(reader["lck"]) == 0)
                        {
                            newLCK = 1;
                            lckType = true;
                        }
                        else
                        {
                            newLCK = 0;
                            lckType = false;
                        }
                        destination = "global";
                    }
                }
                Program.conn.Close();
                if (remCheck == true)
                {
                    command.CommandText = "UPDATE rem SET lck=" + newLCK + " WHERE Trig='" + trigger + "' and Channel='" + destination + "'";
                    Program.conn.Open();
                    command.ExecuteNonQuery();
                    Program.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} now has a lock status of {1}", trigger, lckType));
                    Console.WriteLine("{0} has set {1}'s lock status to {2}", Nick, trigger, lckType);
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have anything stored for {0} sir", trigger)); }
            }
        }

        //restrict to bot OP or Chan OP
        static public void global(string[] args, string Channel, string Nick, IrcClient irc)
        {
            bool remCheck = false;
            bool global = false;
            string newChannel = string.Empty;
            string destination = string.Empty;
            string trigger = string.Empty;

            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.bot_comm_char + "global <trigger>", Nick)); }

            else
            {
                if (args[1].StartsWith("+"))
                {
                    trigger = args[1].Remove(0, 1);
                    global = true;
                }
                else { trigger = args[1]; }

                MySqlCommand command = Program.conn.CreateCommand();
                command.CommandText = "SELECT Trig,Channel FROM rem where Trig='" + trigger + "'";
                try { Program.conn.Open(); }
                catch (Exception e) { Console.WriteLine(e.Message); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == Channel.ToLower())
                    {
                        remCheck = true;
                        newChannel = "global";
                        destination = Channel.ToLower();
                        break;
                    }
                    else if (global == true && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        newChannel = Channel.ToLower();
                        destination = "global";
                        break;
                    }
                    else if (global == false && reader["Trig"].ToString() == trigger && reader["Channel"].ToString() == "global")
                    {
                        remCheck = true;
                        newChannel = Channel.ToLower();
                        destination = "global";
                    }
                }
                Program.conn.Close();
                if (remCheck == true)
                {
                    command.CommandText = "UPDATE rem SET Channel='" + newChannel + "' WHERE Trig='" + trigger + "' and Channel='" + destination + "'";
                    Program.conn.Open();
                    command.ExecuteNonQuery();
                    Program.conn.Close();

                    irc.SendMessage(SendType.Message, Channel, String.Format("{0} is now set to {1}", trigger, newChannel));
                    Console.WriteLine("{0} has set {1} to channel \"{2}\"", Nick, trigger, newChannel);
                }
                else { irc.SendMessage(SendType.Message, Channel, String.Format("I don't have anything stored for {0} sir", trigger)); }
            }
        }

        static private void info(string trigger, string Channel, string Nick, IrcClient irc)
        {
            //first make sure the trigger exists
            bool remCheck = false;
            bool global = false;
            bool lck = false;
            MySqlCommand command = Program.conn.CreateCommand();
            command.CommandText = "SELECT Trig,Channel,Nick,Time,lck FROM rem where Trig='" + trigger + "'";
            try { Program.conn.Open(); }
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
            Program.conn.Close();
            //get info
            if (remCheck == false) { irc.SendMessage(SendType.Notice, Nick, String.Format("I don't have anything stored for {0} sir", trigger)); }
        }

        static private bool authorized(string Channel, string Nick, string type)
        {
            string returnValue = "";
            if (Nick == Program.bot_op) { returnValue = "BotOP"; }
            else if (Nick == getChanOP(Channel)) { returnValue = "ChanOP"; }
            if (returnValue == type) { return true; }
            else if (returnValue == "BotOP" && type == "ChanOP") { return true; }
            else { return false; }
        }

        static private string getChanOP(string Channel)
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
