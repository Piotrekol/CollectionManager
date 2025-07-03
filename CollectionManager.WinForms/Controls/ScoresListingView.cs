namespace CollectionManager.WinForms.Controls;

using BrightIdeasSoftware;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Modules.Mod;
using CollectionManager.Core.Types;
using System;
using System.Windows.Forms;

public partial class ScoresListingView : UserControl, IScoresListingView
{
    private Scores _scores;

    public event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;
    public ModParser ModParser { get; set; }
    public Scores Scores
    {
        get => _scores;
        set
        {
            ListViewScores.SetObjects(value);
            _scores = value;
        }
    }

    public ScoresListingView()
    {
        InitializeComponent();
        InitListView();
        ListViewScores.ColumnWidthChanged += OnColumnReordered;
    }

    private void InitListView()
    {
        ListViewScores.FullRowSelect = true;
        ListViewScores.AllowColumnReorder = true;
        ListViewScores.Sorting = SortOrder.Descending;
        ListViewScores.UseHotItem = true;
        ListViewScores.UseTranslucentHotItem = true;
        ListViewScores.UseFiltering = true;
        ListViewScores.UseNotifyPropertyChanged = true;
        ListViewScores.ShowItemCountOnGroups = true;
        ListViewScores.CellEditActivation = ObjectListView.CellEditActivateMode.DoubleClick;
        ListViewScores.Sort(0);

        Date.AspectToStringConverter = DataListViewFormatter.FormatDateTimeOffset;
        Mods.AspectToStringConverter = cellValue => DataListViewFormatter.FormatMods(cellValue, ModParser);

        Acc.AspectToStringFormat = "{0:0.00}%";
    }

    private void OnColumnReordered(object sender, ColumnWidthChangedEventArgs e)
    {
        IEnumerable<string> visibleColumnAspectNames = ListViewScores.AllColumns
            .Where(column => column.IsVisible)
            .Select(column => column.AspectName);

        ColumnsToggled?.Invoke(this, visibleColumnAspectNames.ToArray());
    }

    public void SetVisibleColumns(string[] visibleColumns)
    {
        foreach (OLVColumn column in ListViewScores.AllColumns)
        {
            column.IsVisible = visibleColumns.Contains(column.AspectName);
        }

        ListViewScores.RebuildColumns();
    }
}
