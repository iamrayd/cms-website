using ProjectCms.Api.Models; //Interface 

namespace ProjectCms.Api.Services;

public interface IActivityLogService
{
    Task LogAsync(
        string userName,
        string action,
        string contentType,
        string contentTitle,
        string contentId,
        string status = "Success");

    Task<List<ActivityLog>> GetLatestAsync(int take = 50, string? contentType = null);
}
