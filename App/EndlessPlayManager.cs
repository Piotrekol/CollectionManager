using System;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManagerExtensionsDll.DataTypes;
using CollectionManagerExtensionsDll.DataTypes.MessageBus;
using CollectionManagerExtensionsDll.Enums;
using CollectionManagerExtensionsDll.Modules;

namespace App
{
    public class EndlessPlayManager
    {
        private readonly MapCacher _mapCacher;
        private readonly StatusListener _statusListener;
        string collectionName;

        public EndlessPlayManager(MapCacher beatmapManager)
        {
            _mapCacher = beatmapManager;
            _statusListener = new StatusListener(beatmapManager);
            _statusListener.NewOsuResult += NewOsuResult;
            collectionName = $"Endless {DateTime.Now:g}";
            CreateCollection(collectionName);
        }

        private void NewOsuResult(object sender, OsuResult osuResult)
        {
            if (osuResult.Beatmap == null || osuResult.Msn.OsuState != OsuState.Playing)
                return;
            MessageBus.Send(CollectionEditArgs.AddBeatmaps(collectionName,
                new Beatmaps { osuResult.Beatmap }));
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