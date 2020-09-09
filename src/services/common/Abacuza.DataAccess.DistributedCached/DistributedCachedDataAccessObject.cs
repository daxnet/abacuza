using Abacuza.Common;
using Abacuza.Common.DataAccess;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Abacuza.DataAccess.DistributedCached
{
    public sealed class DistributedCachedDataAccessObject : IDataAccessObject
    {
        private readonly IDistributedCache _cache;
        private readonly IDataAccessObject _wrappedDao;

        public DistributedCachedDataAccessObject(IDistributedCache cache, IDataAccessObject wrappedDao)
        {
            _cache = cache;
            _wrappedDao = wrappedDao;
        }

        public async Task AddAsync<TObject>(TObject entity) where TObject : IEntity
        {
            await _wrappedDao.AddAsync(entity);
            var key = entity.Id.ToString();
            var val = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity));
            await _cache.SetAsync(key, val);
        }

        public async Task DeleteByIdAsync<TObject>(Guid id) where TObject : IEntity
        {
            await _wrappedDao.DeleteByIdAsync<TObject>(id);
            await _cache.RemoveAsync(id.ToString());
        }

        public void Dispose()
        {
            _wrappedDao.Dispose();
        }

        public Task<IEnumerable<TObject>> FindBySpecificationAsync<TObject>(Expression<Func<TObject, bool>> expr) where TObject : IEntity
        {
            return _wrappedDao.FindBySpecificationAsync(expr);
        }

        public Task<IEnumerable<TObject>> GetAllAsync<TObject>() where TObject : IEntity => _wrappedDao.GetAllAsync<TObject>();

        public async Task<TObject> GetByIdAsync<TObject>(Guid id) where TObject : IEntity
        {
            var resultBinary = await _cache.GetAsync(id.ToString());
            if (resultBinary != null)
            {
                return JsonConvert.DeserializeObject<TObject>(Encoding.UTF8.GetString(resultBinary));
            }

            var result = await _wrappedDao.GetByIdAsync<TObject>(id);
            if (result != null)
            {
                await _cache.SetAsync(id.ToString(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)));
            }

            return result;
        }

        public async Task UpdateByIdAsync<TObject>(Guid id, TObject entity) where TObject : IEntity
        {
            await _wrappedDao.UpdateByIdAsync(id, entity);
            await _cache.SetAsync(id.ToString(), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity)));
        }
    }
}
