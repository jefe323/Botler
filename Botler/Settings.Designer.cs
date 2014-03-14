namespace Botler
{
    partial class Settings
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.ServerAddressBox = new System.Windows.Forms.TextBox();
            this.ServerPortBox = new System.Windows.Forms.TextBox();
            this.ServerPasswordBox = new System.Windows.Forms.TextBox();
            this.BotNickBox = new System.Windows.Forms.TextBox();
            this.BotIdentBox = new System.Windows.Forms.TextBox();
            this.CommandSymbolBox = new System.Windows.Forms.TextBox();
            this.BotOpBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ProxyTypeBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ProxyPasswordBox = new System.Windows.Forms.TextBox();
            this.ProxyUserBox = new System.Windows.Forms.TextBox();
            this.ProxyPortBox = new System.Windows.Forms.TextBox();
            this.ProxyAddressBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(241, 182);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(214, 72);
            this.SaveButton.TabIndex = 0;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ServerAddressBox
            // 
            this.ServerAddressBox.Location = new System.Drawing.Point(114, 25);
            this.ServerAddressBox.Name = "ServerAddressBox";
            this.ServerAddressBox.Size = new System.Drawing.Size(100, 20);
            this.ServerAddressBox.TabIndex = 1;
            // 
            // ServerPortBox
            // 
            this.ServerPortBox.Location = new System.Drawing.Point(114, 51);
            this.ServerPortBox.Name = "ServerPortBox";
            this.ServerPortBox.Size = new System.Drawing.Size(100, 20);
            this.ServerPortBox.TabIndex = 2;
            // 
            // ServerPasswordBox
            // 
            this.ServerPasswordBox.Location = new System.Drawing.Point(114, 77);
            this.ServerPasswordBox.Name = "ServerPasswordBox";
            this.ServerPasswordBox.Size = new System.Drawing.Size(100, 20);
            this.ServerPasswordBox.TabIndex = 3;
            // 
            // BotNickBox
            // 
            this.BotNickBox.Location = new System.Drawing.Point(114, 19);
            this.BotNickBox.Name = "BotNickBox";
            this.BotNickBox.Size = new System.Drawing.Size(100, 20);
            this.BotNickBox.TabIndex = 4;
            // 
            // BotIdentBox
            // 
            this.BotIdentBox.Location = new System.Drawing.Point(114, 45);
            this.BotIdentBox.Name = "BotIdentBox";
            this.BotIdentBox.PasswordChar = '*';
            this.BotIdentBox.Size = new System.Drawing.Size(100, 20);
            this.BotIdentBox.TabIndex = 5;
            this.BotIdentBox.UseSystemPasswordChar = true;
            // 
            // CommandSymbolBox
            // 
            this.CommandSymbolBox.Location = new System.Drawing.Point(115, 71);
            this.CommandSymbolBox.Name = "CommandSymbolBox";
            this.CommandSymbolBox.Size = new System.Drawing.Size(100, 20);
            this.CommandSymbolBox.TabIndex = 6;
            // 
            // BotOpBox
            // 
            this.BotOpBox.Location = new System.Drawing.Point(115, 97);
            this.BotOpBox.Name = "BotOpBox";
            this.BotOpBox.Size = new System.Drawing.Size(100, 20);
            this.BotOpBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "IRC Server Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "IRC Server Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "IRC Server Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Bot Nick";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Ident Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Command Symbol";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(40, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Bot Operator";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ServerAddressBox);
            this.groupBox1.Controls.Add(this.ServerPortBox);
            this.groupBox1.Controls.Add(this.ServerPasswordBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 109);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Connection";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BotNickBox);
            this.groupBox2.Controls.Add(this.BotIdentBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.CommandSymbolBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.BotOpBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(13, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 129);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bot Properties";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.ProxyTypeBox);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.ProxyPasswordBox);
            this.groupBox3.Controls.Add(this.ProxyUserBox);
            this.groupBox3.Controls.Add(this.ProxyPortBox);
            this.groupBox3.Controls.Add(this.ProxyAddressBox);
            this.groupBox3.Location = new System.Drawing.Point(241, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(214, 163);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Proxy Connection";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(32, 132);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Proxy Type";
            // 
            // ProxyTypeBox
            // 
            this.ProxyTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProxyTypeBox.FormattingEnabled = true;
            this.ProxyTypeBox.Items.AddRange(new object[] {
            "None",
            "HTTP",
            "Socks4",
            "Socks4a",
            "Socks5"});
            this.ProxyTypeBox.Location = new System.Drawing.Point(98, 129);
            this.ProxyTypeBox.Name = "ProxyTypeBox";
            this.ProxyTypeBox.Size = new System.Drawing.Size(100, 21);
            this.ProxyTypeBox.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 106);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Proxy Password";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(34, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Proxy User";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(37, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Proxy Port";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Proxy Address";
            // 
            // ProxyPasswordBox
            // 
            this.ProxyPasswordBox.Location = new System.Drawing.Point(98, 103);
            this.ProxyPasswordBox.Name = "ProxyPasswordBox";
            this.ProxyPasswordBox.PasswordChar = '*';
            this.ProxyPasswordBox.Size = new System.Drawing.Size(100, 20);
            this.ProxyPasswordBox.TabIndex = 3;
            this.ProxyPasswordBox.UseSystemPasswordChar = true;
            // 
            // ProxyUserBox
            // 
            this.ProxyUserBox.Location = new System.Drawing.Point(98, 77);
            this.ProxyUserBox.Name = "ProxyUserBox";
            this.ProxyUserBox.Size = new System.Drawing.Size(100, 20);
            this.ProxyUserBox.TabIndex = 2;
            // 
            // ProxyPortBox
            // 
            this.ProxyPortBox.Location = new System.Drawing.Point(98, 51);
            this.ProxyPortBox.Name = "ProxyPortBox";
            this.ProxyPortBox.Size = new System.Drawing.Size(100, 20);
            this.ProxyPortBox.TabIndex = 1;
            // 
            // ProxyAddressBox
            // 
            this.ProxyAddressBox.Location = new System.Drawing.Point(98, 25);
            this.ProxyAddressBox.Name = "ProxyAddressBox";
            this.ProxyAddressBox.Size = new System.Drawing.Size(100, 20);
            this.ProxyAddressBox.TabIndex = 0;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 262);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SaveButton);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TextBox ServerAddressBox;
        private System.Windows.Forms.TextBox ServerPortBox;
        private System.Windows.Forms.TextBox ServerPasswordBox;
        private System.Windows.Forms.TextBox BotNickBox;
        private System.Windows.Forms.TextBox BotIdentBox;
        private System.Windows.Forms.TextBox CommandSymbolBox;
        private System.Windows.Forms.TextBox BotOpBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox ProxyTypeBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ProxyPasswordBox;
        private System.Windows.Forms.TextBox ProxyUserBox;
        private System.Windows.Forms.TextBox ProxyPortBox;
        private System.Windows.Forms.TextBox ProxyAddressBox;
    }
}