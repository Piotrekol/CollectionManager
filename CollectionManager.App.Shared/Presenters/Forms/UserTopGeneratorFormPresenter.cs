namespace CollectionManager.App.Shared.Presenters.Forms;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.App.Shared.Presenters.Controls;
using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;

public class UserTopGeneratorFormPresenter
{
    private readonly IUserTopGeneratorModel _model;
    private readonly IUserTopGeneratorForm _form;

    public UserTopGeneratorFormPresenter(IUserTopGeneratorModel model, IUserTopGeneratorForm form, IUserDialogs userDialogs)
    {
        _model = model;
        _form = form;
        form.Closing += (s, a) =>
        {
            _model.EmitAbort();
            if (_model.Collections != null)
            {
                _model.EmitSaveCollections();
            }
        };
        _ = new UserTopGeneratorPresenter(model, form.UserTopGeneratorView, userDialogs);

    }
}