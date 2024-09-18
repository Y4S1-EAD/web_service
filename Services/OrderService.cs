using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace web_service.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _ordersCollection = database.GetCollection<Order>("Orders");
        }

        public async Task<List<Order>> GetAsync()
        {
            return await _ordersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Order> GetAsync(string id)
        {
            return await _ordersCollection.Find(x => x.OrderId == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Order order)
        {
            await _ordersCollection.InsertOneAsync(order);
        }


        public async Task UpdateAsync(string id, Order updatedOrder)
        {
            await _ordersCollection.ReplaceOneAsync(x => x.OrderId == id, updatedOrder);
        }

        public async Task RemoveAsync(string id)
        {
            await _ordersCollection.DeleteOneAsync(x => x.OrderId == id);
        }
    }
}
