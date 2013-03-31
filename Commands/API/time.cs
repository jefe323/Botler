using System;
using System.Net;
using System.IO;
using Meebey.SmartIrc4net;

namespace Botler.Commands.API
{
    class time
    {
        //WorldWeatherOnline
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $time <location>", Nick)); }
            else
            {
                string str = string.Empty;
                foreach (string ss in args)
                    str += ss + ' ';
                str = str.Substring(args[0].Length + 1).Trim();
                str = str.Replace(' ', '+');
                string final = output(str);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1}", Nick, final));
            }
        }

        private static string output(string input)
        {
            string api = getApi();
            string final = "error";

            using (var client = new WebClient())
            {
                string content = client.DownloadString("http://api.worldweatheronline.com/free/v1/tz.ashx?key="+ api +"&q=" + input + "&format=csv");
                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string data = result[result.Length - 1];
                string[] dataSplit = data.Split(' ');

                final = string.Format("The current date/time for {0} is: {1} {2}GMT", input, dataSplit[0], dataSplit[1]);
            }

            return final;
        }

        private static string getApi()
        {
            string line;
            string final = string.Empty;
            StreamReader file = new StreamReader("api.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("wwo_key"))
                {
                    final = line.Replace("wwo_key=", "");
                    break;
                }
            }
            file.Close();
            return final;
        }
    }
}
