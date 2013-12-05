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
        public static class GlobalVar
        {
            static internal string version = "2.0.3";
            static internal int currentDBVersion = 1;

            public static IrcClient irc = new IrcClient();
            static internal string connString;
            static internal MySqlConnection conn;

            static internal List<string> tellList = new List<string>();
            static internal List<Commands.Core.Seen.person> seenList = new List<Commands.Core.Seen.person>();

            static internal bool active = true;

            //static internal double upTime = 0;
            static internal DateTime startTime = DateTime.Now;
            static internal DateTime currentTime;

            /////////////////////////////////////////////
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
        }
        
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            Console.Title = "Botler - C# IRC Bot";

            ///////////////////////
            //Set various connection info
            ///////////////////////
            start.StepOne();

            GlobalVar.connString = "Server=" + GlobalVar.mysql_server + ";Port=" + GlobalVar.mysql_port + ";Database=" + GlobalVar.mysql_database + ";Uid=" + GlobalVar.mysql_user + ";password=" + GlobalVar.mysql_password + ";";
            GlobalVar.conn = new MySqlConnection(GlobalVar.connString);

            GlobalVar.irc.Encoding = System.Text.Encoding.UTF8;
            GlobalVar.irc.ActiveChannelSyncing = true;
            GlobalVar.irc.AutoReconnect = true;
            GlobalVar.irc.AutoRetry = true;
            GlobalVar.irc.AutoRelogin = true;
            GlobalVar.irc.AutoJoinOnInvite = true;
            GlobalVar.irc.SendDelay = 500;

            start.StepTwo();

            ///////////////////////
            //Set up event handlers
            ///////////////////////
            GlobalVar.irc.OnChannelMessage += new IrcEventHandler(irc_OnChannelMessage);
            GlobalVar.irc.OnQueryMessage += new IrcEventHandler(irc_OnQueryMessage);
            GlobalVar.irc.OnInvite += new InviteEventHandler(irc_OnInvite);
            GlobalVar.irc.OnConnected += new EventHandler(irc_OnConnected);
            GlobalVar.irc.OnError += new Meebey.SmartIrc4net.ErrorEventHandler(irc_OnError);
            GlobalVar.irc.OnKick += new KickEventHandler(irc_OnKick);
            GlobalVar.irc.OnNickChange += new NickChangeEventHandler(irc_OnNickChange);
            GlobalVar.irc.OnDisconnected += new EventHandler(irc_OnDisconnected);
            GlobalVar.irc.OnJoin += new JoinEventHandler(irc_OnJoin);
            GlobalVar.irc.OnPart += new PartEventHandler(irc_OnPart);

            ///////////////////////
            //Connect to irc server
            ///////////////////////

            Console.Write("Identify? (y/n): ");
            string ident = Console.ReadLine();
            if (ident == "y")
            {
                GlobalVar.bot_ident = Ident();
                Console.WriteLine();
            }
            Console.WriteLine("Connecting to " + GlobalVar.irc_server);

            try { GlobalVar.irc.Connect(GlobalVar.irc_server, GlobalVar.irc_port); }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect, reason: " + e.Message);
                Exit();
            }

            try
            {
                GlobalVar.irc.Login(GlobalVar.bot_nick, "Botler", 1, "Botler");
                //irc.RfcJoin("#Botler");
                try { JoinChannels(); }
                catch (Exception fail)
                {
                    TextFormatting.ConsoleERROR(fail.Message + "\n");
                }
                GlobalVar.irc.SendMessage(SendType.Message, "NickServ", "identify " + GlobalVar.bot_ident);
                Utilities.timers.Begin();

                new Thread(new ThreadStart(ReadCommands)).Start();

                while (GlobalVar.active)
                {
                    try
                    {
                        GlobalVar.irc.ListenOnce();
                    }
                    catch (Exception e)
                    {
                        TextFormatting.ConsoleERROR("Listen error: " + e.Message + "\n");
                        Console.WriteLine(e.StackTrace);
                        continue;
                    }
                }
                GlobalVar.irc.Disconnect();
            }
            catch (Exception e) {
                Console.WriteLine("I dun goofed");
                TextFormatting.ConsoleERROR(e.Message + "\n");
                Console.WriteLine(e.StackTrace);
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
                    GlobalVar.irc.Connect(GlobalVar.irc_server, GlobalVar.irc_port);
                    GlobalVar.irc.Login("Botler", "Botler", 1, "Botler");
                    //irc.RfcJoin(channel);
                    JoinChannels();
                    //ghost
                    GlobalVar.irc.SendMessage(SendType.Message, "NickServ", "ghost " + GlobalVar.bot_nick + " " + GlobalVar.bot_ident); 
                    //change nick
                    GlobalVar.irc.RfcNick(GlobalVar.bot_nick);
                    //ident again
                    GlobalVar.irc.SendMessage(SendType.Message, "NickServ", "identify " + GlobalVar.bot_ident);
                    connected = true;
                }
                catch (Exception)
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
            Commands.Core.Channel.join.command(jArgs, e.Data.Channel, e.Data.Nick, GlobalVar.irc);
        }

        static void irc_OnError(object sender, Meebey.SmartIrc4net.ErrorEventArgs e)
        {
            Console.WriteLine("ERROR: " + e.ErrorMessage);
        }

        static void irc_OnKick(object sender, KickEventArgs e)
        {
            if (e.Whom == GlobalVar.bot_nick)
            {
                //make same as part
                string[] pArgs = { "part", e.Data.Channel };
                Commands.Core.Channel.part.command(pArgs, e.Data.Channel, e.Data.Nick, GlobalVar.irc);
            }
        }

        static void irc_OnJoin(object sender, JoinEventArgs e)
        {
            Commands.Core.Seen.set.go(e.Data.Nick, e.Data.Channel, String.Format("{0} has joined the channel", e.Data.Nick));
        }

        static void irc_OnPart(object sender, PartEventArgs e)
        {
            Commands.Core.Seen.set.go(e.Data.Nick, e.Data.Channel, String.Format("{0} has parted the channel", e.Data.Nick));
        }

        static void irc_OnChannelMessage(object sender, IrcEventArgs e)
        {
            Commands.Core.Seen.set.go(e.Data.Nick, e.Data.Channel, e.Data.Message);

            if (GlobalVar.tellList.Contains(e.Data.Nick.ToLower()) && !e.Data.Message.StartsWith(String.Format("{0}showtell", GlobalVar.bot_comm_char)) && !e.Data.Message.StartsWith(String.Format("{0}showtells", GlobalVar.bot_comm_char)) && !e.Data.Message.StartsWith(String.Format("{0}st", GlobalVar.bot_comm_char)))
            {
                try
                {
                    showTells(e.Data.Nick.ToLower());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Show Tells error: " + ex.Message + " " + ex.StackTrace);
                }
            }
            if (e.Data.Message.StartsWith(GlobalVar.bot_comm_char))
            {
                try
                {
                    bool bl = blacklist(e.Data.Nick.ToLower(), e.Data.Host);
                    if (bl == false || e.Data.Nick == GlobalVar.bot_op) { run.Command(e.Data.Channel, e.Data.Nick, e.Data.Message, GlobalVar.irc); }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("blacklist error: " + ex.Message + " " + ex.StackTrace);
                }
            }
        }

        static void irc_OnQueryMessage(object sender, IrcEventArgs e)
        {
            if (e.Data.Nick == GlobalVar.bot_op && e.Data.Message.StartsWith(String.Format("{0}ident", GlobalVar.bot_comm_char)))
            {
                Botler.Commands.Core.nick.ident(e.Data.MessageArray, e.Data.Nick, e.Data.Nick, GlobalVar.irc);
            }
            else if (e.Data.Message.StartsWith(GlobalVar.bot_comm_char))
            {
                bool bl = blacklist(e.Data.Nick.ToLower(), e.Data.Host);
                if (bl == false || e.Data.Nick == GlobalVar.bot_op) { run.Command(e.Data.Nick, e.Data.Nick, e.Data.Message, GlobalVar.irc); }
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
                    GlobalVar.irc.WriteLine(cmd);
                }
            }
        }

        public static void Exit()
        {
            Console.WriteLine("Now Exiting...");
            GlobalVar.active = false;
            Environment.Exit(0);
        }

        private static void JoinChannels()
        {
            bool chanCheck = false;
            MySqlCommand command = GlobalVar.conn.CreateCommand();
            command.CommandText = "SELECT Channel,ChanOP,SuperUsers,Quiet,secret FROM channels";
            try { GlobalVar.conn.Open(); }
            catch (Exception e) { TextFormatting.ConsoleERROR(e.Message + "\n"); }
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                if (read["Channel"].ToString() == GlobalVar.irc_channel) { chanCheck = true; }
            }
            GlobalVar.conn.Close();
            //default channel not found in database, so we'll need to add it
            if (chanCheck == false)
            {
                string[] jArgs = { "join", GlobalVar.irc_channel };
                Commands.Core.Channel.join.command(jArgs, GlobalVar.irc_channel, GlobalVar.bot_op, GlobalVar.irc);
            }

            command = GlobalVar.conn.CreateCommand();
            command.CommandText = "SELECT Channel,ChanOP,SuperUsers,Quiet,secret FROM channels";
            try { GlobalVar.conn.Open(); }
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
                GlobalVar.irc.RfcJoin(reader["Channel"].ToString());
            }
            GlobalVar.conn.Close();
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
            MySqlCommand command = GlobalVar.conn.CreateCommand();
            command.CommandText = "SELECT nick,host FROM blacklist WHERE nick='" + nick + "'";
            try { GlobalVar.conn.Open(); }
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
            GlobalVar.conn.Close();
            return returnValue;
        }

        private static void showTells(string nick)
        {
            DateTime time = DateTime.Now;
            string message = string.Empty;
            MySqlCommand command = GlobalVar.conn.CreateCommand();

            command.CommandText = "SELECT COUNT(Nick_To) FROM tell WHERE Nick_To='" + nick.ToLower() + "'";
            try { GlobalVar.conn.Open(); }
            catch (Exception ex) { Botler.Utilities.TextFormatting.ConsoleERROR(ex.Message + "\n"); GlobalVar.conn.Close(); }
            object result = command.ExecuteScalar();
            GlobalVar.conn.Close();

            int max = Convert.ToInt32(result);

            //get first tell
            if (result != null && max != 0)
            {
                command.CommandText = "SELECT Nick_To,Nick_From,Message,Time FROM tell WHERE Nick_To='" + nick.ToLower() + "' LIMIT 0,1";
                try { GlobalVar.conn.Open(); }
                catch (Exception e) { Botler.Utilities.TextFormatting.ConsoleERROR(e.Message + "\n"); }
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //Compose output
                    TimeSpan elapsed = time.Subtract(DateTime.Parse(reader["Time"].ToString()));
                    message = String.Format("*{5}*({0:%d} days, {1:%h} hours, {2:%m} minutes) Sent from {3} -- {4}", elapsed, elapsed, elapsed, reader["Nick_From"].ToString(), reader["Message"].ToString(), nick);
                    //send output
                    GlobalVar.irc.SendMessage(SendType.Message, nick, message);
                    //remove old tell
                    command.CommandText = "DELETE FROM tell WHERE Nick_To='" + reader["Nick_To"] + "' AND Time='" + reader["Time"] + "'";
                    GlobalVar.conn.Open();
                    command.ExecuteNonQuery();
                    GlobalVar.conn.Close();
                }
                GlobalVar.conn.Close();
            }
            //else { irc.SendMessage(SendType.Notice, nick, String.Format("I don't seem to have any quotes for the nick sir")); }

            if (max > 1)
            {
                GlobalVar.irc.SendMessage(SendType.Message, nick, String.Format("You ({2}) have {0} more messages waiting for you sir, please use {1}showtell to view them", (max - 1), GlobalVar.bot_comm_char, nick));
            }

            try { GlobalVar.tellList.Remove(nick); }
            catch { }
        }
    }
}
