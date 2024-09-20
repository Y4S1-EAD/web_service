using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_service.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _productsCollection = database.GetCollection<Product>("Products");
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _productsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetAsync(string id)
        {
            return await _productsCollection.Find(x => x.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Product product)
        {
            await _productsCollection.InsertOneAsync(product);
        }

        public async Task UpdateAsync(string id, Product updatedProduct)
        {
            await _productsCollection.ReplaceOneAsync(x => x.ProductId == id, updatedProduct);
        }

        public async Task RemoveAsync(string id)
        {
            await _productsCollection.DeleteOneAsync(x => x.ProductId == id);
        }

        public async Task<List<Product>> GetByCategoryIdAsync(string categoryId)
        {
            return await _productsCollection.Find(x => x.CategoryId == categoryId).ToListAsync();
        }
    }
}
