using System;
using Meebey.SmartIrc4net;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Botler.Commands.Sites
{
    //at the request of the mighty citricsquid
    class lastfm
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $lastfm <username> ", Nick));
            }
            else
            {
                string output = lastPlayedOutput(args[1]);
                irc.SendMessage(SendType.Message, Channel, String.Format("{0}, {1}", Nick, output));
            }
        }

        private static string lastPlayedOutput(string input)
        {
            WebClient client = new WebClient();
            string content = client.DownloadString("http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=" + input + "&api_key=3f0dfe9770a153a4d1ee002d9faeb8a9");
            XElement doc = XElement.Parse(content);

            try
            {
                string artist = doc.XPathSelectElement(@"recenttracks/track/artist").Value;
                string LastPlayed = doc.XPathSelectElement(@"recenttracks/track/name").Value;
                string album = doc.XPathSelectElement(@"recenttracks/track/album").Value;

                string output = String.Format("{0} last listened to {1} by {2} from the album {3}", input, LastPlayed, artist, album);
                return output;
            }
            catch
            {
                return input + " does not exist or does not have a \"Last Played\" song";
            }
        }
    }
}
