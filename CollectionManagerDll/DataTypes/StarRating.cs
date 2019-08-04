using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionManager.DataTypes
{
    public class StarRating : IEnumerable<KeyValuePair<int, double>>
    {
        public SortedDictionary<int, double> Values { get; } = new SortedDictionary<int, double>();
        public void Add(int key, double value)
            => this[key] = value;

        public bool ContainsKey(int key)
            => Values.ContainsKey(key);

        public IEnumerator<KeyValuePair<int, double>> GetEnumerator()
        {
            foreach (var kvPair in Values)
            {
                yield return new KeyValuePair<int, double>(kvPair.Key, kvPair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var kvPair in Values)
                {
                    hash = hash * 23 + kvPair.Key.GetHashCode();
                    hash = hash * 23 + kvPair.Value.GetHashCode();
                }
                return hash;
            }
        }


        public double this[int key]
        {
            get
            {
                return Values[key];
            }
            set { Values[key] = value; }
        }
    }
}