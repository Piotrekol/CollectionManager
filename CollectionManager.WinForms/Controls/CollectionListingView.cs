namespace GuiComponents.Controls;

using BrightIdeasSoftware;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Types;
using CollectionManager.WinForms;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IOsuCollection = CollectionManager.Core.Types.IOsuCollection;

public partial class CollectionListingView : UserControl, ICollectionListingView
{
    public event GuiHelpers.LoadFileArgs OnLoadFile;
    public event GuiHelpers.CollectionReorderEventArgs OnCollectionReorder;
    public string SearchText => textBox_collectionNameSearch.Text;
    public OsuCollections Collections
    {
        set
        {
            ArrayList selectedCollections = SelectedCollections;
            ListViewCollections.SetObjects(value);
            SelectedCollections = selectedCollections;
        }
    }

    public IOsuCollection SelectedCollection
    {
        get => (IOsuCollection)ListViewCollections.SelectedObject;
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
    public OsuCollections HighlightedCollections
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
    public event EventHandler<string> RightClick;
    public event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;

    private readonly CollectionRenderer _collectionRenderer = new();
    private readonly RearrangingDropSink _dropsink = new();
    private OLVColumn NameColumn;

    public CollectionListingView()
    {
        InitializeComponent();
        init();
        Bind();
    }

    private void Bind()
    {
        textBox_collectionNameSearch.TextChanged += delegate
         {
             ListViewCollections.AdditionalFilter = TextMatchFilter.Contains(ListViewCollections, SearchText);
         };

        ListViewCollections.SelectionChanged += delegate
        {
            OnSelectedCollectionChanged();
            OnSelectedCollectionsChanged();
        };

        ListViewCollections.ColumnWidthChanged += OnColumnReordered;
    }

    private void OnColumnReordered(object sender, ColumnWidthChangedEventArgs e)
    {
        IEnumerable<string> visibleColumnAspectNames = ListViewCollections.AllColumns
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
        {
            DropFile(dataObject);
        }
    }

    private void DropFile(IDataObject dataObject)
    {
        if (dataObject.GetFormats().Any(f => f == "FileDrop"))
        {
            string[] files = (string[])dataObject.GetData(DataFormats.FileDrop);
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
            else if (e.SourceModels[0] is OsuCollection)
            {

                if (e.DropTargetLocation is not DropTargetLocation.AboveItem and not DropTargetLocation.BelowItem)
                {
                    e.Handled = true;
                    e.Effect = DragDropEffects.None;
                }
                else
                {
                    OsuCollection targetCollection = (OsuCollection)e.TargetModel;
                    foreach (object model in e.SourceModels)
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

    private void ListViewCollectionsOnCellRightClick(object sender, CellRightClickEventArgs cellRightClickEventArgs) => cellRightClickEventArgs.MenuStrip = CollectionContextMenuStrip;

    public void SetFilter(ICommonModelFilter filter) => ListViewCollections.AdditionalFilter = new CommonModelFilter(filter);

    public void SetVisibleColumns(string[] visibleColumnsAspectNames)
    {
        foreach (OLVColumn column in ListViewCollections.AllColumns)
        {
            column.IsVisible = visibleColumnsAspectNames.Contains(column.AspectName);
        }

        ListViewCollections.RebuildColumns();
    }

    public void FilteringStarted() => ListViewCollections.BeginUpdate();

    public void FilteringFinished()
    {
        ListViewCollections.UpdateColumnFiltering();
        ListViewCollections.EndUpdate();
    }

    protected virtual void OnSelectedCollectionChanged() => SelectedCollectionChanged?.Invoke(this, EventArgs.Empty);

    protected virtual void OnSelectedCollectionsChanged() => SelectedCollectionsChanged?.Invoke(this, EventArgs.Empty);

    private void ListViewCollections_ModelDropped(object sender, ModelDropEventArgs e)
    {
        e.Handled = true;
        OsuCollection targetCollection = (OsuCollection)e.TargetModel;
        if (e.SourceModels[0] is OsuCollection)
        {

            OsuCollections collections = [];
            foreach (object collection in e.SourceModels)
            {
                collections.Add((OsuCollection)collection);
            }

            CollectionManager.Core.Enums.SortOrder sortOrder = ListViewCollections.LastSortOrder == SortOrder.Ascending
                ? CollectionManager.Core.Enums.SortOrder.Ascending
                : CollectionManager.Core.Enums.SortOrder.Descending;
            OnCollectionReorder?.Invoke(this, collections, targetCollection, e.DropTargetLocation == DropTargetLocation.AboveItem, ListViewCollections.LastSortColumn.AspectName, sortOrder);

            if (ListViewCollections.LastSortColumn != NameColumn || ListViewCollections.LastSortOrder != SortOrder.Ascending)
            {
                ListViewCollections.Sort(ListViewCollections.AllColumns[0], SortOrder.Ascending);
            }

            return;
        }

        if (targetCollection == null)
        {
            return;
        }

        Beatmaps beatmaps = [];
        foreach (object b in e.SourceModels)
        {
            beatmaps.Add((BeatmapExtension)b);
        }

        BeatmapsDropped?.Invoke(this, beatmaps, targetCollection.Name);
    }

    protected virtual void OnRightClick(string e) => RightClick?.Invoke(this, e);

    private void MenuStripClick(object sender, EventArgs e)
    {
        ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
        if ((string)menuItem.Tag == "Paste")
        {
            PasteCollectionFromClipboard();
            return;
        }

        OnRightClick((string)menuItem.Tag);
    }

    private void PasteCollectionFromClipboard()
    {
        IDataObject data = Clipboard.GetDataObject();
        if (data == null)
        {
            return;
        }

        DropFile(data);
        return;
    }

    private void ListViewCollections_KeyUp(object sender, KeyEventArgs e)
    {
        string eventData = string.Empty;
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
        {
            OnRightClick(eventData);
        }

        switch (e.KeyCode)
        {
            case Keys.V:
                if (e.Control)
                {
                    PasteCollectionFromClipboard();
                }

                break;

            case Keys.C:
                if (e.Control)
                {
                    OnRightClick("Copy");
                }

                break;
        }

        e.Handled = e.SuppressKeyPress = true;
    }

    private class CollectionRenderer : BaseRenderer
    {
        public OsuCollections Collections { get; set; }
        private readonly Brush brush = new SolidBrush(Color.FromArgb(120, 0x7f, 0xff, 0xd4));

        protected override void DrawBackground(Graphics g, Rectangle r)
        {
            base.DrawBackground(g, r);
            if (Column.Index == 0 && Collections != null && Collections.Contains(ListItem.RowObject))
            {
                g.FillRectangle(brush, new Rectangle(r.X + 2, r.Y + 2, r.Width - 4, r.Height - 4));
            }
        }
    }

    private void ListViewCollections_KeyDown(object sender, KeyEventArgs e) => e.Handled = e.SuppressKeyPress = true;

}
