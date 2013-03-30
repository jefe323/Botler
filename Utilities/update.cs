using System;
using System.Text;
using MySql.Data.MySqlClient;
using System.IO;

namespace Botler.Utilities
{
    class update
    {
        static public void checkVersion(int version)
        {
            //make prettier
            Console.WriteLine("Checking DB version");
            if (version == 0) 
            { 
                updateToOne();
                Program.dbVersion = 1;
                //write to file
                StringBuilder newFile = new StringBuilder();
                string[] file = File.ReadAllLines("settings.txt");
                foreach (string line in file)
                {
                    if (line.StartsWith("database_version="))
                    {
                        string temp = line.Replace("database_version=0", "database_version=1");
                        newFile.Append(temp + "\r\n");
                        continue;
                    }
                    newFile.Append(line + "\r\n");
                }
                File.WriteAllText("settings.txt", newFile.ToString());
            }
        }

        static private void updateToOne()
        {//make prettier
            Console.WriteLine("\nDB out of date, updating now...");
            MySqlCommand command = Program.conn.CreateCommand();

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS channels (Channel TEXT, ChanOP TEXT, SuperUsers TEXT, Quiet INT(11), secret INT(11));";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.Write("    [");
            Utilities.TextFormatting.ConsoleGreen("+");
            Console.Write("] ");
            Console.WriteLine("Created \"channels\" table");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS blacklist (nick TEXT, host TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.Write("    [");
            Utilities.TextFormatting.ConsoleGreen("+");
            Console.Write("] ");
            Console.WriteLine("Created \"blacklist\" table");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS help_info (Comm TEXT, info TEXT, how TEXT, ex TEXT, add_info TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.Write("    [");
            Utilities.TextFormatting.ConsoleGreen("+");
            Console.Write("] ");
            Console.WriteLine("Created \"help\" table");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS quote (nick TEXT, message TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.Write("    [");
            Utilities.TextFormatting.ConsoleGreen("+");
            Console.Write("] ");
            Console.WriteLine("Created \"quote\" table");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS rem (Trig TEXT, Message TEXT, Nick TEXT, Channel TEXT, Time TEXT, lck INT(11));";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.Write("    [");
            Utilities.TextFormatting.ConsoleGreen("+");
            Console.Write("] ");
            Console.WriteLine("Created \"rem\" table");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS seen (nick TEXT, channel TEXT, time TEXT, message TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.Write("    [");
            Utilities.TextFormatting.ConsoleGreen("+");
            Console.Write("] ");
            Console.WriteLine("Created \"seen\" table");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS tell (Nick_To TEXT, Nick_From TEXT, Message TEXT, Time TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.Write("    [");
            Utilities.TextFormatting.ConsoleGreen("+");
            Console.Write("] ");
            Console.WriteLine("Created \"tell\" table");


            Console.WriteLine("Database up to date!\n");
        }
    }
}
