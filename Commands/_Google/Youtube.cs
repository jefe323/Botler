using System;
using Meebey.SmartIrc4net;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.YouTube;

namespace Botler.Commands._Google
{
    class Youtube
    {
        //https://developers.google.com/youtube/2.0/developers_guide_dotnet
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $youtube [flag] <search term(s)>", Nick));
            }
            else
            {
                string strVideo = string.Empty;
                string flag = string.Empty;
                if (args[1].StartsWith("+"))
                {
                    for (int i = 2; i < args.Length; i++)
                    {
                        strVideo += args[i] + ' ';
                    }
                    strVideo = strVideo.TrimEnd();
                    flag = args[1];
                }
                else
                {
                    foreach (string ss in args)
                        strVideo += ss + ' ';
                    strVideo = strVideo.Substring(args[0].Length + 1);
                    strVideo = strVideo.TrimEnd(' ');
                }
                string videoOutput = YoutubeOutput(strVideo, flag);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1}", Nick, videoOutput));
            }
        }

        private static string YoutubeOutput(string input, string flag)
        {
            YouTubeRequestSettings settings = new YouTubeRequestSettings("Botler", "AI39si4qO-ubkSyRTofnQsaho8bd2vsIXUd8UI874MI6_ulO6gIyR32tUSQJlok__4H0SoaQ5es7Fl1k6P4fuddYn5zdDjzSvw");
            YouTubeRequest request = new YouTubeRequest(settings);
            YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);

            if (flag == "+v")
                query.OrderBy = "viewCount";
            else if (flag == "+r")
                query.OrderBy = "rating";
            else if (flag == "+p")
                query.OrderBy = "published";
            else
                query.OrderBy = "relevance";
            query.Query = input;
            query.SafeSearch = YouTubeQuery.SafeSearchValues.None;
            Feed<Video> videoFeed = request.Get<Video>(query);

            string output = printVideoFeed(videoFeed);
            return output;
        }

        static string printVideoFeed(Feed<Video> feed)
        {
            foreach (Video entry in feed.Entries)
            {
                string output = printVideoEntry(entry);
                return output;
            }
            return "error";
        }

        static string printVideoEntry(Video video)
        {
            string duration = "";
            foreach (Google.GData.YouTube.MediaContent content in video.Contents)
            {
                TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(content.Duration));
                duration = Botler.Utilities.TextFormatting.Bold(string.Format("{0:D2} minutes, {1:D2} seconds", t.Minutes, t.Seconds));
                break;
            }
            string title = Botler.Utilities.TextFormatting.Bold(video.Title);
            string Rating = Botler.Utilities.TextFormatting.Bold(video.RatingAverage.ToString());
            string viewCount = Botler.Utilities.TextFormatting.Bold(video.ViewCount.ToString());
            string uploader = Botler.Utilities.TextFormatting.Bold(video.Uploader);
            string url = Botler.Utilities.TextFormatting.Bold("http://www.youtube.com/watch?v=" + video.VideoId);

            string result = String.Format("{0} - Time: {1} - Rating: {2} - {3} Views - Uploaded by {4} - URL: {5}", title, duration, Rating, viewCount, uploader, url);
            return result;
        }
    }
}
