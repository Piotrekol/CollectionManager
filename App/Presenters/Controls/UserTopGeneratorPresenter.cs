using System;
using System.Linq;
using App.Interfaces;
using CollectionManagerExtensionsDll.DataTypes;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class UserTopGeneratorPresenter
    {
        private readonly IUserTopGeneratorModel _model;
        private readonly IUserTopGenerator _view;

        public UserTopGeneratorPresenter(IUserTopGeneratorModel model, IUserTopGenerator view)
        {
            _model = model;
            _view = view;

            _view.Start += ViewOnStart;
            _view.Abort += ViewOnAbort;
            _view.GenerateUsernames += (s, a) => _model.EmitGenerateUsernames();
            _view.CollectionNamingFormatChanged += _view_CollectionNamingFormatChanged;

            _model.StatusChanged += ModelOnStatusChanged;
            _model.CollectionsChanged += ModelOnCollectionsChanged;
            
            _view_CollectionNamingFormatChanged(this,EventArgs.Empty);
        }

        private void ViewOnAbort(object sender, EventArgs eventArgs)
        {
            _model.EmitAbort();
        }

        private void _view_CollectionNamingFormatChanged(object sender, EventArgs e)
        {
            _view.CollectionNamingExample = _model.GetCollectionNameExample(_view.CollectionNamingFormat);
        }

        private void ViewOnStart(object sender, EventArgs eventArgs)
        {
            //TODO: show some sort of error on missing api key
            if (string.IsNullOrEmpty(_view.ApiKey))
                return;
            _model.GeneratorConfiguration = new CollectionGeneratorConfiguration()
            {
                CollectionNameSavePattern = _view.CollectionNamingFormat,
                Usernames = _view.Usernames.Split(',').ToList(),
                ApiKey = _view.ApiKey,
                Gamemode = _view.Gamemode,
                ScoreSaveConditions = new ScoreSaveConditions()
                {
                    MinimumPp = _view.PpMin,
                    MaximumPp = _view.PpMax,
                    MinimumAcc = _view.AccMin,
                    MaximumAcc = _view.AccMax,
                    RanksToGet = (RankTypes)_view.AllowedScores
                }
            };
            _model.EmitStart();
            _view.IsRunning = true;
        }


        private void ModelOnCollectionsChanged(object sender, EventArgs eventArgs)
        {
            _view.CollectionListing.Collections = _model.Collections;
            _view.IsRunning = false;
        }

        private void ModelOnStatusChanged(object sender, EventArgs eventArgs)
        {
            _view.ProcessingStatus = _model.GenerationStatus;
            _view.ProcessingCompletionPrecentage = _model.GenerationCompletionPrecentage;
            
        }
    }
}