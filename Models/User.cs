using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace web_service.Models
{
    public class User {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "username is required.")]
        public string Username { get; set; } = null!;
        
        [Required(ErrorMessage = "phone number is required.")]
        public decimal PhoneNumber { get; set; }

        [Required(ErrorMessage = "email is required.")]
        public string Email { get; set; }= null!;

        [Required(ErrorMessage = "address is required.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "role is required.")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "password is required.")]
        public string Password { get; set; } = null!;

        // Optional field
        public string? Ratings { get; set; }

         // Rating stats for vendors
        public double AverageRating { get; set; } = 0.0; // Average rating
        public int NumberOfReviews { get; set; } = 0;    // Total number of reviews

        // New Status field with Enum
        [BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "status is required.")]
        public Status Status { get; set; } = Status.Pending;  // Default to Pending
    }

    // Define the Status Enum
    public enum Status
    {
        Accept,
        Reject,
        Hold,
        Pending
    }
}
