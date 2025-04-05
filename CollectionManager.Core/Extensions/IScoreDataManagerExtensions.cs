namespace CollectionManager.Core.Extensions;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

public static class IScoreDataManagerExtensions
{
    public static IEnumerable<IReplay> GetReplays(this IScoreDataManager scoresManager, string mapHash, PlayMode playMode)
        => scoresManager
            .Scores
            .Where(score =>
                score.MapHash.Equals(mapHash, StringComparison.OrdinalIgnoreCase)
                && score.PlayMode == playMode);

    public static OsuGrade GetTopReplayGrade(this IScoreDataManager scoresManager, string mapHash, PlayMode playMode)
    {
        IReplay topReplay = scoresManager.GetTopReplay(mapHash, playMode);

        return topReplay is not ILazerReplay lazerReplay ? OsuGrade.Null : lazerReplay.Grade;
    }

    public static IReplay GetTopReplay(this IScoreDataManager scoresManager, string mapHash, PlayMode playMode)
    {
        IEnumerable<IReplay> replays = scoresManager.GetReplays(mapHash, playMode);

        if (!replays.Any())
        {
            return null;
        }

        return !replays.Skip(1).Any()
            ? replays.First()
            : replays.Aggregate((first, second) => first.TotalScore > second.TotalScore ? first : second);
    }
}
