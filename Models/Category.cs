using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace web_service.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; } // The null-forgiving operator - Make CategoryId nullable

        [Required(ErrorMessage = "CategoryName is required.")]
        public string CategoryName { get; set; }

        // Description is optional
        public string CategoryDescription { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool IsActive { get; set; }
    }
}
