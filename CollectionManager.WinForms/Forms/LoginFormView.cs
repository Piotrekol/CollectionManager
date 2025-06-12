namespace GuiComponents.Forms;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LoginFormView : BaseForm, ILoginFormView
{
    public LoginFormView()
    {
        InitializeComponent();
        button_Login.Click += (s, a) =>
        {
            ClickedLogin = true;
            Close();
        };
        button_Login.Click += (s, a) => Close();
        AcceptButton = button_Login;
        CancelButton = button_Cancel;
        label_limits.Text = string.Empty;
    }

    public string Login => ClickedLogin ? textBox_login.Text : "";
    public string Password => ClickedLogin ? textBox_password.Text : "";
    public string OsuCookies => ClickedLogin ? textBox_osuCookies.Text : "";
    public bool ClickedLogin { get; set; }
    public string DownloadSource { get; private set; }
    public event EventHandler LoginClick;
    public event EventHandler CancelClick;
    private IReadOnlyList<IDownloadSource> _downloadSources;
    public void SetDownloadSources(IReadOnlyList<IDownloadSource> downloadSources)
    {
        _downloadSources = downloadSources;
        comboBox_downloadSources.Items.Clear();
        comboBox_downloadSources.Items.AddRange(downloadSources.Select(s => s.Name).ToArray());
        comboBox_downloadSources.SelectedIndex = 0;
    }

    private void comboBox_downloadSources_SelectedIndexChanged(object sender, EventArgs e)
    {
        DownloadSource = (string)comboBox_downloadSources.SelectedItem;
        IDownloadSource downloadSource = _downloadSources.First(s => s.Name == DownloadSource);
        label_limits.Text = downloadSource.ThrottleDownloads
            ? $"DL limits: {downloadSource.DownloadsPerMinute}/minute {downloadSource.DownloadsPerHour}/hour"
            : "no limits";
        richTextBox_description.Text = downloadSource.Description;
        groupBox_cookies.Enabled = downloadSource.UseCookiesLogin;
        groupBox_login.Enabled = downloadSource.RequiresLogin && !downloadSource.UseCookiesLogin;
    }

    private void richTextBox_description_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e) => ProcessExtensions.OpenUrl(e.LinkText);
}
