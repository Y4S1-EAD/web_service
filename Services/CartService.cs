using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_service.Services
{
    public class CartService
    {
        private readonly IMongoCollection<Cart> _cartsCollection;

        public CartService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _cartsCollection = database.GetCollection<Cart>("Carts");
        }

        public async Task<List<Cart>> GetAsync()
        {
            return await _cartsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Cart> GetAsync(string id)
        {
            return await _cartsCollection.Find(x => x.CartId == id).FirstOrDefaultAsync();
        }

        public async Task<List<Cart>> GetByUserIdAsync(string userId)
        {
            return await _cartsCollection.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task CreateAsync(Cart cart)
        {
            await _cartsCollection.InsertOneAsync(cart);
        }

        public async Task UpdateAsync(string id, Cart updatedCart)
        {
            await _cartsCollection.ReplaceOneAsync(x => x.CartId == id, updatedCart);
        }

        public async Task RemoveAsync(string id)
        {
            await _cartsCollection.DeleteOneAsync(x => x.CartId == id);
        }
    }
}