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

        public async Task<List<User>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<User> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(User user) =>
            await _collection.InsertOneAsync(user);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task Update(User updated_user) =>
            await _collection.ReplaceOneAsync(x => x.Id == updated_user.Id, updated_user);
    }
}