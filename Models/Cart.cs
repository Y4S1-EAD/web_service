using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace web_service.Models
{
    public class Cart {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CartId { get; set; }

    [Required(ErrorMessage = "userId is required.")]
    public string UserId { get; set; } = null!;
    
    [Required(ErrorMessage = "productId is required.")]
    public string ProdcutId { get; set; } = null!;
    
}
}