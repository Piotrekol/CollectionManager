using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using CollectionManagerExtensionsDll.DataTypes.MessageBus;

namespace CollectionManagerExtensionsDll.Modules
{
    public class CollectionsManagerMessageBus : CollectionsManagerWithCounts
    {
        public CollectionsManagerMessageBus(Beatmaps loadedBeatmaps) : base(loadedBeatmaps)
        {
        }

        public IsCollectionNameValid IsCollectionNameValid(IsCollectionNameValid input)
        {
            input.Response = base.IsCollectionNameValid(input.CollectionName);
            return input;
        }
    }
}