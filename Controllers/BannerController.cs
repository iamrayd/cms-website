using Microsoft.AspNetCore.Mvc;
using ProjectCms.Models;
using ProjectCms.Services;

namespace ProjectCms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannersController : ControllerBase
    {
        private readonly BannerService _bannerService;

        public BannersController(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Banner>>> Get() =>
            Ok(await _bannerService.GetAsync());

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
            return CreatedAtAction(nameof(Get), new { id = newBanner.Id }, newBanner);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Banner updated)
        {
            var existing = await _bannerService.GetAsync(id);
            if (existing is null) return NotFound();

            updated.Id = id;
            await _bannerService.UpdateAsync(id, updated);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _bannerService.GetAsync(id);
            if (existing is null) return NotFound();

            await _bannerService.RemoveAsync(id);
            return NoContent();
        }
    }
}
