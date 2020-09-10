using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Common.Models
{
    public abstract class ItemCollection<T> : ICollection<T>
    {
        protected readonly List<T> _items = new List<T>();

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(T item) => _items.Add(item);

        public void Clear() => _items.Clear();

        public bool Contains(T item) => _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        public bool Remove(T item) => _items.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
