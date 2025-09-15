namespace CollectionManager.App.Shared.Presenters.Forms;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.App.Shared.Presenters.Controls;
using CollectionManager.Common.Interfaces.Forms;

public class CollectionAddRenameFormPresenter
{
    private readonly ICollectionAddRenameForm _form;
    private readonly ICollectionAddRenameModel _model;

    public CollectionAddRenameFormPresenter(ICollectionAddRenameForm form, ICollectionAddRenameModel model)
    {
        _form = form;
        _model = model;

        _ = new CollectionAddRenamePresenter(form.CollectionRenameView, model);
    }
}