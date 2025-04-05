namespace CollectionManagerApp.Interfaces.Controls;

public interface IMusicControlModel : IGenericMapSetterModel, IFormEvents
{
    event EventHandler NextMapRequest;
    void EmitNextMapRequest();
}