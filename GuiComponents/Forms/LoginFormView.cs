using System;
using System.Diagnostics;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
    public partial class LoginFormView : BaseForm, ILoginFormView
    {
        public LoginFormView()
        {
            InitializeComponent();
            button_Login.Click += (s, a) => { ClickedLogin = true; this.Close(); };
            button_Login.Click += (s, a) => { this.Close(); };
            AcceptButton = button_Login;
            CancelButton = button_Cancel;
        }


        public string Login => ClickedLogin ? textBox_login.Text : "";
        public string Password => ClickedLogin ? textBox_password.Text : "";
        public string OsuCookies => ClickedLogin ? textBox_osuCookies.Text : "";
        public bool ClickedLogin { get; set; }
        public event EventHandler LoginClick;
        public event EventHandler CancelClick;

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.gg/N854wYZ");
        }
    }
}
