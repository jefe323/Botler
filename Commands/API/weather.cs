using System;
using System.Net;
using System.IO;
using Meebey.SmartIrc4net;

namespace Botler.Commands.API
{
    class weather
    {
        //WorldWeatherOnline
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1) { irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $weather <location>", Nick)); }
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
            string api = getApiKey.getApi("wwo_key");
            string final = "error";

            if (api == "" || api == null) { return "No valid API key found"; }

            using (var client = new WebClient())
            {
                string content = client.DownloadString("http://api.worldweatheronline.com/free/v1/weather.ashx?key=" + api + "&q=" + input + "&num_of_days=1&format=csv");
                string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string data = result[result.Length - 1];
                string[] dataSplit = data.Split(',');

                final = string.Format("Current Conditions for {0}: {1}C/{2}F Max, {3}C/{4}F Min, Winds out of the {5} at {6}MPH/{7}KPH, Currently {8} with {9}mm precipitation", input.Replace('+', ' '), dataSplit[1], dataSplit[2], dataSplit[3], dataSplit[4], dataSplit[8], dataSplit[5], dataSplit[6], dataSplit[11], dataSplit[12]);
            }
            return final;
        }
    }
}
