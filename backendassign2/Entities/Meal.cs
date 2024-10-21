using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace backendassign2.Entities;

public class Meal
{
    [Key]
    public int mealId { get; set; }
    
    [MaxLength(255)]
    public string Dish { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
        
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }
    
    public Cook Cook { get; set; }
    
    public ICollection<CustomerOrder> CustomerOrder { get; set; }
    
    public ICollection<OrderMeal> OrderMeal { get; set; }

}