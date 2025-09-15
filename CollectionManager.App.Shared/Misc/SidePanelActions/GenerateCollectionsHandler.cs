namespace CollectionManager.App.Shared.Misc.SidePanelActions;

using CollectionManager.App.Shared;
using CollectionManager.App.Shared.Models.Controls;
using CollectionManager.App.Shared.Presenters.Controls;
using CollectionManager.App.Shared.Presenters.Forms;
using CollectionManager.Common;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.Collection;
using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;
using CollectionManager.Extensions.Modules.API.osu;
using CollectionManager.Extensions.Modules.CollectionApiGenerator;
using System.Threading;

public sealed class GenerateCollectionsHandler : IMainSidePanelActionHandler, IDisposable
{
    private readonly IUserDialogs _userDialogs;
    private readonly ICollectionEditor _collectionEditor;

    private CollectionsApiGenerator _collectionGenerator;
    private IUserTopGeneratorForm _userTopGeneratorForm;
    private IUsernameGeneratorForm _usernameGeneratorForm;
    private readonly OsuSite _osuSite = new();

    public MainSidePanelActions Action { get; } = MainSidePanelActions.GenerateCollections;

    public GenerateCollectionsHandler(IUserDialogs userDialogs, ICollectionEditor collectionEditor)
    {
        _userDialogs = userDialogs;
        _collectionEditor = collectionEditor;
        _collectionGenerator = new CollectionsApiGenerator(Initalizer.OsuFileIo.LoadedMaps);
    }

    public Task HandleAsync(object sender, object data)
    {
        InitializeGeneratorForm();
        _userTopGeneratorForm.Show();

        return Task.CompletedTask;
    }

    private void InitializeGeneratorForm()
    {
        if (_userTopGeneratorForm is not null && !_userTopGeneratorForm.IsDisposed)
        {
            return;
        }

        _userTopGeneratorForm = Initalizer.GuiComponentsProvider.GetClassImplementing<IUserTopGeneratorForm>();

        UserTopGeneratorModel model = new((a) => CollectionsApiGenerator.CreateCollectionName(new ApiScore() { EnabledMods = (int)(Mods.Hr | Mods.Hd) }, "Piotrekol", a));
        model.GenerateUsernames += GenerateUsernames;
        _ = new UserTopGeneratorFormPresenter(model, _userTopGeneratorForm, _userDialogs);

        model.Start += (s, a) => _collectionGenerator.GenerateCollection(model.GeneratorConfiguration);
        model.SaveCollections += (s, a) => _collectionEditor.EditCollection(CollectionEditArgs.AddCollections(model.Collections));
        model.Abort += async (s, a) => await _collectionGenerator.AbortAsync();
        _collectionGenerator.StatusUpdated += (s, a) =>
        {
            model.GenerationStatus = _collectionGenerator.Status;
            model.GenerationCompletionPrecentage = _collectionGenerator.ProcessingCompletionPercentage;
        };

        _collectionGenerator.CollectionsUpdated += (s, a) => model.Collections = _collectionGenerator.Collections;
    }

    private void GenerateUsernames(object sender, EventArgs eventArgs)
    {
        if (_usernameGeneratorForm == null || _usernameGeneratorForm.IsDisposed)
        {
            _usernameGeneratorForm = Initalizer.GuiComponentsProvider.GetClassImplementing<IUsernameGeneratorForm>();
            UsernameGeneratorModel model = new();
            model.Start += async (_, _) => await StartProcessingAsync(model);
            model.Complete += (_, _) => Helpers.SetClipboardText(model.GeneratedUsernamesStr);
            _ = new UsernameGeneratorPresenter(model, _usernameGeneratorForm.view);
        }

        _usernameGeneratorForm.ShowAndBlock();
    }

    private async Task StartProcessingAsync(UsernameGeneratorModel model)
    {
        model.GeneratedUsernames = await _osuSite.GetUsernamesAsync(model.StartRank, model.EndRank, CreateProcessingLogger(model), CancellationToken.None);
        model.EmitComplete();
    }

    private static OsuSite.LogUsernameGeneration CreateProcessingLogger(UsernameGeneratorModel model)
        => (string logMessage, int completionPercentage) =>
        {
            model.Status = logMessage;
            model.CompletionPercentage = completionPercentage;
        };

    public void Dispose() => throw new NotImplementedException();
}
