using Meebey.SmartIrc4net;

namespace Botler.Commands._Google.SpecialSearches
{
    class mcf
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $mcf <search term(s)> ", Nick));
            }
            else
            {
                string strMCF = string.Empty;
                foreach (string ss in args)
                    strMCF += ss + ' ';
                strMCF = strMCF.Substring(args[0].Length + 1);
                strMCF = strMCF.TrimEnd(' ');
                string stringMCF = "site:http://www.minecraftforum.net " + strMCF;
                string eMCF = System.Uri.EscapeDataString(stringMCF);
                string mcfURL = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=" + eMCF;
                string mcfOutput = Search.GoogleOutput(mcfURL);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1}", Nick, mcfOutput));
            }
        }
    }
}
