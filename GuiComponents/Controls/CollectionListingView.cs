using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CollectionManager.DataTypes;
using Gui.Misc;
using GuiComponents.Interfaces;
using ICollection = CollectionManager.DataTypes.ICollection;

namespace GuiComponents.Controls
{
    public partial class CollectionListingView : UserControl, ICollectionListingView
    {
        public event GuiHelpers.LoadFileArgs OnLoadFile;
        public event GuiHelpers.CollectionReorderEventArgs OnCollectionReorder;
        public string SearchText => textBox_collectionNameSearch.Text;
        public Collections Collections
        {
            set
            {
                var selectedCollections = SelectedCollections;
                ListViewCollections.SetObjects(value);
                SelectedCollections = selectedCollections;
            }
        }

        public ICollection SelectedCollection
        {
            get { return (ICollection)ListViewCollections.SelectedObject; }
            set
            {
                ListViewCollections.SelectedObject = null;
                ListViewCollections.SelectedObject = value;
            }
        }
        public ArrayList SelectedCollections
        {
            get => (ArrayList)ListViewCollections.SelectedObjects;
            private set
            {
                ListViewCollections.SelectedObjects = value;
                ListViewCollections.EnsureSelectionIsVisible();
            }
        }
        public Collections HighlightedCollections
        {
            get => _collectionRenderer.Collections;
            set
            {
                _collectionRenderer.Collections = value;
                ListViewCollections.Refresh();
            }
        }

        public event EventHandler SearchTextChanged;
        public event EventHandler SelectedCollectionChanged;
        public event EventHandler SelectedCollectionsChanged;
        public event GuiHelpers.CollectionBeatmapsEventArgs BeatmapsDropped;
        public event EventHandler<StringEventArgs> RightClick;
        public event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;

        private CollectionRenderer _collectionRenderer = new CollectionRenderer();
        private RearrangingDropSink _dropsink = new RearrangingDropSink();
        private OLVColumn NameColumn;

        public CollectionListingView()
        {
            InitializeComponent();
            init();
            Bind();
        }

        private void Bind()
        {
            this.textBox_collectionNameSearch.TextChanged += delegate
             {
                 ListViewCollections.AdditionalFilter = TextMatchFilter.Contains(ListViewCollections, SearchText);
             };

            this.ListViewCollections.SelectionChanged += delegate
            {
                OnSelectedCollectionChanged();
                OnSelectedCollectionsChanged();
            };

            ListViewCollections.ColumnWidthChanged += OnColumnReordered;
        }

        private void OnColumnReordered(object sender, ColumnWidthChangedEventArgs e)
        {
            var visibleColumnAspectNames = ListViewCollections.AllColumns
                .Where(column => column.IsVisible)
                .Select(column => column.AspectName);

            ColumnsToggled?.Invoke(this, visibleColumnAspectNames.ToArray());
        }

        private void init()
        {
            //ListViewCollections.SelectedIndexChanged += ListViewCollectionsSelectedIndexChanged;
            ListViewCollections.UseFiltering = true;
            ListViewCollections.FullRowSelect = true;
            ListViewCollections.HideSelection = false;
            ListViewCollections.DefaultRenderer = _collectionRenderer;
            ListViewCollections.IsSimpleDragSource = true;
            ListViewCollections.PrimarySortColumn = NameColumn = ListViewCollections.AllColumns[0];
            ListViewCollections.PrimarySortOrder = SortOrder.Ascending;

            _dropsink.CanDropBetween = true;
            _dropsink.CanDropOnItem = true;
            _dropsink.CanDropOnSubItem = false;
            _dropsink.CanDropOnBackground = false;
            ListViewCollections.DropSink = _dropsink;
            ListViewCollections.ModelDropped += ListViewCollections_ModelDropped;
            ListViewCollections.CellRightClick += ListViewCollectionsOnCellRightClick;
            _dropsink.ModelCanDrop += DropsinkOnModelCanDrop;
            _dropsink.CanDrop += DropsinkOnCanDrop;
            _dropsink.Dropped += DropsinkOnDropped;
        }

        private void DropsinkOnDropped(object sender, OlvDropEventArgs e)
        {
            if (e.DataObject is DataObject dataObject)
                DropFile(dataObject);
        }

        private void DropFile(IDataObject dataObject)
        {
            if (dataObject.GetFormats().Any(f => f == "FileDrop"))
            {
                var files = (string[])dataObject.GetData(DataFormats.FileDrop);
                OnLoadFile?.Invoke(this, files);
            }
        }

