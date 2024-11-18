using System.Globalization;
using backendassign2.Entities;
using System.Linq;
using System.Runtime.Serialization;
using backendassign2.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Serilog;
using ServiceDto = backendassign2.DTOs.ServiceDto;

namespace backendassign2.Services;

public static class CookService
{
    static CookService()
    {
    }
    public static async Task<List<ServiceDto.CookDto>> GetCooks(string name,dbcontext _context)
    {
        return await _context.ApiUsers
            .Where(cook => cook.FullName.Equals(name))
            .Select(cook => new ServiceDto.CookDto()
            {
                Address = cook.Address,
                PhoneNo = cook.PhoneNo,
                CookId = cook.Id
            })
            .Select(foodsafety => new ServiceDto.CookDto()
            {
                HasPassedFoodSafetyCourse = foodsafety.HasPassedFoodSafetyCourse
            })
            .ToListAsync();
    }
    
    public static async Task<List<ServiceDto.MealDto>> GetDishesByCookAsync(string cookId, dbcontext _context)
    {
        return await _context.Meals
            .Where(meal => meal.Cook.Id == cookId)
            .Select(meal => new ServiceDto.MealDto()
            {
                Dish = meal.Dish,
                Quantity = meal.Quantity,
                Price = meal.Price,
                StartTime = meal.StartTime.ToString("dd-MM-yyyy HH:mm"),
                EndTime = meal.EndTime.ToString("dd-MM-yyyy HH:mm")
            })
            .ToListAsync();
    }
    
    
    public static async Task<List<ServiceDto.OrderMealDto>> GetOrderDetailsAsync(int orderId, dbcontext _context)
    {
        return await _context.OrderMeals
            .Where(orderMeal => orderMeal.OrderId == orderId)
            .Join(
                _context.Meals,
                orderMeal => orderMeal.MealId,
                meal => meal.mealId,
                (orderMeal, meal) => new ServiceDto.OrderMealDto()
                {
                    FullName = meal.Cook.FullName,
                    Dish = meal.Dish,
                    Quantity = orderMeal.Quantity,
                })
            .ToListAsync();
    }
    
    public static async Task<List<ServiceDto.TripDto>> GetTripDetailsAsync(int tripId, dbcontext _context)
    {
        return await _context.TripDetails
            .Where(tripdetail => tripdetail.Trip.TripId == tripId)
            .Select(tripdetail => new ServiceDto.TripDto()
            {
                address = tripdetail.Address,
                tripDate = TimeOnly.FromDateTime(tripdetail.TripDate),
                type = tripdetail.Type
            })
            .ToListAsync();
    }
    
    public static async Task<double?> GetAverageRatingForCookAsync(string cookUserId, dbcontext _context)
    {
        // Log the received CookId
        Console.WriteLine($"Cook UserId received in service: {cookUserId}");

        // Query to calculate the average rating
        var averageRating = await _context.OrderMeals
            .Where(orderMeal => orderMeal.Meal.Cook.Id == cookUserId) // Navigate the relationships
            .AverageAsync(orderMeal => (double?)orderMeal.Rating);
        Console.WriteLine(_context.OrderMeals
            .Select(ordermeal => ordermeal.Meal.Cook.Id));
        // Handle no ratings case
        if (averageRating == null)
        {
            Console.WriteLine("No ratings found for the cook.");
            return 0.0; // Default value
        }

        return averageRating;
    }
    
