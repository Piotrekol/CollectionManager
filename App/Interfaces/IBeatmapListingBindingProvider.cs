namespace App.Interfaces
{
    public interface IBeatmapListingBindingProvider
    {
        void Bind(IBeatmapListingModel model);
        void UnBind(IBeatmapListingModel model);
    }
}