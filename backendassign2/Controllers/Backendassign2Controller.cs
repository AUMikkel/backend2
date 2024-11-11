using backendassign2.Entities;
using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;


namespace backendassign2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuController : ControllerBase
{
    private readonly dbcontext _context;
    private readonly ILogger<MenuController> _logger;
    private readonly MongoLogService _mongoLogService;
    public MenuController(dbcontext context, ILogger<MenuController> logger, MongoLogService mongoLogService)
    {
        _context = context;
        _logger = logger;
        _mongoLogService = mongoLogService;
    }   
    private string GetUserName()
    {
        return User?.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";
    }
   
    [Authorize("ManagerAccess")]
    [HttpGet("GetCooks")]
    public async Task<IEnumerable<ServiceDto.CookDto>> Get(string name)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return await CookService.GetCooks(name, _context);
    }
    
    [AllowAnonymous]
    [HttpGet("GetDishesByCook/{cookId}")]
    public async Task<IEnumerable<ServiceDto.MealDto>> GetDishesByCook(int cookId)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return await CookService.GetDishesByCookAsync(cookId, _context);
    }
    [HttpGet("GetOrderDetails")]
     public async Task<IEnumerable<ServiceDto.OrderMealDto>> GetOrderDetails(int orderId)
     {
         var timestamp = new DateTimeOffset(DateTime.UtcNow);
         var logInfo = new 
         { 
             Operation = "Get", 
             Timestamp = timestamp,
             User = GetUserName() 
         };

         _logger.LogInformation("Get called {@LogInfo} ", logInfo);
         return await CookService.GetOrderDetailsAsync(orderId, _context);
     }
    [HttpGet("GetTripDetails")]
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
    
    [Authorize(Policy = "CookOrAdminPolicy")]
    [HttpGet("GetAverageRatingForCookAsync")]
    public async Task<double?> GetAverageRatingForCookAsync(int cookId)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return await CookService.GetAverageRatingForCookAsync(cookId, _context);
    }
    
    [Authorize(Policy = "CyclistOrAdminPolicy")]
    [HttpGet("GetCyclistEarningsAsync")]
    public async Task<dynamic> GetCyclistEarningsAsync(int cyclistID)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return await CookService.GetCyclistEarningsAsync(cyclistID, _context);
    }
    
    [HttpGet("GetAverageRatingForCyclist")]
    public async Task<dynamic> GetAverageRatingCyclist(int cyclistID)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Get called {@LogInfo} ", logInfo);
        return await CookService.GetAverageRatingForDriversAsync(cyclistID, _context);
    }
    
    [HttpPost("AddMeal")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> AddMeal(ServiceDto.AddMealDto meal)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Post", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Post called {@LogInfo} ", logInfo);
        await CookService.AddMealAsync(meal, _context);
        return Ok(meal);
    }
    
    [HttpPut("UpdateQuantity")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> UpdateQuantity(ServiceDto.UpdateQuantityDto meal)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Put", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Put called {@LogInfo} ", logInfo);
        await CookService.UpdateQuantityAsync(meal, _context);
        return Ok(meal);
    }
    
    [HttpDelete("DeleteMeal")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> DeleteMeal(int mealId)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Delete", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Delete called {@LogInfo} ", logInfo);
        await CookService.DeleteMealAsync(mealId, _context);
        return Ok();
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