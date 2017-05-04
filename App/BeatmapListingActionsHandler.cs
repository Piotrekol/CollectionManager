using System;
using System.Collections.Generic;
using System.Diagnostics;
using App.Interfaces;
using CollectionManager.DataTypes;
using CollectionManager.Modules.CollectionsManager;
using CollectionManager.Modules.FileIO;
using CollectionManagerExtensionsDll.Enums;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator;
using CollectionManagerExtensionsDll.Utils;
using Common;
using GuiComponents.Interfaces;

namespace App
{
    public class BeatmapListingActionsHandler
    {
        private readonly ICollectionEditor _collectionEditor;
        private readonly IUserDialogs _userDialogs;
        private readonly ILoginFormView _loginForm;
        private readonly OsuFileIo _osuFileIo;
        private readonly ListGenerator _listGenerator = new ListGenerator();
        public BeatmapListingActionsHandler(ICollectionEditor collectionEditor, IUserDialogs userDialogs, ILoginFormView loginForm, OsuFileIo osuFileIo)
        {
            _collectionEditor = collectionEditor;
            _userDialogs = userDialogs;
            _loginForm = loginForm;
            _osuFileIo = osuFileIo;
        }

        public void Bind(IBeatmapListingModel beatmapListingModel)
        {
            beatmapListingModel.DownloadBeatmapsManaged += DownloadBeatmapsManaged;
            beatmapListingModel.DownloadBeatmaps += DownloadBeatmaps;
            beatmapListingModel.DeleteBeatmapsFromCollection += DeleteBeatmapsFromCollection;
            beatmapListingModel.CopyBeatmapsAsText += CopyBeatmapsAsText;
            beatmapListingModel.OpenBeatmapPages += OpenBeatmapPages;
        }

        public void UnBind(IBeatmapListingModel beatmapListingModel)
        {
            beatmapListingModel.DownloadBeatmapsManaged -= DownloadBeatmapsManaged;
            beatmapListingModel.DownloadBeatmaps -= DownloadBeatmaps;
            beatmapListingModel.DeleteBeatmapsFromCollection -= DeleteBeatmapsFromCollection;
            beatmapListingModel.CopyBeatmapsAsText -= CopyBeatmapsAsText;
            beatmapListingModel.OpenBeatmapPages -= OpenBeatmapPages;
        }

        private void DownloadBeatmapsManaged(object sender, EventArgs args)
        {
            var model = (IBeatmapListingModel)sender;
            var manager = OsuDownloadManager.Instance;

            if (manager.AskUserForSaveDirectoryAndLogin(_userDialogs, _loginForm))
                OsuDownloadManager.Instance.DownloadBeatmaps(model.SelectedBeatmaps);
        }
        private void DeleteBeatmapsFromCollection(object sender, EventArgs args)
        {
            var model = (IBeatmapListingModel)sender;
            _collectionEditor.EditCollection(CollectionEditArgs.RemoveBeatmaps(model.CurrentCollection.Name, model.SelectedBeatmaps));
        }

        private void CopyBeatmapsAsText(object sender, EventArgs args)
        {
            var model = (IBeatmapListingModel)sender;
            var dummyCollection = new Collection(_osuFileIo.LoadedMaps);
            foreach (var beatmap in model.SelectedBeatmaps)
            {
                dummyCollection.AddBeatmap(beatmap);
            }
            var text = _listGenerator.GetAllMapsList(new Collections() { dummyCollection },CollectionListSaveType.BeatmapList);
            try
            {
                System.Windows.Forms.Clipboard.SetText(text);
            }
            catch { }
        }

        private void DownloadBeatmaps(object sender, EventArgs args)
        {
            var model = (IBeatmapListingModel)sender;

            var mapIds = model.SelectedBeatmaps.GetUniqueMapSetIds();
            MassOpen(mapIds, @"https://osu.ppy.sh/d/{0}");
        }

        private void OpenBeatmapPages(object sender, EventArgs args)
        {
            var model = (IBeatmapListingModel)sender;

            var mapIds = model.SelectedBeatmaps.GetUniqueMapSetIds();
            MassOpen(mapIds, @"https://osu.ppy.sh/s/{0}");
        }


        private void MassOpen(HashSet<int> dataSet, string urlFormat)
        {
            bool shouldContinue = true;
            if (dataSet.Count > 100)
            {
                shouldContinue = _userDialogs.YesNoMessageBox("You are going to open " + dataSet.Count +
                                             " map links at the same time in your default browser", "Are you sure?", MessageBoxType.Question);
            }
            if (shouldContinue)
            {
                foreach (var d in dataSet)
                {
                    OpenLink(string.Format(urlFormat, d));
                }
            }
        }
        private void OpenLink(string url)
        {
            Process.Start(url);

        }
    }
}