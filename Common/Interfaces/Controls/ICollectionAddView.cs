using System;

namespace GuiComponents.Interfaces
{
    public interface ICollectionAddView
    {
        event EventHandler CollectionNameChanged;
        event EventHandler Submited;
        event EventHandler Canceled;
        string NewCollectionName { get; }
        string ErrorText { set; }
        bool CanSubmit { set; }
    }
}