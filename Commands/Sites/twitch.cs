using System;
using Meebey.SmartIrc4net;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using Botler.Utilities;

namespace Botler.Commands.Sites
{
	class twitch
	{
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $lastfm <username> ", Nick));
            }
            else
            {
                string output = twitchOuptut(args[1]);
                irc.SendMessage(SendType.Message, Channel, String.Format("{0}, {1}", Nick, output));
            }
        }

        private static string twitchOuptut(string input)
        {
            WebClient client = new WebClient();
            string content = client.DownloadString("http://api.justin.tv/stream/list.xml?channel=" + input);
            XElement doc = XElement.Parse(content);

            try
            {
                string stream = doc.XPathSelectElement(@"stream/format").Value;
                if (stream == "live")
                {
                    return String.Format(input + " is " + TextFormatting.ColorGreen(TextFormatting.Bold("Streaming")));
                }
                else
                {
                    return String.Format(input + " is " + TextFormatting.Bold(TextFormatting.ColorRed("Not Streaming")));
                }
            }
            catch
            {
                return String.Format(input + " is " + TextFormatting.Bold(TextFormatting.ColorRed("Not Streaming")));
            }
        }
	}
}
