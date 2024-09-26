using backendassign2.Entities;
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
    public async Task<IEnumerable<Cook>> Get()
    {
        return await CookService.GetCooks(_context);
    }
    [HttpGet("GetDishesByCook/{cookCPR}")]
    public async Task<IEnumerable<Meal>> GetDishesByCook(string cookCPR)
    {
        return await CookService.GetDishesByCookAsync(cookCPR, _context);
    }
    //[HttpGet("GetOrderDetails")]
    // public async Task<IEnumerable<OrderMeal>> GetOrderDetails(int orderId)
    // {
    //     return await CookService.GetOrderDetailsAsync(orderId, _context);
    // }
    /*[HttpGet("GetTripDetails")]
    public async Task<IEnumerable<TripDetails>> GetTripDetails(int tripId)
    {
        return await CookService.GetTripDetailsAsync(tripId, _context);
    }
    [HttpGet("GetAverageRatingForCookAsync")]
    public async Task<double?> GetAverageRatingForCookAsync(string cookCPR)
    {
        return await CookService.GetAverageRatingForCookAsync(cookCPR, _context);
    }
    [HttpGet("GetCyclistEarningsAsync")]
    public async Task<dynamic> GetCyclistEarningsAsync(int cyclistID)
    {
        return await CookService.GetCyclistEarningsAsync(cyclistID, _context);
    }*/
    
    
    
    
   
}