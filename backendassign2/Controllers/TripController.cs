using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace backendassign2.Controllers;
[ApiController]
[Authorize(Roles = "Cyclist")]
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
    
    
    
    [HttpGet("GetCyclistEarnings")]
    public async Task<dynamic> GetCyclistEarningsAsync()
    {
        string nameIdentifier = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        // Log extracted NameIdentifier for debugging
        if (string.IsNullOrEmpty(nameIdentifier))
        {
            _logger.LogError("NameIdentifier is null or empty. Ensure the claim exists and is configured correctly.");
            return null; // Or handle the error as appropriate
        }

        // Pass the NameIdentifier to the service method
        var earnings = await CookService.GetCyclistEarningsAsync(nameIdentifier, _context);
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
    
    [HttpGet("GetAverageRatingForCyclist")]
    public async Task<dynamic> GetAverageRatingCyclist()
    {
        string nameIdentifier = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        // Log extracted NameIdentifier for debugging
        if (string.IsNullOrEmpty(nameIdentifier))
        {
            _logger.LogError("NameIdentifier is null or empty. Ensure the claim exists and is configured correctly.");
            return null; // Or handle the error as appropriate
        }

        // Pass the NameIdentifier to the service method
        var rating = await CookService.GetAverageRatingForDriversAsync(nameIdentifier, _context);
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
}