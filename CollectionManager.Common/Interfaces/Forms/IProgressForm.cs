namespace CollectionManager.Common.Interfaces.Forms;
using System;

public interface IProgressForm : IForm
{
    event EventHandler AbortClicked;
}