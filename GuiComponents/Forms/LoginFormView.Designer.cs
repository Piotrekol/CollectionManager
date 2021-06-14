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
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Login = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_login = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_osuCookies = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox_description = new System.Windows.Forms.RichTextBox();
            this.label_limits = new System.Windows.Forms.Label();
            this.comboBox_downloadSources = new System.Windows.Forms.ComboBox();
            this.groupBox_login = new System.Windows.Forms.GroupBox();
            this.groupBox_cookies = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox_login.SuspendLayout();
            this.groupBox_cookies.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(213, 318);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(87, 28);
            this.button_Cancel.TabIndex = 11;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Login
            // 
            this.button_Login.Location = new System.Drawing.Point(90, 318);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(87, 28);
            this.button_Login.TabIndex = 10;
            this.button_Login.Text = "Login";
            this.button_Login.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(91, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "password:";
            // 
            // textBox_password
            // 
            this.textBox_password.Enabled = false;
            this.textBox_password.Location = new System.Drawing.Point(174, 46);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(116, 23);
            this.textBox_password.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(118, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "login:";
            // 
            // textBox_login
            // 
            this.textBox_login.Enabled = false;
            this.textBox_login.Location = new System.Drawing.Point(174, 16);
            this.textBox_login.Name = "textBox_login";
            this.textBox_login.Size = new System.Drawing.Size(116, 23);
            this.textBox_login.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "site cookies:";
            // 
            // textBox_osuCookies
            // 
            this.textBox_osuCookies.Location = new System.Drawing.Point(174, 22);
            this.textBox_osuCookies.Name = "textBox_osuCookies";
            this.textBox_osuCookies.Size = new System.Drawing.Size(116, 23);
            this.textBox_osuCookies.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.richTextBox_description);
            this.groupBox1.Controls.Add(this.label_limits);
            this.groupBox1.Controls.Add(this.comboBox_downloadSources);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 146);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Download source";
            // 
            // richTextBox_description
            // 
            this.richTextBox_description.BackColor = System.Drawing.SystemColors.ControlLight;
            this.richTextBox_description.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_description.Location = new System.Drawing.Point(10, 53);
            this.richTextBox_description.Name = "richTextBox_description";
            this.richTextBox_description.ReadOnly = true;
            this.richTextBox_description.Size = new System.Drawing.Size(413, 87);
            this.richTextBox_description.TabIndex = 3;
            this.richTextBox_description.Text = "";
            this.richTextBox_description.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox_description_LinkClicked);
            // 
            // label_limits
            // 
            this.label_limits.AutoSize = true;
            this.label_limits.Location = new System.Drawing.Point(138, 23);
            this.label_limits.Name = "label_limits";
            this.label_limits.Size = new System.Drawing.Size(52, 15);
            this.label_limits.TabIndex = 2;
            this.label_limits.Text = "<limits>";
            // 
            // comboBox_downloadSources
            // 
            this.comboBox_downloadSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_downloadSources.FormattingEnabled = true;
            this.comboBox_downloadSources.Location = new System.Drawing.Point(10, 23);
            this.comboBox_downloadSources.Name = "comboBox_downloadSources";
            this.comboBox_downloadSources.Size = new System.Drawing.Size(121, 23);
            this.comboBox_downloadSources.TabIndex = 0;
            this.comboBox_downloadSources.SelectedIndexChanged += new System.EventHandler(this.comboBox_downloadSources_SelectedIndexChanged);
            // 
            // groupBox_login
            // 
            this.groupBox_login.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_login.Controls.Add(this.label1);
            this.groupBox_login.Controls.Add(this.textBox_login);
            this.groupBox_login.Controls.Add(this.label2);
            this.groupBox_login.Controls.Add(this.textBox_password);
            this.groupBox_login.Location = new System.Drawing.Point(15, 167);
            this.groupBox_login.Name = "groupBox_login";
            this.groupBox_login.Size = new System.Drawing.Size(429, 79);
            this.groupBox_login.TabIndex = 20;
            this.groupBox_login.TabStop = false;
            this.groupBox_login.Text = "Login";
            // 
            // groupBox_cookies
            // 
            this.groupBox_cookies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_cookies.Controls.Add(this.textBox_osuCookies);
            this.groupBox_cookies.Controls.Add(this.label4);
            this.groupBox_cookies.Location = new System.Drawing.Point(15, 253);
            this.groupBox_cookies.Name = "groupBox_cookies";
            this.groupBox_cookies.Size = new System.Drawing.Size(429, 59);
            this.groupBox_cookies.TabIndex = 21;
            this.groupBox_cookies.TabStop = false;
            this.groupBox_cookies.Text = "Cookies Login";
            // 
            // LoginFormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 353);
            this.Controls.Add(this.groupBox_cookies);
            this.Controls.Add(this.groupBox_login);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Login);
            this.Name = "LoginFormView";
            this.Text = "Collection Manager - osu! login form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_login.ResumeLayout(false);
            this.groupBox_login.PerformLayout();
            this.groupBox_cookies.ResumeLayout(false);
            this.groupBox_cookies.PerformLayout();
            this.ResumeLayout(false);

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