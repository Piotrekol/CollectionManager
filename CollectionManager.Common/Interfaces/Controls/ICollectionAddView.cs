namespace CollectionManager.Common.Interfaces.Controls;

using System;

public interface ICollectionAddView
{
    event EventHandler CollectionNameChanged;
    event EventHandler Submited;
    event EventHandler Canceled;
    string NewCollectionName { get; set; }
    string ErrorText { set; }
    bool CanSubmit { set; }
}