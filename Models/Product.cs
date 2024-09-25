using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace web_service.Models
{
   public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ProductId { get; set; }

    [Required(ErrorMessage = "ProductName is required.")]
    public string ProductName { get; set; }

    public string ProductDescription { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }

    // Image stored as a Base64 string
    public string Image { get; set; }

    [Required(ErrorMessage = "CategoryId is required.")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; }

    [Required(ErrorMessage = "VendorId is required.")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string VendorId { get; set; }
}
}
