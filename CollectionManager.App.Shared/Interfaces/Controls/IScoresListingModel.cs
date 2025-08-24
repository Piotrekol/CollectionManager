namespace CollectionManager.App.Shared.Interfaces.Controls;

using CollectionManager.Core.Types;

public interface IScoresListingModel
{
    Scores Scores { get; set; }

    event EventHandler ScoresChanged;
}
