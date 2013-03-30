using System;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Misc
{
    class fire
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $fire <user>", Nick));
            }
            else if (args[1].ToLower() == Program.bot_op.ToLower())
            {
                irc.SendMessage(SendType.Message, Channel, String.Format("{0}, why would I fire my master?", Nick));
            }
            else if (args[1].ToLower() == Program.bot_op.ToLower())
            {
                irc.SendMessage(SendType.Message, Channel, String.Format("{0}, why would I fire myself?", Nick));
            }
            else
            {
                string strFire = string.Empty;
                foreach (string ss in args)
                    strFire += ss + ' ';
                strFire = strFire.Substring(args[0].Length + 1);
                irc.SendMessage(SendType.Message, Channel, string.Format("Firing {0} sir", strFire.TrimEnd()));
            }
        }
    }
}
