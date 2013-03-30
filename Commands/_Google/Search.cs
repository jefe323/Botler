using System;
using System.Net;
using System.Net.Json;
using Meebey.SmartIrc4net;

namespace Botler.Commands._Google
{
    class Search
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $search <search term(s)> ", Nick));
            }
            else
            {
                string strSearch = string.Empty;
                foreach (string ss in args)
                    strSearch += ss + ' ';
                strSearch = strSearch.Substring(args[0].Length + 1);
                strSearch = strSearch.TrimEnd(' ');
                string eSearch = System.Uri.EscapeDataString(strSearch);
                string searchURL = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=" + eSearch;
                string googleOutput = Search.GoogleOutput(searchURL);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1}", Nick, googleOutput));
            }
        }

        public static string GoogleOutput(string input)
        {
            using (var client = new WebClient())
            {
                string output = "";

                string URL = "";
                string title = "";

                string urlVar = "url";
                int urlCheck = 0;

                string titleVar = "titleNoFormatting";
                int titleCheck = 0;

                string content = client.DownloadString(input);
                JsonTextParser parser = new JsonTextParser();

                output = parser.Parse(content).ToString();

                char[] delimiters = new char[] { '\r', '\n' };
                try
                {
                    string[] parts = output.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i].Contains(titleVar) && titleCheck == 0)
                        {
                            parts[i] = parts[i].Replace("titleNoFormatting", " ");
                            parts[i] = parts[i].Replace('"', ' ');
                            parts[i] = parts[i].Replace(':', ' ');
                            parts[i] = parts[i].Replace(',', ' ');
                            parts[i] = parts[i].Trim();
                            title = parts[i];
                            titleCheck = 1;
                        }
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

                    string finalOutput = URL.Remove(0, 3) + " - " + Botler.Utilities.TextFormatting.Bold(title);
                    return finalOutput;
                }
                catch { }
                return "No results found sir";
            }
        }
    }
}
