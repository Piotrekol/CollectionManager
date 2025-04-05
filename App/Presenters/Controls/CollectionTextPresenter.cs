namespace CollectionManagerApp.Presenters.Controls;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Extensions.Enums;
using CollectionManager.Extensions.Modules.CollectionListGenerator;
using CollectionManagerApp.Interfaces.Controls;

public class CollectionTextPresenter
{
    private readonly ICollectionTextView _view;
    private readonly ICollectionTextModel _model;
    private readonly ListGenerator _listGenerator = new();
    public CollectionTextPresenter(ICollectionTextView view, ICollectionTextModel model)
    {
        _view = view;
        _view.SetListTypes(Enum.GetValues<CollectionListSaveType>());
        _view.SaveTypeChanged += _view_SaveTypeChanged;

        _model = model;
        _model.CollectionChanged += ModelOnCollectionChanged;
    }

    private void _view_SaveTypeChanged(object sender, EventArgs e) => ModelOnCollectionChanged(this, null);

    private void ModelOnCollectionChanged(object sender, EventArgs eventArgs)
    {
        _ = Enum.TryParse(_view.SelectedSaveType, out CollectionListSaveType type);
        _view.GeneratedText = _listGenerator.GetAllMapsList(_model.Collections, type);
    }
}