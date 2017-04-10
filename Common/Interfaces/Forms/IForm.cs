using System;

namespace GuiComponents.Interfaces
{
    public interface IForm
    {
        bool IsDisposed { get; }
        void ShowAndBlock();
        void Show();
        void Close();
        event EventHandler Disposed;
        event EventHandler Closing;
    }
}