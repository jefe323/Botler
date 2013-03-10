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
        static internal string version = "2.0dev6";
        static internal int currentDBVersion = 1;

        public static IrcClient irc = new IrcClient();
        static internal string connString;
        static internal MySqlConnection conn;

        static internal Assembly assembly;
        static internal FileInfo[] filePaths;
        static internal string[] fileName;
        static internal Dictionary<string, string[]> pluginCommands = new Dictionary<string, string[]>();

        static internal List<string> tellList = new List<string>();
        static internal List<Commands.Core.Seen.person> seenList = new List<Commands.Core.Seen.person>();

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
            irc.AutoJoinOnInvite = true;

            start.StepTwo();

            ///////////////////////
            //Set up event handlers
            ///////////////////////
            irc.OnChannelMessage += new IrcEventHandler(irc_OnChannelMessage);
            irc.OnQueryMessage += new IrcEventHandler(irc_OnQueryMessage);
            irc.OnInvite += new InviteEventHandler(irc_OnInvite);
            irc.OnMotd += new MotdEventHandler(irc_OnMotd);
            irc.OnError += new Meebey.SmartIrc4net.ErrorEventHandler(irc_OnError);
            irc.OnPart += new PartEventHandler(irc_OnPart);
            irc.OnKick += new KickEventHandler(irc_OnKick);
            irc.OnNickChange += new NickChangeEventHandler(irc_OnNickChange);

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

                irc.Listen();
                irc.Disconnect();
            }
            catch (Exception e) {
                Console.WriteLine("I dun goofed");
                Console.WriteLine(e.Message);
                //Exit();
            }
        }

        static void irc_OnNickChange(object sender, NickChangeEventArgs e)
        {
            //check for nicks on both old and new nicks
        }

        static void irc_OnMotd(object sender, MotdEventArgs e)
        {
            Console.WriteLine("CONNECTED!");
        }

        static void irc_OnInvite(object sender, InviteEventArgs e)
        {
            //make equal to .join command
        }

        static void irc_OnError(object sender, Meebey.SmartIrc4net.ErrorEventArgs e)
        {
            Console.WriteLine("ERROR: " + e.ErrorMessage);
        }

        static void irc_OnKick(object sender, KickEventArgs e)
        {
            if (e.Whom == "Botler")
            {
                //make same as part
                Console.WriteLine("I got kicked :(");
            }
            else
                Console.WriteLine(e.Whom + " got kicked");
        }

        static void irc_OnPart(object sender, PartEventArgs e)
        {
            Console.WriteLine(e.Who + " has left");
        }

        static void irc_OnChannelMessage(object sender, IrcEventArgs e)
        {
            Commands.Core.Seen.set.go(e.Data.Nick, e.Data.Channel, e.Data.Message);
            if (tellList.Contains(e.Data.Nick) && !e.Data.Message.StartsWith(String.Format("{0}showtell", bot_comm_char)) && !e.Data.Message.StartsWith(String.Format("{0}showtells", bot_comm_char)) && !e.Data.Message.StartsWith(String.Format("{0}st", bot_comm_char)))
            {
                irc.SendMessage(SendType.Notice, e.Data.Nick, String.Format("You have messages waiting for you sir, please use {0}showtell to view them", bot_comm_char));
                tellList.Remove(e.Data.Nick);
            }
            if (e.Data.Message.StartsWith(bot_comm_char))
            {
                run.Command(e.Data.Channel, e.Data.Nick, e.Data.Message, irc);
            }
        }

        static void irc_OnQueryMessage(object sender, IrcEventArgs e)
        {
            if (e.Data.Message.StartsWith(bot_comm_char))
            {
                run.Command(e.Data.Nick, e.Data.Nick, e.Data.Message, irc);
            }
        }

        public static void Exit()
        {
            Console.WriteLine("Now Exiting...");
            Environment.Exit(0);
        }

        private static void JoinChannels()
        {
            MySqlCommand command = Program.conn.CreateCommand();
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
    }
}
