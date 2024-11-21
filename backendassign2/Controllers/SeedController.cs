using backendassign2.Entities;
using backendassign2.DTOs;
using backendassign2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MongoDB.Bson.Serialization;
using Exception = System.Exception;

namespace backendassign2.Controllers;

[ApiController]
[Route("api/")]
public class SeedController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<ApiUser> _userManager;
    private readonly dbcontext _context;
    public DbSet<CustomerOrder> CustomerOrders { get; set; }
    public DbSet<TripDetails> TripDetails { get; set; }
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<OrderMeal> OrderMeals { get; set; }
    public DbSet<Trip> Trip { get; set; }
    public DbSet<ApiUser> ApiUsers { get; set; }
    public SeedController(dbcontext context,
                            ILogger<AccountController> logger,
                            UserManager<ApiUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    [HttpPut("Seed")]
    
    public async Task<ActionResult> Seed()
    {
        Console.WriteLine("Seeding data");
        const string adminEmail = "admin@localhost.com";
        const string adminPassword = "admin123!";

        const string managerEmail = "manager@localhost.com";
        const string managerPassword = "manager123!";

        const string cookEmail = "cook@localhost.com";
        const string cookPassword = "cook123!";
        
        const string cookEmail2 = "cook@localhost2";
        const string cookPassword2 = "Cook123!";

        const string cyclistEmail = "cyclist@localhost.com";
        const string cyclistPassword = "cyclist123!";
        
        const string cyclistEmail2 = "cyclist2@localhost.com";
        const string cyclistPassword2 = "cyclist123!";
        
        const string customeremail1 = "cust@localhost.com";
        const string customerpass1 = "cust123!";
        
        const string customeremail2 = "cust2@localhost.com";
        const string customerpass2 = "cust123!";
        
         ApiUser customer1 = new ApiUser()
        {
            FullName = "John Doe",
            PhoneNo = "12345678",
            Address = "Main Street 1, 1234 City",
            Email = customeremail1,
            UserName = customeremail1,
            EmailConfirmed = true
        };
       
        ApiUser customer2 = new ApiUser()
        {
            FullName = "Jane Doe", 
            PhoneNo = "87654321",
            UserName = customeremail2,
            Email = customeremail2,
            Address = "Third Street 3, 4323 City",
            EmailConfirmed = true
        };
        //ApiUsers.AddRange(customer1, customer2);
            var cust1 = customer1;

            IdentityResult identityResult5 = await _userManager.CreateAsync(cust1, customerpass1);
            if (identityResult5.Errors.Any())
            {
                var errors = string.Join(", ", identityResult5.Errors.Select(e => e.Description));
                throw new Exception($"Error while creating user1 {customeremail1}: {errors}");
            }
            var cust2 = customer2;

            IdentityResult identityResult6 = await _userManager.CreateAsync(cust2, customerpass2);
            if (identityResult6.Errors.Any())
            {
                var errors = string.Join(", ", identityResult6.Errors.Select(e => e.Description));
                throw new Exception($"Error while creating user2 {customeremail1}: {errors}");
            }
        
        
        Console.WriteLine("førcooks");
        ApiUser cook1 = new ApiUser()
        {
            FullName = "Jane Cook",
            PhoneNo = "87654321",
            Address = "Second Street 2, 4321 City",
            UserName = cookEmail,
            Email = cookEmail,
            EmailConfirmed = true
        };
        
        ApiUser cook2 = new ApiUser()
        {
            FullName = "John Cook",
            PhoneNo = "12345678",
            Address = "Main Street 1, 1234 City",
            UserName = cookEmail2,
            Email = cookEmail2,
            EmailConfirmed = true
        };
            var cookUser1 = cook1;

            IdentityResult identityResult1 = await _userManager.CreateAsync(cookUser1, cookPassword);
            if (identityResult1.Succeeded)
            {
                var newCookUser = _userManager.FindByNameAsync(cookEmail).Result;
                var cookClaim = new Claim(ClaimTypes.Role, "Cook");
                var claimAdded = _userManager.AddClaimAsync(newCookUser, cookClaim).Result;
                var userclaim = new Claim(ClaimTypes.NameIdentifier, newCookUser.Id);
                _userManager.AddClaimAsync(newCookUser, userclaim);
            }
            else
            {
                var errors = string.Join(", ", identityResult1.Errors.Select(e => e.Description));
                throw new Exception($"Error while creating cook1 {cookEmail}: {errors}");
            }
            var cookUser2 = cook2;

            IdentityResult identityResult2 = await  _userManager.CreateAsync(cookUser2, cookPassword2);
            if (identityResult2.Succeeded)
            {
                var newCookUser = _userManager.FindByNameAsync(cookEmail2).Result;
                var cookClaim = new Claim(ClaimTypes.Role, "Cook");
                var userclaim = new Claim(ClaimTypes.NameIdentifier, newCookUser.Id);
                var claimAdded = _userManager.AddClaimAsync(newCookUser, cookClaim).Result;
                _userManager.AddClaimAsync(newCookUser, userclaim);
            }
            else
            {
                var errors = string.Join(", ", identityResult2.Errors.Select(e => e.Description));
                throw new Exception($"Error while creating cook2 {cookEmail2}: {errors}");
            }

            Cook cook3 = new Cook()
            {
                CookId = cookUser1.Id,
                ApiUser = cookUser1,
                HasPassedFoodSafetyCourse = true
            };
            
            Cook cook4 = new Cook()
            {
                CookId = cookUser2.Id,
                ApiUser = cookUser2,
                HasPassedFoodSafetyCourse = true
            };
            _context.Cooks.AddRange(cook3, cook4);
        /*
        var existingCook1 = await _userManager.FindByNameAsync(cookEmail);
        if (existingCook1 == null)
        {
            throw new Exception($"Cook {cookEmail} not found. Ensure it was created successfully.");
        }*/
        Meal meal1 = new Meal
        {
            Cook = cook1,
            CookId = cook1.Id,
            Dish = "Pizza",
            Quantity = 1,
            Price = 100,
            StartTime = new DateTime(2024,08,12, 12,0,0),
            EndTime = new DateTime(2024,08,12, 18,0,0)
            
        };
        
        Meal meal2 = new Meal
        {
            Cook = cook2,
            CookId = cook2.Id,
            Dish = "Pasta",
            Quantity = 2,
            Price = 200,
            StartTime = new DateTime(2024,08,12, 12,0,0),
            EndTime = new DateTime(2024,08,12, 18,0,0)
        };
        Meal meal3 = new Meal
        {
            Cook = cook2,
            CookId = cook2.Id,
            Dish = "Panini",
            Quantity = 6,
            Price = 150,
            StartTime = new DateTime(2024,08,12, 12,0,0),
            EndTime = new DateTime(2024,08,12, 18,0,0)
        };
        
        Console.WriteLine("eftermeals");
        
        ApiUser driver1 = new ApiUser()
        {
            FullName = "John Driver",
            PhoneNo = "12345678",
            UserName = cyclistEmail,
            Email = cyclistEmail,
            EmailConfirmed = true,
            Address = "Second Street 2, 4321 City"
        };
        ApiUser driver2 = new ApiUser()
        {
            FullName = "Jane Driver",
            PhoneNo = "87654321",
            UserName = cyclistEmail2,
            Email = cyclistEmail2,
            EmailConfirmed = true,
            Address = "Third Street 3, 4323 City"
        };
        
        //ApiUsers.AddRange(driver1, driver2);
            var cyclistUser1 = driver1;

            IdentityResult identityResult3 = await _userManager.CreateAsync(cyclistUser1, cyclistPassword);
            if (identityResult3.Succeeded)
            {
                var newCyclistUser = _userManager.FindByNameAsync(cyclistEmail).Result;
                var cyclistClaim = new Claim(ClaimTypes.Role, "Cyclist");
                var userclaim = new Claim(ClaimTypes.NameIdentifier, newCyclistUser.Id);
                var claimAdded = _userManager.AddClaimAsync(newCyclistUser, cyclistClaim).Result;
                await _userManager.AddClaimAsync(newCyclistUser, userclaim);
            }
            else
            {
                throw new Exception($"Error while creating user {cyclistEmail}");
            }
            
            var cyclistUser = driver2;

            IdentityResult identityResult4 = await _userManager.CreateAsync(cyclistUser, cyclistPassword2);
            if (identityResult4.Succeeded)
            {
                var newCyclistUser = _userManager.FindByNameAsync(cyclistEmail2).Result;
                var cyclistClaim = new Claim(ClaimTypes.Role, "Cyclist");
                var userclaim = new Claim(ClaimTypes.NameIdentifier, newCyclistUser.Id);
                var claimAdded = _userManager.AddClaimAsync(newCyclistUser, cyclistClaim).Result;
                await _userManager.AddClaimAsync(newCyclistUser, userclaim);
            }
            else
            {
                throw new Exception($"Error while creating user {cyclistEmail2}");
            }
            
        
        CustomerOrder order1 = new CustomerOrder
        {
            Customer = customer1,
            Timestamp = new DateTime(2024,08,12, 12,0,0),
            Price = 100,
        };
        CustomerOrder order2 = new CustomerOrder
        {
            Customer = customer2,
            Timestamp = new DateTime(2024,08,12, 14,0,0),
            Price = 200
        };
        
        CustomerOrder order3 = new CustomerOrder
        {
            Customer = customer2,
            Timestamp = new DateTime(2024,08,12, 15,0,0),
            Price = 275
        };
        CustomerOrder order4 = new CustomerOrder
        {
            Customer = customer2,
            Timestamp = new DateTime(2024,08,12, 16,0,0),
            Price = 220
        };
        
        CustomerOrder order5 = new CustomerOrder
        {
            Customer = customer1,
            Timestamp = new DateTime(2024,08,12, 17,0,0),
            Price = 225
        };
        
        
        OrderMeal orderMeal1 = new OrderMeal
        {
            CustomerOrder = order1,
            Meal = meal1,
            Quantity = 1,
            Rating = 4
        };
        OrderMeal orderMeal2 = new OrderMeal
        {
            CustomerOrder = order2,
            Meal = meal2,
            Quantity = 2,
            Rating = 5
        };
        
        OrderMeal orderMeal3 = new OrderMeal
        {
            CustomerOrder = order3,
            Meal = meal3,
            Quantity = 1,
            Rating = 2
        };
        
        OrderMeal orderMeal4 = new OrderMeal
        {
            CustomerOrder = order4,
            Meal = meal3,
            Quantity = 1,
            Rating = 2
        };
        
        OrderMeal orderMeal5 = new OrderMeal
        {
            CustomerOrder = order5,
            Meal = meal3,
            Quantity = 1,
            Rating = 2
        };
        
        

        Trip trip1 = new Trip
        {
            Driver = driver1,
            rating = 5
        };
        
        Trip trip2 = new Trip
        {
            Driver = driver1,
            rating = 4
        };
        
        
        Trip trip3 = new Trip
        {
            Driver = driver1,
            rating = 2
        };
        
        TripDetails tripdetails1 = new TripDetails
        {
            Address = cook1.Address,
            TripDate = new DateTime(2024,08,12, 12,0,0),
            Type = "Pickup",
            Trip = trip1
        };
        TripDetails tripdetails2 = new TripDetails
        {
            Address = customer1.Address,
            TripDate = new DateTime(2024,08,12, 13,0,0),
            Type = "Delivery",
            Trip = trip1
        };
        
        TripDetails tripdetails3= new TripDetails
        {
            Address = cook2.Address,
            TripDate = new DateTime(2024,09,12, 13,0,0),
            Type = "Pickup",
            Trip = trip2
        };
        
        TripDetails tripdetails4 = new TripDetails
        {
            Address = customer2.Address,
            TripDate = new DateTime(2024,09,12, 13,45,0),
            Type = "Delivery",
            Trip = trip2
        };
        
        TripDetails tripdetails5 = new TripDetails
        {
            Address = cook2.Address,
            TripDate = new DateTime(2024,08,12, 15,0,0),
            Type = "Pickup",
            Trip = trip3
        };
        
        TripDetails tripdetails6 = new TripDetails
        {
            Address = customer2.Address,
            TripDate = new DateTime(2024,08,12, 16,0,0),
            Type = "Delivery",
            Trip = trip3
        };
        Console.WriteLine("eftertripdetails");
        _context.Meals.AddRange(meal1, meal2, meal3);
        Console.WriteLine("eftermeals2");
        _context.CustomerOrders.AddRange(order1, order2, order3, order4, order5);
        Console.WriteLine("efterorders");
        _context.OrderMeals.AddRange(orderMeal1, orderMeal2, orderMeal3, orderMeal4, orderMeal5);
        _context.Trip.AddRange(trip1,trip2,trip3);
        order1.Trip = trip1;
        order2.Trip = trip1;
        order3.Trip = trip2;
        order4.Trip = trip3;
        order5.Trip = trip3;
        
        _context.TripDetails.AddRange(tripdetails1, tripdetails2, tripdetails3, tripdetails4, tripdetails5, tripdetails6);
        Console.WriteLine("eftertrip");
        
        _context.SaveChanges();
    
        


        if (_userManager == null)
        {
            _logger.LogError("UserManager is null");
            return StatusCode(500);
        }

        if (_userManager.FindByNameAsync(adminEmail).Result == null)
        {
            var adminUser = new ApiUser();
            adminUser.FullName = "AdminUser";
            adminUser.UserName = adminEmail;
            adminUser.Email = adminEmail;
            adminUser.Address = "First Street 1, 1234 City";
            adminUser.PhoneNo = "12345678";
            adminUser.EmailConfirmed = true;

            IdentityResult identityResult = await _userManager.CreateAsync(adminUser, adminPassword);
            if (identityResult.Succeeded)
            {
                var newAdminUser = _userManager.FindByNameAsync(adminEmail).Result;
                var adminClaim = new Claim(ClaimTypes.Role, "Admin");
                var driverclaim = new Claim(ClaimTypes.Role, "Cyclist");
                var cookclaim = new Claim(ClaimTypes.Role, "Cook");
                var managerclaim = new Claim(ClaimTypes.Role, "Manager");
                await _userManager.AddClaimAsync(newAdminUser, driverclaim);
                await _userManager.AddClaimAsync(newAdminUser, cookclaim);
                await _userManager.AddClaimAsync(newAdminUser, managerclaim);
                await _userManager.AddClaimAsync(newAdminUser, adminClaim);
                
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
            managerUser.FullName = "ManagerUser";
            managerUser.UserName = managerEmail;
            managerUser.Email = managerEmail;
            managerUser.Address = "Second Street 2, 4321 City";
            managerUser.PhoneNo = "87654321";
            managerUser.EmailConfirmed = true;

            IdentityResult identityResult = await _userManager.CreateAsync(managerUser, managerPassword);
            if (identityResult.Succeeded)
            {
                var newManagerUser = _userManager.FindByNameAsync(managerEmail).Result;
                var managerClaim = new Claim(ClaimTypes.Role, "Manager");
                await _userManager.AddClaimAsync(managerUser, managerClaim);
            }
            else
            {
                throw new Exception($"Error while creating user {managerEmail}");
            }

        }

        
        
       
       
       
        return StatusCode(201);
    }
}