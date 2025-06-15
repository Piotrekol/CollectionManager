namespace GuiComponents.Controls;

using BrightIdeasSoftware;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Enums;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.Modules;
using CollectionManager.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

public partial class BeatmapListingView : UserControl, IBeatmapListingView
{
    public event EventHandler SearchTextChanged;
    public event EventHandler SelectedBeatmapChanged;
    public event EventHandler SelectedBeatmapsChanged;
    public event EventHandler BeatmapSearchHelpClicked;

    public event GuiHelpers.BeatmapListingActionArgs BeatmapOperation;
    public event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;
    public event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;

    public string SearchText => textBox_beatmapSearch.Text;
    public string ResultText { get; set; }

    private bool _allowForDeletion;

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

    public Beatmap SelectedBeatmap
    {
        get => (Beatmap)ListViewBeatmaps.SelectedObject;
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
    private Mods _currentMods = Mods.Nm;
    private PlayMode _currentPlayMode = PlayMode.Osu;
    private DifficultyCalculator _difficultyCalculator = new();
    private void InitListView()
    {
        //listview
        ListViewBeatmaps.FullRowSelect = true;
        ListViewBeatmaps.AllowColumnReorder = true;
        ListViewBeatmaps.Sorting = System.Windows.Forms.SortOrder.Descending;
        ListViewBeatmaps.UseHotItem = true;
        ListViewBeatmaps.UseTranslucentHotItem = true;
        ListViewBeatmaps.UseFiltering = true;
        ListViewBeatmaps.UseNotifyPropertyChanged = true;
        ListViewBeatmaps.ShowItemCountOnGroups = true;
        ListViewBeatmaps.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
        string format = "{0:0.##}";
        column_ar.AspectToStringFormat = format;
        column_cs.AspectToStringFormat = format;
        column_od.AspectToStringFormat = format;
        column_hp.AspectToStringFormat = format;
        column_stars.AspectToStringFormat = format;

        LastPlayed.AspectToStringConverter = DataListViewFormatter.FormatDateTimeOffset;
        EditDate.AspectToStringConverter = DataListViewFormatter.FormatDateTimeOffset;
        LastScoreDate.AspectToStringConverter = DataListViewFormatter.FormatDateTimeOffset;

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

    public void FilteringStarted() => ListViewBeatmaps.BeginUpdate();

    public void FilteringFinished()
    {
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
        if (sender == DeleteMapMenuStrip)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.DeleteBeatmapsFromCollection);
        }
        else if (sender == DownloadMapInBrowserMenuStrip)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.DownloadBeatmaps);
        }
        else if (sender == DownloadMapManagedMenuStrip)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.DownloadBeatmapsManaged);
        }
        else if (sender == OpenBeatmapPageMapMenuStrip)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.OpenBeatmapPages);
        }
        else if (sender == copyAsTextMenuStrip)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.CopyBeatmapsAsText);
        }
        else if (sender == copyUrlMenuStrip)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.CopyBeatmapsAsUrls);
        }
        else if (sender == OpenBeatmapFolderMenuStrip)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.OpenBeatmapFolder);
        }
        else if (sender == PullMapsetMenuStrip)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.PullWholeMapSet);
        }
        else if (sender == exportBeatmapSetsMenuItem)
        {
            BeatmapOperation?.Invoke(this, BeatmapListingAction.ExportBeatmapSets);
        }
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
}
