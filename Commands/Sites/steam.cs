using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Sites
{
    class steam
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $steam <steamID>", Nick));
            }
            else
            {
                string output = steamUser(args[1]);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, {1}", Nick, output));
            }
        }

        private static string steamUser(string user)
        {
            using (WebClient client = new WebClient())
            {
                int gCount, bCount, iCount, fCount, hours;
                gCount = bCount = iCount = fCount = hours = 0;
                string name, level, lastPlayed, mostPlayed;
                name = level = lastPlayed = mostPlayed = string.Empty;

                try
                {
                    string output = client.DownloadString("http://steamcommunity.com/id/" + user);
                    string[] lines = output.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (lines[4] == "<title>Steam Community :: Error</title>")
                    {
                        return "User does not exist";
                    }
                    string data = string.Empty;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        //get name
                        if (lines[i].Contains("g_rgProfileData"))
                        {
                            try
                            {
                                data = lines[i].Trim().Replace("g_rgProfileData = ", "").Replace(";", "");
                                JObject o = JObject.Parse(data);
                                name = o["personaname"].ToString();
                            }
                            catch { }
                        }
                        //get level
                        else if (lines[i].Contains("<div class=\"persona_name persona_level\">Level"))
                        {
                            string[] temp = lines[i].Split('>');
                            level = temp[3].Replace("</span", "");
                        }
                        //get number of badges
                        else if (lines[i].Contains("<span class=\"count_link_label\">Badges</span>"))
                        {
                            bCount = Convert.ToInt32(lines[i + 2].Trim().Replace("</span>", ""));
                        }
                        //get inventory
                        else if (lines[i].Contains("<span class=\"count_link_label\">Inventory</span>"))
                        {
                            iCount = Convert.ToInt32(lines[i + 2].Trim().Replace("</span>", ""));
                        }
                        //get games
                        else if (lines[i].Contains("<span class=\"count_link_label\">Games</span>"))
                        {
                            gCount = Convert.ToInt32(lines[i + 2].Trim().Replace("</span>", ""));
                        }
                        //get friends
                        else if (lines[i].Contains("<span class=\"count_link_label\">Friends</span>"))
                        {
                            fCount = Convert.ToInt32(lines[i + 2].Trim().Replace("</span>", ""));
                        }
                        //get last game played
                        else if (lines[i].Contains("<div class=\"recent_games\">"))
                        {
                            string[] temps = lines[i + 8].Split('>');
                            lastPlayed += temps[2].Replace("</a", "") + " (";
                            lastPlayed += lines[i + 7].Replace("</div>", "").Trim() + ")";
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message + "\n" + e.StackTrace); }

                //get total time played
                try
                {
                    string output = client.DownloadString("http://steamcommunity.com/id/" + user + "/games?tab=all");
                    string[] lines = output.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string data = string.Empty;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("	var rgGames = "))
                        {
                            data = lines[i].Trim().Replace("var rgGames = ", "").Replace(";", "");
                        }
                    }
                    JArray j = JArray.Parse(data);
                    mostPlayed = j[0]["name"].ToString() + " (" + j[0]["hours_forever"].ToString() + " hours)";
                    foreach (var game in j)
                    {
                        hours += Convert.ToInt32(game["hours_forever"]);
                    }
                }
                catch { }
                string final = String.Format("{0} ({1}) -- Level {2}, {3} Badges, {4} items in Inventory, {5} Games Owned, {6} friends. Last game they played was {7} and their most played game is {8}. {0} has {9} hours total time played on Steam", name, user, level, bCount, iCount, gCount, fCount, lastPlayed, mostPlayed, hours);
                return final;
            }
        }

        //someday I'll use this :/
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
