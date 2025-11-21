using Microsoft.AspNetCore.Mvc;
using ProjectCms.Models;
using ProjectCms.Services;

namespace ProjectCms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagesController : ControllerBase
    {
        private readonly PageService _pageService;

        public PagesController(PageService pageService)
        {
            _pageService = pageService;
        }

        // ----------------------------
        // HEALTH CHECK (MongoDB Test)
        // ----------------------------
        // GET: /api/Pages/health
        [HttpGet("health")]
        public async Task<IActionResult> Health()
        {
            try
            {
                // Just try to read something from MongoDB
                await _pageService.GetAsync();
                return Ok(new { status = "ok", database = "connected" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        // ----------------------------
        // COUNT PAGES
        // GET: /api/Pages/count
        // ----------------------------
        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            var pages = await _pageService.GetAsync();
            return Ok(pages.Count);
        }

        // ----------------------------
        // GET ALL PAGES
        // GET: /api/pages
        // ----------------------------
        [HttpGet]
        public async Task<ActionResult<List<Page>>> Get()
        {
            var pages = await _pageService.GetAsync();
            return Ok(pages);
        }

        // ----------------------------
        // GET ONE PAGE BY ID
        // GET: /api/pages/{id}
        // ----------------------------
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Page>> Get(string id)
        {
            var page = await _pageService.GetAsync(id);

            if (page is null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        // ----------------------------
        // CREATE NEW PAGE
        // POST: /api/pages
        // ----------------------------
        [HttpPost]
        public async Task<ActionResult<Page>> Post(Page newPage)
        {
            await _pageService.CreateAsync(newPage);
            return CreatedAtAction(nameof(Get), new { id = newPage.Id }, newPage);
        }

        // ----------------------------
        // UPDATE PAGE
        // PUT: /api/pages/{id}
        // ----------------------------
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Page updatedPage)
        {
            var page = await _pageService.GetAsync(id);

            if (page is null)
            {
                return NotFound();
            }

            updatedPage.Id = page.Id;

            await _pageService.UpdateAsync(id, updatedPage);

            return NoContent();
        }

        // ----------------------------
        // DELETE PAGE
        // DELETE: /api/pages/{id}
        // ----------------------------
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var page = await _pageService.GetAsync(id);

            if (page is null)
            {
                return NotFound();
            }

            await _pageService.RemoveAsync(id);

            return NoContent();
        }
    }
}
