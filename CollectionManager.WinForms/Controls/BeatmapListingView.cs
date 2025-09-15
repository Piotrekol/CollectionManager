namespace GuiComponents.Controls;

using BrightIdeasSoftware;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules;
using CollectionManager.WinForms;
using CollectionManager.WinForms.FastObjectListView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

public partial class BeatmapListingView : UserControl, IBeatmapListingView, IDisposable
{
    private bool _allowForDeletion;
    private Mods _currentMods = Mods.Nm;
    private PlayMode _currentPlayMode = PlayMode.Osu;
    private DifficultyCalculator _difficultyCalculator = new();
    private List<BeatmapGroupColumn> _displayColumns = [];
    private readonly Dictionary<object, BeatmapListingAction> _menuStripClickActions;
    private SortableFastListBeatmapGroupingStrategy _groupingStrategy;

    public event EventHandler SearchTextChanged;
    public event EventHandler SelectedBeatmapChanged;
    public event EventHandler SelectedBeatmapsChanged;
    public event EventHandler BeatmapSearchHelpClicked;
    public event GuiHelpers.BeatmapListingActionArgs BeatmapOperation;
    public event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;
    public event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;
    public event GuiHelpers.BeatmapGroupColumnChangedEventArgs BeatmapGroupColumnChanged;
    public event GuiHelpers.BeatmapGroupCollapsedChangedEventArgs BeatmapGroupCollapsedChanged;

    public string SearchText => textBox_beatmapSearch.Text;
    public string ResultText { get; set; }

    [Description("Should user be able to delete beatmaps from the list?"), Category("Layout")]
    public bool AllowForDeletion
    {
        get => _allowForDeletion;
        set
        {
            _allowForDeletion = value;
            DeleteMapMenuStrip.Enabled = value;
        }
    }

    public void SetCurrentPlayMode(PlayMode playMode) => _currentPlayMode = playMode;
    public void SetCurrentMods(Mods mods) => _currentMods = mods;

    public void SetBeatmaps(IEnumerable beatmaps)
    {
        ListViewBeatmaps.SetObjects(beatmaps);
        UpdateResultsCount();
    }

    public void SetVisibleColumns(string[] visibleColumnsAspectNames)
    {
        foreach (OLVColumn column in ListViewBeatmaps.AllColumns)
        {
            column.IsVisible = visibleColumnsAspectNames.Contains(column.AspectName);
        }

        ListViewBeatmaps.RebuildColumns();
    }

    public void SetGroupColumn(string columnName)
    {
        BeatmapGroupColumn groupColumn = _displayColumns.FirstOrDefault(groupColumn => groupColumn.Text == columnName);

        if (groupColumn is null)
        {
            return;
        }

        comboBox_grouping.SelectedItem = groupColumn;
    }

    public Beatmap SelectedBeatmap
    {
        get => (Beatmap)ListViewBeatmaps.SelectedObject
                ?? (ListViewBeatmaps.SelectedObjects.Count != 0
                    ? (Beatmap)ListViewBeatmaps.SelectedObjects[0]
                    : null);

        private set
        {
            ListViewBeatmaps.SelectedObject = value;
            ListViewBeatmaps.EnsureSelectionIsVisible();
        }
    }

    public Beatmaps SelectedBeatmaps
    {
        get
        {
            Beatmaps beatmaps = [];
            if (ListViewBeatmaps.SelectedObjects.Count > 0)
            {
                foreach (object o in ListViewBeatmaps.SelectedObjects)
                {
                    beatmaps.Add((BeatmapExtension)o);
                }
            }

            return beatmaps;
        }
    }

    public BeatmapListingView()
    {
        InitializeComponent();
        InitListView();
        Bind();

        _menuStripClickActions = new()
        {
            [DeleteMapMenuStrip] = BeatmapListingAction.DeleteBeatmapsFromCollection,
            [DownloadMapInBrowserMenuStrip] = BeatmapListingAction.DownloadBeatmaps,
            [DownloadMapManagedMenuStrip] = BeatmapListingAction.DownloadBeatmapsManaged,
            [OpenBeatmapPageMapMenuStrip] = BeatmapListingAction.OpenBeatmapPages,
            [copyAsTextMenuStrip] = BeatmapListingAction.CopyBeatmapsAsText,
            [copyUrlMenuStrip] = BeatmapListingAction.CopyBeatmapsAsUrls,
            [OpenBeatmapFolderMenuStrip] = BeatmapListingAction.OpenBeatmapFolder,
            [PullMapsetMenuStrip] = BeatmapListingAction.PullWholeMapSet,
            [exportBeatmapSetsMenuItem] = BeatmapListingAction.ExportBeatmapSets,
        };
    }

