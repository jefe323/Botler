using System;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web;
using Meebey.SmartIrc4net;

namespace Botler.Commands.API
{
    class translate
    {
        public static void command(string[] args, string Channel, string Nick, IrcClient irc)
        {
            if (args.Length < 3)
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $translate <New Language> <text>", Nick));
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, available languages:  arabic, bulgarian, chinese (Simplified), chinese (Traditional), czech, danish, dutch, english, estonian, finnish, french, german, greek, hebrew, hungarian, italian, japanese, korean, lithuanian, norwegian, polish, portuguese, romanian, russian, slovak, slovenian, spanish, swedish, thai, turkish, ukrainian", Nick));
            }
            else if (LangFormat(args[2]) == "Error")
            {
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, usage: $translate <New Language> <text>", Nick));
                irc.SendMessage(SendType.Message, Channel, string.Format("{0}, available languages:  arabic, bulgarian, chinese (Simplified), chinese (Traditional), czech, danish, dutch, english, estonian, finnish, french, german, greek, hebrew, hungarian, italian, japanese, korean, lithuanian, norwegian, polish, portuguese, romanian, russian, slovak, slovenian, spanish, swedish, thai, turkish, ukrainian", Nick));
            }
            else
            {
                string ClientID = getApiKey.getApi("tran_id");
                string ClientSecret = getApiKey.getApi("tran_secret");

                string strTran = string.Empty;
                if ((ClientID != "" && ClientSecret != "") || (ClientID != null && ClientSecret != null))
                {
                    for (int i = 2; i < args.Length; i++)
                    {
                        strTran += args[i] + " ";
                    }
                    string tranOutput = translate.translateOutput(args[1], strTran, ClientID, ClientSecret);
                    irc.SendMessage(SendType.Message, Channel, string.Format("{0}, {1}", Nick, tranOutput));
                }
                else
                {
                    irc.SendMessage(SendType.Notice, Nick, string.Format("{0} has not properly configured this command so it has been disabled sir", Program.GlobalVar.bot_op));
                }
            }
        }

        private static string translateOutput(string to, string text, string ID, string Secret)
        {
            AdmAccessToken admToken;
            string headerValue;
            //check usage here: https://datamarket.azure.com/account/datasets
            AdmAuthentication admAuth = new AdmAuthentication(ID, Secret);
            try
            {
                admToken = admAuth.GetAccessToken();
                // Create a header with the access_token property of the returned token
                headerValue = "Bearer " + admToken.access_token;
                string from = DetectMethod(headerValue, text);
                string final = TranslateMethod(headerValue, from, LangFormat(to), text);
                return final;
            }
            catch (WebException e)
            {
                ProcessWebException(e);
                return "error";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "error";
            }
        }

        private static string LangFormat(string input)
        {
            switch (input.ToLower())
            {
                case "arabic":
                    return "ar";
                case "bulgarian":
                    return "bg";
                case "chinese (Simplified)":
                    return "zh-chs";
                case "chinese (Traditional)":
                    return "zh-cht";
                case "czech":
                    return "cs";
                case "danish":
                    return "da";
                case "dutch":
                    return "nl";
                case "english":
                    return "en";
                case "estonian":
                    return "et";
                case "finnish":
                    return "fi";
                case "french":
                    return "fr";
                case "german":
                    return "de";
                case "greek":
                    return "el";
                case "hebrew":
                    return "he";
                case "hungarian":
                    return "hu";
                case "italian":
                    return "it";
                case "japanese":
                    return "ja";
                case "korean":
                    return "ko";
                case "lithuanian":
                    return "lv";
                case "norwegian":
                    return "no";
                case "polish":
                    return "pl";
                case "portuguese":
                    return "pt";
                case "romanian":
                    return "ro";
                case "russian":
                    return "ru";
                case "slovak":
                    return "sk";
                case "slovenian":
                    return "sl";
                case "spanish":
                    return "es";
                case "swedish":
                    return "sv";
                case "thai":
                    return "th";
                case "turkish":
                    return "tr";
                case "ukrainian":
                    return "uk-UA";
                default:
                    return "Error";
            }
        }

        private static string DetectMethod(string authToken, string text)
        {
            /*Console.WriteLine("Enter Text to detect language:");
            string textToDetect = Console.ReadLine();*/
            //Keep appId parameter blank as we are sending access token in authorization header.
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Detect?text=" + text;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            WebResponse response = null;
            try
            {
                response = httpWebRequest.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                    string languageDetected = (string)dcs.ReadObject(stream);
                    //Console.WriteLine(string.Format("Language detected:{0}", languageDetected));
                    /*Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);*/
                    return languageDetected;
                }
            }

            catch
            {
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
        }

        private static string TranslateMethod(string authToken, string from, string to, string text)
        {
            //language codes: http://msdn.microsoft.com/en-us/library/hh456380.aspx

            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            WebResponse response = null;
            try
            {
                response = httpWebRequest.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                    string translation = (string)dcs.ReadObject(stream);
                    /*Console.WriteLine("Translation for source text '{0}' from {1} to {2} is", text, "en", "es");
                    Console.WriteLine(translation);*/
                    return translation;

                }
                /*Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);*/
            }
            catch
            {
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
        }

        private static void ProcessWebException(WebException e)
        {
            Console.WriteLine("{0}", e.ToString());
            // Obtain detailed error information
            string strResponse = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)e.Response)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream, System.Text.Encoding.ASCII))
                    {
                        strResponse = sr.ReadToEnd();
                    }
                }
            }
            Console.WriteLine("Http status code={0}, error message={1}", e.Status, strResponse);
        }
    }
    [DataContract]
    public class AdmAccessToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }

    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string clientId;
        private string cientSecret;
        private string request;

        public AdmAuthentication(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.cientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            this.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
        }

        public AdmAccessToken GetAccessToken()
        {
            return HttpPost(DatamarketAccessUri, this.request);
        }

        private AdmAccessToken HttpPost(string DatamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(DatamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
            webRequest.ContentLength = bytes.Length;
            using (Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }
}
