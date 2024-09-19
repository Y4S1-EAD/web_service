using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace web_service.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            // Assuming you have a collection named "Users"
            _usersCollection = database.GetCollection<User>("Users");
        }

        // Get all users
        public async Task<List<User>> GetAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }

        // Get a user by ID
        public async Task<User> GetAsync(string id)
        {
            return await _usersCollection.Find(x => x.UserId == id).FirstOrDefaultAsync();
        }

        // Create a new user
        public async Task CreateAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        // Update an existing user
        public async Task UpdateAsync(string id, User updatedUser)
        {
            await _usersCollection.ReplaceOneAsync(x => x.UserId == id, updatedUser);
        }

        // Remove a user by ID
        public async Task RemoveAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(x => x.UserId == id);
        }
    }
}
