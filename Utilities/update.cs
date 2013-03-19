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
            Console.WriteLine("DB out of date, updating now...");
            MySqlCommand command = Program.conn.CreateCommand();

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS channels (id Channel TEXT, id ChanOP TEXT, id SuperUsers TEXT, id Quiet INT(11), id secret INT(11));";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.WriteLine("");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS blacklist (id nick TEXT, id host TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.WriteLine("");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS help (id Command TEXT, id Desc TEXT, id Usage TEXT, id Example TEXT, id Notes TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.WriteLine("");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS quote (id nick TEXT, id message TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.WriteLine("");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS rem (id Trig TEXT, id Message TEXT, id Nick TEXT, id Channel TEXT, id Time TEXT, id lck INT(11));";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.WriteLine("");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS seen (id nick TEXT, id channel TEXT, id time TEXT, id message TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.WriteLine("");

            //create table
            command.CommandText = "CREATE TABLE IF NOT EXISTS tell (id Nick_To TEXT, id Nick_From TEXT, id Message TEXT, id Time TEXT);";
            try { Program.conn.Open(); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            command.ExecuteNonQuery();
            Program.conn.Close();
            //output info
            //make prettier
            Console.WriteLine("");
        }
    }
}
