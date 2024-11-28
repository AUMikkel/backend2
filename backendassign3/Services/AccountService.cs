using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using backendassign3.Controllers;
using backendassign3.DTOs;
using backendassign3.Entities;

namespace backendassign3.Services;

public static class AccountService
{
    public static async Task<ActionResult> Register(RegisterDTO input, dbcontext _context,
                                        UserManager<ApiUser> _userManager, ILogger<AccountController> _logger, ModelStateDictionary modelState)
    {
        try
        {
            if (modelState.IsValid)
            {
                var newUser = new ApiUser();
                newUser.UserName = input.Email;
                newUser.Email = input.Email;
                newUser.FullName = input.FullName;
                newUser.Address = input.Address;
                newUser.PhoneNo = input.PhoneNo;
                // Check if the mail exists
                var existingUser = await _userManager.FindByEmailAsync(input.Email);
                if (existingUser != null)
                {
                    return new ObjectResult("Email already in use") { StatusCode = 400 };
                }
                var result = await _userManager.CreateAsync(newUser, input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation(
                    "User {userName} ({email}) has been created.",
                    newUser.UserName, newUser.Email, newUser.Address, newUser.PhoneNo);
                    return new ObjectResult($"User '{newUser.UserName}' has been created.") { StatusCode = 201 };
                }
                else
                    throw new Exception(
                    string.Format("Error: {0}", string.Join(" ",
                    result.Errors.Select(e => e.Description))));
            }
            else {
                return new ObjectResult("Invalid registration attempt") { StatusCode = 400 };
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Register");
            return new ObjectResult("Internal server error") { StatusCode = 500 };
        }
    }

    public static async Task<ActionResult> Login(LoginDTO input, dbcontext _context,
                                        UserManager<ApiUser> _userManager, ILogger<AccountController> _logger,
                                         ModelStateDictionary modelState, IConfiguration _configuration)
    {
        try
        {
            if (modelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(input.UserName);
                if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password)){
                    //throw new Exception("Invalid login attempt.");
                    return new ObjectResult("Invalid login attempt") { StatusCode = 401 };
                }
                else
                {
                    var signingCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"])),
                            SecurityAlgorithms.HmacSha256);
                    
                    var userRole = await _userManager.GetRolesAsync(user);
                    var role = userRole.FirstOrDefault();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName, null)
                    };
                    if (role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    
                    var jwtObject = new JwtSecurityToken(
                        issuer: _configuration["JWT:Issuer"],
                        audience: _configuration["JWT:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddSeconds(300),
                        signingCredentials: signingCredentials
                    );
                    var jwtString = new JwtSecurityTokenHandler()
                    .WriteToken(jwtObject);
                    return new ObjectResult(jwtString) { StatusCode = StatusCodes.Status200OK };
                }
            }
            else {
                return new ObjectResult("Invalid login attempt") { StatusCode = 401 };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Login");
            return new ObjectResult("Internal server error") { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}