    private void Bind()
    {
        textBox_beatmapSearch.TextChanged += delegate
         {
             OnSearchTextChanged();
         };
        ListViewBeatmaps.SelectionChanged += (_, __) =>
        {
            OnSelectedBeatmapChanged();
            OnSelectedBeatmapsChanged();
        };
        ListViewBeatmaps.CellRightClick += delegate (object s, CellRightClickEventArgs args)
        {
            args.MenuStrip = BeatmapsContextMenuStrip;
        };
        ListViewBeatmaps.ColumnWidthChanged += OnColumnReordered;
    }

    private void OnColumnReordered(object sender, ColumnWidthChangedEventArgs e)
    {
        IEnumerable<string> visibleColumnAspectNames = ListViewBeatmaps.AllColumns
            .Where(column => column.IsVisible)
            .Select(column => column.AspectName);

        ColumnsToggled?.Invoke(this, visibleColumnAspectNames.ToArray());
    }

    private void UpdateResultsCount()
    {
        int count = 0;
        foreach (object b in ListViewBeatmaps.FilteredObjects)
        {
            count++;
        }

        label_resultsCount.Text = string.Format("{0} {1}", count, count == 1 ? "map" : "maps");
    }
    private void InitListView()
    {
        // listview
        ListViewBeatmaps.FullRowSelect = true;
        ListViewBeatmaps.AllowColumnReorder = true;
        ListViewBeatmaps.Sorting = System.Windows.Forms.SortOrder.Descending;
        ListViewBeatmaps.UseHotItem = true;
        ListViewBeatmaps.UseTranslucentHotItem = true;
        ListViewBeatmaps.UseFiltering = true;
        ListViewBeatmaps.UseNotifyPropertyChanged = true;
        ListViewBeatmaps.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;

        InitListViewGrouping();

        // column display format
        string format = "{0:0.##}";
        column_ar.AspectToStringFormat = format;
        column_cs.AspectToStringFormat = format;
        column_od.AspectToStringFormat = format;
        column_hp.AspectToStringFormat = format;
        column_stars.AspectToStringFormat = format;

        column_LastPlayed.AspectToStringConverter = DataListViewFormatter.FormatDateTimeOffset;
        column_EditDate.AspectToStringConverter = DataListViewFormatter.FormatDateTimeOffset;
        column_LastScoreDate.AspectToStringConverter = DataListViewFormatter.FormatDateTimeOffset;

        column_stars.AspectGetter = rowObject =>
        {
            if (rowObject is Beatmap beatmap)
            {
                return beatmap.Stars(_currentPlayMode, _currentMods);
            }

            return null;
        };

        column_ar.AspectGetter = rowObject =>
        {
            if (rowObject is Beatmap beatmap)
            {
                return _difficultyCalculator.ApplyMods(beatmap, _currentMods).Ar;
            }

            return null;
        };
        column_od.AspectGetter = rowObject =>
        {
            if (rowObject is Beatmap beatmap)
            {
                return _difficultyCalculator.ApplyMods(beatmap, _currentMods).Od;
            }

            return null;
        };
        column_cs.AspectGetter = rowObject =>
        {
            if (rowObject is Beatmap beatmap)
            {
                return _difficultyCalculator.ApplyMods(beatmap, _currentMods).Cs;
            }

            return null;
        };
        column_hp.AspectGetter = rowObject =>
        {
            if (rowObject is Beatmap beatmap)
            {
                return _difficultyCalculator.ApplyMods(beatmap, _currentMods).Hp;
            }

            return null;
        };

        column_bpm.AspectGetter = rowObject =>
        {
            if (rowObject is Beatmap beatmap)
            {
                return beatmap.MainBpm * _difficultyCalculator.ApplyMods(beatmap, _currentMods).BpmMultiplier;
            }

            return null;
        };
        column_bpm.AspectToStringConverter = delegate (object cellValue)
        {
            if (cellValue == null)
            {
                return string.Empty;
            }

            return $"{cellValue:0.##}";
        };

        column_state.AspectGetter = rowObject =>
        {
            if (rowObject is BeatmapExtension beatmap)
            {
                return beatmap.LocalBeatmapMissing ? "Missing" : beatmap.StateStr;
            }

            return null;
        };

        AspectToStringConverterDelegate gradeConverter = new(cellValue =>
        {
            if (cellValue == null || (OsuGrade)cellValue == CollectionManager.Core.Enums.OsuGrade.Null)
            {
                return "";
            }

            return cellValue.ToString();
        });
        OsuGrade.AspectToStringConverter = gradeConverter;
        TaikoGrade.AspectToStringConverter = gradeConverter;
        CatchGrade.AspectToStringConverter = gradeConverter;
        ManiaGrade.AspectToStringConverter = gradeConverter;

        RearrangingDropSink dropsink = new()
        {
            CanDropBetween = false,
            CanDropOnItem = false,
            CanDropOnSubItem = false,
            CanDropOnBackground = true
        };
        dropsink.ModelDropped += DropsinkOnModelDropped;
        ListViewBeatmaps.DropSink = dropsink;

    }

