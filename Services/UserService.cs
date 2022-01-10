using MongoDB.Driver;
using Microsoft.Extensions.Options;
using todos.Models;

namespace todos.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _collection;

        public UserService(IOptions<UserDBSettings> settings)
        {
            var mongoClient = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<User>(
                settings.Value.CollectionName);
        }

        public IMongoCollection<User> getCollection() =>
            _collection;
    }
}