using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Meebey.SmartIrc4net;

namespace Botler.Commands.Misc
{
    class brainfuck
    {
        const int MAX_RAM = 10000;
        const int MAX_LOOP = 1000;

        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length != 2)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, Usage: $brainfuck <brainfuck program text>", Nick));
            }
            else
            {
                string output = bfOutput(args[1]);
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, {1}", Nick, output));
            }
        }

        private static string bfOutput(string bfProgram)
        {
            string blah = string.Empty;
            // BrainRAM
            byte[] ramData = new byte[MAX_RAM];
            int ramPtr = 0;

            // loop stack
            int[] loopPos = new int[MAX_LOOP];
            int loopPtr = -1;
            int skipNum = 0;

            // instruction pointer and current instruction
            int bfIP = -1;
            char inst = '\0';

            //bfProgram = "++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.";

            while (bfIP < bfProgram.Length - 1)
            {
                inst = Convert.ToChar(bfProgram.Substring(++bfIP, 1));

                switch (inst)
                {
                    case '+':
                        if (ramData[ramPtr] == 255) ramData[ramPtr] = 0;
                        else ramData[ramPtr]++;
                        break;
                    case '-':
                        if (ramData[ramPtr] == 0) ramData[ramPtr] = 255;
                        else ramData[ramPtr]--;
                        break;
                    case '>':
                        if (++ramPtr == MAX_RAM - 1) ramPtr = 0;
                        break;
                    case '<':
                        if (--ramPtr == -1) ramPtr = MAX_RAM - 1;
                        break;
                    case '.':
                        if (ramData[ramPtr] == 10) Console.Write("\n");
                        else blah += (char)ramData[ramPtr];
                        break;
                    case ',':
                        ramData[ramPtr] = (byte)Console.ReadKey(true).KeyChar;
                        break;
                    case '[':
                        if (ramData[ramPtr] != 0) loopPos[++loopPtr] = bfIP;
                        else
                        {
                            skipNum++;
                            while (skipNum > 0)
                            {
                                switch (bfProgram.Substring(++bfIP, 1))
                                {
                                    case "[":
                                        skipNum++;
                                        break;
                                    case "]":
                                        skipNum--;
                                        break;
                                }
                            }
                        }
                        break;
                    case ']':
                        if (ramData[ramPtr] != 0) bfIP = loopPos[loopPtr];
                        else loopPtr--;
                        break;
                }
            }
            return blah;
        }
    }
}
