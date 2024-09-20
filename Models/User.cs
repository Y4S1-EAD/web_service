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
    
     // This field is now optional (not required)
    public string? Ratings { get; set; }
    
}
}