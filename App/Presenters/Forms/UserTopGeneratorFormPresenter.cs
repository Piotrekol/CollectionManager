namespace CollectionManagerApp.Presenters.Forms;

using CollectionManager.Common.Interfaces;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManagerApp.Interfaces.Controls;
using CollectionManagerApp.Presenters.Controls;

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