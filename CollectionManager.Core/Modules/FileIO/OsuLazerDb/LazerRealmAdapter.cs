namespace CollectionManager.Core.Modules.FileIo.OsuLazerDb;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using Realms;
using System;
using System.Collections.Generic;

internal abstract class LazerRealmAdapter
{
    public abstract Type[] ObjectTypes { get; }

    public abstract int CountScores(Realm realm);

    public abstract IEnumerable<LazerReplay> LoadScores(Realm realm);

    public abstract int CountBeatmapSets(Realm realm);

    public abstract IEnumerable<IEnumerable<LazerBeatmap>> LoadBeatmapSets(Realm realm, IScoreDataManager scoreDatabase);

    public abstract IEnumerable<OsuCollection> ReadCollections(Realm realm, MapCacher mapCacher);

    public abstract void WriteCollections(Realm realm, OsuCollections collections);
}
