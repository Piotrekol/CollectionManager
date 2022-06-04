using Gui.Misc;

namespace GuiComponents.Interfaces
{
    public interface IStartupView
    {
        event GuiHelpers.StartupCollectionEventArgs StartupCollectionOperation;
        event GuiHelpers.StartupDatabaseEventArgs StartupDatabaseOperation;

        string LoadDatabaseStatusText { get; set; }
        string CollectionStatusText { get; set; }
        bool UseSelectedOptionsOnStartup { get; set; }
        bool UseSelectedOptionsOnStartupEnabled { get; set; }
        bool CollectionButtonsEnabled { get; set; }
        bool DatabaseButtonsEnabled { get; set; }
        bool LoadDefaultCollectionButtonEnabled { get; set; }
    }
}