using System;
using System.Collections.Generic;
using System.Net;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Sites
{
    class anagram
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $anagram <phrase>", Nick));
            }
            else
            {
                string strAna = string.Empty;
                foreach (string ss in args)
                    strAna += ss + ' ';
                strAna = strAna.Substring(args[0].Length + 1);
                strAna = strAna.TrimEnd(' ');
                //encode input
                string input = System.Uri.EscapeDataString(strAna);
                string output = aOutput(input);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, {1}", Nick, output));
            }
        }

        private static string aOutput(string input)
        {
            List<string> anagrams = new List<string>();
            using (var client = new WebClient())
            {
                //client.DownloadFile("http://wordsmith.org/anagram/anagram.cgi?anagram=Tehpillowstar&language=english&t=100&d=&include=&exclude=&n=&m=&source=adv&a=n&l=n&q=n&k=1", "output.txt");
                string startCheck = "Displaying first 10:";
                string endCheck = "<bottomlinks>";

                string content = client.DownloadString("http://wordsmith.org/anagram/anagram.cgi?anagram=" + input + "&language=english&t=10&d=&include=&exclude=&n=&m=&source=adv&a=n&l=n&q=n&k=1");
                content.Trim();

                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].Contains(startCheck))
                    {
                        for (int k = i + 1; k < result.Length; k++)
                        {
                            if (!result[k].Contains(endCheck))
                            {
                                string final = result[k];
                                final = final.Replace("<br>", "");
                                final = final.Replace("</b>", "");
                                //Console.WriteLine(other + " - " + final);
                                //break;
                                anagrams.Add(final);
                            }
                            else if (result[k].Contains(result[k]))
                            {
                                break;
                            }
                        }
                    }
                }

                if (anagrams.Count > 0)
                {
                    Random r = new Random();
                    int choice = r.Next(0, anagrams.Count);
                    //Console.WriteLine(anagrams[choice]);
                    return anagrams[choice];
                }
                else
                {
                    //Console.WriteLine("No anagrams found :(");
                    return "No anagrams found";
                }
            }
        }
    }
}
