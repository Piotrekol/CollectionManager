using System;
using System.Windows.Forms;
using Common;
using Gui.Misc;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class MainSidePanelView : UserControl, IMainSidePanelView
    {
        public event GuiHelpers.SidePanelActionsHandlerArgs SidePanelOperation;

        public MainSidePanelView()
        {
            InitializeComponent();
            Menu_loadCollection.Click += delegate { OnLoadCollection(); };
            Menu_loadDefaultCollection.Click += delegate { OnLoadDefaultCollection(); };
            Menu_unloadCollections.Click += delegate { OnClearCollections(); };
            Menu_saveAllCollections.Click += delegate { OnSaveCollections(); };
            Menu_collectionsSplit.Click += delegate { OnSaveInvidualCollections(); };
            Menu_listAllCollections.Click += delegate { OnListAllMaps(); };
            Menu_listMissingMaps.Click += delegate { OnListMissingMaps(); };
            Menu_beatmapListing.Click += delegate { OnShowBeatmapListing(); };
            Menu_mapDownloads.Click += delegate { OnShowDownloadManager(); };
            Menu_downloadAllMissing.Click += delegate { OnDownloadAllMissing(); };
            Menu_GenerateCollections.Click += delegate { OnGenerateCollections(); };
            Menu_GetMissingMapData.Click += delegate { OnGetMissingMapData(); };
        }
        private void OnLoadCollection()
        {
            SidePanelOperation?.Invoke(this,MainSidePanelActions.LoadCollection);
        }

        private void OnLoadDefaultCollection()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.LoadDefaultCollection);
        }

        private void OnClearCollections()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.ClearCollections);
        }

        private void OnSaveCollections()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveCollections);
        }

        private void OnSaveInvidualCollections()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveInvidualCollections);
        }

        private void OnListAllMaps()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.ListAllBeatmaps);
        }

        private void OnListMissingMaps()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.ListMissingMaps);
        }

        private void OnShowBeatmapListing()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.ShowBeatmapListing);
        }

        private void OnShowDownloadManager()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.ShowDownloadManager);
        }

        protected virtual void OnDownloadAllMissing()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.DownloadAllMissing);
        }

        protected virtual void OnGenerateCollections()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.GenerateCollections);
        }
        protected virtual void OnGetMissingMapData()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.GetMissingMapData);
        }

    }
}
