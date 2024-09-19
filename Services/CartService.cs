using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace web_service.Services
{
    public class CartService
    {
        private readonly IMongoCollection<Cart> _cartsCollection;

        public CartService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            // Assuming you have a collection named "Carts"
            _cartsCollection = database.GetCollection<Cart>("Carts");
        }

        // Get all carts
        public async Task<List<Cart>> GetAsync()
        {
            return await _cartsCollection.Find(_ => true).ToListAsync();
        }

        // Get a cart by ID
        public async Task<Cart> GetAsync(string id)
        {
            return await _cartsCollection.Find(x => x.CartId == id).FirstOrDefaultAsync();
        }

        // Create a new cart
        public async Task CreateAsync(Cart cart)
        {
            await _cartsCollection.InsertOneAsync(cart);
        }

        // Update an existing cart
        public async Task UpdateAsync(string id, Cart updatedCart)
        {
            await _cartsCollection.ReplaceOneAsync(x => x.CartId == id, updatedCart);
        }

        // Remove a cart by ID
        public async Task RemoveAsync(string id)
        {
            await _cartsCollection.DeleteOneAsync(x => x.CartId == id);
        }
    }
}
