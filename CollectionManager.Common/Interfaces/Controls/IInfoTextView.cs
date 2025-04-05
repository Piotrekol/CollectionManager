namespace CollectionManager.Common.Interfaces.Controls;

using System;

public interface IInfoTextView
{
    bool ColorUpdateText { set; }
    string UpdateText { set; }
    string CollectionManagerStatus { set; }

    event EventHandler UpdateTextClicked;
}