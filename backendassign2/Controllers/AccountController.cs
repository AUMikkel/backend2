using backendassign2.Entities;
using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace backendassign2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly dbcontext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApiUser> _userManager;
    private readonly SignInManager<ApiUser> _signInManager;
    public AccountController(dbcontext context,
                            ILogger<AccountController> logger,
                            IConfiguration configuration,
                            UserManager<ApiUser> userManager,
                            SignInManager<ApiUser> signInManager)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO user)
    {
        return await AccountService.Register(user, _context, _userManager, _logger, ModelState);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LoginDTO>> Login(LoginDTO input)
    {
        return await AccountService.Login(input,_context, _userManager, _logger, ModelState, _configuration);
    }
}