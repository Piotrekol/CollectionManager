using System;

namespace GuiComponents.Interfaces
{
    public interface ILoginFormView :IForm
    {
        string Login { get; }
        string Password { get; }
        string OsuCookies { get; }
        bool ClickedLogin { get; }
        event EventHandler LoginClick;
        event EventHandler CancelClick;
    }
}