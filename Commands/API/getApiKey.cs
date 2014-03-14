using System.IO;

namespace Botler.Commands.API
{
    class getApiKey
    {
        internal static string getApi(string key)
        {
            string line;
            string final = string.Empty;
            StreamReader file = new StreamReader("api.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith(key))
                {
                    final = line.Replace(key + "=", "");
                    break;
                }
            }
            file.Close();
            return final;
        }
    }
}
