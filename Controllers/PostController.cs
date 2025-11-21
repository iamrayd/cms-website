using Microsoft.AspNetCore.Mvc;
using ProjectCms.Models;
using ProjectCms.Services;
using ProjectCms.Api.Services;            // ⭐ NEW: para ma gamit ang IActivityLogService

namespace ProjectCms.Controllers
{
    [ApiController]
    [Route("api/Posts")]                  // ⭐ EXPLICIT: /api/Posts
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly IActivityLogService _activityLogService;   // ⭐ NEW

        // ⭐ UPDATED: gi-inject nato ang IActivityLogService
        public PostsController(PostService postService, IActivityLogService activityLogService)
        {
            _postService = postService;
            _activityLogService = activityLogService;
        }

        // GET /api/Posts
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAll()
        {
            var posts = await _postService.GetAsync();
            return Ok(posts);
        }

        // ⭐ NEW: COUNT POSTS
        // GET /api/Posts/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            var posts = await _postService.GetAsync();
            return Ok(posts.Count);
        }

        // GET /api/Posts/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Post>> GetById(string id)
        {
            var post = await _postService.GetAsync(id);
            return post is null ? NotFound() : Ok(post);
        }

        // POST /api/Posts
        [HttpPost]
        public async Task<ActionResult<Post>> Create([FromBody] Post newPost)
        {
            if (string.IsNullOrWhiteSpace(newPost.Title))
                return BadRequest("Title is required.");

            await _postService.CreateAsync(newPost);

            // ⭐ NEW: LOG – New post created
            await _activityLogService.LogAsync(
                userName: "Admin",               // pwede nato ilisan og real user later
                action: "Created Post",
                contentType: "post",
                contentTitle: newPost.Title,
                contentId: newPost.Id ?? string.Empty,
                status: "Success"
            );

            return CreatedAtAction(
                nameof(GetById),
                new { id = newPost.Id },
                newPost
            );
        }

        // PUT /api/Posts/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Post updatedPost)
        {
            var existing = await _postService.GetAsync(id);
            if (existing is null) return NotFound();

            updatedPost.Id = id;
            await _postService.UpdateAsync(id, updatedPost);

            // ⭐ NEW: LOG – Post updated
            await _activityLogService.LogAsync(
                userName: "Admin",
                action: "Updated Post",
                contentType: "post",
                contentTitle: updatedPost.Title,
                contentId: id,
                status: "Success"
            );

            return NoContent();
        }

        // DELETE /api/Posts/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _postService.GetAsync(id);
            if (existing is null) return NotFound();

            await _postService.RemoveAsync(id);

            // ⭐ NEW: LOG – Post deleted
            await _activityLogService.LogAsync(
                userName: "Admin",
                action: "Deleted Post",
                contentType: "post",
                contentTitle: existing.Title,
                contentId: id,
                status: "Success"
            );

            return NoContent();
        }
    }
}
