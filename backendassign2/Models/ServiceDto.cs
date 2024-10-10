using System.ComponentModel.DataAnnotations;
using backendassign2.Attributes;

namespace backendassign2.Models;

public class ServiceDto
{
    public class CookDto
    {
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public int CookId { get; set; }
        public bool HasPassedFoodSafetyCourse { get; set; }
    }

    public class MealDto
    {
        public string Dish { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    

    public class AddMealDto
    {
        public string Dish { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //Price must be above 0
        [PriceValidation]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CookId { get; set; }
        
    }

  
    public class OrderMealDto
    {
        
        public string Dish { get; set; }
        public int Quantity { get; set; }
        
        public string FullName { get; set; }
        
    }
    
    public class UpdateQuantityDto
    {
        public int Quantity { get; set; }
        public int mealId { get; set; }
    }

    public class TripDto
    {
        public string address { get; set; }
        public TimeOnly tripDate { get; set; }
        public string type { get; set; }
    }

    public class EarningsDto
    {
        public string month { get; set; }
        public decimal hours { get; set; }
        public decimal earnings { get; set; }
    }
    
}