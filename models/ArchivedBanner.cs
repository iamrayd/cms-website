using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectCms.Models
{
    public class ArchivedBanner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("link")]
        public string Link { get; set; } = string.Empty;

        [BsonElement("publishAt")]
        public DateTime PublishAt { get; set; }

        [BsonElement("expireAt")]
        public DateTime ExpireAt { get; set; }

        [BsonElement("content")]
        public string Content { get; set; } = string.Empty;
    }
}
