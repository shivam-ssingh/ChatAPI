using ChatAPI.Options;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChatAPI.Data
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;
        private readonly DatabaseOptions _dbOptions;

        public MongoDBService(IOptions<DatabaseOptions> dbOptions)
        {
            _dbOptions = dbOptions.Value;
            var client = new MongoClient(_dbOptions.ConnectionString);
            _database = client.GetDatabase(_dbOptions.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public async Task CreateDocument<T>(string collectionName, T document)
        {
            var databaseCollection = _database.GetCollection<T>(collectionName);
            await databaseCollection.InsertOneAsync(document);
        }

        public async Task<List<T>> GetAllDocument<T>(string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return (List<T>)await collection.FindAsync(doc => true);
        }

        public async Task<T> GetDocumentById<T>(string collectionName, string identifier, string id)
        {
            var collection = _database.GetCollection<T>(collectionName);
            if (identifier == "_id")
            {
                var filter = Builders<T>.Filter.Eq(identifier, new ObjectId(id));
                return await collection.Find(filter).FirstOrDefaultAsync();
            }
            else
            {
                var filter = Builders<T>.Filter.Eq(identifier, id);
                return await collection.Find(filter).FirstOrDefaultAsync();
            }
        }

    }
}
