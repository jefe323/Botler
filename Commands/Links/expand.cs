using System;
using System.Net;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Links
{
    class expand
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $deadfly <adf.ly link>", Nick));
            }
            else
            {
                string output = expandOutput(args[1]);
                irc.SendMessage(SendType.Message, Channel, String.Format("{0}, {1}", Nick, output));
            }
        }

        private static string expandOutput(string input)
        {
            using (var client = new WebClient())
            {
                if (!input.Contains("adf.ly"))
                {
                    try
                    {
                        if (!input.StartsWith("http://") && !input.StartsWith("https://"))
                        {
                            input = "http://" + input;
                        }
                        Uri ourUri = new Uri(input);
                        WebRequest request = WebRequest.Create(input);
                        WebResponse response = request.GetResponse();
                        string finalUrl = response.ResponseUri.ToString();
                        response.Close();
                        return finalUrl;
                    }
                    catch
                    {
                        return "Something bad happened sir";
                    }
                }
                else
                {
                    return String.Format("Please use {0}deadfly for adf.ly links sir", Program.bot_comm_char);
                }
            }
        }
    }
}
