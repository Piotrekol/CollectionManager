using System;

namespace GuiComponents.Interfaces
{
    public interface ICollectionTextView
    {
        string GeneratedText { set; }
        string SelectedSaveType { get; }
        event EventHandler SaveTypeChanged;
        void SetListTypes(Array types);
    }
}