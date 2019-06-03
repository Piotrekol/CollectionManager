using System;
using System.Collections;
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
        public string SearchText => textBox_collectionNameSearch.Text;
        public Collections Collections { set { ListViewCollections.SetObjects(value); } }

        public ICollection SelectedCollection
        {
            get { return (ICollection) ListViewCollections.SelectedObject; }
            set
            {
                ListViewCollections.SelectedObject = null;
                ListViewCollections.SelectedObject = value;
            }
        }
        public ArrayList SelectedCollections => (ArrayList)ListViewCollections.SelectedObjects;

        public event EventHandler SearchTextChanged;
        public event EventHandler SelectedCollectionChanged;
        public event EventHandler SelectedCollectionsChanged;
        public event GuiHelpers.CollectionBeatmapsEventArgs BeatmapsDropped;
        public event EventHandler<StringEventArgs> RightClick;

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

        }

        private void init()
        {
            //ListViewCollections.SelectedIndexChanged += ListViewCollectionsSelectedIndexChanged;
            ListViewCollections.UseFiltering = true;
            ListViewCollections.FullRowSelect = true;
            ListViewCollections.HideSelection = false;

            var dropsink = new RearrangingDropSink();
            dropsink.CanDropBetween = false;
            dropsink.CanDropOnItem = true;
            dropsink.CanDropOnSubItem = false;
            dropsink.CanDropOnBackground = false;
            ListViewCollections.DropSink = dropsink;
            ListViewCollections.ModelDropped += ListViewCollections_ModelDropped;

            ListViewCollections.CellRightClick += ListViewCollectionsOnCellRightClick;
            dropsink.ModelCanDrop+=DropsinkOnModelCanDrop;
            dropsink.CanDrop+=DropsinkOnCanDrop;
            dropsink.Dropped+=DropsinkOnDropped;
        }

        private void DropsinkOnDropped(object sender, OlvDropEventArgs e)
        {
            if (e.DataObject is DataObject dataObject && dataObject.GetFormats().Any(f => f == "FileDrop"))
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
            var collection = (Collection)e.TargetModel;
            if (collection == null) return;
            var beatmaps = new Beatmaps();
            foreach (var b in e.SourceModels)
            {
                beatmaps.Add((BeatmapExtension)b);
            }

            BeatmapsDropped?.Invoke(this, beatmaps, collection.Name);
        }

        protected virtual void OnRightClick(StringEventArgs e)
        {
            RightClick?.Invoke(this, e);
        }

        private void MenuStripClick(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            OnRightClick(new StringEventArgs((string)menuItem.Tag));
        }

    }
}
