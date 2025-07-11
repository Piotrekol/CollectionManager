namespace CollectionManagerApp.Models.Controls;

using CollectionManager.Core.Types;
using CollectionManagerApp.Interfaces.Controls;

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
