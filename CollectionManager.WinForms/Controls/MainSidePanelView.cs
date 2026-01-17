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

        Menu_loadCollection.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.LoadCollection);
        Menu_loadDefaultCollection.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.LoadDefaultCollection);
        Menu_unloadCollections.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.ClearCollections);
        Menu_saveAllCollectionsAsDb.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveCollectionsAsDb);
        Menu_saveAllCollectionsAsOsdb.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveCollectionsAsOsdb);
        Menu_collectionsSplit.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveIndividualCollections);
        Menu_listAllCollections.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.ListAllBeatmaps);
        Menu_listMissingMaps.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.ListMissingMaps);
        Menu_beatmapListing.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.ShowBeatmapListing);
        Menu_mapDownloads.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.ShowDownloadManager);
        Menu_downloadAllMissing.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.DownloadAllMissing);
        Menu_GenerateCollections.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.GenerateCollections);
        Menu_GetMissingMapData.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.GetMissingMapData);
        Menu_osustatsLogin.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.OsustatsLogin);
        Menu_saveOsuCollection.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.SaveDefaultCollection);
        Menu_resetSettings.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.ResetApplicationSettings);
        Menu_searchSyntax.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.SyntaxHelp);
        Menu_discord.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.Discord);
        Menu_github.Click += (_, _) => SidePanelOperation?.Invoke(this, MainSidePanelActions.Github);

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
}
