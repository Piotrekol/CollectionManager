using System;

namespace GuiComponents.Interfaces
{
    public interface ICollectionRenameView : ICollectionAddView
    {
        string OrginalCollectionName { get; set; }
    }
}