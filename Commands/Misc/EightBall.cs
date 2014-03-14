using System;
using Meebey.SmartIrc4net;
using Botler.Utilities;

namespace Botler.Commands.Misc
{
    class EightBall
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length == 1)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $8ball <question>?", Nick));
            }
            else
            {
                string strEi = string.Empty;
                foreach (string ss in args)
                    strEi += ss + ' ';
                strEi = strEi.Substring(args[0].Length + 1);
                string EiOutput = EightBall.EightBallOuput(strEi);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}: {1}", Nick, EiOutput));
            }
        }

        private static string EightBallOuput(string input)
        {
            Random R = new Random();
            string result = "";
            int value = R.Next(1, 20);
            switch (value)
            {
                case 1:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("It is certain"));
                    return result;
                case 2:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("It is decidedly so"));
                    return result;
                case 3:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("Without a doubt"));
                    return result;
                case 4:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("Yes, definitely"));
                    return result;
                case 5:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("You may rely on it"));
                    return result;
                case 6:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("As I see it, yes"));
                    return result;
                case 7:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("Most likely"));
                    return result;
                case 8:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("Outlook good"));
                    return result;
                case 9:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("Signs point to yes"));
                    return result;
                case 10:
                    result = input + " - " + TextFormatting.ColorGreen(TextFormatting.Bold("Yes"));
                    return result;
                case 11:
                    result = input + " - " + TextFormatting.ColorOrange(TextFormatting.Bold("Reply hazy, try again"));
                    return result;
                case 12:
                    result = input + " - " + TextFormatting.ColorOrange(TextFormatting.Bold("Ask again later"));
                    return result;
                case 13:
                    result = input + " - " + TextFormatting.ColorOrange(TextFormatting.Bold("Better not tell you now"));
                    return result;
                case 14:
                    result = input + " - " + TextFormatting.ColorOrange(TextFormatting.Bold("Cannot predict now"));
                    return result;
                case 15:
                    result = input + " - " + TextFormatting.ColorOrange(TextFormatting.Bold("Concentrate and ask again"));
                    return result;
                case 16:
                    result = input + " - " + TextFormatting.ColorRed(TextFormatting.Bold("Don't count on it"));
                    return result;
                case 17:
                    result = input + " - " + TextFormatting.ColorRed(TextFormatting.Bold("My reply is no"));
                    return result;
                case 18:
                    result = input + " - " + TextFormatting.ColorRed(TextFormatting.Bold("My sources say no"));
                    return result;
                case 19:
                    result = input + " - " + TextFormatting.ColorRed(TextFormatting.Bold("Outlook not so good"));
                    return result;
                case 20:
                    result = input + " - " + TextFormatting.ColorRed(TextFormatting.Bold("Very doubtful"));
                    return result;
                default:
                    break;
            }
            return "error";
        }
    }
}
