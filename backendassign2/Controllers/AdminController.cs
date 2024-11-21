
using backendassign2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendassign2.Controllers;
[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly dbcontext _context;
    private readonly ILogger<AdminController> _logger;
    private readonly MongoLogService _mongoLogService;
    private string GetUserName()
    {
        return User?.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";
    }
    
    public AdminController(dbcontext context, ILogger<AdminController> logger, MongoLogService mongoLogService)
    {
        _context = context;
        _logger = logger;
        _mongoLogService = mongoLogService;
    }
    
    [HttpGet("GetCyclistEarnings/{cyclistId}")]
    public async Task<dynamic> GetCyclistEarningsAsync(string cyclistId)
    {
        var earnings = await CookService.GetCyclistEarningsAsync(cyclistId, _context);
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return earnings;
    }
    [HttpGet("GetAverageRatingForCyclist/{cyclistId}")]
    public async Task<dynamic> GetAverageRatingCyclist(string cyclistId)
    {
        // Log extracted NameIdentifier for debugging

        // Pass the NameIdentifier to the service method
        var rating = await CookService.GetAverageRatingForDriversAsync(cyclistId, _context);
        Console.WriteLine(rating);
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return rating;
    }
    
    [HttpGet("GetAverageRatingForCook/{cookId}")]
    public async Task<double?> GetAverageRatingForCookAsync(string cookId)
    {
        // Extract NameIdentifier from claims
        
        // Log extracted NameIdentifier for debugging
        
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);

        // Pass the NameIdentifier to the service method
        var averageRating = await CookService.GetAverageRatingForCookAsync(cookId, _context);

        // Log the result

        return averageRating;
    }
}