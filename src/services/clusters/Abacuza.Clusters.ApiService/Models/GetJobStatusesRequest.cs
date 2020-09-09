using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Models
{
    public class GetJobStatusesRequest : ICollection<GetJobStatusesRequestItem>
    {
        private readonly List<GetJobStatusesRequestItem> _items = new List<GetJobStatusesRequestItem>();

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(GetJobStatusesRequestItem item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(GetJobStatusesRequestItem item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(GetJobStatusesRequestItem[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GetJobStatusesRequestItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(GetJobStatusesRequestItem item)
        {
            return _items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public override string ToString()
        {
            return $"Number of items: {Count}";
        }
    }
}
