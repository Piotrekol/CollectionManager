using App.Interfaces;
using App.Presenters.Forms;
using CollectionManager.Modules.FileIO;
using GuiComponents.Interfaces;

namespace App
{
    public class GuiActionsHandler: IBeatmapListingBindingProvider
    {

        private SidePanelActionsHandler _sidePanelActionsHandler;
        private BeatmapListingActionsHandler _beatmapListingActionsHandler;


        public GuiActionsHandler(OsuFileIo osuFileIo, ICollectionEditor collectionEditor, IUserDialogs userDialogs, IMainFormView mainFormView, MainFormPresenter mainFormPresenter, ILoginFormView loginForm)
        {
            _sidePanelActionsHandler = new SidePanelActionsHandler(osuFileIo, collectionEditor, userDialogs, mainFormView,this,mainFormPresenter, loginForm);

            _beatmapListingActionsHandler = new BeatmapListingActionsHandler(collectionEditor,userDialogs,loginForm);
            _beatmapListingActionsHandler.Bind(mainFormPresenter.BeatmapListingModel);
        }

        public void Bind(IBeatmapListingModel model)
        {
            _beatmapListingActionsHandler.Bind(model);
        }

        public void UnBind(IBeatmapListingModel model)
        {
            _beatmapListingActionsHandler.UnBind(model);
        }
    }
}