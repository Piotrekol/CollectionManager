using System.Threading.Tasks;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.DataTypes
{
    public class WebCollection : Collection
    {
        private readonly string _loadPath;
        public bool Loading { get; private set; }
        public bool Loaded { get; private set; }
        public WebCollection(string loadPath, int onlineId, MapCacher instance) : base(instance)
        {
            _loadPath = loadPath;
            OnlineId = onlineId;
        }
        /// <summary>
        /// Fetches this collection data from internet
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task Load(IWebCollectionProvider provider)
        {
            if(Loading || Loaded || !provider.CanFetch())
                return;

            var collection = await provider.GetCollection(OnlineId);

            foreach (var b in collection.AllBeatmaps())
            {
                AddBeatmap(b);
            }

            Loaded = true;
            Loading = false;
        }

        public async Task Save()
        {

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

        public override int NumberOfBeatmaps { get; set; }
    }

    public interface IWebCollectionProvider
    {
        Task<ICollection> GetCollection(int collectionId);
        Task<bool> SaveCollection(ICollection collection);
        bool CanFetch();
    }
}