    private void InitListViewGrouping()
    {
        ListViewBeatmaps.ShowGroups = false;
        ListViewBeatmaps.ShowItemCountOnGroups = true;
        ListViewBeatmaps.HasCollapsibleGroups = true;
        ListViewBeatmaps.AlwaysGroupByColumn = column_Directory;
        ListViewBeatmaps.GroupingStrategy = _groupingStrategy = new SortableFastListBeatmapGroupingStrategy(column_name, column_SetId);
        ListViewBeatmaps.SortGroupItemsByPrimaryColumn = false;
        ListViewBeatmaps.PrimarySortColumn = column_name;
        ListViewBeatmaps.OLVGroups ??= [];

        string singularFormat = "{0}";
        string pluralFormat = "{0} ({1} maps)";

        foreach (OLVColumn column in ListViewBeatmaps.Columns)
        {
            column.GroupWithItemCountFormat = pluralFormat;
            column.GroupWithItemCountSingularFormat = singularFormat;
        }

        ListViewBeatmaps.GroupExpandingCollapsing += OnGroupExpandingCollapsing;

        // user group selection
        comboBox_grouping.DisplayMember = nameof(BeatmapGroupColumn.Text);
        List<OLVColumn> excludedGroupingColumns =
        [
            column_Comment,
            column_LastPlayed,
            column_LastScoreDate,
            column_EditDate,
            column_MapId,
            column_LocalVersionDiffers,
        ];

        _displayColumns = [
            new(null, "No grouping"),
            new(column_Directory, "Beatmap Sets"),
            new(null, "---------"),
            new(column_stars, "Star rating"),
            new(column_ar, "AR"),
            new(column_cs, "CS"),
            new(column_hp, "HP"),
            new(column_od, "OD"),
            new(column_bpm, "BPM"),
            new(column_SetId, "Set id"),
            new(ScoresCount, "Scores set count"),
            new(column_state, "Status"),
            new(OsuGrade, "Osu top rank"),
            new(CatchGrade, "Catch top rank"),
            new(TaikoGrade, "Taiko top rank"),
            new(ManiaGrade, "Mania top rank"),
        ];

        comboBox_grouping.DataSource = _displayColumns;
    }

    private void OnGroupExpandingCollapsing(object sender, GroupExpandingCollapsingEventArgs args)
    {
        if (args.Group is null)
        {
            return;
        }

        args.Group.Collapsed = !args.Group.Collapsed;
        args.Canceled = true;
    }

    private void DropsinkOnModelDropped(object sender, ModelDropEventArgs modelDropEventArgs)
    {
        modelDropEventArgs.Handled = true;
        Beatmaps beatmaps = [];
        foreach (object sourceModel in modelDropEventArgs.SourceModels)
        {
            beatmaps.Add((BeatmapExtension)sourceModel);
        }

        BeatmapsDropped?.Invoke(this, beatmaps);
    }

    public void SetFilter(ICommonModelFilter filter) => ListViewBeatmaps.AdditionalFilter = new CommonModelFilter(filter);

