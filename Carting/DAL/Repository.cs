using Carting.BLL.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Carting.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<TEntity>(databaseSettings.Value.CollectionName);
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

        public TEntity GetDocumentById(string code)
        {
           return  _collection.Find(x => x.Code == code).FirstOrDefault();            
        }

        public bool DeleteDocument(string code)
        {
            _collection.DeleteOne(x => x.Code == code);
            return true;
        }  
    }
}
