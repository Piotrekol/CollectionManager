namespace CollectionManager.Core.Types;

using CollectionManager.Core.Modules.FileIo.OsuDb;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WebCollection : OsuCollection
{
    public bool Loading { get; private set; }
    public bool Loaded { get; private set; }
    public bool Modified { get; private set; }
    public WebCollection(int onlineId, MapCacher instance, bool loaded = false) : base(instance)
    {
        OnlineId = onlineId;
        Loaded = loaded;
    }
    /// <summary>
    /// Fetches this collection data from internet
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public async Task Load(IWebCollectionProvider provider)
    {
        if (Loading || Loaded || !provider.CanFetch())
        {
            return;
        }

        Loading = true;

        IOsuCollection collection = await provider.GetCollection(OnlineId);

        foreach (BeatmapExtension b in collection.AllBeatmaps())
        {
            AddBeatmap(b);
        }

        Loaded = true;
        Loading = false;
    }

    public async Task<IEnumerable<WebCollection>> Save(IWebCollectionProvider provider) => await provider.SaveCollection(this);

    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string Title
    {
        get => Name;
        set => Name = value;
    }

    public new int Id
    {
        get => OnlineId;
        set => OnlineId = value;
    }

    public int OriginalNumberOfBeatmaps { get; private set; }
    public override int NumberOfBeatmaps
    {
        get => Loaded ? base.NumberOfBeatmaps : OriginalNumberOfBeatmaps;
        set => OriginalNumberOfBeatmaps = value;
    }

    protected override void ProcessNewlyAddedMap(BeatmapExtension map)
    {
        if (Loading || Loaded)
        {
            base.ProcessNewlyAddedMap(map);
        }

        if (Loaded)
        {
            Modified = true;
        }
    }

    public override bool RemoveBeatmap(string hash)
    {
        Modified = true;

        return base.RemoveBeatmap(hash);
    }

    public override int RemoveBeatmaps(IEnumerable<string> hashes)
    {
        Modified = true;

        return base.RemoveBeatmaps(hashes);
    }
}

public interface IWebCollectionProvider
{
    Task<IOsuCollection> GetCollection(int collectionId);
    Task<IEnumerable<WebCollection>> SaveCollection(IOsuCollection collection);
    bool CanFetch();
}