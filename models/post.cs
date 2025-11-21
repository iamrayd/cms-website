using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectCms.Models
{
    public class Post
    {
        // 👇 IMPORTANT PART: mao ni ang gi-fix
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("slug")]
        public string Slug { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("content")]
        public string Content { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "draft"; // "draft" | "published"

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("publishedAt")]
        public DateTime? PublishedAt { get; set; }
    }
}
