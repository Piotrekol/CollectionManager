namespace CollectionManager.App.Shared.Interfaces;

public interface IUpdateModel
{
    bool UpdateIsAvailable { get; }
    bool Error { get; }
    Version OnlineVersion { get; }
    string NewVersionLink { get; }
    Version CurrentVersion { get; }
    bool CheckForUpdates();
}