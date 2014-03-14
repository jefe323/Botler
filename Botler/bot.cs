using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;
using System.Threading;
using System.Drawing;
using System.Configuration;

namespace Botler
{
    class bot
    {
        private static IrcClient irc = new IrcClient();
        private static MainWindow form;
        private static bool active;
        
        public static void Start(string address, int port, MainWindow fm)
        {
            //Settings
            string ircServer = ConfigurationManager.AppSettings["IRC_Server_Address"];
            int ircPort = Convert.ToInt32(ConfigurationManager.AppSettings["IRC_Server_Port"]);
            string ircServerPassword = ConfigurationManager.AppSettings["IRC_Server_Password"];
            string botNick = ConfigurationManager.AppSettings["Bot_Nick"];
            
            form = fm;
            active = true;

            Thread.CurrentThread.Name = "Main";

            irc.Encoding = System.Text.Encoding.UTF8;
            irc.ActiveChannelSyncing = true;

            #region proxy connection settings
            if (ConfigurationManager.AppSettings["Proxy_Address"] != "" && ConfigurationManager.AppSettings["Proxy_Address"] != null)
            {
                irc.ProxyHost = ConfigurationManager.AppSettings["Proxy_Address"];
                irc.ProxyPort = Convert.ToInt32(ConfigurationManager.AppSettings["Proxy_Port"]);
                irc.ProxyUsername = ConfigurationManager.AppSettings["Proxy_User"];
                irc.ProxyPassword = ConfigurationManager.AppSettings["Proxy_Password"];
                string pType = ConfigurationManager.AppSettings["Proxy_Type"];
                switch (pType.ToLower())
                {
                    case "http":
                        irc.ProxyType = ProxyType.Http;
                        break;
                    case "socks4":
                        irc.ProxyType = ProxyType.Socks4;
                        break;
                    case "socks4a":
                        irc.ProxyType = ProxyType.Socks4a;
                        break;
                    case "socks5":
                        irc.ProxyType = ProxyType.Socks5;
                        break;
                    default:
                        irc.ProxyType = ProxyType.None;
                        break;
                }
            }
            #endregion

            irc.OnChannelMessage += irc_OnChannelMessage;
            irc.OnQueryMessage += irc_OnQueryMessage;
            irc.OnError += irc_OnError;
            irc.OnRawMessage += irc_OnRawMessage;
            irc.OnCtcpRequest += irc_OnCtcpRequest;

            fm.OutputTextBox.SelectionBackColor = Color.Green;
            fm.OutputTextBox.SelectionColor = Color.White;
            
            fm.OutputTextBox.AppendText(String.Format("Connecting to {0}:{1}\n", ircServer, ircPort));

            fm.OutputTextBox.SelectionBackColor = Color.Transparent;
            fm.OutputTextBox.SelectionColor = Color.Black;
            try 
            { 
                irc.Connect(ircServer, ircPort);
                fm.OutputTextBox.AppendText(string.Format("Connected to {0}!\n", ircServer)); 
            }
            catch (Exception e) { fm.OutputTextBox.AppendText("Connecting Error: " + e.Message + "\n"); }

            try
            {
                irc.Login(botNick, "Botler", 0, "Botler", ircServerPassword);
                irc.RfcJoin("#Tavern");
                try
                {
                    while (active)
                    {
                        irc.ListenOnce();
                    }
                }
                catch (Exception e) { fm.OutputTextBox.AppendText("Listen Error Error: " + e.Message + "\n"); }
            }
            catch (Exception e) { fm.OutputTextBox.AppendText("Channel Join Error: " + e.Message + "\n"); }
        }

        static void irc_OnCtcpRequest(object sender, CtcpEventArgs e)
        {
            form.OutputTextBox.SelectionColor = Color.DarkSeaGreen;
            form.OutputTextBox.AppendText(e.CtcpCommand + "\n");
            form.OutputTextBox.SelectionColor = Color.Black;
            if (e.CtcpCommand == "VERSION")
            { e.Data.Irc.SendMessage(SendType.CtcpReply, e.Data.Nick, "VERSION 0.1"); }
            else if (e.CtcpCommand == "CLIENTINFO" || e.CtcpCommand == "PING")
            { e.Data.Irc.SendMessage(SendType.CtcpReply, e.Data.Nick, e.CtcpCommand + " That tickles sir ;)"); }
            else if (e.CtcpCommand == "TIME")
            { e.Data.Irc.SendMessage(SendType.CtcpReply, e.Data.Nick, "CTCP-TIME" + DateTime.Now.ToString()); }
        }

        static void irc_OnRawMessage(object sender, IrcEventArgs e)
        {
            //throw new NotImplementedException();
        }

        static void irc_OnError(object sender, ErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        static void irc_OnQueryMessage(object sender, IrcEventArgs e)
        {
            //throw new NotImplementedException();
        }

        static void irc_OnChannelMessage(object sender, IrcEventArgs e)
        {
            form.OutputTextBox.AppendText("<" + e.Data.Channel + "> " + e.Data.Nick + " -- " + e.Data.Message + "\n");
            
            //sample command working with active channel syncing
            if (e.Data.MessageArray[0] == ".info")
            {
                Channel channel = irc.GetChannel(e.Data.Channel);
                string users = string.Empty;
                foreach (ChannelUser u in channel.Users.Values)
                {
                    string nick = u.Nick;
                    if (u.IsOp) { nick = "@" + nick; }
                    else if (u.IsVoice) { nick = "+" + nick; }
                    users += nick + ", ";                   
                }
                irc.SendMessage(SendType.Notice, e.Data.Nick, users);
            }
        }
    }
}
