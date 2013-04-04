using System;
using Meebey.SmartIrc4net;
using MySql.Data.MySqlClient;

namespace Botler
{
    class run
    {
        static public void Command(string Channel, string Nick, string Message, IrcClient irc)
        {
            string[] args = Message.TrimEnd().Substring(Program.bot_comm_char.Length).Split(' ');
            string cmd = args[0];
            string chanOP = Botler.Utilities.authorized.getChanOP(Channel);

            try
            {
                switch (cmd.ToLower())
                {
                    case "info":
                        irc.SendMessage(SendType.Message, Channel, string.Format("{0} is a C# IRC bot written by jefe323 using the SmartIRC4Net library. Version {1}. Please see http://www.jefe323.com for more information or support", Program.bot_nick, Program.version));
                        break;
                    case "ghost":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "BotOP") == true) { Commands.Core.nick.ghost(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "nick":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "BotOP") == true) { Commands.Core.nick.change(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "quit":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "BotOP") == true) { Commands.Core.quit.exit(Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "join":
                        Commands.Core.Channel.join.command(args, Channel, Nick, irc);
                        break;
                    case "part":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "ChanOP") == true) { Commands.Core.Channel.part.command(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "chanop":
                        Commands.Core.Channel.chanop.command(args, Channel, Nick, irc);
                        break;
                    case "setsecret":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "ChanOP") == true) { Commands.Core.Channel.chantoggles.setSecret(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "getsecret":
                        Commands.Core.Channel.chantoggles.getSecret(args, Channel, Nick, irc);
                        break;
                    case "setquiet":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "ChanOP") == true) { Commands.Core.Channel.chantoggles.setQuiet(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "getquiet":
                        Commands.Core.Channel.chantoggles.getQuiet(args, Channel, Nick, irc);
                        break;
                    case "say":
                        Commands.Messaging.say.send(args, Channel, Nick, irc);
                        break;
                    case "send":
                        Commands.Messaging.broadcast.sendSpecific(args, Channel, Nick, irc);
                        break;
                    case "broadcast":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "BotOP") == true) { Commands.Messaging.broadcast.sendAll(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "rem":
                    case "remember":
                        Commands.Core.Rem.set.command(args, Channel, Nick, irc);
                        break;
                    case "forget":
                        Commands.Core.Rem.remove.command(args, Channel, Nick, irc);
                        break;
                    case "remlock":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "ChanOP") == true) { Commands.Core.Rem.toggle.lck(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "global":
                        Commands.Core.Rem.toggle.global(args, Channel, Nick, irc);
                        break;
                    case "tell":
                        Commands.Core.tell.set(args, Channel, Nick, irc);
                        break;
                    case "showtell":
                    case "showtells":
                    case "st":
                        Commands.Core.tell.get(args, Channel, Nick, irc);
                        break;
                    case "seen":
                        Commands.Core.Seen.get.command(args, Channel, Nick, irc);
                        break;
                    case "addquote":
                    case "aq":
                        Commands.Core.quote.set(args, Channel, Nick, irc);
                        break;
                    case "quote":
                    case "q":
                        Commands.Core.quote.get(args, Channel, Nick, irc);
                        break;
                    case "blacklist":
                        if (Botler.Utilities.authorized.check(Channel, Nick, "BotOP") == true) { Commands.Core.blacklist.set(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "google":
                    case "search":
                    case "g":
                        Commands._Google.Search.command(args, Channel, Nick, irc);
                        break;
                    case "image":
                    case "gis":
                        Commands._Google.ImageSearch.command(args, Channel, Nick, irc);
                        break;
                    case "youtube":
                    case "y":
                        Commands._Google.Youtube.command(args, Channel, Nick, irc);
                        break;
                    case "mcf":
                        Commands._Google.SpecialSearches.mcf.command(args, Channel, Nick, irc);
                        break;
                    case "wiki":
                        Commands._Google.SpecialSearches.wiki.command(args, Channel, Nick, irc);
                        break;
                    case "imdb":
                        Commands._Google.SpecialSearches.imdb.command(args, Channel, Nick, irc);
                        break;
                    case "shrink":
                        Commands.Links.shrink.command(args, Channel, Nick, irc);
                        break;
                    case "expand":
                        Commands.Links.expand.command(args, Channel, Nick, irc);
                        break;
                    case "deadfly":
                        Commands.Links.deadfly.command(args, Channel, Nick, irc);
                        break;
                    case "choose":
                        Commands.Misc.choose.command(args, Channel, Nick, irc);
                        break;
                    case "fire":
                        Commands.Misc.fire.command(args, Channel, Nick, irc);
                        break;
                    case "random":
                    case "r":
                        Commands.Misc.random.command(args, Channel, Nick, irc);
                        break;
                    case "shoot":
                        Commands.Misc.shoot.command(args, Channel, Nick, irc);
                        break;
                    case "eightball":
                    case "8ball":
                    case "ei":
                        Commands.Misc.EightBall.command(args, Channel, Nick, irc);
                        break;
                    case "urban":
                    case "ur":
                        Commands.Sites.UrbanDict.command(args, Channel, Nick, irc);
                        break;
                    case "u":
                        Commands.Sites.u.command(args, Channel, Nick, irc);
                        break;
                    case "twitter":
                        Commands.Sites.twitter.command(args, Channel, Nick, irc);
                        break;
                    case "twitch":
                        Commands.Sites.twitch.command(args, Channel, Nick, irc);
                        break;
                    case "synonym":
                    case "syn":
                        Commands.Sites.Synonym.command(args, Channel, Nick, irc);
                        break;
                    case "lastfm":
                    case "l":
                        Commands.Sites.lastfm.command(args, Channel, Nick, irc);
                        break;
                    case "dict":
                    case "define":
                    case "d":
                        Commands.Sites.Dict.command(args, Channel, Nick, irc);
                        break;
                    case "anagram":
                    case "an":
                        Commands.Sites.anagram.command(args, Channel, Nick, irc);
                        break;
                    case "weather":
                    case "we":
                    case "w":
                        Commands.API.weather.command(args, Channel, Nick, irc);
                        break;
                    case "location":
                    case "loc":
                        Commands.API.location.command(args, Channel, Nick, irc);
                        break;
                    case "time":
                        Commands.API.time.command(args, Channel, Nick, irc);
                        break;
                    default:
                        Commands.Core.Rem.get.command(args, Channel, Nick, irc);
                        break;
                }
            }
            catch (Exception e) { Utilities.TextFormatting.ConsoleERROR(e.Message + "\n"); }
        }
    }
}

