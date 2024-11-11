using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace backendassign2.Controllers;
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TripController : ControllerBase
{
    
    private readonly dbcontext _context;
    private readonly ILogger<TripController> _logger;
    private readonly MongoLogService _mongoLogService;
    
    public TripController(dbcontext context, ILogger<TripController> logger, MongoLogService mongoLogService)
    {
        _context = context;
        _logger = logger;
        _mongoLogService = mongoLogService;
    }   
    private string GetUserName()
    {
        return User?.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";
    }
   
    [HttpGet("GetTripDetails/{tripId}")]
    public async Task<IEnumerable<ServiceDto.TripDto>> GetTripDetails(int tripId)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return await CookService.GetTripDetailsAsync(tripId, _context);
    }
    
    
    
    [HttpGet("GetCyclistEarningsAsync/{cyclistId}")]
    public async Task<dynamic> GetCyclistEarningsAsync(int cyclistId)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return await CookService.GetCyclistEarningsAsync(cyclistId, _context);
    }
    
    [HttpGet("GetAverageRatingForCyclist/{cyclistId}")]
    public async Task<dynamic> GetAverageRatingCyclist(int cyclistId)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return await CookService.GetAverageRatingForDriversAsync(cyclistId, _context);
    }
}