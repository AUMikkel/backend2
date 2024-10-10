using System.Globalization;
using backendassign2.Entities;
using System.Linq;
using System.Runtime.Serialization;
using backendassign2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace backendassign2.Services;

public static class CookService
{
    static CookService()
    {
    }
    public static async Task<List<ServiceDto.CookDto>> GetCooks(string name,dbcontext _context)
    {
        return await _context.Cooks
            .Where(cook => cook.FullName.Equals(name))
            .Select(cook => new ServiceDto.CookDto()
            {
                Address = cook.StreetName + " " + cook.HouseNumber + ", " + cook.Zipcode + ", " + cook.City,
                PhoneNo = cook.PhoneNo,
                CookId = cook.CookId,
                HasPassedFoodSafetyCourse = cook.HasPassedFoodSafetyCourse
            })
            .ToListAsync();
    }
    public static async Task<List<ServiceDto.MealDto>> GetDishesByCookAsync(int cookId, dbcontext _context)
    {
        return await _context.Meals
            .Where(meal => meal.Cook.CookId == cookId)
            .Select(meal => new ServiceDto.MealDto()
            {
                Dish = meal.Dish,
                Quantity = meal.Quantity,
                Price = meal.Price,
                StartTime = meal.StartTime.ToString("ddMMyyyy HHmm"),
                EndTime = meal.EndTime.ToString("ddMMyyyy HHmm")
            })
            .ToListAsync();
    }
    /*public static async Task<List<OrderMeal>> GetOrderDetailsAsync(int orderId, dbcontext _context)
    {
        return await _context.OrderMeals
            .Where(orderMeal => orderMeal.OrderId == orderId)
            .Join(
                _context.Cooks,
                orderMeal => orderMeal.OrderId,
                cook => cook.CookCPR,
                (orderMeal, cook) => new OrderMeal
                {
                    Dish = orderMeal.Dish,
                    Quantity = orderMeal.Quantity,
                    CookCPR = cook.CookCPR
                })
            .ToListAsync();
    }*/
    
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
    /*public static async Task<List<TripDetails>> GetTripDetailsAsync(int tripId, dbcontext _context)
    {
        return await _context.TripDetails
            .Where(tripDetail => tripDetail.TripID == tripId)
            .Where(TripDetails => TripDetails.Type == "Delivery")
            .Select(TripDetails => new TripDetails()
            {
                CustomerOrder = TripDetails.CustomerOrder,
                Address = TripDetails.Address,
                TripDate = TripDetails.TripDate,
                Type = TripDetails.Type
                
            } )
            .OrderBy(tripDetail => tripDetail.TripDate)
            .ToListAsync();
    }*/
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
    /*public static async Task<List<TripDetails>> GetTripDetailsAsync(int tripID, dbcontext _context)
    {
        return await _context.TripDetails
            .Where(tripDetail => tripDetail.TripID == tripID)
            .OrderBy(tripDetail => tripDetail.Time)
            .ToListAsync();
    }*/
    public static async Task<double?> GetAverageRatingForCookAsync(int cookId, dbcontext _context)
    {
        return await _context.OrderMeals
            .Where(orderMeal => orderMeal.Meal.Cook.CookId == cookId)
            .AverageAsync(orderMeal => (double?)orderMeal.Rating);
    }
    
    public static async Task<List<dynamic>> GetCyclistEarningsAsync(int DeliveryDriverId, dbcontext _context)
    {
        // First part: Calculate trip durations and hours worked
        var tripDurations = await _context.TripDetails
            .Where(t1 => t1.Trip.DeliveryDriver.CyclistID == DeliveryDriverId)
            .Join(
                _context.TripDetails.Where(t2 => t2.Type == "delivery"),
                t1 => t1.Trip.TripId,
                t2 => t2.Trip.TripId,
                (t1, t2) => new
                {
                    t1.Trip.TripId,
                    t1.Trip.DeliveryDriver.CyclistID,
                    PickupTime = t1.TripDate,
                    DeliveryTime = t2.TripDate,
                    TripDate = t1.TripDate,
                    HoursWorked = (double)(t2.TripDate - t1.TripDate).TotalMinutes / 60.0,  // Calculate hours worked
                    Month = t1.TripDate.Month  // Get the month
                }
            )
            .ToListAsync();

        // Second part: Group by cyclist and month, then calculate total hours and earnings
        var earnings = tripDurations
            .GroupBy(td => new { td.CyclistID, td.Month })
            .Select(g => new
            {
                Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(g.Key.Month),
                Hours = g.Sum(td => td.HoursWorked),  // Sum the hours worked
                Earnings = g.Sum(td => td.HoursWorked) * 150  // Calculate earnings
            })
            .OrderBy(e => e.Month)
            .ToList<dynamic>(); 

        return earnings;
    }
   
    /*
    public static async Task<List<dynamic>> GetCyclistEarningsAsync(int DeliveryDriver, dbcontext _context)
    {
        return await _context.TripDetails
            .Where(td => td.DeliveryDriver.CyclistID == DeliveryDriver)
            .GroupBy(td => new { td.DeliveryDriver.CyclistID, td.TripDate })
            .Select(g => new
            {
                CyclistID = g.Key.CyclistID,
                Month = g.Key.TripDate.Month,
                Hours = g.Sum(td => (double?)(td.TripDate - td.TripDate).TotalMinutes / 60.0),
                Earnings = g.Sum(td => (double?)(td.TripDate - td.TripDate).TotalMinutes / 60.0) * 150
            })
            .OrderBy(e => e.CyclistID)
            .ThenBy(e => e.Month)
            .ToListAsync<dynamic>();
        
    }*/
    public static async Task<double> GetAverageRatingForDriversAsync(int driverid, dbcontext _context)
    {
        return await _context.Trip
            .Where(Trip => Trip.DeliveryDriver.CyclistID == driverid)
            .AverageAsync(Trip => (double)Trip.rating);
    }
    
    
    //Post to add a new meal.
    
    //Example Query:
    /*
     * {
         "Dish": "Pasta",
         "startTime": "10:15:00",
         "endTime": "12:30:00",
         "price": 55,
         "quantity": 3,
         "cookCPR": "0987654321"
       }
     */
    public static async Task AddMealAsync(ServiceDto.AddMealDto AddmealDto, dbcontext _context)
    {
        var cook = await _context.Cooks
            .Where(cook => cook.CookId == AddmealDto.CookId)
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
    
    //Update the Quantity of a Dish.
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



}