namespace CollectionManagerApp.Presenters.Controls;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManagerApp.Interfaces.Controls;

internal class CollectionAddRenamePresenter
{
    private readonly ICollectionRenameView _view;
    private readonly ICollectionAddRenameModel _model;

    public CollectionAddRenamePresenter(ICollectionRenameView view, ICollectionAddRenameModel model)
    {
        _view = view;
        _model = model;

        _view.CollectionNameChanged += ViewOnCollectionNameChanged;
        _view.Submited += ViewOnSubmited;
        _view.Canceled += ViewOnCanceled;
    }

    private void Unbind()
    {
        _view.CollectionNameChanged -= ViewOnCollectionNameChanged;
        _view.Submited -= ViewOnSubmited;
        _view.Canceled -= ViewOnCanceled;
    }
    private void ViewOnCanceled(object sender, EventArgs eventArgs)
    {
        _model.UserCanceled = true;
        Unbind();
    }

    private void ViewOnSubmited(object sender, EventArgs eventArgs)
    {
        _model.EmitSubmited();
        Unbind();
    }

    private void ViewOnCollectionNameChanged(object sender, EventArgs eventArgs)
    {
        bool isValid = _model.IsCollectionNameValid(_view.NewCollectionName);
        _view.CanSubmit = isValid;
        _view.ErrorText = isValid ? "" : "Collection name is invalid!";

        _model.NewCollectionNameIsValid = isValid;
        _model.NewCollectionName = _view.NewCollectionName;
    }
}
