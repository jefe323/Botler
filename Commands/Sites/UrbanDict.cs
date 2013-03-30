using System;
using System.Net;
using Botler.Utilities;
using Meebey.SmartIrc4net;

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
                string urOutput = UrbanDict.UDOutput(strUr);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1} - {2}", Nick, strUr, urOutput));
            }
        }

        private static string UDOutput(string input)
        {
            using (var client = new WebClient())
            {
                string check = "<div class=\"definition\">";
                string content = client.DownloadString("http://www.urbandictionary.com/define.php?term=" + input);
                content.Trim();

                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].Contains(check))
                    {
                        string final = result[i].Replace("<div class=\"example\">", " ");
                        final = HtmlRemoval.StripTagsRegex(/*result[i]*/final);
                        //final = final.Trim();
                        final = final.Replace("&quot;", "\"");
                        return final;
                    }
                }
            }
            return "No definition found";
        }
    }
}
