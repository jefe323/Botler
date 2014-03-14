using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace Botler
{
    class start
    {
        public static void StepOne()
        {
            splash();
            getSettings();
            IRCConnectionInfo();
        }

        public static void StepTwo()
        {
            MySQLConnCheck();
            Utilities.update.checkVersion(Program.GlobalVar.dbVersion);
        }

        private static void splash()
        {
            Console.WriteLine("                                                              o$o$o$o$o");
            Console.WriteLine("                                                            o$$      \"\"$oo");
            Console.WriteLine("                                                           $$\"          \"\"o");
            Console.WriteLine("                                                         o$$ o            $$");
            Console.WriteLine("                                                     \"$$\"\"  \"$     $o$ooooo$");
            Console.WriteLine("                                                      \"$o          \"\"$$$$$$$");
            Console.WriteLine("                                                       o$             $$$$$$");
            Console.WriteLine("                                                       o$             $$$$$$");
            Console.WriteLine("                                                       o$              \"\"$\"");
            Console.WriteLine("                                                       o$               $$\"");
            Console.WriteLine("                                                       o$o            o$$");
            Console.WriteLine("                                                         \"$o    oo$$$\"\"");
            Console.WriteLine("                  o oooo                                   \"$o$$\"\"\"    oo$");
            Console.WriteLine("                 o$$$$$$oo                              $oo        oo$$$$$$");
            Console.WriteLine("             o oo$$$\"$\"$\"$\"$$oo                         $$$o    oo$$$$$$$$$o");
            Console.WriteLine("            o$$\"\"            \"\"$$oo                    \"\"\"    o$$$$$$$$$$$$$$");
            Console.WriteLine("         o$$\"\"                  \"$$$                         o$$$$$$$$$$$$$$$o");
            Console.WriteLine("        $$\"                         $$o                     o$$$$$$$$$$$$$$$$$");
            Console.WriteLine("       $$   o$$$$\"                  \"$$o                   o$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("     o$$\"  $\" o$o                      $o                  o$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("    o$$   $$$ $$                       \"$o                o$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("    o$    \"$\"\"\"\"                        $$o              o$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("    $\"                                  \"$$             o$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("    $                      oo$$ooooooooo$$$$$          o$$$$$$$$$$$$$$$$$$$$\"");
            Console.WriteLine("    $       o o o oooooooo$$$o  \"\"\"$$$\"$$$$$$          o$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("   $$$$$$$$$$$$$\"\"\"\"$$$$ooo$$$$\"    \"$$$$$$$$$oooo$$$$$$$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("  $$$\"$$$oooo$$$$$\"$\"$$$$\"\"\"\"\"         $\"\"\"$$$$\"\"\"\"\"$$$$$$$$$$$$$$$\"$o$$$$$$");
            Console.WriteLine("   \"\"\"\"\"\"              \"\"$$oo          $   $$$$      $$$$$$$$$$$$$$o$$$$$$$$");
            Console.WriteLine("                          \"\"\"$\"$$ooooo$$   $$$$      $$$$$$$$$$$$$o$$$$$$$$");
            Console.WriteLine("                                      $o  o$$$       $$$$$$$$$$\"$$$$$$$$$$$");
            Console.WriteLine("                                      $\"$\"$$$$       $$$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("                                          \"$$        $$$$$$$$o$$$$$$$$$$$$$o");
            Console.WriteLine("                                           \"$        $$$$$$$$$$$$$$$$$$$$$$$o");
            Console.WriteLine("                                           $\"        $$o$$$$$$$$$$$$$$$$$$$$$o");
            Console.WriteLine("                                           $         $$o$$$$$$$$$$$$$$$$$$$$$$");
            Console.WriteLine("                                           $        o$$o $$$$$$$$$\"\"\"\"\"\"\"\"");
            Console.WriteLine("                                          o$$$$\"$\"$$\"$\"  \"\"\"");
            Console.WriteLine();
            Console.WriteLine("         .-.                    .-.;;;;;;'    .-.           .-.-.");
            Console.WriteLine("        (_) )-.      .;;.    .-(_)  .;      ;' (_)  .;;;.`-' (_) )-.     ");
            Console.WriteLine("          .: __)    ;;  `;`-'       :     .:'      ;;  (_)     .:   \\    ");
            Console.WriteLine("         .:'   `.  ;;    :.       .:'    .:'       .;;; .-.   .::.   )   ");
            Console.WriteLine("         :'      );;     ;'     .-:._  .-:.    .-.;;  .;  ; .-:. `:-'    ");
            Console.WriteLine("      (_/  `----' `;.__.'      (_/  `-(_/ `;._.   `;.___.' (_/     `:._. ");
            Console.WriteLine();

        }

        private static void getSettings()
        {
            try
            {
                string[] settings = File.ReadAllLines("settings.txt");
                Program.GlobalVar.irc_server = settings[0].Replace("server=", "");
                Program.GlobalVar.irc_port = Convert.ToInt32(settings[1].Replace("port=", ""));
                Program.GlobalVar.bot_nick = settings[2].Replace("nick=", "");
                Program.GlobalVar.bot_op = settings[3].Replace("op=", "");
                Program.GlobalVar.irc_channel = settings[4].Replace("channel=", "");
                Program.GlobalVar.bot_comm_char = settings[5].Replace("command_char=", "");
                Program.GlobalVar.mysql_server = settings[7].Replace("mysql_server=", "");
                Program.GlobalVar.mysql_port = settings[8].Replace("mysql_port=", "");
                Program.GlobalVar.mysql_database = settings[9].Replace("mysql_database=", "");
                Program.GlobalVar.mysql_user = settings[10].Replace("mysql_user=", "");
                Program.GlobalVar.mysql_password = settings[11].Replace("mysql_pass=", "");
                Program.GlobalVar.dbVersion = Convert.ToInt32(settings[14].Replace("database_version=", ""));
            }
            catch (FileNotFoundException e)
            {
                Utilities.TextFormatting.ConsoleERROR("Unable to find settings file\n");
                //create file and fill with basic info
                Console.WriteLine(e.Message);
                Program.Exit();
            }
        }

        private static void IRCConnectionInfo()
        {
            string info = string.Empty;
            Console.WriteLine("        +============================+==============================+");
            Console.Write("        |");
            Utilities.TextFormatting.ConsoleTITLE("IRC Connection Information: ");
            Console.Write("|");
            Utilities.TextFormatting.ConsoleTITLE(" MySQL Connection Information:");
            Console.WriteLine("|");

            Console.Write("        |Server: ");
            info = formatInfo(Program.GlobalVar.irc_server, 19);
            Utilities.TextFormatting.ConsoleWhite(info);
            Console.Write("| Server: ");
            info = formatInfo(Program.GlobalVar.mysql_server, 20);
            Utilities.TextFormatting.ConsoleWhite(info);
            Console.WriteLine("|");


            Console.Write("        |Port: ");
            info = formatInfo(Program.GlobalVar.irc_port.ToString(), 21);
            Utilities.TextFormatting.ConsoleWhite(info);
            Console.Write("| Port: ");
            info = formatInfo(Program.GlobalVar.mysql_port.ToString(), 22);
            Utilities.TextFormatting.ConsoleWhite(info);
            Console.WriteLine("|");


            Console.Write("        |Nick: ");
            info = formatInfo(Program.GlobalVar.bot_nick, 21);
            Utilities.TextFormatting.ConsoleWhite(info);
            Console.Write("| Database: ");
            info = formatInfo(Program.GlobalVar.mysql_database, 18);
            Utilities.TextFormatting.ConsoleWhite(info);
            Console.WriteLine("|");


            Console.Write("        |Operator: ");
            info = formatInfo(Program.GlobalVar.bot_op, 17);
            Utilities.TextFormatting.ConsoleWhite(info);
            Console.Write("| User: ");
            info = formatInfo(Program.GlobalVar.mysql_user, 22);
            Utilities.TextFormatting.ConsoleWhite(info);
            Console.WriteLine("|");

            Console.WriteLine("        +============================+==============================+");
        }

        private static string formatInfo(string Data, int length)
        {
            string output = string.Empty;
            char[] input = Data.ToCharArray();
            if (input.Length < length)
            {
                for (int i = input.Length; i <= length; i++)
                {
                    Data += " ";
                }
                return Data;
            }
            else if (input.Length > length)
            {
                for (int i = 0; i <= (length - 3); i++)
                {
                    output += input[i];
                }
                output += "...";
                return output;
            }
            else { return Data; }
        }

        private static void MySQLConnCheck()
        {
            Console.Write("MySQL Server Check ");
            try
            {
                Program.GlobalVar.conn.Open();
                Console.Write("[");
                Utilities.TextFormatting.ConsoleGreen("Online");
                Console.WriteLine("]");
            }
            catch (Exception e)
            {
                Console.Write("[");
                Utilities.TextFormatting.ConsoleRed("Offline");
                Console.WriteLine("]");
                Console.Write("   Error: ");
                Utilities.TextFormatting.ConsoleERROR(e.Message + "\n");
                //Program.GlobalVar.active = false;
                Console.Write("Press any key to exit the Program...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
            Program.GlobalVar.conn.Close();
        }

    }
}
