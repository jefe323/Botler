using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.IO;
using System.Threading;
using Botler.Utilities;

namespace Botler
{
    class Program
    {
        static internal string version = "2.0";
        static internal int currentDBVersion = 1;

        public static IrcClient irc = new IrcClient();
        static internal string connString;
        static internal MySqlConnection conn;

        static internal List<string> tellList = new List<string>();
        static internal List<Commands.Core.Seen.person> seenList = new List<Commands.Core.Seen.person>();

        static internal bool active = true;

        //settings variables
        /////////////////////////////////////////////
        static internal int dbVersion = 0;
        static internal string irc_server = "";
        static internal int irc_port = 0;
        static internal string irc_channel = "";
        static internal string bot_nick = "";
        static internal string bot_op = "";
        static internal string bot_ident = "";
        static internal string bot_comm_char = "";
        static internal string mysql_server = "";
        static internal string mysql_port = "";
        static internal string mysql_database = "";
        static internal string mysql_user = "";
        static internal string mysql_password = "";
        /////////////////////////////////////////////

        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            Console.Title = "Botler - C# IRC Bot";

            ///////////////////////
            //Set various connection info
            ///////////////////////
            start.StepOne();

            connString = "Server=" + mysql_server + ";Port=" + mysql_port + ";Database=" + mysql_database + ";Uid=" + mysql_user + ";password=" + mysql_password + ";";
            conn = new MySqlConnection(connString);

            irc.Encoding = System.Text.Encoding.UTF8;
            irc.ActiveChannelSyncing = true;
            irc.AutoReconnect = true;
            irc.AutoRetry = true;
            irc.AutoRelogin = true;
            irc.AutoJoinOnInvite = true;

            start.StepTwo();

            ///////////////////////
            //Set up event handlers
            ///////////////////////
            irc.OnChannelMessage += new IrcEventHandler(irc_OnChannelMessage);
            irc.OnQueryMessage += new IrcEventHandler(irc_OnQueryMessage);
            irc.OnInvite += new InviteEventHandler(irc_OnInvite);
            irc.OnConnected += new EventHandler(irc_OnConnected);
            irc.OnError += new Meebey.SmartIrc4net.ErrorEventHandler(irc_OnError);
            irc.OnKick += new KickEventHandler(irc_OnKick);
            irc.OnNickChange += new NickChangeEventHandler(irc_OnNickChange);
            irc.OnDisconnected += new EventHandler(irc_OnDisconnected);

            ///////////////////////
            //Connect to irc server
            ///////////////////////

            Console.Write("Identify? (y/n): ");
            string ident = Console.ReadLine();
            if (ident == "y")
            {
                bot_ident = Ident();
                Console.WriteLine();
            }
            Console.WriteLine("Connecting to " + irc_server);

            try { irc.Connect(irc_server, irc_port); }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect, reason: " + e.Message);
                Exit();
            }

