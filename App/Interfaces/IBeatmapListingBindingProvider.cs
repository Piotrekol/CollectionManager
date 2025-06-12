namespace CollectionManagerApp.Interfaces;
using CollectionManagerApp.Interfaces.Controls;

public interface IBeatmapListingBindingProvider
{
    void Bind(IBeatmapListingModel model);
    void UnBind(IBeatmapListingModel model);
}