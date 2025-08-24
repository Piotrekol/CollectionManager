namespace CollectionManager.WinForms.Forms;

using CollectionManager.Common.Interfaces.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

public class BaseForm : Form, IForm
{
    protected BaseForm()
    {
        FormClosing += (s, e) => Closing?.Invoke(this, new((Common.CloseReason)e.CloseReason));
        StartPosition = FormStartPosition.CenterParent;
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        Font = new Font("Segoe UI", 9f);
    }

    public void ShowAndBlock() => ShowDialog();

    public event EventHandler<Common.FormClosingEventArgs> Closing;
}