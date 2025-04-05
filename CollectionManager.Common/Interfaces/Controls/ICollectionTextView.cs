namespace CollectionManager.Common.Interfaces.Controls;

using System;

public interface ICollectionTextView
{
    string GeneratedText { set; }
    string SelectedSaveType { get; }
    event EventHandler SaveTypeChanged;
    void SetListTypes(Array types);
}