namespace CollectionManager.Common.Interfaces.Forms;

using System;

public interface IForm
{
    bool IsDisposed { get; }
    void ShowAndBlock();
    void Show();
    void Close();
    event EventHandler Disposed;
    event EventHandler<FormClosingEventArgs> Closing;
}