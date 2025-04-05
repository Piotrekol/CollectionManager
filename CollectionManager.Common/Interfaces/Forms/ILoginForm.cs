namespace CollectionManager.Common.Interfaces.Forms;
using System;
using System.Collections.Generic;

public interface ILoginFormView : IForm
{
    string Login { get; }
    string Password { get; }
    string OsuCookies { get; }
    bool ClickedLogin { get; }
    string DownloadSource { get; }
    event EventHandler LoginClick;
    event EventHandler CancelClick;
    void SetDownloadSources(IReadOnlyList<IDownloadSource> downloadSources);
}