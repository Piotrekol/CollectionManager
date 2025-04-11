namespace GuiComponents.Controls;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Types;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

public partial class MainSidePanelView : UserControl, IMainSidePanelView, IOnlineCollectionList
{
    public event GuiHelpers.SidePanelActionsHandlerArgs SidePanelOperation;
    public RangeObservableCollection<WebCollection> WebCollections { get; private set; } = [];

    public RangeObservableCollection<OsuCollection> Collections
    {
        set => CollectionsOnCollectionChanged(value);
    }

    public IUserInformation UserInformation
    {
        set
        {
            if (value != null)
            {
                Menu_osustatsLogin.Text = $"Logged in as {value.UserName}";
            }
        }
    }

    public MainSidePanelView()
    {
        InitializeComponent();
        Menu_loadCollection.Click += delegate
        { OnLoadCollection(); };
        Menu_loadDefaultCollection.Click += delegate
        { OnLoadDefaultCollection(); };
        Menu_unloadCollections.Click += delegate
        { OnClearCollections(); };
        Menu_saveAllCollections.Click += delegate
        { OnSaveCollections(); };
        Menu_collectionsSplit.Click += delegate
        { OnSaveInvidualCollections(); };
        Menu_listAllCollections.Click += delegate
        { OnListAllMaps(); };
        Menu_listMissingMaps.Click += delegate
        { OnListMissingMaps(); };
        Menu_beatmapListing.Click += delegate
        { OnShowBeatmapListing(); };
        Menu_mapDownloads.Click += delegate
        { OnShowDownloadManager(); };
        Menu_downloadAllMissing.Click += delegate
        { OnDownloadAllMissing(); };
        Menu_GenerateCollections.Click += delegate
        { OnGenerateCollections(); };
        Menu_GetMissingMapData.Click += delegate
        { OnGetMissingMapData(); };
        Menu_osustatsLogin.Click += (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.OsustatsLogin);
        Menu_saveOsuCollection.Click += (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveDefaultCollection);
        Menu_resetSettings.Click += (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.ResetApplicationSettings);

        WebCollections.CollectionChanged += WebCollectionsOnCollectionChanged;
    }

    private void CollectionsOnCollectionChanged(RangeObservableCollection<OsuCollection> collections)
    {
        Menu_newCollection.DropDownItems.Clear();
        if (collections.Count == 0)
        {
            _ = Menu_newCollection.DropDownItems.Add("No new collections loaded");
            return;
        }

        List<ToolStripMenuItem> toolStripMenuItems = [];
        foreach (OsuCollection c in collections)
        {
            ToolStripMenuItem item = new()
            {
                Text = $"{c.Name} ({c.NumberOfBeatmaps})"
            };
            item.Click += (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.UploadNewCollections, new List<IOsuCollection> { c });
            toolStripMenuItems.Add(item);
        }

        Menu_newCollection.DropDownItems.AddRange(toolStripMenuItems.ToArray());
    }

    private void WebCollectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        ToolStripMenuItem rootMenu = Menu_osustatsCollections;
        rootMenu.DropDownItems.Clear();
        _ = rootMenu.DropDownItems.Add("Click any collection to add it to main view");
        _ = rootMenu.DropDownItems.Add(new ToolStripSeparator());
        _ = rootMenu.DropDownItems.Add("Add all", null, (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.AddCollections, WebCollections));
        _ = rootMenu.DropDownItems.Add(new ToolStripSeparator());

        foreach (WebCollection c in WebCollections)
        {
            ToolStripMenuItem item = new()
            {
                Text = $"{c.Name} (Online:{c.OriginalNumberOfBeatmaps}, Local:{c.NumberOfBeatmaps})"
            };
            item.Click += (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.AddCollections, new List<WebCollection> { c });
            item.DropDownItems.AddRange(GetCollectionSubmenus(c));
            _ = rootMenu.DropDownItems.Add(item);
        }
    }

    private ToolStripItem[] GetCollectionSubmenus(WebCollection webCollection)
    {
        ToolStripMenuItem loadCollection = new()
        {
            Text = $"Load"
        };
        loadCollection.Click += (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.AddCollections, new List<WebCollection> { webCollection });
        ToolStripMenuItem uploadChanges = new()
        {
            Text = $"Upload changes"
        };
        uploadChanges.Click += (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.UploadCollectionChanges, new List<WebCollection> { webCollection });
        ToolStripMenuItem deleteCollection = new()
        {
            Text = $"Delete"
        };
        deleteCollection.Click += (s, a) => SidePanelOperation?.Invoke(this, MainSidePanelActions.RemoveWebCollection, new List<WebCollection> { webCollection });
        ToolStripMenuItem openOnWeb = new()
        {
            Text = $"Open in browser"
        };
        openOnWeb.Click += (s, a) => ProcessExtensions.OpenUrl($"https://osustats.ppy.sh/collection/{webCollection.OnlineId}");

        return new ToolStripItem[] { loadCollection, uploadChanges, deleteCollection, openOnWeb };
    }

    private void OnLoadCollection() => SidePanelOperation?.Invoke(this, MainSidePanelActions.LoadCollection);

    private void OnLoadDefaultCollection() => SidePanelOperation?.Invoke(this, MainSidePanelActions.LoadDefaultCollection);

    private void OnClearCollections() => SidePanelOperation?.Invoke(this, MainSidePanelActions.ClearCollections);

    private void OnSaveCollections() => SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveCollections);

    private void OnSaveInvidualCollections() => SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveInvidualCollections);

    private void OnListAllMaps() => SidePanelOperation?.Invoke(this, MainSidePanelActions.ListAllBeatmaps);

    private void OnListMissingMaps() => SidePanelOperation?.Invoke(this, MainSidePanelActions.ListMissingMaps);

    private void OnShowBeatmapListing() => SidePanelOperation?.Invoke(this, MainSidePanelActions.ShowBeatmapListing);

    private void OnShowDownloadManager() => SidePanelOperation?.Invoke(this, MainSidePanelActions.ShowDownloadManager);

    protected virtual void OnDownloadAllMissing() => SidePanelOperation?.Invoke(this, MainSidePanelActions.DownloadAllMissing);

    protected virtual void OnGenerateCollections() => SidePanelOperation?.Invoke(this, MainSidePanelActions.GenerateCollections);
    protected virtual void OnGetMissingMapData() => SidePanelOperation?.Invoke(this, MainSidePanelActions.GetMissingMapData);
}
