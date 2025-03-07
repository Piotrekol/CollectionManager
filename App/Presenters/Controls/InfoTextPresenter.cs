using System;
using App.Interfaces;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class InfoTextPresenter
    {
        private readonly IInfoTextView _view;
        public readonly IInfoTextModel Model;

        private const string UpdateAvaliable = "Update is avaliable!({0})";
        private const string UpdateError = "Error while checking for updates.";
        private const string NoUpdatesAvailable = "No updates avaliable ({0})";
        private const string FetchingUpdateInformation = "Fetching update information..";

        public InfoTextPresenter(IInfoTextView view, IInfoTextModel model)
        {
            _view = view;
            Model = model;

            Model.CountsUpdated += ModelOnCountsUpdated;
            _view.UpdateTextClicked += ViewOnUpdateTextClicked;
        }

        private void ViewOnUpdateTextClicked(object sender, EventArgs eventArgs)
        {
            Model.EmitUpdateTextClicked();
            SetUpdateText();
        }


        private void ModelOnCountsUpdated(object sender, EventArgs eventArgs)
        {
            _view.CollectionManagerStatus = $"Loaded {Model.BeatmapCount} beatmaps && {Model.CollectionsCount} collections with {Model.BeatmapsInCollectionsCount} beatmaps. Missing {Model.MissingMapSetsCount} downloadable map sets. {Model.UnknownMapCount} unknown maps.";
            SetUpdateText();
        }

        private void SetUpdateText()
        {
            var updater = Model.GetUpdater();
            if (updater != null)
            {
                _view.ColorUpdateText = true;

                if (updater.OnlineVersion is null)
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
            else
            {
                _view.ColorUpdateText = false;
                _view.UpdateText = "";
            }
        }
    }
}