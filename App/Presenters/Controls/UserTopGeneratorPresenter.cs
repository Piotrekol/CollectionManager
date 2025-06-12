﻿namespace CollectionManagerApp.Presenters.Controls;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;
using CollectionManagerApp.Interfaces.Controls;
using System.Text.RegularExpressions;

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

        _view_CollectionNamingFormatChanged(this, EventArgs.Empty);
    }

    private void ViewOnAbort(object sender, EventArgs eventArgs) => _model.EmitAbort();

    private void _view_CollectionNamingFormatChanged(object sender, EventArgs e) => _view.CollectionNamingExample = _model.GetCollectionNameExample(_view.CollectionNamingFormat);

    private void ViewOnStart(object sender, EventArgs eventArgs)
    {
        //TODO: show some sort of error on missing api key
        if (string.IsNullOrEmpty(_view.ApiKey))
        {
            return;
        }

        List<Mods> modCombinations = [];
        if (!string.IsNullOrWhiteSpace(_view.AllowedModCombinations) &&
            !_view.AllowedModCombinations.Trim().Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            string strMods = _view.AllowedModCombinations.Trim().ToLowerInvariant();
            string[] splitModCombinations = strMods.Split(',');
            foreach (string splitModCombination in splitModCombinations)
            {
                List<string> splitMods = Regex.Split(splitModCombination, @"([A-Za-z]{2})").Where(s => !string.IsNullOrEmpty(s)).ToList();
                Mods mods = Mods.Nm;
                foreach (string mod in splitMods)
                {
                    if (Enum.TryParse(mod, true, out Mods parsedMod))
                    {
                        mods |= parsedMod;
                    }
                }

                modCombinations.Add(mods);
            }
        }

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
                RanksToGet = (RankTypes)_view.AllowedScores,
                ModCombinations = modCombinations
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