using Abacuza.Common;
using Abacuza.Common.DataAccess;
using Abacuza.Common.Utilities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Abacuza.DataAccess.Mongo
{
    public sealed class MongoDataAccessObject : IDataAccessObject
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        private bool disposedValue = false;

        public MongoDataAccessObject(string databaseName, string server = "localhost", int port = 27017)
        {
            _client = new MongoClient($"mongodb://{server}:{port}/{databaseName}");
            _database = _client.GetDatabase(databaseName);
        }

        public async Task AddAsync<TObject>(TObject entity) where TObject : IHasGuidId
        {
            var collection = GetCollection<TObject>();
            var options = new InsertOneOptions { BypassDocumentValidation = true };
            await collection.InsertOneAsync(entity, options);
        }

        public async Task DeleteByIdAsync<TObject>(Guid id) where TObject : IHasGuidId
        {
            var filterDefinition = Builders<TObject>.Filter.Eq(x => x.Id, id);
            await GetCollection<TObject>().DeleteOneAsync(filterDefinition);
        }

        public void Dispose() => Dispose(true);

        public async Task<IEnumerable<TObject>> FindBySpecificationAsync<TObject>(Expression<Func<TObject, bool>> expr) where TObject : IHasGuidId
            => await(await GetCollection<TObject>().FindAsync(expr)).ToListAsync();

        public async Task<IEnumerable<TObject>> GetAllAsync<TObject>() where TObject : IHasGuidId
            => await FindBySpecificationAsync<TObject>(_ => true);

        public async Task<TObject> GetByIdAsync<TObject>(Guid id) where TObject : IHasGuidId
            => (await FindBySpecificationAsync<TObject>(x => x.Id.Equals(id))).FirstOrDefault();

        public async Task UpdateByIdAsync<TObject>(Guid id, TObject entity)
             where TObject : IHasGuidId
        {
            var filterDefinition = Builders<TObject>.Filter.Eq(x => x.Id, id);
            await GetCollection<TObject>().ReplaceOneAsync(filterDefinition, entity);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        private IMongoCollection<TObject> GetCollection<TObject>() where TObject : IHasGuidId => _database.GetCollection<TObject>(typeof(TObject).Name.Pluralize());
    }
}
