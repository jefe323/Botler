using Meebey.SmartIrc4net;

namespace Botler.Commands.Sites
{
    class u
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: http://u.mcf.li/{1}", Nick, args[1]));
            }
            else if (args.Length == 3)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: http://u.mcf.li/{1}/{2}", Nick, args[1], args[2]));
            }
            else
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $u <user> [profile, posts, topics, warnings, videos, friends, pm, names, admin, edit, modcp, validate, warn, suspend, iphistory]", Nick));
            }
        }
    }
}
