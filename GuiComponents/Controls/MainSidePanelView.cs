using System;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class MainSidePanelView : UserControl, IMainSidePanelView
    {
        public event EventHandler LoadCollection;
        public event EventHandler LoadDefaultCollection;
        public event EventHandler ClearCollections;
        public event EventHandler SaveCollections;
        public event EventHandler SaveInvidualCollections;
        public event EventHandler ListAllMaps;
        public event EventHandler ListMissingMaps;
        public event EventHandler ShowBeatmapListing;
        public event EventHandler ShowDownloadManager;
        public event EventHandler DownloadAllMissing;
        public event EventHandler GenerateCollections;

        public MainSidePanelView()
        {
            InitializeComponent();
            button_loadCollection.Click += delegate { OnLoadCollection(); };
            button_loadDefaultCollection.Click += delegate { OnLoadDefaultCollection(); };
            button_unloadCollections.Click += delegate { OnClearCollections(); };
            button_saveAllCollections.Click += delegate { OnSaveCollections(); };
            button_collectionsSplit.Click += delegate { OnSaveInvidualCollections(); };
            button_listAllCollections.Click += delegate { OnListAllMaps(); };
            button_listMissingMaps.Click += delegate { OnListMissingMaps(); };
            button_beatmapListing.Click += delegate { OnShowBeatmapListing(); };
            button_mapDownloads.Click += delegate { OnShowDownloadManager(); };
            button_downloadAllMissing.Click += delegate { OnDownloadAllMissing(); };
            button_GenerateCollections.Click += delegate { OnGenerateCollections(); };
        }
        private void OnLoadCollection()
        {
            LoadCollection?.Invoke(this, null);
        }

        private void OnLoadDefaultCollection()
        {
            LoadDefaultCollection?.Invoke(this, null);
        }

        private void OnClearCollections()
        {
            ClearCollections?.Invoke(this, null);
        }

        private void OnSaveCollections()
        {
            SaveCollections?.Invoke(this, null);
        }

        private void OnSaveInvidualCollections()
        {
            SaveInvidualCollections?.Invoke(this, null);
        }

        private void OnListAllMaps()
        {
            ListAllMaps?.Invoke(this, null);
        }

        private void OnListMissingMaps()
        {
            ListMissingMaps?.Invoke(this, null);
        }

        private void OnShowBeatmapListing()
        {
            ShowBeatmapListing?.Invoke(this, null);
        }
        
        private void OnShowDownloadManager()
        {
            ShowDownloadManager?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDownloadAllMissing()
        {
            DownloadAllMissing?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnGenerateCollections()
        {
            GenerateCollections?.Invoke(this, EventArgs.Empty);
        }
    }
}
