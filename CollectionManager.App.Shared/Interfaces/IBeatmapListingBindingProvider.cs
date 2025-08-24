namespace CollectionManager.App.Shared.Interfaces;

using CollectionManager.App.Shared.Interfaces.Controls;

public interface IBeatmapListingBindingProvider
{
    void Bind(IBeatmapListingModel model);
    void UnBind(IBeatmapListingModel model);
}