using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace web_service.Models
{
    public class Notification {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? NotificationId { get; set; }
    
    [Required(ErrorMessage = "date/time is required.")]
    public DateTime DateTime { get; set; }

    [Required(ErrorMessage = "title is required.")]
    public string Title { get; set; }= null!;

    [Required(ErrorMessage = "message is required.")]
    public string Message { get; set; } = null!;

    public string? Role { get; set; }
    public string? UserId { get; set; }
    
}
}