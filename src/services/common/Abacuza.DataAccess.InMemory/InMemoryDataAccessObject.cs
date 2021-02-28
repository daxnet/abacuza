using Abacuza.Common;
using Abacuza.Common.DataAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Abacuza.DataAccess.InMemory
{
    public sealed class InMemoryDataAccessObject : IDataAccessObject
    {
        private readonly ConcurrentDictionary<Guid, IEntity> _storage = new ConcurrentDictionary<Guid, IEntity>();

        public IEnumerable<KeyValuePair<Guid, IEntity>> Storage => _storage;

        public Task AddAsync<TObject>(TObject entity) where TObject : IEntity
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            _storage.AddOrUpdate(entity.Id, entity, (idKey, oldValue) => entity);
            return Task.CompletedTask;
        }

        public Task DeleteByIdAsync<TObject>(Guid id) where TObject : IEntity
        {
            _storage.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public void Dispose() { }

        public Task<IEnumerable<TObject>> FindBySpecificationAsync<TObject>(Expression<Func<TObject, bool>> expr) where TObject : IEntity
        {
            var result = _storage.Values.Select(x => (TObject)x).Where(expr.Compile());
            return Task.FromResult(result);
        }

        public Task<IEnumerable<TObject>> GetAllAsync<TObject>() where TObject : IEntity => Task.FromResult(_storage.Values.Select(x => (TObject)x));

        public Task<TObject?> GetByIdAsync<TObject>(Guid id) where TObject : IEntity
        {
            if (_storage.ContainsKey(id))
            {
                return Task.FromResult((TObject?)_storage[id]);
            }

            return Task.FromResult<TObject?>(default);
        }

        public Task UpdateByIdAsync<TObject>(Guid id, TObject entity) where TObject : IEntity
        {
            var oldObj = _storage[id];
            _storage.TryUpdate(id, entity, oldObj);

            return Task.CompletedTask;
        }
    }
}
