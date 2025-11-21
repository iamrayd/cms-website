using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectCms.Models
{
    public class Banner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = "";

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } = "";

        [BsonElement("status")]
        public string Status { get; set; } = "draft";

        [BsonElement("link")]
        public string? Link { get; set; }

        // DATES: publish + expire
        [BsonElement("publishAt")]
        public DateTime? PublishAt { get; set; }

        [BsonElement("expireAt")]
        public DateTime? ExpireAt { get; set; }

        // ⭐ NEW: content/description of the banner ⭐
        [BsonElement("content")]
        public string Content { get; set; } = "";
    }
}
