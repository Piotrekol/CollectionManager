using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManagerExtensionsDll.Modules;
using Common;
using GuiComponents.Interfaces;
using Gui.Misc;

namespace GuiComponents.Controls
{
    public partial class BeatmapListingView : UserControl, IBeatmapListingView
    {
        public event EventHandler SearchTextChanged;
        public event EventHandler SelectedBeatmapChanged;
        public event EventHandler SelectedBeatmapsChanged;

        public event GuiHelpers.BeatmapListingActionArgs BeatmapOperation;
        public event GuiHelpers.BeatmapsEventArgs BeatmapsDropped;

        public string SearchText => textBox_beatmapSearch.Text;
        public string ResultText { get; set; }

        private bool _allowForDeletion = false;

        [Description("Should user be able to delete beatmaps from the list?"), Category("Layout")]
        public bool AllowForDeletion
        {
            get { return _allowForDeletion; }
            set
            {
                _allowForDeletion = value;
                DeleteMapMenuStrip.Enabled = value;
            }
        }

        public void SetCurrentPlayMode(PlayMode playMode)
        {
            _currentPlayMode = playMode;
        }
        public void SetCurrentMods(Mods mods)
        {
            _currentMods = mods;
        }

        public void SetBeatmaps(IEnumerable beatmaps)
        {
            ListViewBeatmaps.SetObjects(beatmaps);
            UpdateResultsCount();
        }
        public Beatmap SelectedBeatmap => (Beatmap)ListViewBeatmaps.SelectedObject;

        public Beatmaps SelectedBeatmaps
        {
            get
            {
                var beatmaps = new Beatmaps();
                if (ListViewBeatmaps.SelectedObjects.Count > 0)
                    foreach (var o in ListViewBeatmaps.SelectedObjects)
                    {
                        beatmaps.Add((BeatmapExtension)o);
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
            ListViewBeatmaps.SelectionChanged += delegate
            {
                OnSelectedBeatmapChanged();
                OnSelectedBeatmapsChanged();
            };
            ListViewBeatmaps.CellRightClick += delegate (object s, CellRightClickEventArgs args)
            {
                args.MenuStrip = BeatmapsContextMenuStrip;
            };
        }
        private void UpdateResultsCount()
        {
            int count = 0;
            foreach (var b in ListViewBeatmaps.FilteredObjects)
            {
                count++;
            }
            label_resultsCount.Text = string.Format("{0} {1}", count, count == 1 ? "map" : "maps");
        }
        public static DateTime d = new DateTime(2006, 1, 1);
        private Mods _currentMods = Mods.Omod;
        private PlayMode _currentPlayMode = PlayMode.Osu;
        private DifficultyCalculator _difficultyCalculator = new DifficultyCalculator();
        private void InitListView()
        {
            //listview
            ListViewBeatmaps.FullRowSelect = true;
            ListViewBeatmaps.AllowColumnReorder = true;
            ListViewBeatmaps.Sorting = SortOrder.Descending;
            ListViewBeatmaps.UseHotItem = true;
            ListViewBeatmaps.UseTranslucentHotItem = true;
            ListViewBeatmaps.UseFiltering = true;
            ListViewBeatmaps.UseNotifyPropertyChanged = true;
            ListViewBeatmaps.ShowItemCountOnGroups = true;
            ListViewBeatmaps.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
            var format = "{0:0.##}";
            column_ar.AspectToStringFormat = format;
            column_cs.AspectToStringFormat = format;
            column_od.AspectToStringFormat = format;
            column_hp.AspectToStringFormat = format;

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
            LastPlayed.AspectToStringConverter = delegate (object cellValue)
            {
                if (cellValue == null) return "Never";
                var val = (DateTime)cellValue;
                return val > d ? $"{val}" : "Never";
            };
            MainBpm.AspectToStringConverter = delegate (object cellValue)
            {
                if (cellValue == null) return string.Empty;
                return $"{cellValue:0.##}";
            };

            var dropsink = new RearrangingDropSink();
            dropsink.CanDropBetween = false;
            dropsink.CanDropOnItem = false;
            dropsink.CanDropOnSubItem = false;
            dropsink.CanDropOnBackground = true;
            dropsink.ModelDropped += DropsinkOnModelDropped;
            this.ListViewBeatmaps.DropSink = dropsink;

        }


        private void DropsinkOnModelDropped(object sender, ModelDropEventArgs modelDropEventArgs)
        {
            modelDropEventArgs.Handled = true;
            var beatmaps = new Beatmaps();
            foreach (var sourceModel in modelDropEventArgs.SourceModels)
            {
                beatmaps.Add((BeatmapExtension)sourceModel);
            }
            BeatmapsDropped?.Invoke(this, beatmaps);
        }

        public void SetFilter(IModelFilter filter)
        {
            ListViewBeatmaps.AdditionalFilter = filter;
        }

        public void FilteringStarted()
        {
            ListViewBeatmaps.BeginUpdate();
        }

        public void FilteringFinished()
        {
            ListViewBeatmaps.UpdateColumnFiltering();
            ListViewBeatmaps.EndUpdate();
            UpdateResultsCount();
        }

        public void ClearSelection()
        {
            ListViewBeatmaps.SelectedIndex = -1;
        }

        public void SelectNextOrFirst()
        {
            ListViewBeatmaps.SelectNextOrFirst();
            ListViewBeatmaps.EnsureSelectionIsVisible();
        }

        protected virtual void OnSearchTextChanged()
        {
            SearchTextChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSelectedBeatmapsChanged()
        {
            SelectedBeatmapsChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSelectedBeatmapChanged()
        {
            SelectedBeatmapChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MenuStripClick(object sender, EventArgs e)
        {
            if (sender == DeleteMapMenuStrip)
                BeatmapOperation?.Invoke(this, Common.BeatmapListingAction.DeleteBeatmapsFromCollection);
            else if (sender == DownloadMapInBrowserMenuStrip)
                BeatmapOperation?.Invoke(this, Common.BeatmapListingAction.DownloadBeatmaps);
            else if (sender == DownloadMapManagedMenuStrip)
                BeatmapOperation?.Invoke(this, Common.BeatmapListingAction.DownloadBeatmapsManaged);
            else if (sender == OpenBeatmapPageMapMenuStrip)
                BeatmapOperation?.Invoke(this, Common.BeatmapListingAction.OpenBeatmapPages);
            else if (sender == copyAsTextMenuStrip)
                BeatmapOperation?.Invoke(this, Common.BeatmapListingAction.CopyBeatmapsAsText);
            else if (sender == copyUrlMenuStrip)
                BeatmapOperation?.Invoke(this, Common.BeatmapListingAction.CopyBeatmapsAsUrls);
            else if (sender == OpenBeatmapFolderMenuStrip)
                BeatmapOperation?.Invoke(this, Common.BeatmapListingAction.OpenBeatmapFolder);
            else if (sender == PullMapsetMenuStrip)
                BeatmapOperation?.Invoke(this, Common.BeatmapListingAction.PullWholeMapSet);
        }
    }


}
