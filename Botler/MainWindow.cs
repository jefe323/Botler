using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Botler
{
    public partial class MainWindow : Form
    {
        private static Task RunBot;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Create necessary directories if they don't already exist
            Directory.CreateDirectory("Data");
            Directory.CreateDirectory("Data/Logs");
            Directory.CreateDirectory("Data/Rems");
            Directory.CreateDirectory("Data/Seen");
            Directory.CreateDirectory("Plugins");

            //Create necessary files if they don't already exist
            //Channels.xml
            if (!File.Exists("Data/Channels.xml")) { CreateChannelFile(); }
            //Tells.xml
            if (!File.Exists("Data/Tells.xml")) { CreateTellsFile(); }
            //blacklist?
            //help?
            //alias?
            //stats?
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunBot = Task.Factory.StartNew(() => Bot.Start(this));
            button1.Enabled = false;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings set = new Settings();
            set.Show();
        }

        static private void CreateChannelFile()
        {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("Channels",
                    new XElement("Channel",
                        new XAttribute("Name", "#Tavern"),
                        new XElement("Secret", false),
                        new XElement("Quiet", false),
                        new XElement("WelcomeMessage", ""),
                        new XElement("Rules", ""),
                        new XElement("InvitedBy", "jefe")
            )));

            xDoc.Save("Data/Channels.xml");
        }

        static private void CreateTellsFile()
        {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("Tells",
                    new XElement("Tell",
                        new XAttribute("To", "jefe_"),
                        new XAttribute("From", "OtherJefe"),
                        new XElement("Message", "This is a test message"),
                        new XElement("TimeSent", "1/1/2014 11:11:11")
            )));

            xDoc.Save("Data/Tells.xml");
        }

        internal void OutputTextBox_TextChanged(object sender, EventArgs e)
        {
            //OutputTextBox.ScrollToCaret();
            RichTextBox box = (RichTextBox)sender;
            box.ScrollToCaret();
        }

        private void ChannelListIndexChanged(object sender, EventArgs e)
        {
            string selected = (string)ChannelList.SelectedItem;
            //MessageBox.Show(selected);
            ChannelTabControl.SelectedTab = Bot.FindOutputControl(ChannelTabControl, selected + "Tab") as TabPage;
        }

        private void ChannelTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selected = ChannelTabControl.SelectedTab;
            string name = selected.Name.Replace("Tab", "");

            ChannelList.SelectedItem = name;
        }

        public void updateStatus(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(updateStatus), text);
                return;
            }
            else 
            { 
                this.StatusOutput.AppendText(text + "\n"); 
                this.StatusOutput.ScrollToCaret(); 
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            Bot.Disconnect();
        }
    }
}
