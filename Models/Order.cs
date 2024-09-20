using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace web_service.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [Required(ErrorMessage = "OrderDate is required.")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // OrderDescription is optional
        public string OrderDescription { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "DeliveryMethod is required.")]
        public string DeliveryMethod { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(cancelled|delivered|processing)$", ErrorMessage = "Status must be 'cancelled', 'delivered', or 'processing'.")]
        public string Status { get; set; } // Possible values: cancelled, delivered, processing

        [Required(ErrorMessage = "At least one ProductId is required.")]
        [MinLength(1, ErrorMessage = "At least one ProductId must be provided.")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> ProductIds { get; set; } = new List<string>();

        [Required(ErrorMessage = "PaymentId is required.")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PaymentId { get; set; }
    }
}