        private void DropsinkOnCanDrop(object sender, OlvDropEventArgs e)
        {
            if (e.DataObject is DataObject dataObject && dataObject.GetFormats().Any(f => f == "FileDrop"))
            {
                e.Effect = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        private void DropsinkOnModelCanDrop(object sender, ModelDropEventArgs e)
        {
            if (e.TargetModel != null)
            {
                if (e.TargetModel is WebCollection wb && !wb.Loaded)
                {
                    e.Handled = true;
                    e.Effect = DragDropEffects.None;
                }
                else if (e.SourceModels[0] is Collection)
                {

                    if (e.DropTargetLocation != DropTargetLocation.AboveItem && e.DropTargetLocation != DropTargetLocation.BelowItem)
                    {
                        e.Handled = true;
                        e.Effect = DragDropEffects.None;
                    }
                    else
                    {
                        var targetCollection = (Collection)e.TargetModel;
                        foreach (var model in e.SourceModels)
                        {
                            if (model == targetCollection)
                            {
                                e.Handled = true;
                                e.Effect = DragDropEffects.None;
                            }
                        }
                    }
                }
            }
        }

        private void ListViewCollectionsOnCellRightClick(object sender, CellRightClickEventArgs cellRightClickEventArgs)
        {
            cellRightClickEventArgs.MenuStrip = CollectionContextMenuStrip;
        }

        public void SetFilter(IModelFilter filter)
        {
            ListViewCollections.AdditionalFilter = filter;
        }

        public void SetVisibleColumns(string[] visibleColumnsAspectNames)
        {
            foreach (var column in ListViewCollections.AllColumns)
            {
                column.IsVisible = visibleColumnsAspectNames.Contains(column.AspectName);
            }

            ListViewCollections.RebuildColumns();
        }

        public void FilteringStarted()
        {
            ListViewCollections.BeginUpdate();
        }

        public void FilteringFinished()
        {
            ListViewCollections.UpdateColumnFiltering();
            ListViewCollections.EndUpdate();
        }

        protected virtual void OnSelectedCollectionChanged()
        {
            SelectedCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSelectedCollectionsChanged()
        {
            SelectedCollectionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ListViewCollections_ModelDropped(object sender, ModelDropEventArgs e)
        {
            e.Handled = true;
            var targetCollection = (Collection)e.TargetModel;
            if (e.SourceModels[0] is Collection)
            {

                var collections = new Collections();
                foreach (var collection in e.SourceModels)
                    collections.Add((Collection)collection);

                var sortOrder = ListViewCollections.LastSortOrder == SortOrder.Ascending
                    ? CollectionManager.Enums.SortOrder.Ascending
                    : CollectionManager.Enums.SortOrder.Descending;
                OnCollectionReorder?.Invoke(this, collections, targetCollection, e.DropTargetLocation == DropTargetLocation.AboveItem, ListViewCollections.LastSortColumn.AspectName, sortOrder);

                if (ListViewCollections.LastSortColumn != NameColumn || ListViewCollections.LastSortOrder != SortOrder.Ascending)
                    ListViewCollections.Sort(ListViewCollections.AllColumns[0], SortOrder.Ascending);

                return;
            }

            if (targetCollection == null)
                return;
            var beatmaps = new Beatmaps();
            foreach (var b in e.SourceModels)
            {
                beatmaps.Add((BeatmapExtension)b);
            }

            BeatmapsDropped?.Invoke(this, beatmaps, targetCollection.Name);
        }

        protected virtual void OnRightClick(StringEventArgs e)
        {
            RightClick?.Invoke(this, e);
        }

        private void MenuStripClick(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            if ((string)menuItem.Tag == "Paste")
            {
                PasteCollectionFromClipboard();
                return;
            }

            OnRightClick(new StringEventArgs((string)menuItem.Tag));
        }

        private void PasteCollectionFromClipboard()
        {
            var data = Clipboard.GetDataObject();
            if (data == null)
                return;
            DropFile(data);
            return;
        }

        private void ListViewCollections_KeyUp(object sender, KeyEventArgs e)
        {
            var eventData = string.Empty;
            switch (e.KeyCode)
            {
                case Keys.F2:
                    eventData = "Rename";
                    break;
                case Keys.Delete:
                    eventData = "Delete";
                    break;
            }

            if (ListViewCollections.SelectedObjects.Count > 0 && !string.IsNullOrEmpty(eventData))
                OnRightClick(new StringEventArgs(eventData));

            switch (e.KeyCode)
            {
                case Keys.V:
                    if (e.Control)
                        PasteCollectionFromClipboard();
                    break;

                case Keys.C:
                    if (e.Control)
                        OnRightClick(new StringEventArgs("Copy"));
                    break;
            }

            e.Handled = e.SuppressKeyPress = true;
        }

        private class CollectionRenderer : BaseRenderer
        {
            public Collections Collections { get; set; }
            private Brush brush = new SolidBrush(Color.FromArgb(120, 0x7f, 0xff, 0xd4));

            protected override void DrawBackground(Graphics g, Rectangle r)
            {
                base.DrawBackground(g, r);
                if (Column.Index == 0 && Collections != null && Collections.Contains(ListItem.RowObject))
                    g.FillRectangle(brush, new Rectangle(r.X + 2, r.Y + 2, r.Width - 4, r.Height - 4));
            }
        }

        private void ListViewCollections_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.SuppressKeyPress = true;
        }

    }
}
