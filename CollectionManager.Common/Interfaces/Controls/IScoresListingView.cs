namespace CollectionManager.Common.Interfaces.Controls;

using CollectionManager.Core.Modules.Mod;
using CollectionManager.Core.Types;

public interface IScoresListingView
{
    public Scores Scores { get; set; }
    public ModParser ModParser { get; set; }

    event GuiHelpers.ColumnsToggledEventArgs ColumnsToggled;

    void SetVisibleColumns(string[] visibleColumns);
}
