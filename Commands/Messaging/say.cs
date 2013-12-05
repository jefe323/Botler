using System;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Messaging
{
    class say
    {
        public static void send(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, String.Format("({0}) Usage: " + Program.GlobalVar.bot_comm_char + "say <message>", Nick)); }
            else
            {
                string str = string.Empty;
                foreach (string s in args)
                    str += s + " ";
                str = str.Substring(args[0].Length + 1);
                irc.SendMessage(SendType.Message, Channel, str);
            }
        }
    }
}
