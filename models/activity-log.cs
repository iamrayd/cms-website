using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectCms.Api.Models;

public class ActivityLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [BsonElement("userName")]
    public string UserName { get; set; } = string.Empty; // e.g. "Jane Doe"

    [BsonElement("action")]
    public string Action { get; set; } = string.Empty;   // e.g. "Updated", "Published", "Archived"

    [BsonElement("contentType")]
    public string ContentType { get; set; } = string.Empty;
    // "page" | "post" | "banner" | "popup" | "system"

    [BsonElement("contentTitle")]
    public string ContentTitle { get; set; } = string.Empty;
    // e.g. Page - "About Us"

    [BsonElement("contentId")]
    public string ContentId { get; set; } = string.Empty; // eg "page_001" or Mongo Id

    [BsonElement("status")]
    public string Status { get; set; } = "Success";       // "Success", "Failed", "Complete"
}
