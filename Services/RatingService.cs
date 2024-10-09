using MongoDB.Driver;
using web_service.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace web_service.Services
{
    public class RatingService
    {
        private readonly IMongoCollection<Ratings> _ratingsCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public RatingService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _ratingsCollection = database.GetCollection<Ratings>("Ratings");
            _usersCollection = database.GetCollection<User>("Users");
        }

        // Create a new rating and update the vendor's average rating
        public async Task AddRatingAsync(Ratings rating)
        {
            await _ratingsCollection.InsertOneAsync(rating);

            var vendor = await _usersCollection.Find(u => u.UserId == rating.VendorId && u.Role == "Vendor").FirstOrDefaultAsync();

            if (vendor != null)
            {
                var allRatings = await _ratingsCollection.Find(r => r.VendorId == rating.VendorId).ToListAsync();

                // Update the vendor's average rating and review count
                vendor.AverageRating = allRatings.Average(r => r.RatingValue);
                vendor.NumberOfReviews = allRatings.Count;

                await _usersCollection.ReplaceOneAsync(u => u.UserId == vendor.UserId, vendor);
            }
        }

        // Get all ratings for a specific vendor
        public async Task<List<Ratings>> GetRatingsByVendorAsync(string vendorId)
        {
            return await _ratingsCollection.Find(r => r.VendorId == vendorId).ToListAsync();
        }

        // Get rating summary for a vendor (average rating, total reviews, star distribution)
        public async Task<VendorRatingSummary> GetVendorRatingSummaryAsync(string vendorId)
        {
            var ratings = await _ratingsCollection.Find(r => r.VendorId == vendorId).ToListAsync();

            if (ratings == null || !ratings.Any())
            {
                return new VendorRatingSummary
                {
                    AverageRating = 0,
                    TotalReviews = 0,
                    StarDistribution = new Dictionary<int, int> { { 5, 0 }, { 4, 0 }, { 3, 0 }, { 2, 0 }, { 1, 0 } }
                };
            }

            // Calculate average rating
            double averageRating = ratings.Average(r => r.RatingValue);

            // Count total reviews
            int totalReviews = ratings.Count;

            // Calculate star distribution
            var starDistribution = ratings.GroupBy(r => r.RatingValue)
                                          .ToDictionary(g => g.Key, g => g.Count());

            // Fill missing star ratings with 0
            for (int i = 1; i <= 5; i++)
            {
                if (!starDistribution.ContainsKey(i))
                {
                    starDistribution[i] = 0;
                }
            }

            return new VendorRatingSummary
            {
                AverageRating = averageRating,
                TotalReviews = totalReviews,
                StarDistribution = starDistribution
            };
        }

        // Get all ratings for a specific user
        public async Task<List<Ratings>> GetRatingsByUserAsync(string userId)
        {
            return await _ratingsCollection.Find(r => r.UserId == userId).ToListAsync();
        }

        // Get a specific rating by ID (new method to fix the issue)
        public async Task<Ratings> GetRatingByIdAsync(string ratingId)
        {
            return await _ratingsCollection.Find(r => r.RatingId == ratingId).FirstOrDefaultAsync();
        }

        // Update a specific rating
        public async Task<bool> UpdateRatingAsync(string ratingId, Ratings updatedRating)
        {
            var filter = Builders<Ratings>.Filter.Eq(r => r.RatingId, ratingId);
            var update = Builders<Ratings>.Update
                .Set(r => r.RatingValue, updatedRating.RatingValue)
                .Set(r => r.Comment, updatedRating.Comment)
                .Set(r => r.DatePosted, DateTime.Now); // Optional: Update the date to the current time

            var result = await _ratingsCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }

    public class VendorRatingSummary
    {
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> StarDistribution { get; set; } // Key: Rating value (1-5), Value: Count
    }
}
