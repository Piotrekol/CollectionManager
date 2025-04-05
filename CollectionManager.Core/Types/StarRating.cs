namespace CollectionManager.Core.Types;

using System.Collections;
using System.Collections.Generic;

public class StarRating : IEnumerable<KeyValuePair<int, double>>
{
    public SortedList<int, double> Values { get; } = [];
    public void Add(int key, double value)
        => this[key] = value;

    public bool ContainsKey(int key)
        => Values.ContainsKey(key);

    public IEnumerator<KeyValuePair<int, double>> GetEnumerator()
    {
        foreach (KeyValuePair<int, double> kvPair in Values)
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
            foreach (KeyValuePair<int, double> kvPair in Values)
            {
                hash = (hash * 23) + kvPair.Key.GetHashCode();
                hash = (hash * 23) + kvPair.Value.GetHashCode();
            }

            return hash;
        }
    }

    public double this[int key]
    {
        get => Values[key]; set => Values[key] = value;
    }
}