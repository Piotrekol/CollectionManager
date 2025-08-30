namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.App.Shared.Interfaces;
using CollectionManager.App.Shared.Presenters.Forms;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;

public sealed class ShowBeatmapListingHandler : IMainSidePanelActionHandler
{
    private readonly IBeatmapListingBindingProvider _beatmapListingBindingProvider;
    private readonly IUserDialogs _userDialogs;
    private IBeatmapListingForm _beatmapListingForm;

    public MainSidePanelActions Action { get; } = MainSidePanelActions.ShowBeatmapListing;

    public ShowBeatmapListingHandler(IBeatmapListingBindingProvider beatmapListingBindingProvider, IUserDialogs userDialogs)
    {
        _beatmapListingBindingProvider = beatmapListingBindingProvider;
        _userDialogs = userDialogs;
    }

    public Task HandleAsync(object sender, object data)
    {
        if (_beatmapListingForm == null || _beatmapListingForm.IsDisposed)
        {
            _beatmapListingForm = Initalizer.GuiComponentsProvider.GetClassImplementing<IBeatmapListingForm>();
            BeatmapListingFormPresenter presenter = new(_beatmapListingForm, _userDialogs);
            _beatmapListingBindingProvider.Bind(presenter.BeatmapListingModel);
            _beatmapListingForm.Closing += (s, a) => _beatmapListingBindingProvider.UnBind(presenter.BeatmapListingModel);
        }

        _beatmapListingForm.Show();
        return Task.CompletedTask;
    }
}
