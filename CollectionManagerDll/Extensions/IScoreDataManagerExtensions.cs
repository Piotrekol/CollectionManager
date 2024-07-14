using CollectionManager.Enums;
using CollectionManager.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CollectionManager.Extensions;

public static class IScoreDataManagerExtensions
{
    public static IEnumerable<IReplay> GetReplays(this IScoreDataManager scoresManager, string mapHash, PlayMode playMode)
        => scoresManager
            .Scores
            .Where(score =>
                score.MapHash.Equals(mapHash)
                && score.PlayMode == playMode);

    public static OsuGrade GetTopReplayGrade(this IScoreDataManager scoresManager, string mapHash, PlayMode playMode)
    {
        IReplay topReplay = GetTopReplay(scoresManager, mapHash, playMode);

        if (topReplay is not ILazerReplay lazerReplay)
        {
            return OsuGrade.Null;
        }

        return lazerReplay.Grade;
    }

    public static IReplay GetTopReplay(this IScoreDataManager scoresManager, string mapHash, PlayMode playMode)
    {
        IEnumerable<IReplay> replays = GetReplays(scoresManager, mapHash, playMode);

        if (!replays.Any())
        {
            return null;
        }

        if (!replays.Skip(1).Any())
        {
            return replays.First();
        }

        return replays.Aggregate((first, second)
            => first.TotalScore > second.TotalScore ? first : second);
    }
}
