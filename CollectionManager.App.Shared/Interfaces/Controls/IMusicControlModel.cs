namespace CollectionManager.App.Shared.Interfaces.Controls;

using CollectionManager.App.Shared.Interfaces;

public interface IMusicControlModel : IGenericMapSetterModel, IFormEvents
{
    event EventHandler NextMapRequest;
    void EmitNextMapRequest();
}