using backendassign2.Entities;
using System.Linq;
using backendassign2.Models;
using Microsoft.EntityFrameworkCore;

namespace backendassign2.Services;

public static class CookService
{
    static CookService()
    {
    }
    public static async Task<List<Cook>> GetCooks(dbcontext _context)
    {
        return await _context.Cooks
            .Select(cook => new Cook
            {
                FullName = cook.FullName,
                Address = cook.StreetName + " " + cook.HouseNumber + ", " + cook.Zipcode + ", " + cook.City,
                PhoneNo = cook.PhoneNo,
                CookCPR = cook.CookCPR
            })
            .ToListAsync();
    }
    public static async Task<List<Meal>> GetDishesByCookAsync(string cookCPR, dbcontext _context)
    {
        return await _context.Meals
            .Where(meal => meal.Cook.CookCPR == cookCPR)
            .Select(meal => new Meal
            {
                Dish = meal.Dish,
                Quantity = meal.Quantity,
                Price = meal.Price,
                StartTime = meal.StartTime,
                EndTime = meal.EndTime
                
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
                    MealId = meal.mealId,
                    OrderId = orderMeal.OrderId,
                    Dish = meal.Dish,
                    Quantity = orderMeal.Quantity,
                    Rating = orderMeal.Rating
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
    public static async Task<List<Trip>> GetTripDetailsAsync(int tripId, dbcontext _context)
    {
        return await _context.Trip
            .Where(trip => trip.TripId == tripId)
            .ToListAsync();
    }
    /*public static async Task<List<TripDetails>> GetTripDetailsAsync(int tripID, dbcontext _context)
    {
        return await _context.TripDetails
            .Where(tripDetail => tripDetail.TripID == tripID)
            .OrderBy(tripDetail => tripDetail.Time)
            .ToListAsync();
    }*/
    public static async Task<double?> GetAverageRatingForCookAsync(string cookCPR, dbcontext _context)
    {
        return await _context.OrderMeals
            .Where(orderMeal => orderMeal.Meal.Cook.CookCPR == cookCPR)
            .AverageAsync(orderMeal => (double?)orderMeal.Rating);
    }
    
    /*public static async Task<List<dynamic>> GetCyclistEarningsAsync(int DeliveryDriver, dbcontext _context)
    {
        // First part: Calculate trip durations and hours worked
        var tripDurations = await _context.TripDetails
            .Where(t1 => t1.DeliveryDriver.CyclistID == DeliveryDriver && t1.Type == "pickup")
            .Join(
                _context.TripDetails.Where(t2 => t2.Type == "delivery"),
                t1 => t1.TripID,
                t2 => t2.TripID,
                (t1, t2) => new
                {
                    t1.TripID,
                    t1.DeliveryDriver.CyclistID,
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
                CyclistID = g.Key.CyclistID,
                Month = g.Key.Month,
                Hours = g.Sum(td => td.HoursWorked),  // Sum the hours worked
                Earnings = g.Sum(td => td.HoursWorked) * 150  // Calculate earnings
            })
            .OrderBy(e => e.CyclistID)
            .ThenBy(e => e.Month)
            .ToList<dynamic>();  // Return as a dynamic list

        return earnings;
    }*/
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
    
    /*
    public static async Task<List<dynamic>> GetAverageRatingForDriversAsync(dbcontext _context)
    {
        return await _context.TripDetails
            .Join(
                _context.DeliveryDrivers,
                tripDetail => tripDetail.CyclistID,
                driver => driver.CyclistID,
                (tripDetail, driver) => new
                {
                    driver.FullName,
                    tripDetail.Rating
                })
            .Where(td => td.Rating.HasValue)
            .GroupBy(td => td.FullName)
            .Select(g => new
            {
                Driver = g.Key,
                AverageRating = g.Average(td => (double?)td.Rating)
            })
            .OrderByDescending(dto => dto.AverageRating)
            .ToListAsync<dynamic>();
    }*/
    
    

   
}