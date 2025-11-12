namespace CollectionManager.Core.Types;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

public class RangeObservableCollection<T> : ObservableCollection<T>
{
    private bool _suppressNotifications;

    public RangeObservableCollection()
    {
    }

    public RangeObservableCollection(IEnumerable<T> collection)
        : base(collection)
    {
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!_suppressNotifications)
        {
            base.OnCollectionChanged(e);
        }
    }

    public void SilentRemove(T item)
    {
        _suppressNotifications = true;
        _ = Remove(item);
        _suppressNotifications = false;
    }

    public void SilentAdd(T item)
    {
        _suppressNotifications = true;
        Add(item);
        _suppressNotifications = false;
    }

    public void CallReset() => OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

    public void AddRange(IEnumerable<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        _suppressNotifications = true;

        foreach (T item in list)
        {
            Add(item);
        }

        _suppressNotifications = false;
    }

    /// <summary>
    /// Temporarily suspends collection changed events until the returned context is disposed.
    /// </summary>
    /// <returns>The suspend context.</returns>
    public SuspendContext SuspendCollectionChangedEvents() => new(this);

    public sealed class SuspendContext : IDisposable
    {
        private readonly RangeObservableCollection<T> _collection;

        public SuspendContext(RangeObservableCollection<T> collection)
        {
            _collection = collection;
            _collection._suppressNotifications = true;
        }

        public void Dispose() => _collection._suppressNotifications = false;
    }
}
