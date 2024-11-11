using backendassign2.Entities;
using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace backendassign2.Controllers;

[ApiController]
[Route("api/")]
public class SeedController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<ApiUser> _userManager;
    public SeedController(dbcontext context, ILogger<AccountController> logger, UserManager<ApiUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPut("Seed")]
    public async Task<ActionResult> Seed()
    {
        const string adminEmail = "admin@localhost";
        const string adminPassword = "Admin123!";

        const string managerEmail = "manager@localhost";
        const string managerPassword = "Manager123!";

        const string cookEmail = "cook@localhost";
        const string cookPassword = "Cook123!";
        
        const string cookEmail2 = "cook@localhost2";
        const string cookPassword2 = "Cook123!";

        const string cyclistEmail = "cyclist@localhost";
        const string cyclistPassword = "Cyclist123!";


        if (_userManager == null)
        {
            _logger.LogError("UserManager is null");
            return StatusCode(500);
        }
        if (_userManager.FindByNameAsync(adminEmail).Result == null)
        {
            var adminUser = new ApiUser();
            adminUser.FullName = "Admin";
            adminUser.UserName = adminEmail;
            adminUser.Email = adminEmail;
            adminUser.EmailConfirmed = true;

            IdentityResult identityResult = _userManager.CreateAsync(adminUser, adminPassword).Result;
            if (identityResult.Succeeded)
            {
                var newAdminUser = _userManager.FindByNameAsync(adminEmail).Result;
                var adminClaim = new Claim(ClaimTypes.Role, "Admin");
                var claimAdded = _userManager.AddClaimAsync(newAdminUser, adminClaim).Result;
            }
            else
            {
                throw new Exception($"Error while creating user {adminEmail}");
            }

        }

        // For manager
        if (_userManager.FindByNameAsync(managerEmail).Result == null)
        {
            var managerUser = new ApiUser();
            managerUser.FullName = "Manager";
            managerUser.UserName = managerEmail;
            managerUser.Email = managerEmail;
            managerUser.EmailConfirmed = true;

            IdentityResult identityResult = _userManager.CreateAsync(managerUser, managerPassword).Result;
            if (identityResult.Succeeded)
            {
                var newManagerUser = _userManager.FindByNameAsync(managerEmail).Result;
                var managerClaim = new Claim(ClaimTypes.Role, "Manager");
                var claimAdded = _userManager.AddClaimAsync(newManagerUser, managerClaim).Result;
            }
            else
            {
                throw new Exception($"Error while creating user {managerEmail}");
            }

        }

        // For cook
        if (_userManager.FindByNameAsync(cookEmail).Result == null)
        {
            var cookUser = new ApiUser();
            cookUser.FullName = "Jane Cook";
            cookUser.UserName = cookEmail;
            cookUser.Email = cookEmail;
            cookUser.EmailConfirmed = true;

            IdentityResult identityResult = _userManager.CreateAsync(cookUser, cookPassword).Result;
            if (identityResult.Succeeded)
            {
                var newCookUser = _userManager.FindByNameAsync(cookEmail).Result;
                var cookClaim = new Claim(ClaimTypes.Role, "Cook");
                var claimAdded = _userManager.AddClaimAsync(newCookUser, cookClaim).Result;
                var fullNameClaim = new Claim("FullName", cookUser.FullName);
                _userManager.AddClaimAsync(newCookUser, fullNameClaim).Wait();
            }
            else
            {
                throw new Exception($"Error while creating user {cookEmail}");
            }

        }
        if (_userManager.FindByNameAsync(cookEmail2).Result == null)
        {
            var cookUser = new ApiUser();
            cookUser.FullName = "Jane Cook";
            cookUser.UserName = cookEmail2;
            cookUser.Email = cookEmail2;
            cookUser.EmailConfirmed = true;

            IdentityResult identityResult = _userManager.CreateAsync(cookUser, cookPassword2).Result;
            if (identityResult.Succeeded)
            {
                var newCookUser = _userManager.FindByNameAsync(cookEmail2).Result;
                var cookClaim = new Claim(ClaimTypes.Role, "Cook");
                var claimAdded = _userManager.AddClaimAsync(newCookUser, cookClaim).Result;
                var fullNameClaim = new Claim("FullName", cookUser.FullName);
                _userManager.AddClaimAsync(newCookUser, fullNameClaim).Wait();
            }
            else
            {
                throw new Exception($"Error while creating user {cookEmail}");
            }

        }

        // For cyclist
        if (_userManager.FindByNameAsync(cyclistEmail).Result == null)
        {
            var cyclistUser = new ApiUser();
            cyclistUser.FullName = "Cyclist";
            cyclistUser.UserName = cyclistEmail;
            cyclistUser.Email = cyclistEmail;
            cyclistUser.EmailConfirmed = true;

            IdentityResult identityResult = _userManager.CreateAsync(cyclistUser, cyclistPassword).Result;
            if (identityResult.Succeeded)
            {
                var newCyclistUser = _userManager.FindByNameAsync(cyclistEmail).Result;
                var cyclistClaim = new Claim(ClaimTypes.Role, "Cyclist");
                var claimAdded = _userManager.AddClaimAsync(newCyclistUser, cyclistClaim).Result;
            }
            else
            {
                throw new Exception($"Error while creating user {cyclistEmail}");
            }

        }

        return StatusCode(201);
    }
}