using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectCms.Models;

namespace ProjectCms.Services
{
    public class ArchivedBannerService
    {
        private readonly IMongoCollection<ArchivedBanner> _collection;

        public ArchivedBannerService(IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var db = client.GetDatabase(mongoSettings.Value.DatabaseName);

            // ⚠️ Name must match your collection: "ArchivedBanners"
            _collection = db.GetCollection<ArchivedBanner>("ArchivedBanners");
        }

        // ⭐ This is the method your controller is trying to call
        public async Task<List<ArchivedBanner>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        // ⭐ And this one too
        public async Task<long> CountAsync() =>
            await _collection.CountDocumentsAsync(_ => true);
    }
}