    public void FilteringStarted()
    {
        if (ListViewBeatmaps.InvokeRequired)
        {
            _ = ListViewBeatmaps.BeginInvoke(FilteringStarted);

            return;
        }

        ListViewBeatmaps.BeginUpdate();
    }

    public void FilteringFinished()
    {
        if (ListViewBeatmaps.InvokeRequired)
        {
            _ = ListViewBeatmaps.BeginInvoke(FilteringFinished);

            return;
        }

        Beatmap selectedBeatmap = SelectedBeatmap;
        ListViewBeatmaps.UpdateColumnFiltering();
        ListViewBeatmaps.EndUpdate();
        UpdateResultsCount();

        SelectedBeatmap = selectedBeatmap;
    }

    public void ClearSelection() => ListViewBeatmaps.SelectedIndex = -1;

    public void SelectNextOrFirst()
    {
        ListViewBeatmaps.SelectNextOrFirst();
        ListViewBeatmaps.EnsureSelectionIsVisible();
    }

    protected virtual void OnSearchTextChanged() => SearchTextChanged?.Invoke(this, EventArgs.Empty);

    protected virtual void OnSelectedBeatmapsChanged() => SelectedBeatmapsChanged?.Invoke(this, EventArgs.Empty);

    protected virtual void OnSelectedBeatmapChanged() => SelectedBeatmapChanged?.Invoke(this, EventArgs.Empty);

    private void MenuStripClick(object sender, EventArgs e)
    {
        if (!_menuStripClickActions.TryGetValue(sender, out BeatmapListingAction action))
        {
            throw new InvalidOperationException("Menu item has no associated action.");
        }

        BeatmapOperation?.Invoke(this, action);
    }

    private void ListViewBeatmaps_KeyUp(object sender, KeyEventArgs e)
    {
        if (ListViewBeatmaps.SelectedObjects.Count == 0)
        {
            return;
        }

        if (AllowForDeletion && e.KeyCode == Keys.Delete)
        {
            MenuStripClick(DeleteMapMenuStrip, e);
        }
    }

    private void button_searchHelp_Click(object sender, EventArgs e) => BeatmapSearchHelpClicked?.Invoke(this, EventArgs.Empty);

    private void comboBox_grouping_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBox_grouping.SelectedItem is not BeatmapGroupColumn selectedColumn)
        {
            return;
        }

        BeatmapGroupColumnChanged?.Invoke(this, selectedColumn.Text);

        bool hasGroupingColumn = selectedColumn.OlvColumn != null;
        button_toggleCollapse.Enabled = hasGroupingColumn;

        if (!hasGroupingColumn)
        {
            if (ListViewBeatmaps.ShowGroups)
            {
                ListViewBeatmaps.ShowGroups = false;
            }

            return;
        }

        IEnumerable currentObjects = ListViewBeatmaps.Objects;
        ListViewBeatmaps.SetObjects(Enumerable.Empty<Beatmap>());

        ListViewBeatmaps.AlwaysGroupByColumn = selectedColumn.OlvColumn;
        ListViewBeatmaps.Sorting = System.Windows.Forms.SortOrder.Ascending;
        ListViewBeatmaps.LastSortColumn = selectedColumn.OlvColumn;
        ListViewBeatmaps.ShowGroups = true;

        ListViewBeatmaps.SetObjects(currentObjects);
    }

    private void button_toggleCollapse_Click(object sender, EventArgs e) => SetGroupCollapse(!_groupingStrategy.Collapsed);

    public void SetGroupCollapse(bool collapseAllByDefault)
    {
        if (ListViewBeatmaps.InvokeRequired)
        {
            ListViewBeatmaps.Invoke(() => SetGroupCollapse(collapseAllByDefault));
            return;
        }

        BeatmapGroupCollapsedChanged?.Invoke(this, collapseAllByDefault);
        button_toggleCollapse.Text = collapseAllByDefault
            ? BeatmapListingView_Resources.DoubleArrowDown
            : BeatmapListingView_Resources.DoubleArrowUp;

        foreach (OLVGroup group in ListViewBeatmaps.OLVGroups)
        {
            group.Collapsed = collapseAllByDefault;
        }

        _groupingStrategy.Collapsed = collapseAllByDefault;
    }

    public new void Dispose() => ListViewBeatmaps.SetObjects(Enumerable.Empty<Beatmap>());
}
