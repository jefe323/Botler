using System;
using System.Net;
using System.IO;
using Meebey.SmartIrc4net;

namespace Botler.Commands.API
{
    class location
    {
        //WorldWeatherOnline
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $location <IP>", Nick)); }
            else
            {
                string str = string.Empty;
                foreach (string ss in args)
                    str += ss + ' ';
                str = str.Substring(args[0].Length + 1).Trim();
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
                string content = client.DownloadString("http://api.worldweatheronline.com/free/v1/search.ashx?query=" + input + "&num_of_results=1&format=tab&key=" + api);
                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string data = result[result.Length - 1];
                string[] dataSplit = data.Split('\t');

                final = string.Format("{0} is most likely located: {1}, {2}, {3} (Latitude: {4}/Longitude: {5})", input, dataSplit[0], dataSplit[2], dataSplit[1], dataSplit[3], dataSplit[4]);
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
