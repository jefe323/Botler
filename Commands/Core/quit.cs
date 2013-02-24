using System;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Core
{
    class quit
    {
        public static void exit(string Channel, string nick, IrcClient irc)
        {
            irc.SendMessage(SendType.Message, Channel, String.Format("Goodbye cruel world..."));
            irc.Disconnect();
            Program.Exit();
        }
    }
}
