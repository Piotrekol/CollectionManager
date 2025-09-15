namespace CollectionManager.App.Shared.Models.Controls;

using CollectionManager.App.Shared.Interfaces.Controls;
using CollectionManager.Core.Types;

public class ScoresListingModel : IScoresListingModel
{
    private Scores _scores;

    public virtual Scores Scores
    {
        get => _scores;
        set
        {
            _scores = value;
            OnScoresChanged();
        }
    }

    public event EventHandler ScoresChanged;

    protected virtual void OnScoresChanged() => ScoresChanged?.Invoke(this, EventArgs.Empty);
}
