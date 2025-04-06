namespace CollectionManager.Common.Interfaces.Controls;

using System;

public interface ICollectionTextView
{
    event EventHandler SaveTypeChanged;
    event EventHandler IsVisibleChanged;

    bool IsVisible { get; }

    string GeneratedText { set; }
    string SelectedSaveType { get; }
    void SetListTypes(Array types);
}