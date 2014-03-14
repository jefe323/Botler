using System;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Misc
{
    class choose
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            Random R = new Random();
            string chStr = string.Empty;
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $choose <option 1>, <option 2>, ... , <option n> ", Nick));
            }
            //recombine the choices
            foreach (string ss in args)
                chStr += ss + " ";
            chStr = chStr.Substring(args[0].Length + 1);
            //re-break it up using commas as delimiters instead of spaces
            string[] chArgs = chStr.Split(',');
            int temp = R.Next(0, chArgs.Length);
            irc.SendMessage(SendType.Message, Channel, string.Format("{0}, {1}", Nick, chArgs[temp].Trim()));
        }
    }
}
