using System;
using Botler.Utilities;
using System.Net;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Sites
{
    class Synonym
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $synonym <word>", Nick));
            }
            else if (args.Length == 2)
            {
                string synOutput = Synonym.SynOutput(args[1]);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1} - {2}", Nick, args[1], synOutput));
            }
            else if (args.Length > 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: Please enter one word at a time sir, thank you", Nick));
            }
        }

        private static string SynOutput(string input)
        {
            using (var client = new WebClient())
            {
                string startCheck = "<td valign=\"top\">Synonyms:</td>";
                string endCheck = "</span></td>";
                string output = "";
                int check = 0;
                string content = client.DownloadString("http://thesaurus.com/browse/" + input);

                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].Contains(startCheck) && check == 0)
                    {
                        check = 1;
                        for (int k = i; k < result.Length; k++)
                        {
                            if (!result[k].Contains(endCheck))
                            {
                                output = output + " " + result[k];
                            }
                            else
                                break;
                        }

                    }
                }
                output = HtmlRemoval.StripTagsRegex(output);
                return output;
            }
        }
    }
}
