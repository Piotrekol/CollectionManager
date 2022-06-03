using Gui.Misc;

namespace GuiComponents.Interfaces
{
    public interface IStartupView
    {
        event GuiHelpers.StartupCollectionEventArgs StartupCollectionOperation;
        event GuiHelpers.StartupDatabaseEventArgs StartupDatabaseOperation;

        string LoadDatabaseStatusText { get; set; }
        bool UseSelectedOptionsOnStartup { get; set; }
        bool ButtonsEnabled { get; set; }
        bool LoadDefaultCollectionButtonEnabled { get; set; }
    }
}