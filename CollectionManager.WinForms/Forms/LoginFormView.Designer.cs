namespace GuiComponents.Forms
{
    partial class LoginFormView
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
            button_Cancel = new System.Windows.Forms.Button();
            button_Login = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            textBox_password = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            textBox_login = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            textBox_osuCookies = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            richTextBox_description = new System.Windows.Forms.RichTextBox();
            label_limits = new System.Windows.Forms.Label();
            comboBox_downloadSources = new System.Windows.Forms.ComboBox();
            groupBox_login = new System.Windows.Forms.GroupBox();
            groupBox_cookies = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            groupBox_login.SuspendLayout();
            groupBox_cookies.SuspendLayout();
            SuspendLayout();
            // 
            // button_Cancel
            // 
            button_Cancel.Location = new System.Drawing.Point(213, 318);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new System.Drawing.Size(87, 28);
            button_Cancel.TabIndex = 11;
            button_Cancel.Text = "Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Login
            // 
            button_Login.Location = new System.Drawing.Point(90, 318);
            button_Login.Name = "button_Login";
            button_Login.Size = new System.Drawing.Size(87, 28);
            button_Login.TabIndex = 10;
            button_Login.Text = "Login";
            button_Login.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Enabled = false;
            label2.Location = new System.Drawing.Point(91, 49);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 15);
            label2.TabIndex = 9;
            label2.Text = "password:";
            // 
            // textBox_password
            // 
            textBox_password.Enabled = false;
            textBox_password.Location = new System.Drawing.Point(174, 46);
            textBox_password.Name = "textBox_password";
            textBox_password.PasswordChar = '*';
            textBox_password.Size = new System.Drawing.Size(116, 23);
            textBox_password.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Enabled = false;
            label1.Location = new System.Drawing.Point(118, 19);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(37, 15);
            label1.TabIndex = 7;
            label1.Text = "login:";
            // 
            // textBox_login
            // 
            textBox_login.Enabled = false;
            textBox_login.Location = new System.Drawing.Point(174, 16);
            textBox_login.Name = "textBox_login";
            textBox_login.Size = new System.Drawing.Size(116, 23);
            textBox_login.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(80, 26);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(71, 15);
            label4.TabIndex = 15;
            label4.Text = "site cookies:";
            // 
            // textBox_osuCookies
            // 
            textBox_osuCookies.Location = new System.Drawing.Point(174, 22);
            textBox_osuCookies.Name = "textBox_osuCookies";
            textBox_osuCookies.Size = new System.Drawing.Size(116, 23);
            textBox_osuCookies.TabIndex = 14;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(richTextBox_description);
            groupBox1.Controls.Add(label_limits);
            groupBox1.Controls.Add(comboBox_downloadSources);
            groupBox1.Location = new System.Drawing.Point(15, 15);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(429, 146);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            groupBox1.Text = "Download source";
            // 
            // richTextBox_description
            // 
            richTextBox_description.BackColor = System.Drawing.SystemColors.ControlLight;
            richTextBox_description.BorderStyle = System.Windows.Forms.BorderStyle.None;
            richTextBox_description.Location = new System.Drawing.Point(10, 53);
            richTextBox_description.Name = "richTextBox_description";
            richTextBox_description.ReadOnly = true;
            richTextBox_description.Size = new System.Drawing.Size(413, 87);
            richTextBox_description.TabIndex = 3;
            richTextBox_description.Text = "";
            richTextBox_description.LinkClicked += richTextBox_description_LinkClicked;
            // 
            // label_limits
            // 
            label_limits.AutoSize = true;
            label_limits.Location = new System.Drawing.Point(138, 23);
            label_limits.Name = "label_limits";
            label_limits.Size = new System.Drawing.Size(52, 15);
            label_limits.TabIndex = 2;
            label_limits.Text = "<limits>";
            // 
            // comboBox_downloadSources
            // 
            comboBox_downloadSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_downloadSources.FormattingEnabled = true;
            comboBox_downloadSources.Location = new System.Drawing.Point(10, 23);
            comboBox_downloadSources.Name = "comboBox_downloadSources";
            comboBox_downloadSources.Size = new System.Drawing.Size(121, 23);
            comboBox_downloadSources.TabIndex = 0;
            comboBox_downloadSources.SelectedIndexChanged += comboBox_downloadSources_SelectedIndexChanged;
            // 
            // groupBox_login
            // 
            groupBox_login.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox_login.Controls.Add(label1);
            groupBox_login.Controls.Add(textBox_login);
            groupBox_login.Controls.Add(label2);
            groupBox_login.Controls.Add(textBox_password);
            groupBox_login.Location = new System.Drawing.Point(15, 167);
            groupBox_login.Name = "groupBox_login";
            groupBox_login.Size = new System.Drawing.Size(429, 79);
            groupBox_login.TabIndex = 20;
            groupBox_login.TabStop = false;
            groupBox_login.Text = "Login";
            // 
            // groupBox_cookies
            // 
            groupBox_cookies.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox_cookies.Controls.Add(textBox_osuCookies);
            groupBox_cookies.Controls.Add(label4);
            groupBox_cookies.Location = new System.Drawing.Point(15, 253);
            groupBox_cookies.Name = "groupBox_cookies";
            groupBox_cookies.Size = new System.Drawing.Size(429, 59);
            groupBox_cookies.TabIndex = 21;
            groupBox_cookies.TabStop = false;
            groupBox_cookies.Text = "Cookies Login";
            // 
            // LoginFormView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(459, 353);
            Controls.Add(groupBox_cookies);
            Controls.Add(groupBox_login);
            Controls.Add(groupBox1);
            Controls.Add(button_Cancel);
            Controls.Add(button_Login);
            Name = "LoginFormView";
            Text = "Collection Manager - osu! login form";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox_login.ResumeLayout(false);
            groupBox_login.PerformLayout();
            groupBox_cookies.ResumeLayout(false);
            groupBox_cookies.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Login;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_login;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_osuCookies;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_downloadSources;
        private System.Windows.Forms.GroupBox groupBox_login;
        private System.Windows.Forms.GroupBox groupBox_cookies;
        private System.Windows.Forms.Label label_limits;
        private System.Windows.Forms.RichTextBox richTextBox_description;
    }
}