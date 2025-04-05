namespace CollectionManagerApp.Presenters.Forms;

using CollectionManager.Common.Interfaces.Forms;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Presenters.Controls;

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