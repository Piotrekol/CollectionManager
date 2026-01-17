namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Core.Modules.FileIo;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;

public sealed class SaveDefaultCollectionHandler : IMainSidePanelActionHandler
{
    private readonly OsuFileIo _osuFileIo;
    private readonly IUserDialogs _userDialogs;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.SaveDefaultCollection;

    public SaveDefaultCollectionHandler(OsuFileIo osuFileIo, IUserDialogs userDialogs)
    {
        _osuFileIo = osuFileIo;
        _userDialogs = userDialogs;
    }

    public async Task HandleAsync(object sender, object data)
    {
        bool isLegacyCollectionFile = true;
        string fileLocation = Path.Combine(Initalizer.OsuDirectory, "collection.db");

        if (!File.Exists(fileLocation))
        {
            fileLocation = Path.Combine(Initalizer.OsuDirectory, "client.realm");
            isLegacyCollectionFile = false;

            if (!File.Exists(fileLocation))
            {
                await _userDialogs.OkMessageBoxAsync("Could not find collection file to overwritte!", caption("Error"), MessageBoxType.Error);
                return;
            }
        }

        try
        {
            if (SidePanelActionHelpers.OsuIsRunning(isLegacyCollectionFile))
            {
                await _userDialogs.OkMessageBoxAsync("Close your osu! before saving collections!", caption("Error"), MessageBoxType.Error);
                return;
            }
        }
        catch (Win32Exception ex)
        {
            // access denied
            if (ex.NativeErrorCode != 5)
            {
                throw;
            }

            await _userDialogs.OkMessageBoxAsync("Could not determine if osu! is running due to a permissions error.", caption("Warning"), MessageBoxType.Warning);
        }

        if (await _userDialogs.YesNoMessageBoxAsync($"Are you sure that you want to overwrite your existing osu! collection at \"{fileLocation}\"?",
            caption(), MessageBoxType.Question))
        {
            await SidePanelActionHelpers.BeforeCollectionSave(Initalizer.LoadedCollections);
            string backupFolder = Path.Combine(Initalizer.OsuDirectory, "collectionBackups");

            if (!TryBackupOsuCollection(backupFolder))
            {
                await _userDialogs.OkMessageBoxAsync("Could not create collection backup. Save aborted.", caption("Error"), MessageBoxType.Error);
                return;
            }

            _osuFileIo.CollectionLoader.SaveCollection(Initalizer.LoadedCollections, fileLocation);
            await _userDialogs.OkMessageBoxAsync($"Collections saved.{Environment.NewLine}Previous collection backup was saved in \"{backupFolder}\" and will be kept for 30 days.", caption("Success"), MessageBoxType.Success);
        }
        else
        {
            await _userDialogs.OkMessageBoxAsync("Save Aborted", caption("Info"), MessageBoxType.Info);
        }

        static string caption(string subAction = default) => subAction is null
            ? "Default collection save"
            : $"Default collection save - {subAction}";
    }

    private static bool TryBackupOsuCollection(string backupFolder)
    {
        if (!Directory.Exists(backupFolder))
        {
            _ = Directory.CreateDirectory(backupFolder);
        }

        string sourceCollectionFile = Path.Combine(Initalizer.OsuDirectory, "collection.db");
        string destinationCollectionFile;
        if (File.Exists(sourceCollectionFile))
        {
            destinationCollectionFile = Path.Combine(backupFolder, $"collection_{CalculateMD5(sourceCollectionFile)}.db");
        }
        else
        {
            sourceCollectionFile = Path.Combine(Initalizer.OsuDirectory, "client.realm");

            if (!File.Exists(sourceCollectionFile))
            {
                return false;
            }

            destinationCollectionFile = Path.Combine(backupFolder, $"client_{CalculateMD5(sourceCollectionFile)}.realm");
        }

        if (File.Exists(destinationCollectionFile))
        {
            // Update file save date to indicate latest collection version
            File.SetLastWriteTime(destinationCollectionFile, DateTime.Now);
            return true;
        }

        CleanupBackups("*.db");
        CleanupBackups("*.realm");
        File.Copy(sourceCollectionFile, destinationCollectionFile);

        return true;

        [SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms", Justification = "Not relevant for file names.")]
        static string CalculateMD5(string filename)
        {
            using MD5 md5 = MD5.Create();
            using FileStream stream = File.OpenRead(filename);
            byte[] hash = md5.ComputeHash(stream);
            return Convert.ToHexStringLower(hash);
        }

        void CleanupBackups(string searchPattern)
        {
            DateTime deleteDateThreshold = DateTime.UtcNow.AddDays(-30);
            string[] collectionFilePaths = Directory.GetFiles(backupFolder, searchPattern, SearchOption.TopDirectoryOnly);
            IEnumerable<FileInfo> collectionFiles = collectionFilePaths.Select(f => new FileInfo(f))
                .Where(f => f.LastWriteTimeUtc < deleteDateThreshold);

            foreach (FileInfo collectionFile in collectionFiles)
            {
                collectionFile.Delete();
            }
        }
    }
}
