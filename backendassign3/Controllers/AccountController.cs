using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using backendassign3.DTOs;
using backendassign3.Entities;
using backendassign3.Services;

namespace backendassign3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly dbcontext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApiUser> _userManager;
    private readonly SignInManager<ApiUser> _signInManager;
    private readonly TokenService _tokenService;
    public AccountController(dbcontext context,
                            ILogger<AccountController> logger,
                            IConfiguration configuration,
                            UserManager<ApiUser> userManager,
                            SignInManager<ApiUser> signInManager,
                            TokenService tokenService)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    
    private string GetUserName()
    {
        return User?.Identity?.IsAuthenticated == true ? User.Identity.Name : "Anonymous";
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO user)
    {
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };
        _logger.LogInformation("Post called {@LogInfo} ", logInfo);
        return await AccountService.Register(user, _context, _userManager, _logger, ModelState);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Unauthorized("Invalid username or password.");
        }
        var timestamp = new DateTimeOffset(DateTime.UtcNow);
        var logInfo = new 
        { 
            Operation = "Get", 
            Timestamp = timestamp,
            User = GetUserName() 
        };
        _logger.LogInformation("Post called {@LogInfo} ", logInfo);

        var userClaims = await _userManager.GetClaimsAsync(user);

        var token = _tokenService.GenerateJwtToken(user, userClaims);

        return Ok(new { token });
    }
}