namespace CollectionManagerApp;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo;
using CollectionManager.Core.Types;
using CollectionManagerApp.Interfaces;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Presenters.Forms;
using System.Collections.Specialized;

public class GuiActionsHandler : IBeatmapListingBindingProvider
{
    private readonly IMainFormView _mainFormView;

    public SidePanelActionsHandler SidePanelActionsHandler { get; private set; }
    private readonly BeatmapListingActionsHandler _beatmapListingActionsHandler;

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
        IOnlineCollectionList sidePanel = (IOnlineCollectionList)_mainFormView.SidePanelView;

        RangeObservableCollection<OsuCollection> collections =
        [
            .. Initalizer.CollectionsManager.LoadedCollections
                .Where(c => c is not WebCollection)
                .Select(cc => (OsuCollection)cc)
,
        ];

        sidePanel.Collections = collections;

        sidePanel.WebCollections.AddRange(Initalizer.CollectionsManager.LoadedCollections.Except(collections).Except(sidePanel.WebCollections).Select(c => (WebCollection)c));
        sidePanel.WebCollections.CallReset();

    }

    public void Bind(IBeatmapListingModel model) => _beatmapListingActionsHandler.Bind(model, null);

    public void UnBind(IBeatmapListingModel model) => _beatmapListingActionsHandler.UnBind(model, null);
}