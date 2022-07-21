﻿using System.Collections.Specialized;
using System.Linq;
using App.Interfaces;
using App.Presenters.Forms;
using CollectionManager.DataTypes;
using CollectionManager.Modules.FileIO;
using GuiComponents.Interfaces;

namespace App
{
    public class GuiActionsHandler : IBeatmapListingBindingProvider
    {
        private readonly IMainFormView _mainFormView;

        public SidePanelActionsHandler SidePanelActionsHandler { get; private set; }
        private BeatmapListingActionsHandler _beatmapListingActionsHandler;


        public GuiActionsHandler(OsuFileIo osuFileIo, ICollectionEditor collectionEditor, IUserDialogs userDialogs, IMainFormView mainFormView, MainFormPresenter mainFormPresenter, ILoginFormView loginForm)
        {
            _mainFormView = mainFormView;
            SidePanelActionsHandler = new SidePanelActionsHandler(osuFileIo, collectionEditor, userDialogs, mainFormView, this, mainFormPresenter, loginForm);

            _beatmapListingActionsHandler = new BeatmapListingActionsHandler(collectionEditor, userDialogs, loginForm, osuFileIo);
            _beatmapListingActionsHandler.Bind(mainFormPresenter.BeatmapListingModel, mainFormPresenter.CollectionListingModel);

            Initalizer.CollectionsManager.LoadedCollections.CollectionChanged += LoadedCollectionsOnCollectionChanged;
        }

        private void LoadedCollectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var sidePanel = (IOnlineCollectionList)_mainFormView.SidePanelView;

            var collections = new RangeObservableCollection<Collection>();
            collections.AddRange(
                Initalizer.CollectionsManager.LoadedCollections
                    .Where(c => !(c is WebCollection))
                    .Select(cc => (Collection)cc)
            );

            sidePanel.Collections = collections;

            sidePanel.WebCollections.AddRange(Initalizer.CollectionsManager.LoadedCollections.Except(collections).Except(sidePanel.WebCollections).Select(c => (WebCollection)c));
            sidePanel.WebCollections.CallReset();

        }

        public void Bind(IBeatmapListingModel model)
        {
            _beatmapListingActionsHandler.Bind(model, null);
        }

        public void UnBind(IBeatmapListingModel model)
        {
            _beatmapListingActionsHandler.UnBind(model, null);
        }
    }
}