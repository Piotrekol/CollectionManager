using System;
using System.Threading;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO.OsuDb;
using CollectionManagerExtensionsDll.DataTypes;

namespace CollectionManagerExtensionsDll.Modules.CollectionGenerator
{
    public class CollectionsGenerator
    {
        public event EventHandler CollectionsUpdated;
        public event EventHandler StatusUpdated;
        private string _status = "";
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                StatusUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
        public double ProcessingCompletionPrecentage { get; set; }

        private Thread _processingThread;
        private Collections _collections;
        private readonly MapCacher _loadedBeatmaps;
        private UserTopGenerator _userTopGenerator;

        public Collections Collections
        {
            get { return _collections; }
            set
            {
                _collections = value;
                CollectionsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
        public CollectionsGenerator(MapCacher loadedBeatmaps)
        {
            _loadedBeatmaps = loadedBeatmaps;
        }

        public string CreateCollectionName(ApiScore score, string username, string collectionNameFormat)
        {
            return UserTopGenerator.CreateCollectionName(score, username, collectionNameFormat);
        }

        public void GenerateCollection(CollectionGeneratorConfiguration configuration)
        {
            if (_userTopGenerator == null)
                _userTopGenerator = new UserTopGenerator(configuration.ApiKey, _loadedBeatmaps);
            if (_processingThread==null || !_processingThread.IsAlive)
            {
                _processingThread = new Thread(() =>
                {
                    Collections = _userTopGenerator.GetPlayersCollections(configuration, Log);
                });
                _processingThread.Start();
            }

        }

        public void Log(string message,double precentage)
        {
            ProcessingCompletionPrecentage = precentage;
            Status = message;
        }

        public void Abort()
        {
            if (_processingThread?.IsAlive ?? false)
            {
                _processingThread.Abort();
                try
                {
                    _processingThread.Join();
                }
                catch { }
                Collections = new Collections();

            }
        }
    }
}