using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Botler
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            //load values from config file
            this.ServerAddressBox.Text = ConfigurationManager.AppSettings["IRC_Server_Address"];
            this.ServerPortBox.Text = ConfigurationManager.AppSettings["IRC_Server_Port"];
            this.ServerPasswordBox.Text = ConfigurationManager.AppSettings["IRC_Server_Password"];

            this.BotNickBox.Text = ConfigurationManager.AppSettings["Bot_Nick"];
            this.BotIdentBox.Text = ConfigurationManager.AppSettings["Bot_Ident_Password"];
            this.CommandSymbolBox.Text = ConfigurationManager.AppSettings["Bot_Command_Symbol"];
            this.BotOpBox.Text = ConfigurationManager.AppSettings["Bot_Operator"];

            this.ProxyAddressBox.Text = ConfigurationManager.AppSettings["Proxy_Address"];
            this.ProxyPortBox.Text = ConfigurationManager.AppSettings["Proxy_Port"];
            this.ProxyUserBox.Text = ConfigurationManager.AppSettings["Proxy_User"];
            this.ProxyPasswordBox.Text = ConfigurationManager.AppSettings["Proxy_Password"];
            //Proxy Type Selection
            string pType = ConfigurationManager.AppSettings["Proxy_Type"];
            switch (pType.ToLower())
            {
                case "http":
                    this.ProxyTypeBox.SelectedIndex = this.ProxyTypeBox.Items.IndexOf("HTTP");
                    break;
                case "socks4":
                    this.ProxyTypeBox.SelectedIndex = this.ProxyTypeBox.Items.IndexOf("Socks4");
                    break;
                case "socks4a":
                    this.ProxyTypeBox.SelectedIndex = this.ProxyTypeBox.Items.IndexOf("Socks4a");
                    break;
                case "socks5":
                    this.ProxyTypeBox.SelectedIndex = this.ProxyTypeBox.Items.IndexOf("Socks5");
                    break;
                default:
                    this.ProxyTypeBox.SelectedIndex = this.ProxyTypeBox.Items.IndexOf("None");
                    break;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            foreach (XmlElement element in xmlDoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        try
                        {
                            if (node.Attributes[0].Value.Equals("IRC_Server_Address"))
                            {
                                node.Attributes[1].Value = this.ServerAddressBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("IRC_Server_Port"))
                            {
                                node.Attributes[1].Value = this.ServerPortBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("IRC_Server_Password"))
                            {
                                node.Attributes[1].Value = this.ServerPasswordBox.Text;
                            }

                            if (node.Attributes[0].Value.Equals("Bot_Nick"))
                            {
                                node.Attributes[1].Value = this.BotNickBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("Bot_Ident_Password"))
                            {
                                node.Attributes[1].Value = this.BotIdentBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("Bot_Command_Symbol"))
                            {
                                node.Attributes[1].Value = this.CommandSymbolBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("Bot_Operator"))
                            {
                                node.Attributes[1].Value = this.BotOpBox.Text;
                            }

                            if (node.Attributes[0].Value.Equals("Proxy_Address"))
                            {
                                node.Attributes[1].Value = this.ProxyAddressBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("Proxy_Port"))
                            {
                                node.Attributes[1].Value = this.ProxyPortBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("Proxy_User"))
                            {
                                node.Attributes[1].Value = this.ProxyUserBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("Proxy_Password"))
                            {
                                node.Attributes[1].Value = this.ProxyPasswordBox.Text;
                            }
                            if (node.Attributes[0].Value.Equals("Proxy_Type"))
                            {
                                if (this.ProxyTypeBox.SelectedIndex == 0)
                                {
                                    node.Attributes[1].Value = "";
                                }
                                else if (this.ProxyTypeBox.SelectedIndex == 1)
                                {
                                    node.Attributes[1].Value = "HTTP";
                                }
                                else if (this.ProxyTypeBox.SelectedIndex == 2)
                                {
                                    node.Attributes[1].Value = "Socks4";
                                }
                                else if (this.ProxyTypeBox.SelectedIndex == 3)
                                {
                                    node.Attributes[1].Value = "Socks4a";
                                }
                                else if (this.ProxyTypeBox.SelectedIndex == 4)
                                {
                                    node.Attributes[1].Value = "Socks5";
                                }                                
                            }
                        }
                        catch { }
                    }
                }
            }

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("appSettings");

            //close window
            this.Close();
        }
    }
}
