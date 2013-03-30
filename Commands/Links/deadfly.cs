using System;
using System.Net;
using Meebey.SmartIrc4net;
using System.IO;

namespace Botler.Commands.Links
{
    class deadfly
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $deadfly <adf.ly link>", Nick));
            }
            if (args.Length == 2)
            {
                if (!args[1].StartsWith("http://") && !args[1].StartsWith("https://"))
                {
                    args[1] = "http://" + args[1];
                }
                string daOut = deadfly.deadflyOutput(args[1]);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, {1}", Nick, daOut));
            }
        }

        private static string deadflyOutput(string input)
        {
            using (var client = new WebClient())
            {
                string check = "var url = ";
                string final = "error";

                //client.DownloadFile(input, "output.txt");
                string content = client.DownloadString(input);
                content.Trim();

                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].Contains(check))
                    {
                        final = result[i];
                        final = final.Replace("var url = '", " ");
                        final = final.Replace("'", " ");
                        final = final.Replace(";", " ");
                        final = final.Trim();
                    }
                }

                //just in case it returns only a piece of the needed URL
                if (!final.StartsWith("http://adf.ly") && !final.StartsWith("https://adf.ly"))
                {
                    final = "http://adf.ly" + final;
                }

                //get the true URL instead of a garbled ad.fly redirect
                Uri ourUri = new Uri(final);
                WebRequest request = WebRequest.Create(final);
                WebResponse response = request.GetResponse();
                string finalUrl = response.ResponseUri.ToString();
                response.Close();
                return finalUrl;
            }
        }
    }
}
