using System;
using System.Collections.Generic;
using Common.Interfaces;

namespace GuiComponents.Interfaces
{
    public interface ILoginFormView :IForm
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
}