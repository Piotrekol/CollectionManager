using System.Collections;
using System.Collections.Generic;
using CollectionManager.Enums;

namespace CollectionManager.DataTypes
{
    public class PlayModeStars : IEnumerable<KeyValuePair<PlayMode, StarRating>>
    {
        public StarRating Osu { get; set; }
        public StarRating Taiko { get; set; }
        public StarRating Ctb { get; set; }
        public StarRating Mania { get; set; }

        public IEnumerator<KeyValuePair<PlayMode, StarRating>> GetEnumerator()
        {
            if (Osu != null)
                yield return new KeyValuePair<PlayMode, StarRating>(PlayMode.Osu, Osu);
            if (Taiko != null)
                yield return new KeyValuePair<PlayMode, StarRating>(PlayMode.Taiko, Taiko);
            if (Ctb != null)
                yield return new KeyValuePair<PlayMode, StarRating>(PlayMode.CatchTheBeat, Ctb);
            if (Mania != null)
                yield return new KeyValuePair<PlayMode, StarRating>(PlayMode.OsuMania, Mania);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Osu?.GetHashCode() ?? 1;
                hash = hash * 23 + Taiko?.GetHashCode() ?? 2;
                hash = hash * 23 + Ctb?.GetHashCode() ?? 3;
                hash = hash * 23 + Mania?.GetHashCode() ?? 4;
                return hash;
            }
        }


        public void Add(PlayMode playMode, StarRating starRating)
        => this[playMode] = starRating;

        public bool ContainsKey(PlayMode playMode)
            => this[playMode] != null;

        public StarRating this[PlayMode key]
        {
            get
            {
                switch (key)
                {
                    case PlayMode.Osu:
                        return Osu;
                    case PlayMode.Taiko:
                        return Taiko;
                    case PlayMode.CatchTheBeat:
                        return Ctb;
                    case PlayMode.OsuMania:
                        return Mania;
                }

                throw new KeyNotFoundException();
            }

            set
            {
                switch (key)
                {
                    case PlayMode.Osu:
                        Osu = value;
                        return;
                    case PlayMode.Taiko:
                        Taiko = value;
                        return;
                    case PlayMode.CatchTheBeat:
                        Ctb = value;
                        return;
                    case PlayMode.OsuMania:
                        Mania = value;
                        return;
                }

                throw new KeyNotFoundException();
            }
        }
    }
}