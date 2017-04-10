namespace App.Interfaces
{
    public interface IUpdateModel
    {
        bool IsUpdateAvaliable();
        bool Error { get; }
        string newVersion { get; }
        string newVersionLink { get; }
        string currentVersion { get; }
        void CheckIfUpdateIsAvaliable();
    }
}