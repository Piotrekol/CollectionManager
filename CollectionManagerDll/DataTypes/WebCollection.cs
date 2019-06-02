using System.Collections.Generic;
using System.Threading.Tasks;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.DataTypes
{
    public class WebCollection : Collection
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
                return;

            Loading = true;

            var collection = await provider.GetCollection(OnlineId);

            foreach (var b in collection.AllBeatmaps())
            {
                AddBeatmap(b);
            }

            Loaded = true;
            Loading = false;
        }

        public async Task<IEnumerable<WebCollection>> Save(IWebCollectionProvider provider)
        {
            return await provider.SaveCollection(this);
        }

        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Title
        {
            get => Name;
            set => Name = value;
        }

        public int Id
        {
            get => OnlineId;
            set => OnlineId = value;
        }

        public int OriginalNumberOfBeatmaps { get; private set; }
        public override int NumberOfBeatmaps
        {
            get
            {
                if (this.Loaded)
                {
                    return base.NumberOfBeatmaps;
                }

                return OriginalNumberOfBeatmaps;
            }
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
    }

    public interface IWebCollectionProvider
    {
        Task<ICollection> GetCollection(int collectionId);
        Task<IEnumerable<WebCollection>> SaveCollection(ICollection collection);
        bool CanFetch();
    }
}