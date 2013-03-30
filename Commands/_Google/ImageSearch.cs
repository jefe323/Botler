using System;
using System.Net;
using System.Net.Json;
using Meebey.SmartIrc4net;

namespace Botler.Commands._Google
{
    class ImageSearch
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $image <search term(s)> ", Nick));
            }
            else
            {
                string strImg = string.Empty;
                foreach (string ss in args)
                    strImg += ss + ' ';
                strImg = strImg.Substring(args[0].Length + 1);
                strImg = strImg.TrimEnd(' ');
                //encode input
                string eImage = System.Uri.EscapeDataString(strImg);
                //set test URL
                string imageURL = "https://ajax.googleapis.com/ajax/services/search/images?v=1.0&q=" + eImage;
                string imageOutput = ImageSearch.GoogleImageOutput(imageURL);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, {1}", Nick, imageOutput));
            }
        }

        private static string GoogleImageOutput(string input)
        {
            using (var client = new WebClient())
            {
                string URL = "error";
                string urlVar = "url";
                int urlCheck = 0;

                string content = client.DownloadString(input);
                JsonTextParser parser = new JsonTextParser();

                string output = parser.Parse(content).ToString();

                char[] delimiters = new char[] { '\r', '\n' };
                string[] parts = output.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Contains(urlVar) && urlCheck == 0)
                    {
                        parts[i] = parts[i].Replace("url", " ");
                        parts[i] = parts[i].Replace('"', ' ');
                        parts[i] = parts[i].Replace(',', ' ');
                        parts[i] = parts[i].Trim();
                        URL = parts[i];
                        urlCheck = 1;
                    }
                }

                return URL.Remove(0, 3);
            }
        }
    }
}
