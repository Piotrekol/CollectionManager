using System;
using CollectionManager.Interfaces;
using CollectionManagerExtensionsDll.DataTypes;
using CollectionManagerExtensionsDll.Enums;
using CollectionManagerExtensionsDll.Modules.Msn;

namespace CollectionManagerExtensionsDll.Modules
{
    public class StatusListener:IDisposable
    {
        private readonly IMapDataManager _beatmapManager;
        private readonly MsnManager _msnManager = new MsnManager();
        public EventHandler<OsuResult> NewOsuResult;

        public StatusListener(IMapDataManager beatmapManager)
        {
            _beatmapManager = beatmapManager;
            _msnManager.NewMsnMessage+=NewMsnMessage;
        }

        private void NewMsnMessage(object sender, MsnResult msnResult)
        {
            Console.WriteLine("{0}: {1}",msnResult.OsuState.ToString(),msnResult.Raw);
            var maps = _beatmapManager.GetByMapString(msnResult.Artist, msnResult.Title, msnResult.Diff);
            Console.WriteLine("Found {0} maps",maps.Count);

            NewOsuResult?.Invoke(this, new OsuResult()
            {
                Beatmaps = maps,
                BeatmapSource = BeatmapSource.OsuDb,
                Msn = msnResult,
            });
        }

        public void Dispose()
        {
            _msnManager?.Dispose();
        }
    }
}