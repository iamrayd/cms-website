using Microsoft.AspNetCore.Mvc;
using ProjectCms.Models;
using ProjectCms.Services;
using ProjectCms.Api.Services;   // ⭐ NEW

namespace ProjectCms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannersController : ControllerBase
    {
        private readonly BannerService _bannerService;
        private readonly IActivityLogService _activityLogService;  // ⭐ NEW

        // ⭐ UPDATED: inject IActivityLogService
        public BannersController(BannerService bannerService, IActivityLogService activityLogService)
        {
            _bannerService = bannerService;
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Banner>>> Get() =>
            Ok(await _bannerService.GetAsync());

        // ⭐ NEW: COUNT BANNERS
        // GET: /api/Banners/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            var banners = await _bannerService.GetAsync();
            return Ok(banners.Count);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Banner>> Get(string id)
        {
            var banner = await _bannerService.GetAsync(id);
            return banner is null ? NotFound() : Ok(banner);
        }

        [HttpPost]
        public async Task<ActionResult<Banner>> Post(Banner newBanner)
        {
            await _bannerService.CreateAsync(newBanner);

            // ⭐ NEW: log create
            await _activityLogService.LogAsync(
                userName: "Admin",              // in future: actual logged-in user
                action: "Created Banner",
                contentType: "banner",
                contentTitle: newBanner.Title,
                contentId: newBanner.Id ?? string.Empty,
                status: "Success"
            );

            return CreatedAtAction(nameof(Get), new { id = newBanner.Id }, newBanner);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Banner updated)
        {
            var existing = await _bannerService.GetAsync(id);
            if (existing is null) return NotFound();

            updated.Id = id;
            await _bannerService.UpdateAsync(id, updated);

            // ⭐ NEW: log update
            await _activityLogService.LogAsync(
                userName: "Admin",
                action: "Updated Banner",
                contentType: "banner",
                contentTitle: updated.Title,
                contentId: id,
                status: "Success"
            );

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _bannerService.GetAsync(id);
            if (existing is null) return NotFound();

            await _bannerService.RemoveAsync(id);

            // ⭐ NEW: log delete
            await _activityLogService.LogAsync(
                userName: "Admin",
                action: "Deleted Banner",
                contentType: "banner",
                contentTitle: existing.Title,
                contentId: id,
                status: "Success"
            );

            return NoContent();
        }
    }
}
