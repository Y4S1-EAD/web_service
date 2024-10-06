using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace web_service.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CartId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [Required(ErrorMessage = "At least one ProductId is required.")]
        [MinLength(1, ErrorMessage = "At least one ProductId must be provided.")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> ProductIds { get; set; } = new List<string>();
    }
}
