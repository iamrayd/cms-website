using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectCms.Api.Models;


// or ProjectCms.Api.Models, depende unsay naa nimo

using ProjectCms.Api.Services;
using ProjectCms.Models;      // MongoDbSettings

namespace project_cms.services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IMongoCollection<ActivityLog> _collection;

        public ActivityLogService(IOptions<MongoDbSettings> mongoSettings)
        {
            // same pattern as PageService/PostService
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _collection = database.GetCollection<ActivityLog>("ActivityLogs");
        }

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
