using System.Collections.Generic;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManagerExtensionsDll.DataTypes.MessageBus;
using CollectionManagerExtensionsDll.Enums;
using CollectionManagerExtensionsDll.Modules;

namespace App
{
    public class EndlessPlayManager
    {
        private readonly MapCacher _mapCacher;
        private readonly Dictionary<OsuState, string> _collectionNames = new Dictionary<OsuState, string>
        {
            {OsuState.Playing,"Played" },
            {OsuState.Listening,"Listened" },
            {OsuState.Watching,"Watched" },
            {OsuState.Passed,"Passed" },
            {OsuState.Failed,"Failed" }
        };
        string collectionPrefix = "Endless ";

        public EndlessPlayManager(MapCacher mapCacher, OsuFileIo osuFileIo)
        {
            _mapCacher = mapCacher;
            var playHistoryManager = new PlayHistoryManager(osuFileIo, new StatusListener(mapCacher));
            playHistoryManager.NewHistoryEntry += NewHistoryEntry;
            foreach (var name in _collectionNames)
            {
                CreateCollection($"{collectionPrefix}{name.Value}");
            }
        }

        private void NewHistoryEntry(object sender, PlayHistoryManager.PlayHistoryEntry playHistoryEntry)
        {
            if (_collectionNames.ContainsKey(playHistoryEntry.State) && playHistoryEntry.Beatmap != null)
            {
                var collectionName = $"{collectionPrefix}{_collectionNames[playHistoryEntry.State]}";
                MessageBus.Send(CollectionEditArgs.AddBeatmaps(collectionName,
                    new Beatmaps { playHistoryEntry.Beatmap }));
            }
        }

        private void CreateCollection(string name)
        {
            var isValid = (bool)(MessageBus.SendWait<IsCollectionNameValid, IsCollectionNameValid>(
                new IsCollectionNameValid(name, "EndlessPlayManager"))).Response;
            if (isValid)
                MessageBus.Send(CollectionEditArgs.AddCollections(
                    new Collections { new Collection(_mapCacher) { Name = name } }));
        }
    }
}