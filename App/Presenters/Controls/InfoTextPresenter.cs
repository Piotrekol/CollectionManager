using System;
using System.Threading.Tasks;
using App.Interfaces;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class InfoTextPresenter
    {
        private readonly IInfoTextView _view;
        private readonly IInfoTextModel _model;
        private static Task CheckForUpdatesTask;

        private const string UpdateAvaliable = "Update is avaliable!({0})";
        private const string UpdateError = "Error while checking for updates.";
        private const string NoUpdatesAvailable = "No updates avaliable ({0})";
        private const string FetchingUpdateInformation = "Fetching update information..";

        public InfoTextPresenter(IInfoTextView view, IInfoTextModel model)
        {
            _view = view;
            _model = model;

            _model.CountsUpdated += ModelOnCountsUpdated;
            _view.UpdateTextClicked += ViewOnUpdateTextClicked;

            if (CheckForUpdatesTask == null)
            {
                CheckForUpdatesTask = Task.Run(CheckForUpdates);
            }
            else
            {
                _ = CheckForUpdatesTask.ContinueWith((_) => SetUpdateText(null));
            }
        }

        private void ViewOnUpdateTextClicked(object sender, EventArgs eventArgs)
        {
            _model.EmitUpdateTextClicked();
        }

        private void ModelOnCountsUpdated(object sender, EventArgs eventArgs)
        {
            _view.CollectionManagerStatus = $"Loaded {_model.BeatmapCount} beatmaps && {_model.CollectionsCount} collections with {_model.BeatmapsInCollectionsCount} beatmaps. Missing {_model.MissingMapSetsCount} downloadable map sets. {_model.UnknownMapCount} unknown maps.";
        }

        private Task CheckForUpdates()
        {
            try
            {
                var updater = _model.GetUpdater();
                
                if (updater == null)
                {
                    return Task.CompletedTask;
                }

                _ = updater.CheckForUpdates();
                SetUpdateText(updater);
            }
            catch { }

            return Task.CompletedTask;
        }

        private void SetUpdateText(IUpdateModel updater)
        {
            updater ??= _model.GetUpdater();
            
            if (updater == null)
            {
                _view.ColorUpdateText = false;
                _view.UpdateText = "";
                
                return;
            }

            _view.ColorUpdateText = true;

            if (updater.OnlineVersion == null)
            {
                _view.UpdateText = FetchingUpdateInformation;
                _view.ColorUpdateText = false;
            }
            else if (updater.UpdateIsAvailable)
            {
                _view.UpdateText = string.Format(UpdateAvaliable, updater.OnlineVersion);
            }
            else if (updater.Error)
            {
                _view.UpdateText = UpdateError;
            }
            else
            {
                _view.ColorUpdateText = false;
                _view.UpdateText = string.Format(NoUpdatesAvailable, updater.CurrentVersion);
            }
        }
    }
}