            try
            {
                irc.Login("Botler", "Botler", 1, "Botler");
                //irc.RfcJoin(channel);
                try { JoinChannels(); }
                catch (Exception e)
                {
                    TextFormatting.ConsoleERROR(e.Message + "\n");
                }
                irc.SendMessage(SendType.Message, "NickServ", "identify " + bot_ident);
                Utilities.timers.Begin();

                new Thread(new ThreadStart(ReadCommands)).Start();

                while (active)
                {
                    irc.ListenOnce();
                }
                irc.Disconnect();
            }
            catch (Exception e) {
                Console.WriteLine("I dun goofed");
                Console.WriteLine(e.Message);
                //Exit();
            }
        }

        static void irc_OnDisconnected(object sender, EventArgs e)
        {
            bool connected = false;
            Console.WriteLine("Lost connection to server, attempting to reconnect...");

            while (connected == false)
            {
                try 
                { 
                    irc.Connect(irc_server, irc_port);
                    irc.Login("Botler", "Botler", 1, "Botler");
                    //irc.RfcJoin(channel);
                    JoinChannels();
                    connected = true;
                }
                catch (Exception fail)
                {
                    Console.WriteLine("Failed to connect, trying again in 30 seconds...");
                    Thread.Sleep(30000);
                }
            }
        }

        static void irc_OnConnected(object sender, EventArgs e)
        {
            Console.WriteLine("\nCONNECTED!\n");
        }

        static void irc_OnNickChange(object sender, NickChangeEventArgs e)
        {
            //check for nicks on both old and new nicks
            showTells(e.NewNickname);
            showTells(e.OldNickname);
        }

        static void irc_OnInvite(object sender, InviteEventArgs e)
        {
            //make equal to .join command
            string[] jArgs = { "join", e.Data.Channel };
            Commands.Core.Channel.join.command(jArgs, e.Data.Channel, e.Data.Nick, irc);
        }

        static void irc_OnError(object sender, Meebey.SmartIrc4net.ErrorEventArgs e)
        {
            Console.WriteLine("ERROR: " + e.ErrorMessage);
        }

        static void irc_OnKick(object sender, KickEventArgs e)
        {
            if (e.Whom == bot_nick)
            {
                //make same as part
                string[] pArgs = { "part", e.Data.Channel };
                Commands.Core.Channel.part.command(pArgs, e.Data.Channel, e.Data.Nick, irc);
            }
        }

        static void irc_OnChannelMessage(object sender, IrcEventArgs e)
        {
            Commands.Core.Seen.set.go(e.Data.Nick, e.Data.Channel, e.Data.Message);
            
            if (tellList.Contains(e.Data.Nick.ToLower()) && !e.Data.Message.StartsWith(String.Format("{0}showtell", bot_comm_char)) && !e.Data.Message.StartsWith(String.Format("{0}showtells", bot_comm_char)) && !e.Data.Message.StartsWith(String.Format("{0}st", bot_comm_char)))
            {
                showTells(e.Data.Nick);
            }
            if (e.Data.Message.StartsWith(bot_comm_char))
            {
                bool bl = blacklist(e.Data.Nick.ToLower(), e.Data.Host);
                if (bl == false || e.Data.Nick == bot_op) { run.Command(e.Data.Channel, e.Data.Nick, e.Data.Message, irc); }
            }
        }

        static void irc_OnQueryMessage(object sender, IrcEventArgs e)
        {
            if (e.Data.Message.StartsWith(bot_comm_char))
            {
                bool bl = blacklist(e.Data.Nick.ToLower(), e.Data.Host);
                if (bl == false || e.Data.Nick == bot_op) { run.Command(e.Data.Nick, e.Data.Nick, e.Data.Message, irc); }
            }
        }

        public static void ReadCommands()
        {
            // here we read the commands from the stdin and send it to the IRC API
            // WARNING, it uses WriteLine() means you need to enter RFC commands
            // like "JOIN #test" and then "PRIVMSG #test :hello to you"
            while (true)
            {
                string cmd = System.Console.ReadLine();
                if (cmd.StartsWith("/test"))
                {
                    Console.WriteLine("Success sir");
                }
                else
                {
                    irc.WriteLine(cmd);
                }
            }
        }

        public static void Exit()
        {
            Console.WriteLine("Now Exiting...");
            active = false;
            Environment.Exit(0);
        }

        private static void JoinChannels()
        {
            bool chanCheck = false;
            MySqlCommand command = Program.conn.CreateCommand();
            command.CommandText = "SELECT Channel,ChanOP,SuperUsers,Quiet,secret FROM channels";
            try { Program.conn.Open(); }
            catch (Exception e) { TextFormatting.ConsoleERROR(e.Message + "\n"); }
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                if (read["Channel"].ToString() == irc_channel) { chanCheck = true; }
            }
            Program.conn.Close();
            //default channel not found in database, so we'll need to add it
            if (chanCheck == false)
            {
                string[] jArgs = { "join", irc_channel };
                Commands.Core.Channel.join.command(jArgs, irc_channel, bot_op, irc);
            }
            
            command = Program.conn.CreateCommand();
            command.CommandText = "SELECT Channel,ChanOP,SuperUsers,Quiet,secret FROM channels";
            try { Program.conn.Open(); }
            catch (Exception e) { TextFormatting.ConsoleERROR(e.Message + "\n"); }
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("Joining the following channels:");
            while (reader.Read())
            {
                string quiet = "No";
                string secret = "No";
                if (Convert.ToInt32(reader["secret"]) == 1) { secret = "Yes"; }
                if (Convert.ToInt32(reader["Quiet"]) == 1) { quiet = "Yes"; }
                TextFormatting.ConsoleGreen("  " + reader["Channel"].ToString() + "\n");
                Console.Write("    Channel Operator: ");
                TextFormatting.ConsoleWhite(reader["ChanOP"].ToString() + "\n");
                Console.Write("    Super Users: ");
                TextFormatting.ConsoleWhite(reader["SuperUsers"].ToString() + "\n");
                Console.Write("    Secret: ");
                TextFormatting.ConsoleWhite(secret);
                Console.Write(" / Quiet: ");
                TextFormatting.ConsoleWhite(quiet + "\n");
                irc.RfcJoin(reader["Channel"].ToString());
            }
            Program.conn.Close();
        }

        public static string Ident()
        {
            Console.Write("   Enter ident:");
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    password += info.KeyChar;
                    info = Console.ReadKey(true);
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring
                        (0, password.Length - 1);
                    }
                    info = Console.ReadKey(true);
                }
            }
            for (int i = 0; i < password.Length; i++)
                Console.Write("*");
            return password;
        }

        private static bool blacklist(string nick, string host)
        {
            bool returnValue = false;
            MySqlCommand command = Program.conn.CreateCommand();
            command.CommandText = "SELECT nick,host FROM blacklist WHERE nick='" + nick + "'";
            try { conn.Open(); }
            catch (Exception e) { TextFormatting.ConsoleERROR(e.Message + "\n"); }
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["nick"].ToString() == nick && reader["host"].ToString() == host)
                {
                    returnValue = true;
                }
                else if (reader["host"].ToString() == host)
                {
                    returnValue = true;
                }
            }
            conn.Close();
            return returnValue;
        }

        private static void showTells(string nick)
        {
            DateTime time = DateTime.Now;
            string message = string.Empty;
            MySqlCommand command = Program.conn.CreateCommand();

            command.CommandText = "SELECT COUNT(Nick_To) FROM tell WHERE Nick_To='" + nick.ToLower() + "'";
            try { Program.conn.Open(); }
            catch (Exception ex) { Botler.Utilities.TextFormatting.ConsoleERROR(ex.Message + "\n"); }
            object result = command.ExecuteScalar();
            Program.conn.Close();

            int max = Convert.ToInt32(result);

            //get first tell
            if (result != null && max != 0)
            {
                command.CommandText = "SELECT Nick_To,Nick_From,Message,Time FROM tell WHERE Nick_To='" + nick.ToLower() + "' LIMIT 0,1";
                try { Program.conn.Open(); }
                catch (Exception e) { Botler.Utilities.TextFormatting.ConsoleERROR(e.Message + "\n"); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //Compose output
                    TimeSpan elapsed = time.Subtract(DateTime.Parse(reader["Time"].ToString()));
                    message = String.Format("*{5}*({0:%d} days, {1:%h} hours, {2:%m} minutes) Sent from {3} -- {4}", elapsed, elapsed, elapsed, reader["Nick_From"].ToString(), reader["Message"].ToString(), nick);
                    //send output
                    irc.SendMessage(SendType.Message, nick, message);
                }
                Program.conn.Close();
            }
            //else { irc.SendMessage(SendType.Notice, nick, String.Format("I don't seem to have any quotes for the nick sir")); }

            if (max > 1)
            {
                irc.SendMessage(SendType.Message, nick, String.Format("You ({2}) have {0} more messages waiting for you sir, please use {1}showtell to view them", (max-1), bot_comm_char, nick));
            }
            
            try { tellList.Remove(nick); }
            catch { }
        }
    }
}
