using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class CustomerOrder
{
    [Key] 
    public int OrderId{ get; set; }
    
    public int Price { get; set; }

    //public int CustomerID { get; set; }
        
    //[Column(TypeName = "numeric(2,0)")]
    //public decimal? Rating { get; set; } // Numeric(2,0) can be represented as decimal
    public DateTime Timestamp { get; set; }

    // Navigation property
    public Customer Customer { get; set; }
    
    public ICollection<Meal> Meal { get; set; }
    
    public ICollection<OrderMeal> OrderMeal { get; set; }
    
    public Trip Trip { get; set; }

    
}