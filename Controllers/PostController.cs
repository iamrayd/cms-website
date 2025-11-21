using Microsoft.AspNetCore.Mvc;
using ProjectCms.Models;
using ProjectCms.Services;

namespace ProjectCms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]          // → /api/Posts
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        // 🔹 GIPANGALANAN: GetAll para di libog
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAll() =>
            Ok(await _postService.GetAsync());

        // 🔹 GIPANGALANAN: GetById para klaro, same route template
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Post>> GetById(string id)
        {
            var post = await _postService.GetAsync(id);
            return post is null ? NotFound() : Ok(post);
        }

        // 🔹 GI-ADD [FromBody] (di required pero mas klaro)
        // 🔹 GI-USAB name sa action: Create
        // 🔹 GI-AYO CreatedAtAction → mo-point na sa GetById()
        [HttpPost]
        public async Task<ActionResult<Post>> Create([FromBody] Post newPost)
        {
            if (string.IsNullOrWhiteSpace(newPost.Title))
                return BadRequest("Title is required.");

            await _postService.CreateAsync(newPost);

            return CreatedAtAction(
                nameof(GetById),          // <── HIGHLIGHT: kani na ang gi gamit
                new { id = newPost.Id },  //         para sa /api/Posts/{id}
                newPost);
        }

        // 🔹 GI-USAB name sa action: Update
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Post updatedPost)
        {
            var post = await _postService.GetAsync(id);
            if (post is null) return NotFound();

            updatedPost.Id = id;
            await _postService.UpdateAsync(id, updatedPost);
            return NoContent();
        }

        // 🔹 GI-USAB name sa action: Delete
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var post = await _postService.GetAsync(id);
            if (post is null) return NotFound();

            await _postService.RemoveAsync(id);
            return NoContent();
        }
    }
}
