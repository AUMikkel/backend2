using backendassign2.Entities;
using System.Linq;
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
            .Where(meal => meal.CookCPR == cookCPR)
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
    public static async Task<List<OrderMeal>> GetOrderDetailsAsync(int orderId, dbcontext _context)
    {
        return await _context.OrderMeals
            .Where(orderMeal => orderMeal.OrderID == orderId)
            .Join(
                _context.Cooks,
                orderMeal => orderMeal.CookCPR,
                cook => cook.CookCPR,
                (orderMeal, cook) => new OrderMeal
                {
                    Dish = orderMeal.Dish,
                    Quantity = orderMeal.Quantity,
                    CookCPR = cook.CookCPR
                })
            .ToListAsync();
    }
    public static async Task<List<TripDetails>> GetTripDetailsAsync(int tripId, dbcontext _context)
    {
        return await _context.TripDetails
            .Where(tripDetail => tripDetail.TripID == tripId)
            .Join(
                _context.TripAddresses,
                tripDetail => tripDetail.AddressID,
                address => address.AddressID,
                (tripDetail, address) => new TripDetails
                {
                    Address = address.StreetName + " " + address.HouseNumber + ", " + address.Zipcode,
                    Time = tripDetail.Time,
                    Type = tripDetail.Type
                })
            .OrderBy(tripDetail => tripDetail.Time)
            .ToListAsync();
    }
    public static async Task<double?> GetAverageRatingForCookAsync(string cookCPR, dbcontext _context)
    {
        return await _context.CustomerOrders
            .Join(
                _context.OrderMeals,
                order => order.OrderID,
                meal => meal.OrderID,
                (order, meal) => new { order.Rating, meal.CookCPR })
            .Where(orderMeal => orderMeal.CookCPR == cookCPR)
            .AverageAsync(orderMeal => (double?)orderMeal.Rating);
    }
    public static async Task<List<dynamic>> GetCyclistEarningsAsync(int cyclistID, dbcontext _context)
    {
        // First part: Calculate trip durations and hours worked
        var tripDurations = await _context.TripDetails
            .Where(t1 => t1.CyclistID == cyclistID && t1.Type == "pickup")
            .Join(
                _context.TripDetails.Where(t2 => t2.Type == "delivery"),
                t1 => t1.TripID,
                t2 => t2.TripID,
                (t1, t2) => new
                {
                    t1.TripID,
                    t1.CyclistID,
                    PickupTime = t1.Time,
                    DeliveryTime = t2.Time,
                    TripDate = t1.TripDate,
                    HoursWorked = (double)(t2.Time - t1.Time).TotalMinutes / 60.0,  // Calculate hours worked
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
    }
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
    }
    
    

   
}