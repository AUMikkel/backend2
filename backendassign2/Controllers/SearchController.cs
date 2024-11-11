using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendassign2.Controllers;
[ApiController]
[Authorize]
[Route("api/")]
public class SearchController : ControllerBase
{
    private readonly dbcontext _context;
    private readonly ILogger<SearchController> _logger;
    private readonly MongoLogService _mongoLogService;
    
    public SearchController(dbcontext context, ILogger<SearchController> logger, MongoLogService mongoLogService)
    {
        _context = context;
        _logger = logger;
        _mongoLogService = mongoLogService;
    }   
    private string GetUserName()
    {
        return User?.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";
    }
    
    [HttpGet("SearchLogs")]
    public async Task<IEnumerable<ServiceDto.LogDto>> SearchLogs(
        DateTime startTime, 
        DateTime endTime, 
        string? user = null, 
        string? operation = null)
    {    
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };
    
        // Ensure startTime and endTime are treated as UTC
        var utcStartTime = DateTime.SpecifyKind(startTime, DateTimeKind.Utc);
        var utcEndTime = DateTime.SpecifyKind(endTime, DateTimeKind.Utc);

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);

        // Fetch logs with the specified criteria
        var logs = await _mongoLogService.GetLogsAsync(utcStartTime, utcEndTime, user, operation);
        return logs;
    }
}