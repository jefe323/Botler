using System;
using System.Linq;
using Meebey.SmartIrc4net;
using System.Web;
using System.Xml.Linq;

namespace Botler.Commands.Links
{
    class shrink
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $shrink <URL> ", Nick));
            }
            else
            {
                string output = shrinkOutput(args[1]);
                irc.SendMessage(SendType.Message, Channel, String.Format("{0}, {1}", Nick, output));
            }
        }

        private static string shrinkOutput(string url)
        {
            var shortUrl = BitlyApi.ShortenUrl(url).ShortUrl;
            return shortUrl;
        }
    }

    //http://www.emadibrahim.com/2009/05/07/shortening-urls-with-bitlys-api-in-net/
    public static class BitlyApi
    {
        private const string apiKey = "R_75d5305000f435aa22688675cb64494e";
        private const string login = "jefe323";

        public static BitlyResults ShortenUrl(string longUrl)
        {
            var url = String.Format("http://api.bit.ly/shorten?format=xml&version=2.0.1&longUrl={0}&login={1}&apiKey={2}", HttpUtility.UrlEncode(longUrl), login, apiKey);
            var resultXml = XDocument.Load(url);
            var x = (from result in resultXml.Descendants("nodeKeyVal")
                     select new BitlyResults
                         {
                             UserHash = result.Element("userHash").Value,
                             ShortUrl = result.Element("shortUrl").Value
                         });
            return x.Single();
        }
    }

    public class BitlyResults
    {
        public string UserHash { get; set; }
        public string ShortUrl { get; set; }
    }
}
