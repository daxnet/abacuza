using Abacuza.Clusters.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.ApiService.Models
{
    /// <summary>
    /// Represents the object that holds a collection of clusters.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.ICollection{Abacuza.Clusters.Common.ICluster}" />
    public sealed class ClusterCollection : ICollection<ICluster>
    {
        private readonly List<ICluster> _clusters = new List<ICluster>();

        public int Count => _clusters.Count;

        public bool IsReadOnly => false;

        public void Add(ICluster item)
        {
            _clusters.Add(item);
        }

        public void Clear()
        {
            _clusters.Clear();
        }

        public bool Contains(ICluster item) => _clusters.Contains(item);

        public void CopyTo(ICluster[] array, int arrayIndex) => _clusters.CopyTo(array, arrayIndex);

        public IEnumerator<ICluster> GetEnumerator() => _clusters.GetEnumerator();

        public bool Remove(ICluster item) => _clusters.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => _clusters.GetEnumerator();
    }
}
