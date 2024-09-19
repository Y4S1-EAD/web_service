using web_service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace web_service.Services
{
    public class NotificationService
    {
        private readonly IMongoCollection<Notification> _notificationsCollection;

        public NotificationService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            // Assuming you have a collection named "Notifications"
            _notificationsCollection = database.GetCollection<Notification>("Notifications");
        }

        // Get all notifications
        public async Task<List<Notification>> GetAsync()
        {
            return await _notificationsCollection.Find(_ => true).ToListAsync();
        }

        // Get a notification by ID
        public async Task<Notification> GetAsync(string id)
        {
            return await _notificationsCollection.Find(x => x.NotificationId == id).FirstOrDefaultAsync();
        }

        // Create a new notification
        public async Task CreateAsync(Notification notification)
        {
            await _notificationsCollection.InsertOneAsync(notification);
        }

        // Update an existing notification
        public async Task UpdateAsync(string id, Notification updatedNotification)
        {
            await _notificationsCollection.ReplaceOneAsync(x => x.NotificationId == id, updatedNotification);
        }

        // Remove a notification by ID
        public async Task RemoveAsync(string id)
        {
            await _notificationsCollection.DeleteOneAsync(x => x.NotificationId == id);
        }
    }
}
