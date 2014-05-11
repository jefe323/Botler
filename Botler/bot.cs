using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;
using System.Threading;
using System.Drawing;
using System.Configuration;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Botler
{
    class Bot
    {
        internal static IrcClient irc = new IrcClient();
        private static MainWindow form;
        private static bool active;

        private static string ircServer;
        private static int ircPort;
        private static string ircServerPassword;
        internal static string botNick;
        private static string botOp;
        private static string botIdent;

        public static void Start(MainWindow fm)
        {
            //Settings
            ircServer = ConfigurationManager.AppSettings["IRC_Server_Address"];
            ircPort = Convert.ToInt32(ConfigurationManager.AppSettings["IRC_Server_Port"]);
            if (ConfigurationManager.AppSettings["IRC_Server_Password"].Length >= 1)
            {
                try
                {
                    ircServerPassword = Utilities.Crypto.Decrypt(ConfigurationManager.AppSettings["IRC_Server_Password"]);
                }
                catch { }
            }
            else { ircServerPassword = ""; }
            botNick = ConfigurationManager.AppSettings["Bot_Nick"];
            botOp = ConfigurationManager.AppSettings["Bot_Operator"];
            if (ConfigurationManager.AppSettings["Bot_Ident_Password"].Length >= 1)
            {
                try
                {
                    botIdent = Utilities.Crypto.Decrypt(ConfigurationManager.AppSettings["Bot_Ident_Password"]);
                }
                catch { }
            }
            else { botIdent = ""; }
            
            form = fm;
            active = true;

            //to test crypt/decrypt functionality...
            form.OutputTextBox.AppendText(botIdent + "\n");
            //form.OutputTextBox.AppendText(ircServerPassword + "\n");

            //misc settings
            irc.Encoding = System.Text.Encoding.UTF8;
            irc.ActiveChannelSyncing = true;
            irc.AutoReconnect = true;
            irc.AutoRetry = true;
            irc.AutoRelogin = true;
            irc.AutoJoinOnInvite = true;
            irc.SendDelay = 500;

            #region proxy connection settings
            if (ConfigurationManager.AppSettings["Proxy_Address"] != "" && ConfigurationManager.AppSettings["Proxy_Address"] != null)
            {
                irc.ProxyHost = ConfigurationManager.AppSettings["Proxy_Address"];
                irc.ProxyPort = Convert.ToInt32(ConfigurationManager.AppSettings["Proxy_Port"]);
                irc.ProxyUsername = ConfigurationManager.AppSettings["Proxy_User"];
                if (ConfigurationManager.AppSettings["Proxy_Password"].Length >= 1)
                {
                    try
                    {
                        irc.ProxyPassword = Utilities.Crypto.Decrypt(ConfigurationManager.AppSettings["Proxy_Password"]);
                    }
                    catch { }
                }
                else { irc.ProxyPassword = ""; }
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

            //event handlers
            irc.OnChannelMessage += irc_OnChannelMessage;
            irc.OnQueryMessage += irc_OnQueryMessage;
            irc.OnInvite += irc_OnInvite;
            irc.OnConnected += irc_OnConnected;
            irc.OnError += irc_OnError;
            irc.OnKick += irc_OnKick;
            irc.OnNickChange += irc_OnNickChange;
            irc.OnDisconnected += irc_OnDisconnected;
            irc.OnJoin += irc_OnJoin;
            irc.OnPart += irc_OnPart;
            irc.OnRawMessage += irc_OnRawMessage;
            irc.OnCtcpRequest += irc_OnCtcpRequest;



            fm.OutputTextBox.SelectionBackColor = Color.Green;
            fm.OutputTextBox.SelectionColor = Color.White;
            
            fm.OutputTextBox.AppendText(String.Format("Connecting to {0}:{1}\n", ircServer, ircPort));

            Connect();
        }

        private static void Connect()
        {
            try
            {
                irc.Connect(ircServer, ircPort);                
            }
            catch (Exception e) { form.OutputTextBox.AppendText("Connecting Error: " + e.Message + "\n"); }            
        }

        static void irc_OnConnected(object sender, EventArgs e)
        {
            form.OutputTextBox.AppendText(string.Format("Connected to {0}!\n", ircServer));
            try
            {
                irc.Login(botNick, "Botler", 0, "Botler", ircServerPassword);
                joinChannels();
                try
                {
                    while (active)
                    {
                        irc.ListenOnce();
                    }
                    irc.Disconnect();
                }
                catch (Exception ex) { form.OutputTextBox.AppendText("Listen Error Error: " + ex.Message + "\n"); }
            }
            catch (Exception ex) { form.OutputTextBox.AppendText("Channel Join Error: " + ex.Message + "\n"); }
        }

        private static void joinChannels()
        {
            XDocument xDoc = XDocument.Load("Data/Channels.xml");
            var channels = from element in xDoc.Elements("Channels").Elements("Channel")
                           select element;

            foreach (var c in channels)
            {
                var t = Task.Factory.StartNew(() => irc.RfcJoin(c.Attribute("Name").Value));
                //display message in logger
                createTabPage(c.Attribute("Name").Value);
            }
        }

        private static void createTabPage(string channel)
        {
            TabPage tab = new TabPage(channel);
            tab.BackColor = Color.White;
            tab.Name = channel + "Tab";
            RichTextBox box = new RichTextBox();
            box.Height = 302;
            box.Width = 435;
            box.ReadOnly = true;
            box.Name = channel;
            box.Location = new Point(7, 7);
            //scroll to bottom
            box.TextChanged += form.OutputTextBox_TextChanged;
            tab.Controls.Add(box);
            form.ChannelTabControl.TabPages.Add(tab);
            
            //add to listbox
            form.ChannelList.Items.Add(channel);
            
        }

        static void irc_OnPart(object sender, PartEventArgs e)
        {
            //update seen
        }

        static void irc_OnJoin(object sender, JoinEventArgs e)
        {
            //update seen
            //display welcome message for channel if configured
        }

        static void irc_OnDisconnected(object sender, EventArgs e)
        {
            //use 'active' bool to differentiate between planned and unplanned dc
            //active = unplanned, !active = planned
            //if (!planned)
            //need to reconnect
            //Connect();
        }

        static void irc_OnNickChange(object sender, NickChangeEventArgs e)
        {
            //check tells
            //update alias list?
        }

        static void irc_OnKick(object sender, KickEventArgs e)
        {
            //same as part
        }

        static void irc_OnInvite(object sender, InviteEventArgs e)
        {
            //join channel (no more join command, invite only)
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
            form.updateStatus(e.Data.Message);
        }

        static void irc_OnError(object sender, ErrorEventArgs e)
        {
            //display in logger
        }

        static void irc_OnQueryMessage(object sender, IrcEventArgs e)
        {
            //same as channel message, just a different destination
        }

        static void irc_OnChannelMessage(object sender, IrcEventArgs e)
        {
            RichTextBox box = FindOutputControl(form.ChannelTabControl, e.Data.Channel) as RichTextBox;
            Task.Factory.StartNew(() => box.AppendText("<" + e.Data.Nick + "> - " + e.Data.Message + "\n"));

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
            else if (e.Data.MessageArray[0] == ".kill")
            {
                Disconnect();                
            }
        }

        static internal void Disconnect()
        {
            active = false;
            irc.SendMessage(SendType.Message, botOp, "Goodbye...");
            //required to kill bot on time, else it will take approx. 1.5 minutes to dc :(
            Thread.Sleep(1000);
            irc.SendMessage(SendType.Message, botNick, "die");
        }

        //used to find the proper rtb to output to
        //props to http://stackoverflow.com/a/1641282
        static internal Control FindOutputControl(Control container, string name)
        {
            if (container.Name == name) return container;

            foreach (Control ctrl in container.Controls)
            {
                Control foundCtrl = FindOutputControl(ctrl, name);

                if (foundCtrl != null) return foundCtrl;
            }

            return null;
        }
    }
}
