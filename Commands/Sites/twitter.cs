using System;
using System.Net;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Sites
{
    class twitter
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $twitter <user>", Nick));
            }
            else
            {
                string output = twitterOutput(args[1]);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, {1}", Nick, output));
            }
        }

        private static string twitterOutput(string input)
        {
            using (var client = new WebClient())
            {
                string check = "<item>";
                string final = "";
                string content = client.DownloadString("http://api.twitter.com/1/statuses/user_timeline.rss?screen_name=" + input);
                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].Contains(check))
                    {
                        final = result[i + 1];
                        final = final.Replace("<title>", "");
                        final = final.Replace("</title>", "");
                        final = final.Trim();
                        return final;
                    }
                }
            }
            return "Twitter account not found sir";
        }
    }
}
