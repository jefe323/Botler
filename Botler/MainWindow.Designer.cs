namespace Botler
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OutputTextBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.HomePage = new System.Windows.Forms.TabPage();
            this.ChannelPage = new System.Windows.Forms.TabPage();
            this.RemPage = new System.Windows.Forms.TabPage();
            this.PluginPage = new System.Windows.Forms.TabPage();
            this.CommandPage = new System.Windows.Forms.TabPage();
            this.InfoPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ChannelList = new System.Windows.Forms.ListBox();
            this.ChannelTabControl = new System.Windows.Forms.TabControl();
            this.StatusPage = new System.Windows.Forms.TabPage();
            this.ChannelTextBox = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.HomePage.SuspendLayout();
            this.ChannelPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ChannelTabControl.SuspendLayout();
            this.StatusPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.OutputTextBox.Location = new System.Drawing.Point(282, 6);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.Size = new System.Drawing.Size(323, 370);
            this.OutputTextBox.TabIndex = 0;
            this.OutputTextBox.Text = "";
            this.OutputTextBox.TextChanged += new System.EventHandler(this.OutputTextBox_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(42, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Connect!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(641, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.HomePage);
            this.tabControl1.Controls.Add(this.ChannelPage);
            this.tabControl1.Controls.Add(this.RemPage);
            this.tabControl1.Controls.Add(this.PluginPage);
            this.tabControl1.Controls.Add(this.CommandPage);
            this.tabControl1.Controls.Add(this.InfoPage);
            this.tabControl1.ItemSize = new System.Drawing.Size(103, 18);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(619, 408);
            this.tabControl1.TabIndex = 3;
            // 
            // HomePage
            // 
            this.HomePage.Controls.Add(this.groupBox2);
            this.HomePage.Controls.Add(this.groupBox1);
            this.HomePage.Controls.Add(this.OutputTextBox);
            this.HomePage.Location = new System.Drawing.Point(4, 22);
            this.HomePage.Name = "HomePage";
            this.HomePage.Padding = new System.Windows.Forms.Padding(3);
            this.HomePage.Size = new System.Drawing.Size(611, 382);
            this.HomePage.TabIndex = 0;
            this.HomePage.Text = "Home";
            this.HomePage.UseVisualStyleBackColor = true;
            // 
            // ChannelPage
            // 
            this.ChannelPage.Controls.Add(this.ChannelTextBox);
            this.ChannelPage.Controls.Add(this.ChannelTabControl);
            this.ChannelPage.Controls.Add(this.ChannelList);
            this.ChannelPage.Location = new System.Drawing.Point(4, 22);
            this.ChannelPage.Name = "ChannelPage";
            this.ChannelPage.Padding = new System.Windows.Forms.Padding(3);
            this.ChannelPage.Size = new System.Drawing.Size(611, 382);
            this.ChannelPage.TabIndex = 1;
            this.ChannelPage.Text = "Channels";
            this.ChannelPage.UseVisualStyleBackColor = true;
            // 
            // RemPage
            // 
            this.RemPage.Location = new System.Drawing.Point(4, 22);
            this.RemPage.Name = "RemPage";
            this.RemPage.Padding = new System.Windows.Forms.Padding(3);
            this.RemPage.Size = new System.Drawing.Size(611, 382);
            this.RemPage.TabIndex = 2;
            this.RemPage.Text = "Rems";
            this.RemPage.UseVisualStyleBackColor = true;
            // 
            // PluginPage
            // 
            this.PluginPage.Location = new System.Drawing.Point(4, 22);
            this.PluginPage.Name = "PluginPage";
            this.PluginPage.Padding = new System.Windows.Forms.Padding(3);
            this.PluginPage.Size = new System.Drawing.Size(611, 382);
            this.PluginPage.TabIndex = 3;
            this.PluginPage.Text = "Plugins";
            this.PluginPage.UseVisualStyleBackColor = true;
            // 
            // CommandPage
            // 
            this.CommandPage.Location = new System.Drawing.Point(4, 22);
            this.CommandPage.Name = "CommandPage";
            this.CommandPage.Padding = new System.Windows.Forms.Padding(3);
            this.CommandPage.Size = new System.Drawing.Size(611, 382);
            this.CommandPage.TabIndex = 4;
            this.CommandPage.Text = "Commands";
            this.CommandPage.UseVisualStyleBackColor = true;
            // 
            // InfoPage
            // 
            this.InfoPage.Location = new System.Drawing.Point(4, 22);
            this.InfoPage.Name = "InfoPage";
            this.InfoPage.Padding = new System.Windows.Forms.Padding(3);
            this.InfoPage.Size = new System.Drawing.Size(611, 382);
            this.InfoPage.TabIndex = 5;
            this.InfoPage.Text = "Information";
            this.InfoPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DisconnectButton);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 118);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Location = new System.Drawing.Point(42, 86);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(180, 23);
            this.DisconnectButton.TabIndex = 2;
            this.DisconnectButton.Text = "Disconnect!";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(7, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 245);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stats";
            // 
            // ChannelList
            // 
            this.ChannelList.FormattingEnabled = true;
            this.ChannelList.Items.AddRange(new object[] {
            "Status"});
            this.ChannelList.Location = new System.Drawing.Point(7, 7);
            this.ChannelList.Name = "ChannelList";
            this.ChannelList.Size = new System.Drawing.Size(135, 368);
            this.ChannelList.TabIndex = 0;
            // 
            // ChannelTabControl
            // 
            this.ChannelTabControl.Controls.Add(this.StatusPage);
            this.ChannelTabControl.Location = new System.Drawing.Point(149, 7);
            this.ChannelTabControl.Name = "ChannelTabControl";
            this.ChannelTabControl.SelectedIndex = 0;
            this.ChannelTabControl.Size = new System.Drawing.Size(456, 341);
            this.ChannelTabControl.TabIndex = 1;
            // 
            // StatusPage
            // 
            this.StatusPage.Controls.Add(this.richTextBox1);
            this.StatusPage.Location = new System.Drawing.Point(4, 22);
            this.StatusPage.Name = "StatusPage";
            this.StatusPage.Padding = new System.Windows.Forms.Padding(3);
            this.StatusPage.Size = new System.Drawing.Size(448, 315);
            this.StatusPage.TabIndex = 0;
            this.StatusPage.Text = "Status";
            this.StatusPage.UseVisualStyleBackColor = true;
            // 
            // ChannelTextBox
            // 
            this.ChannelTextBox.Location = new System.Drawing.Point(153, 354);
            this.ChannelTextBox.Name = "ChannelTextBox";
            this.ChannelTextBox.Size = new System.Drawing.Size(448, 20);
            this.ChannelTextBox.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(7, 7);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(435, 302);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 440);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Botler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.HomePage.ResumeLayout(false);
            this.ChannelPage.ResumeLayout(false);
            this.ChannelPage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ChannelTabControl.ResumeLayout(false);
            this.StatusPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.RichTextBox OutputTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage HomePage;
        private System.Windows.Forms.TabPage ChannelPage;
        private System.Windows.Forms.TabPage RemPage;
        private System.Windows.Forms.TabPage PluginPage;
        private System.Windows.Forms.TabPage CommandPage;
        private System.Windows.Forms.TabPage InfoPage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox ChannelTextBox;
        private System.Windows.Forms.TabControl ChannelTabControl;
        private System.Windows.Forms.TabPage StatusPage;
        private System.Windows.Forms.ListBox ChannelList;
        private System.Windows.Forms.RichTextBox richTextBox1;

    }
}

