using System;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Core
{
    class nick
    {
        public static void ghost(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "ghost", Nick)); }
            else
            {
                irc.SendMessage(SendType.Message, "NickServ", String.Format("ghost {0}, {1}", Program.GlobalVar.bot_nick, Program.GlobalVar.bot_ident));
                irc.SendMessage(SendType.Message, Channel, String.Format("The imposter has been killed sir"));
            }
        }

        public static void change(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "nick <new nick>", Nick)); }
            {
                irc.RfcNick(args[1]);
            }
        }

        public static void ident(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "ident <password>", Nick)); }
            {
                irc.SendMessage(SendType.Message, "NickServ", String.Format("identify {0}", args[1]));
            }
        }
    }
}
