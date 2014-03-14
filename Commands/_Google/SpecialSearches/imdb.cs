using Meebey.SmartIrc4net;

namespace Botler.Commands._Google.SpecialSearches
{
    class imdb
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $imdb <search term(s)> ", Nick));
            }
            else
            {
                string strIMDB = string.Empty;
                foreach (string ss in args)
                    strIMDB += ss + ' ';
                strIMDB = strIMDB.Substring(args[0].Length + 1);
                strIMDB = strIMDB.TrimEnd(' ');
                string stringIMDB = "site:http://www.imdb.com " + strIMDB;
                string eIMDB = System.Uri.EscapeDataString(stringIMDB);
                string imdbURL = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=" + eIMDB;
                string imdbOutput = Search.GoogleOutput(imdbURL);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1}", Nick, imdbOutput));
            }
        }
    }
}
