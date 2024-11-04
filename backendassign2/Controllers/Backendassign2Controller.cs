using backendassign2.Entities;
using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace backendassign2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly dbcontext _context;
    private readonly ILogger<MenuController> _logger;
    public MenuController(dbcontext context, ILogger<MenuController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    
    
    [HttpGet("GetCooks")]
    public async Task<IEnumerable<ServiceDto.CookDto>> Get(string name)
    {
        _logger.LogInformation("Log message {object}", new {message = "success", age = 22, passed = true});
        return await CookService.GetCooks(name, _context);
    }
    
    [HttpGet("GetDishesByCook/{cookId}")]
    public async Task<IEnumerable<ServiceDto.MealDto>> GetDishesByCook(int cookId)
    {
        return await CookService.GetDishesByCookAsync(cookId, _context);
    }
    [HttpGet("GetOrderDetails")]
     public async Task<IEnumerable<ServiceDto.OrderMealDto>> GetOrderDetails(int orderId)
     {
         return await CookService.GetOrderDetailsAsync(orderId, _context);
     }
    [HttpGet("GetTripDetails")]
    public async Task<IEnumerable<ServiceDto.TripDto>> GetTripDetails(int tripId)
    {
        return await CookService.GetTripDetailsAsync(tripId, _context);
    }
    
    [HttpGet("GetAverageRatingForCookAsync")]
    public async Task<double?> GetAverageRatingForCookAsync(int cookId)
    {
        return await CookService.GetAverageRatingForCookAsync(cookId, _context);
    }
    
    [HttpGet("GetCyclistEarningsAsync")]
    public async Task<dynamic> GetCyclistEarningsAsync(int cyclistID)
    {
        return await CookService.GetCyclistEarningsAsync(cyclistID, _context);
    }
    
    [HttpGet("GetAverageRatingForCyclist")]
    public async Task<dynamic> GetAverageRatingCyclist(int cyclistID)
    {
        return await CookService.GetAverageRatingForDriversAsync(cyclistID, _context);
    }
    
    [HttpPost("AddMeal")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> AddMeal(ServiceDto.AddMealDto meal)
    {
        _logger.LogInformation("Adding meal");
        await CookService.AddMealAsync(meal, _context);
        return Ok();
    }
    
    [HttpPut("UpdateQuantity")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> UpdateQuantity(ServiceDto.UpdateQuantityDto meal)
    {
        await CookService.UpdateQuantityAsync(meal, _context);
        return Ok();
    }
    
    [HttpDelete("DeleteMeal")]
    public async Task<ActionResult<ServiceDto.AddMealDto>> DeleteMeal(int mealId)
    {
        await CookService.DeleteMealAsync(mealId, _context);
        return Ok();
    }
    
    
    [HttpGet("SearchLogs")]
    public async Task<IEnumerable<ServiceDto.Log>> SearchLogs(DateTime StartTime, DateTime EndTime)
    {
        _logger.LogInformation("Searching logs");
        return await CookService.SearchLogsAsync(StartTime, EndTime);
    }
    
}