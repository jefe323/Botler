using System;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Misc
{
    class random
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            Random R = new Random();
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $random <max value> ", Nick));
            }
            else
            {
                int max = Convert.ToInt32(args[1]);
                int num = R.Next(max);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, I picked {1}", Nick, num));
            }
        }
    }
}
