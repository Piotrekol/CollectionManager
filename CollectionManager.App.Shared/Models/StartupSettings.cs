namespace CollectionManager.App.Shared.Models;

using CollectionManager.Common;

public class StartupSettings
{
    public StartupDatabaseAction StartupDatabaseAction { get; set; }
    public StartupCollectionAction StartupCollectionAction { get; set; }
    public bool AutoLoadMode { get; set; }
    public string OsuLocation { get; set; }
}