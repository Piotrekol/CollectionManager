namespace CollectionManager.Common.Interfaces.Controls;
public interface IStartupView
{
    event GuiHelpers.StartupCollectionEventArgs StartupCollectionOperation;
    event GuiHelpers.StartupDatabaseEventArgs StartupDatabaseOperation;

    IInfoTextView InfoTextView { get; }

    string LoadDatabaseStatusText { get; set; }
    string CollectionStatusText { get; set; }
    bool UseSelectedOptionsOnStartup { get; set; }
    bool UseSelectedOptionsOnStartupEnabled { get; set; }
    bool CollectionButtonsEnabled { get; set; }
    bool DatabaseButtonsEnabled { get; set; }
    bool LoadOsuCollectionButtonEnabled { get; set; }
    bool LoadLazerDatabaseButtonEnabled { get; set; }
}