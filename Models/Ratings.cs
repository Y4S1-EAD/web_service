using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace web_service.Models
{
    public class Ratings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? RatingId { get; set; } 

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; } 

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } 

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int RatingValue { get; set; }

        public string Comment { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DatePosted { get; set; } = DateTime.Now;
    }
}
