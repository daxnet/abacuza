using Abacuza.Common;
using Abacuza.Common.DataAccess;
using Abacuza.Common.Utilities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Abacuza.DataAccess.Mongo
{
    public sealed class MongoDataAccessObject : IDataAccessObject
    {
        private static readonly Dictionary<Type, string> _normalizedCollectionNames = new Dictionary<Type, string>();
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        private bool disposedValue = false;

        public MongoDataAccessObject(string databaseName, string server = "localhost", int port = 27017)
        {
            _client = new MongoClient($"mongodb://{server}:{port}/{databaseName}");
            _database = _client.GetDatabase(databaseName);
        }

        public MongoDataAccessObject(MongoUrl url, string databaseName)
        {
            _client = new MongoClient(url);
            _database = _client.GetDatabase(databaseName);
        }

        public async Task AddAsync<TObject>(TObject entity) where TObject : IEntity
        {
            var collection = GetCollection<TObject>();
            var options = new InsertOneOptions { BypassDocumentValidation = true };
            await collection.InsertOneAsync(entity, options);
        }

        public async Task DeleteByIdAsync<TObject>(Guid id) where TObject : IEntity
        {
            var filterDefinition = Builders<TObject>.Filter.Eq(x => x.Id, id);
            await GetCollection<TObject>().DeleteOneAsync(filterDefinition);
        }

        public void Dispose() => Dispose(true);

        public async Task<IEnumerable<TObject>> FindBySpecificationAsync<TObject>(Expression<Func<TObject, bool>> expr) where TObject : IEntity
            => await(await GetCollection<TObject>().FindAsync(expr)).ToListAsync();

        public async Task<IEnumerable<TObject>> GetAllAsync<TObject>() where TObject : IEntity
            => await FindBySpecificationAsync<TObject>(_ => true);

        public async Task<TObject> GetByIdAsync<TObject>(Guid id) where TObject : IEntity
            => (await FindBySpecificationAsync<TObject>(x => x.Id.Equals(id))).FirstOrDefault();

        public async Task UpdateByIdAsync<TObject>(Guid id, TObject entity)
             where TObject : IEntity
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

        private IMongoCollection<TObject> GetCollection<TObject>() where TObject : IEntity => _database.GetCollection<TObject>(NormalizedCollectionName<TObject>());

        private string NormalizedCollectionName<TObject>() where TObject : IEntity
        {
            if (_normalizedCollectionNames.ContainsKey(typeof(TObject)))
            {
                return _normalizedCollectionNames[typeof(TObject)];
            }
            else
            {
                string name;
                if (typeof(TObject).IsDefined(typeof(StorageModelAttribute), false))
                {
                    var storageModelAttribute = typeof(TObject).GetCustomAttribute<StorageModelAttribute>();
                    name = storageModelAttribute.TableName;
                }
                else
                {
                    if (typeof(TObject).IsInterface && typeof(TObject).Name.StartsWith("I"))
                    {
                        return typeof(TObject).Name.Substring(1).Pluralize();
                    }

                    name = typeof(TObject).Name.Pluralize();
                }

                _normalizedCollectionNames[typeof(TObject)] = name;

                return name;
            }
        }
    }
}
