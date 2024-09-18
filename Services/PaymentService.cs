using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace web_service.Services
{
    public class PaymentService
    {
        private readonly IMongoCollection<Payment> _paymentsCollection;

        public PaymentService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            // Assuming you have a collection named "Payments"
            _paymentsCollection = database.GetCollection<Payment>("Payments");
        }

        public async Task<List<Payment>> GetAsync()
        {
            return await _paymentsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Payment> GetAsync(string id)
        {
            return await _paymentsCollection.Find(x => x.PaymentId == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Payment payment)
        {
            await _paymentsCollection.InsertOneAsync(payment);
        }

        public async Task UpdateAsync(string id, Payment updatedPayment)
        {
            await _paymentsCollection.ReplaceOneAsync(x => x.PaymentId == id, updatedPayment);
        }

        public async Task RemoveAsync(string id)
        {
            await _paymentsCollection.DeleteOneAsync(x => x.PaymentId == id);
        }
    }
}
