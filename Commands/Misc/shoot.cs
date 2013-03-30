using System;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Misc
{
    class shoot
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $shoot <user>", Nick));
            }
            else if (args[1].ToLower() == Program.bot_op.ToLower())
            {
                irc.SendMessage(SendType.Message, Channel, String.Format("{0}, why would I shoot my master?", Nick));
            }
            else if (args[1].ToLower() == Program.bot_op.ToLower())
            {
                irc.SendMessage(SendType.Message, Channel, String.Format("{0}, why would I shoot myself?", Nick));
            }
            else
            {
                string strShoot = string.Empty;
                foreach (string ss in args)
                    strShoot += ss + ' ';
                strShoot = strShoot.Substring(args[0].Length + 1);
                irc.SendMessage(SendType.Message, Channel, string.Format("Shooting {0} sir", strShoot.TrimEnd()));
            }
        }
    }
}
