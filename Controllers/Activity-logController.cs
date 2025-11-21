using Microsoft.AspNetCore.Mvc;
using ProjectCms.Api.Models;
using ProjectCms.Api.Services;

namespace ProjectCms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityLogsController : ControllerBase
{
    private readonly IActivityLogService _activityLogService;

    public ActivityLogsController(IActivityLogService activityLogService)
    {
        _activityLogService = activityLogService;
    }

    // GET: /api/ActivityLogs?take=20&contentType=page
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityLog>>> Get(
        [FromQuery] int take = 50,
        [FromQuery] string? contentType = null)
    {
        var logs = await _activityLogService.GetLatestAsync(take, contentType);
        return Ok(logs);
    }
}
