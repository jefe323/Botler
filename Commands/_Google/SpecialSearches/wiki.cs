using Meebey.SmartIrc4net;

namespace Botler.Commands._Google.SpecialSearches
{
    class wiki
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $wiki <search term(s)> ", Nick));
            }
            else
            {
                string strWiki = string.Empty;
                foreach (string ss in args)
                    strWiki += ss + ' ';
                strWiki = strWiki.Substring(args[0].Length + 1);
                strWiki = strWiki.TrimEnd(' ');
                string stringWiki = "site:http://en.wikipedia.org " + strWiki;
                string eWiki = System.Uri.EscapeDataString(stringWiki);
                string wikiURL = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=" + eWiki;
                string wikiOutput = Search.GoogleOutput(wikiURL);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1}", Nick, wikiOutput));
            }
        }
    }
}
