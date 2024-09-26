using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class Meal
{
    [Key]
    public int mealId { get; set; }
    
    //[Key, Column(Order = 1)]
    // [MaxLength(15)]
    // public string CookCPR { get; set; }

    //[Key, Column(Order = 2)]
    [MaxLength(255)]
    public string Dish { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }
        
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }
    
    public Cook Cook { get; set; }
    
    public ICollection<CustomerOrder> CustomerOrder { get; set; }
    
    public ICollection<OrderMeal> OrderMeal { get; set; }

}