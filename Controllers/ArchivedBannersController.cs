using Microsoft.AspNetCore.Mvc;
using ProjectCms.Models;
using ProjectCms.Services;

namespace ProjectCms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchivedBannersController : ControllerBase
    {
        private readonly ArchivedBannerService _archivedBannerService;

        public ArchivedBannersController(ArchivedBannerService archivedBannerService)
        {
            _archivedBannerService = archivedBannerService;
        }

        // GET: api/ArchivedBanners
        [HttpGet]
        public async Task<ActionResult<List<ArchivedBanner>>> Get()
        {
            var items = await _archivedBannerService.GetAsync();
            return Ok(items);
        }

        // GET: api/ArchivedBanners/count
        [HttpGet("count")]
        public async Task<ActionResult<long>> Count()
        {
            var count = await _archivedBannerService.CountAsync();
            return Ok(count);
        }
    }
}
