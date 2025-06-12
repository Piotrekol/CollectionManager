namespace CollectionManager.Core.Types;

using System.Collections;
using System.Collections.Generic;

public class StarRating : IEnumerable<KeyValuePair<int, float>>
{
    public SortedList<int, float>? Values { get; private set; }
    public void Add(int key, float value)
        => this[key] = value;

    public bool ContainsKey(int key)
        => Values?.ContainsKey(key) ?? false;

    public IEnumerator<KeyValuePair<int, float>> GetEnumerator()
    {
        if (Values is null)
        {
            yield break;
        }

        foreach (KeyValuePair<int, float> kvPair in Values)
        {
            yield return kvPair;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            if (Values is null)
            {
                return hash;
            }

            foreach (KeyValuePair<int, float> kvPair in Values)
            {
                hash = (hash * 23) + kvPair.Key.GetHashCode();
                hash = (hash * 23) + kvPair.Value.GetHashCode();
            }

            return hash;
        }
    }

    public float this[int key]
    {
        get => Values[key];

        set
        {
            Values ??= [];
            Values[key] = value;
        }
    }
}