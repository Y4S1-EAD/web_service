using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_service.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categoriesCollection;

        public CategoryService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _categoriesCollection = database.GetCollection<Category>("Categories");
        }

        public async Task<List<Category>> GetAsync()
        {
            return await _categoriesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Category> GetAsync(string id)
        {
            return await _categoriesCollection.Find(x => x.CategoryId == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Category category)
        {
            await _categoriesCollection.InsertOneAsync(category);
        }

        public async Task UpdateAsync(string id, Category updatedCategory)
        {
            await _categoriesCollection.ReplaceOneAsync(x => x.CategoryId == id, updatedCategory);
        }
        
        public async Task RemoveAsync(string id)
        {
            await _categoriesCollection.DeleteOneAsync(x => x.CategoryId == id);
        }
    }
}
