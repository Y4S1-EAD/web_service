using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.JsonPatch;

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

        // Get a user by username (used in the login process)
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();
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

        // Partially update a user
        public async Task<bool> UpdatePartialAsync(string id, JsonPatchDocument<User> patchDocument)
        {
            var user = await GetAsync(id);

            if (user == null)
            {
                return false;
            }

            // Apply the patch to the user object
            patchDocument.ApplyTo(user);

            // Replace the updated user in the database
            await _usersCollection.ReplaceOneAsync(x => x.UserId == id, user);
            
            return true;
        }

        // Remove a user by ID
        public async Task RemoveAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(x => x.UserId == id);
        }
    }
}
