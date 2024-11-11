using backendassign2.Entities;
using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace backendassign2.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
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
    //no authorization for this endpoint
    
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
    [Authorize(Policy = "MatchFullNamePolicy")]
    [HttpGet("GetAverageRatingForCookAsync/{cookId}")]
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
    
    [Authorize(Policy = "MatchFullNamePolicy")]
    [HttpGet("testcookget/{fullName}")]
    public async Task<string> testcookget(string fullName)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Post", 
            Timestamp = timestamp,
            User = GetUserName() 
        };
        return "Hello " + fullName;
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
        return Ok(mealId);
    }
    
    
    
    
}