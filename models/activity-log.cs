using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectCms.Api.Models
{
    public class ActivityLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [BsonElement("userName")]
        public string UserName { get; set; } = string.Empty;

        [BsonElement("action")]
        public string Action { get; set; } = string.Empty;

        [BsonElement("contentType")]
        public string ContentType { get; set; } = string.Empty;

        [BsonElement("contentTitle")]
        public string ContentTitle { get; set; } = string.Empty;

        [BsonElement("contentId")]
        public string ContentId { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "Success";
    }
}
