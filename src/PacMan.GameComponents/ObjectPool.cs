using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace PacMan.GameComponents
{
    public class ObjectPool<T>
    {
        readonly ConcurrentBag<T> _objects;
        readonly Func<T> _objectGenerator;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public ObjectPool(Func<T> objectGenerator)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _objects = new ConcurrentBag<T>();
        }

        public T Get() => _objects.TryTake(out T item) ? item : _objectGenerator();

        public void Return(T item) => _objects.Add(item);
    }
}