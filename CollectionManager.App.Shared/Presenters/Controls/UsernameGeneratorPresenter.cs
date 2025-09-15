namespace CollectionManager.App.Shared.Presenters.Controls;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Controls;

public class UsernameGeneratorPresenter
{
    private readonly IUsernameGeneratorModel _model;
    private readonly IUsernameGeneratorView _view;
    public UsernameGeneratorPresenter(IUsernameGeneratorModel model, IUsernameGeneratorView view)
    {
        _model = model;
        _view = view;
        _view.Start += ViewOnStart;
        _view.Abort += (s, a) => _model.EmitAbort();
        //_view. += (s, a) => _model.EmitComplete();
        _model.StatusChanged += _model_StatusChanged;
        _model.Complete += _model_Complete;
    }

    private void _model_Complete(object sender, EventArgs e)
    {
        _view.GeneratedUsernames = string.Join(",", _model.GeneratedUsernames);
        _view.StartEnabled = true;
    }

    private void _model_StatusChanged(object sender, EventArgs e)
    {
        _view.CompletionPrecentage = _model.CompletionPercentage;
        _view.Status = _model.Status;
    }

    private void ViewOnStart(object sender, EventArgs eventArgs)
    {
        _view.StartEnabled = false;
        _view.AbortEnabled = false;
        _model.StartRank = _view.RankMin;
        _model.EndRank = _view.RankMax;
        _model.EmitStart();
    }
}