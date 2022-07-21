using System;

namespace GuiComponents.Interfaces
{
    public interface IProgressForm : IForm
    {
        event EventHandler AbortClicked;
    }
}