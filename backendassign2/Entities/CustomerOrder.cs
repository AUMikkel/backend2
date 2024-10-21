using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class CustomerOrder
{
    [Key] 
    public int OrderId{ get; set; }
    
    public int Price { get; set; }

   
    public DateTime Timestamp { get; set; }

    public Customer Customer { get; set; }
    
    public ICollection<Meal> Meal { get; set; }
    
    public ICollection<OrderMeal> OrderMeal { get; set; }
    
    public Trip Trip { get; set; }

    
}