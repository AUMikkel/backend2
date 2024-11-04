using backendassign2.Entities;
using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Mvc;

namespace backendassign2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly dbcontext _context;
    public MenuController(dbcontext context)
    {
        _context = context;
    }
    [HttpGet("GetCooks")]
    public async Task<IEnumerable<ServiceDto.CookDto>> Get(string name)
    {
        return await CookService.GetCooks(name, _context);
    }
    
    [HttpGet("GetDishesByCook/{cookId}")]
    public async Task<IEnumerable<ServiceDto.MealDto
    >> GetDishesByCook(int cookId)
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
}