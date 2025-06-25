using Carting.BLL.Interfaces;
using Carting.BLL.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Carting.DAL.Persistence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<TEntity> _collection;
        private IMongoCollection<Cart>? _newCollection;

        public ILogger<Repository<TEntity>> _logger { get; }

        public Repository(IOptions<DatabaseSettings> databaseSettings, ILogger<Repository<TEntity>> logger)
        {
            var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

            _database = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _collection = _database.GetCollection<TEntity>(databaseSettings.Value.CollectionName);
            _logger = logger;
        }

        public bool InsertDocument(TEntity item)
        {
            _collection.InsertOne(item);
            return true;
        }

        public bool UpdateDocument(TEntity item)
        {
            _collection.ReplaceOne(x => x.Code == item.Code, item);
            return true;
        }

        public TEntity GetDocumentById(string id) => _collection.Find(x => x.Code == id).FirstOrDefault();

        public bool DeleteDocument(string id)
        {
            _collection.DeleteOne(x => x.Code == id);
            return true;
        }

        public IList<Cart> GetDocumentsByItemId(string itemId)
        {
            try
            {
                var filter = Builders<Cart>.Filter.ElemMatch<Item>(
                    x => x.Items,
                    item => item.Id == int.Parse(itemId)
                );
                _newCollection = _database.GetCollection<Cart>("Cart");
                var documents = _newCollection.Find(filter).ToList();
                return documents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Array.Empty<Cart>();
            }
        }
    }
}
