using System;
using System.Net;
using Botler.Utilities;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Sites
{
    class Dict
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $dict <word>", Nick));
            }
            else if (args.Length == 2)
            {
                string dictOutput = Dict.DictOutput(args[1]);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1} - {2}", Nick, args[1], dictOutput));
            }
            else if (args.Length > 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: Please enter one word at a time sir, thank you", Nick));
            }
        }

        private static string DictOutput(string input)
        {
            using (var client = new WebClient())
            {
                string check = "<span class=\"definition-marker\">";
                string output = client.DownloadString("http://ninjawords.com/" + input);
                output.Trim();

                string[] result = output.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].Contains(check))
                    {
                        string final = HtmlRemoval.StripTagsRegex(result[i]);
                        final = final.Replace("&deg;", " ");
                        final = final.Trim();
                        return final;
                    }
                }
            }
            return "No definition found";
        }
    }
}
