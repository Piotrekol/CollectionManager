using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using CollectionManager.DataTypes;
using Common;
using Gui.Misc;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class MainSidePanelView : UserControl, IMainSidePanelView, IOnlineCollectionList
    {
        public event GuiHelpers.SidePanelActionsHandlerArgs SidePanelOperation;
        public RangeObservableCollection<WebCollection> Collections { get; private set; } = new RangeObservableCollection<WebCollection>();

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
            Menu_osustatsLogin.Click += delegate { SidePanelOperation?.Invoke(this, MainSidePanelActions.OsustatsLogin); };

            Collections.CollectionChanged += CollectionsOnCollectionChanged;
        }

        private void CollectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var rootMenu = Menu_osustatsLoadCollections;
            rootMenu.DropDownItems.Clear();
            rootMenu.DropDownItems.Add("Click any collection to add it to main view");
            rootMenu.DropDownItems.Add(new ToolStripSeparator());
            rootMenu.DropDownItems.Add("Add all", null, (s, a) =>
             {
                 SidePanelOperation?.Invoke(this, MainSidePanelActions.AddCollections, Collections);
             });
            rootMenu.DropDownItems.Add(new ToolStripSeparator());

            foreach (var c in Collections)
            {
                var item = new ToolStripMenuItem
                {
                    Text = $"{c.Name} ({c.NumberOfBeatmaps})"
                };
                item.Click += (s, a) =>
                {
                    SidePanelOperation?.Invoke(this, MainSidePanelActions.AddCollections, new List<WebCollection> { c });
                };
                item.DropDownItems.AddRange(GetCollectionSubmenus(c));
                rootMenu.DropDownItems.Add(item);
            }
        }

        private ToolStripItem[] GetCollectionSubmenus(WebCollection webCollection)
        {
            var loadCollection = new ToolStripMenuItem
            {
                Text = $"Load"
            };
            loadCollection.Click += (s, a) =>
            {
                SidePanelOperation?.Invoke(this, MainSidePanelActions.AddCollections, new List<WebCollection> { webCollection });
            };
            var uploadChanges = new ToolStripMenuItem
            {
                Text = $"Upload changes"
            };
            uploadChanges.Click += (s, a) =>
            {
                SidePanelOperation?.Invoke(this, MainSidePanelActions.UploadCollectionChanges, new List<WebCollection> { webCollection });
            };

            return new ToolStripItem[] { loadCollection, uploadChanges };
        }

        private void OnLoadCollection()
        {
            SidePanelOperation?.Invoke(this, MainSidePanelActions.LoadCollection);
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
