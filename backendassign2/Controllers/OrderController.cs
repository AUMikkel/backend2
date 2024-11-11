using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendassign2.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly dbcontext _context;
    private readonly ILogger<OrderController> _logger;
    private readonly MongoLogService _mongoLogService;
    
    public OrderController(dbcontext context, ILogger<OrderController> logger, MongoLogService mongoLogService)
    {
        _context = context;
        _logger = logger;
        _mongoLogService = mongoLogService;
    }   
    private string GetUserName()
    {
        return User?.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";
    }
    [HttpGet("GetOrderDetails/{orderId}")]
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
}