    public static async Task<List<dynamic>> GetCyclistEarningsAsync(string DeliveryDriverId, dbcontext _context)
    {
        // Calculate trip durations and hours worked
        var tripDurations = await _context.TripDetails
            .Where(t1 => t1.Trip.Driver.Id == DeliveryDriverId)
            .Join(
                _context.TripDetails.Where(t2 => t2.Type == "delivery"),
                t1 => t1.Trip.TripId,
                t2 => t2.Trip.TripId,
                (t1, t2) => new
                {
                    t1.Trip.TripId,
                    t1.Trip.Driver.Id,
                    PickupTime = t1.TripDate,
                    DeliveryTime = t2.TripDate,
                    TripDate = t1.TripDate,
                    HoursWorked = (double)(t2.TripDate - t1.TripDate).TotalMinutes / 60.0,  // Calculate hours worked
                    Month = t1.TripDate.Month  // Get the month
                }
            )
            .ToListAsync();

        // Group by cyclist and month, then calculate total hours and earnings
        var earnings = tripDurations
            .GroupBy(td => new { td.Id, td.Month })
            .Select(g => new
            {
                Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(g.Key.Month),
                Hours = g.Sum(td => td.HoursWorked),  // Calculate hours
                Earnings = g.Sum(td => td.HoursWorked) * 150  // Calculate earnings
            })
            .OrderBy(e => e.Month)
            .ToList<dynamic>(); 

        return earnings;
    }
   
   
    public static async Task<double> GetAverageRatingForDriversAsync(string driverid, dbcontext _context)
    {
        return await _context.Trip
            .Where(Trip => Trip.Driver.Id == driverid)
            .AverageAsync(Trip => (double)Trip.rating);
    }
    
    //Example Query:
    /*
     * {
         "dish": "Pasta",
         "startTime": "2024-10-21T09:11:23.158Z",
         "endTime": "2024-10-21T09:11:23.158Z",
         "price": -1,
         "quantity": 5,
         "cookId": 1
       }
     */
    public static async Task AddMealAsync(ServiceDto.AddMealDto AddmealDto, dbcontext _context)
    {
        var cook = await _context.ApiUsers
            .Where(cook => cook.Id == AddmealDto.CookId)
            .FirstOrDefaultAsync();
        if (cook == null)
        {
            throw new Exception("Cook not found");
        }
        var meal = new Meal()
        {
            Dish = AddmealDto.Dish,
            Quantity = AddmealDto.Quantity,
            Price = AddmealDto.Price,
            StartTime = AddmealDto.StartTime,
            EndTime = AddmealDto.EndTime,
            Cook = cook
        };
        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();
    }
    
    public static async Task UpdateQuantityAsync(ServiceDto.UpdateQuantityDto updateQuantityDto, dbcontext _context)
    {
        var meal = await _context.Meals
            .Where(meal => meal.mealId == updateQuantityDto.mealId)
            .FirstOrDefaultAsync();
        if (meal == null)
        {
            throw new Exception("Meal not found");
        }
        
        meal.Quantity = updateQuantityDto.Quantity;
        await _context.SaveChangesAsync();
    }
    
    public static async Task DeleteMealAsync(int mealId, dbcontext _context)
    {
        var meal = await _context.Meals
            .Where(meal => meal.mealId == mealId)
            .FirstOrDefaultAsync();
        if (meal == null)
        {
            throw new Exception("Meal not found");
        }
        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync();
    }
    
    //SearchController Logs in MongoDB
    public static async Task<List<ServiceDto.LogDto>> SearchLogsAsync(DateTime startDate, DateTime endDate, string search = null)
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("assign3");
        var logsCollection = database.GetCollection<ServiceDto.LogDto>("logs");

        // Define the date range filter on Timestamp
        var dateFilter = Builders<ServiceDto.LogDto>.Filter.Gte(log => log.Timestamp, startDate) &
                         Builders<ServiceDto.LogDto>.Filter.Lte(log => log.Timestamp, endDate);

        // Optionally add a text search filter if a search term is provided
        var filter = dateFilter;
        if (!string.IsNullOrEmpty(search))
        {
            var textFilter = Builders<ServiceDto.LogDto>.Filter.Text(search);
            filter = Builders<ServiceDto.LogDto>.Filter.And(dateFilter, textFilter);
        }

        // Fetch logs within the time interval (and matching the search term, if provided)
        var logs = await logsCollection.Find(filter).ToListAsync();
        return logs;
    }
    


}