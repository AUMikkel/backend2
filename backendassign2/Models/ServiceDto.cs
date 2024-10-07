using System.ComponentModel.DataAnnotations;

namespace backendassign2.Models;

public class ServiceDto
{
    public class CookDto
    {
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string CookCPR { get; set; }
    }

    public class MealDto
    {
        public string Dish { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    public class AddMealDto
    {
        public string Dish { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        //Price must be above 0
        //[Range(0, double.MaxValue, ErrorMessage = "Price must be above 0")]
        [PriceValidation]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CookCPR { get; set; }
        
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