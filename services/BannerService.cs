using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProjectCms.Models;

namespace ProjectCms.Services
{
    public class BannerService
    {
        private readonly IMongoCollection<Banner> _banners;

        public BannerService(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);

            _banners = database.GetCollection<Banner>("Banners");
        }

        public async Task<List<Banner>> GetAsync() =>
            await _banners.Find(_ => true).ToListAsync();

        public async Task<Banner?> GetAsync(string id) =>
            await _banners.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Banner banner) =>
            await _banners.InsertOneAsync(banner);

        public async Task UpdateAsync(string id, Banner updated) =>
            await _banners.ReplaceOneAsync(x => x.Id == id, updated);

        public async Task RemoveAsync(string id) =>
            await _banners.DeleteOneAsync(x => x.Id == id);
    }
}
