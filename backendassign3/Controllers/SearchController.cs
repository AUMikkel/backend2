using backendassign3.DTOs;
using backendassign3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendassign3.Controllers;
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
    [Authorize(Roles = "Admin")]
    [HttpGet("SearchLogs")]
    public async Task<IEnumerable<ServiceDto.LogDto>> SearchLogs(
        DateTime? startTime = null, 
        DateTime? endTime = null, 
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

        // max and min if not specified and convert to UTC.
        DateTime utcStartTime;
        DateTime utcEndTime;
        if (startTime.HasValue)
            utcStartTime = DateTime.SpecifyKind(startTime.Value, DateTimeKind.Utc);
        else
            utcStartTime = DateTime.MinValue;
        
        if (endTime.HasValue)
            utcEndTime = DateTime.SpecifyKind(endTime.Value, DateTimeKind.Utc);
        else
            utcEndTime = DateTime.MaxValue;
        Console.WriteLine("Before LogInformation");
        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        Console.WriteLine("After LogInformation");
        // Fetch logs with the specified criteria
        var logs = await _mongoLogService.GetLogsAsync(utcStartTime, utcEndTime, user, operation);
        Console.WriteLine("Logs fetched");
        return logs;
    }
}