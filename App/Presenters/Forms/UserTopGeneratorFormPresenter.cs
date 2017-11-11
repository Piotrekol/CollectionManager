using App.Interfaces;
using App.Presenters.Controls;
using GuiComponents.Interfaces;

namespace App.Presenters.Forms
{
    public class UserTopGeneratorFormPresenter
    {
        private readonly IUserTopGeneratorModel _model;
        private readonly IUserTopGeneratorForm _form;

        public UserTopGeneratorFormPresenter(IUserTopGeneratorModel model, IUserTopGeneratorForm form)
        {
            _model = model;
            _form = form;
            form.Closing += (s, a) =>
            {
                _model.EmitAbort();
                if (_model.Collections != null)
                    _model.EmitSaveCollections();
            };
            new UserTopGeneratorPresenter(model, form.UserTopGeneratorView);

        }
    }
}