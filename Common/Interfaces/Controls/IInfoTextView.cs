using System;

namespace GuiComponents.Interfaces
{
    public interface IInfoTextView
    {
        bool ColorUpdateText { set; }
        string UpdateText { set; }
        string CollectionManagerStatus { set; }

        event EventHandler UpdateTextClicked;
    }
}