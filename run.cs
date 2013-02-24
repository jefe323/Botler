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
            string chanOP = getChanOP(Channel);

            try
            {
                switch (cmd.ToLower())
                {
                    case "ghost":
                        if (authorized(Channel, Nick, "BotOP") == true) { Commands.Core.nick.ghost(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "nick":
                        if (authorized(Channel, Nick, "BotOP") == true) { Commands.Core.nick.change(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "quit":
                        if (authorized(Channel, Nick, "BotOP") == true) { Commands.Core.quit.exit(Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "join":
                        Commands.Core.channel.join(args, Channel, Nick, irc);
                        break;
                    case "part":
                        if (authorized(Channel, Nick, "ChanOP") == true) { Commands.Core.channel.part(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "chanop":
                        Commands.Core.channel.chanOP(args, Channel, Nick, irc);
                        break;
                    case "setsecret":
                        if (authorized(Channel, Nick, "ChanOP") == true) { Commands.Core.channel.setSecret(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "getsecret":
                        Commands.Core.channel.getSecret(args, Channel, Nick, irc);
                        break;
                    case "setquiet":
                        if (authorized(Channel, Nick, "ChanOP") == true) { Commands.Core.channel.setQuiet(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "getquiet":
                        Commands.Core.channel.getQuiet(args, Channel, Nick, irc);
                        break;
                    case "say":
                        Commands.Messaging.say.send(args, Channel, Nick, irc);
                        break;
                    case "send":
                        Commands.Messaging.broadcast.sendSpecific(args, Channel, Nick, irc);
                        break;
                    case "broadcast":
                        if (authorized(Channel, Nick, "BotOP") == true) { Commands.Messaging.broadcast.sendAll(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "rem":
                    case "remember":
                        Commands.Core.remember.set(args, Channel, Nick, irc);
                        break;
                    case "forget":
                        Commands.Core.remember.remove(args, Channel, Nick, irc);
                        break;
                    case "remlock":
                        if (authorized(Channel, Nick, "ChanOP") == true) { Commands.Core.remember.lck(args, Channel, Nick, irc); }
                        else { irc.SendMessage(SendType.Notice, Nick, String.Format("You don't have permission to access that command sir")); }
                        break;
                    case "global":
                        Commands.Core.remember.global(args, Channel, Nick, irc);
                        break;
                    case "tell":
                        Commands.Core.tell.set(args, Channel, Nick, irc);
                        break;
                    case "showtell":
                    case "showtells":
                    case "st":
                        Commands.Core.tell.get(args, Channel, Nick, irc);
                        break;
                    default:
                        Commands.Core.remember.get(args, Channel, Nick, irc);
                        break;
                }
            }
            catch (Exception e) { Utilities.TextFormatting.ConsoleERROR(e.Message + "\n"); }
        }

        static private bool authorized(string Channel, string Nick, string type)
        {
            string returnValue = "";
            if (Nick == Program.bot_op) { returnValue = "BotOP"; }
            else if (Nick == getChanOP(Channel)) { returnValue = "ChanOP"; }
            if (returnValue == type) { return true; }
            else if (returnValue == "BotOP" && type == "ChanOP") { return true; }
            else { return false; }
        }

        static private string getChanOP(string Channel)
        {
            string op = "";
            MySqlCommand command = Program.conn.CreateCommand();
            command.CommandText = "SELECT Channel,ChanOP FROM channels WHERE Channel='" + Channel.ToLower() + "'";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Channel"].ToString() == Channel.ToLower())
                {
                    op = reader["ChanOP"].ToString();
                }
            }
            Program.conn.Close();
            return op;
        }
    }
}

