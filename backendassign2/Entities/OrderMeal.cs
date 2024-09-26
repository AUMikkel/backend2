using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class OrderMeal
{
    public int MealId { get; set; }
    public int OrderId { get; set; }
    
    public int Rating { get; set; }
    // [MaxLength(15)]
    // public string CookCPR { get; set; }

    // [MaxLength(255)]
    // public string Dish { get; set; }

    public int Quantity { get; set; }

    // Foreign key relationships
    public CustomerOrder CustomerOrder { get; set; }

    public Meal Meal { get; set; }
    
    

}