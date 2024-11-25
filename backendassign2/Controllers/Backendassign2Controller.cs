using System.Security.Claims;
using backendassign2.Entities;
using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNet.Identity;
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
   
    [Authorize(Roles = "Manager")]
    [HttpGet("GetCook")]
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
    public async Task<IEnumerable<ServiceDto.MealDto>> GetDishesByCook(string cookId)
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
    [Authorize(Roles = "Cook")]
    [HttpGet("GetAverageRatingForCook")]
    public async Task<double?> GetAverageRatingForCookAsync()
    {
        // Extract NameIdentifier from claims
        string nameIdentifier = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        // Log extracted NameIdentifier for debugging
        if (string.IsNullOrEmpty(nameIdentifier))
        {
            _logger.LogError("NameIdentifier is null or empty. Ensure the claim exists and is configured correctly.");
            return null; // Or handle the error as appropriate
        }

        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };
        _logger.LogInformation("Post called {@LogInfo} ", logInfo);
        // Pass the NameIdentifier to the service method
        var averageRating = await CookService.GetAverageRatingForCookAsync(nameIdentifier, _context);

        return averageRating;
    }
    [Authorize(Roles = "Cook")] 
    [HttpPost("AddMeal")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> AddMeal(ServiceDto.AddMealDto meal)
    {
        string nameIdentifier = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Post", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Post called {@LogInfo} ", logInfo);
        await CookService.AddMealAsync(nameIdentifier,meal, _context);
        return Created("Add meal",meal);
    }
    
    [Authorize(Roles = "Cook")]
    [HttpPut("UpdateQuantity")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> UpdateQuantity(ServiceDto.UpdateQuantityDto meal)
    {
        string nameIdentifier = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Put", 
            Timestamp = timestamp,
            User = GetUserName() 
        };

        _logger.LogInformation("Put called {@LogInfo} ", logInfo);

        try
        {
            await CookService.UpdateQuantityAsync(nameIdentifier, meal, _context);
            return Ok(meal);
        }
        catch (KeyNotFoundException ex) // Custom exception for "meal not found"
        {
            _logger.LogWarning(ex, "Meal not found or unauthorized access for Cook ID {CookId}", nameIdentifier);
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during UpdateQuantity.");
            return StatusCode(500, new { Message = "An unexpected error occurred." });
        }
    }
    
    [Authorize(Roles = "Cook")]
    [HttpDelete("DeleteMeal")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> DeleteMeal(int mealId)
    {
        string nameIdentifier = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Delete", 
            Timestamp = timestamp,
            User = GetUserName() 
        };
        _logger.LogInformation("Delete called {@LogInfo} ", logInfo);
        try
        {
            await CookService.DeleteMealAsync(nameIdentifier, mealId, _context);
            return Ok(mealId);
        }
        catch (KeyNotFoundException ex) // Custom exception for "meal not found"
        {
            _logger.LogWarning(ex, "Meal not found or unauthorized access for Cook ID {CookId}", nameIdentifier);
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during UpdateQuantity.");
            return StatusCode(500, new { Message = "An unexpected error occurred." });
        }
    }
    
    
    
    
}