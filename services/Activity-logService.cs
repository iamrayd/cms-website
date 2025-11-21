using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectCms.Api.Models;
using ProjectCms.Models;

namespace ProjectCms.Api.Services
{
    public interface IActivityLogService
    {
        Task LogAsync(
            string userName,
            string action,
            string contentType,
            string contentTitle,
            string contentId,
            string status = "Success"
        );

        Task<List<ActivityLog>> GetLatestAsync(int take = 50, string? contentType = null);
    }

    public class ActivityLogService : IActivityLogService
    {
        private readonly IMongoCollection<ActivityLog> _collection;

        public ActivityLogService(IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var db = client.GetDatabase(mongoSettings.Value.DatabaseName);

            _collection = db.GetCollection<ActivityLog>("ActivityLogs");
        }

        // ⭐ FIXED: main logging function
        public async Task LogAsync(
            string userName,
            string action,
            string contentType,
            string contentTitle,
            string contentId,
            string status = "Success")
        {
            var log = new ActivityLog
            {
                Timestamp = DateTime.UtcNow,
                UserName = userName,
                Action = action,
                ContentType = contentType,
                ContentTitle = contentTitle,
                ContentId = contentId,
                Status = status
            };

            await _collection.InsertOneAsync(log);
        }

        // ⭐ FIXED: filtering + sorting
        public async Task<List<ActivityLog>> GetLatestAsync(int take = 50, string? contentType = null)
        {
            var filter = string.IsNullOrEmpty(contentType)
                ? Builders<ActivityLog>.Filter.Empty
                : Builders<ActivityLog>.Filter.Eq(x => x.ContentType, contentType);

            return await _collection
                .Find(filter)
                .SortByDescending(x => x.Timestamp)
                .Limit(take)
                .ToListAsync();
        }
    }
}
