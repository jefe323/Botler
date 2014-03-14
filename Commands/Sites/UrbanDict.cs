using System;
using System.Net;
using Botler.Utilities;
using Meebey.SmartIrc4net;
using System.Collections.Generic;

namespace Botler.Commands.Sites
{
    class UrbanDict
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $urban <phrase>", Nick));
            }
            else
            {
                string strUr = string.Empty;
                foreach (string ss in args)
                    strUr += ss + ' ';
                strUr = strUr.Substring(args[0].Length + 1);
                string urOutput = UrbanDict.output(strUr);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1} - {2}", Nick, strUr, urOutput));
            }
        }

        private static string output(string input)
        {
            using (var client = new WebClient())
            {
                string check = "<div class=\"definition\">";
                Random R = new Random();
                List<string> defList = new List<string>();

                string content = client.DownloadString("http://www.urbandictionary.com/define.php?term=" + input);
                content.Trim();

                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].StartsWith(check))
                    {
                        //parse and add to list
                        string final;
                        result[i] = result[i].Replace("&#x27;", "'").Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
                        string[] splits = result[i].Split(new string[] {"</div>"}, StringSplitOptions.None);
                        splits[0] = HtmlRemoval.StripTagsRegex(splits[0]);
                        splits[1] = HtmlRemoval.StripTagsRegex(splits[1]);
                        final = splits[0] + " - Example: " + splits[1];
                        defList.Add(final);
                    }
                }
                if (defList.Count > 0)
                {
                    int returnIndex = R.Next(defList.Count - 1);
                    string numbers = string.Format("({0}/{1}) ", returnIndex+1, defList.Count);
                    return numbers + defList[returnIndex];
                }
            }
            return "No definition found";
        }
    }
}
