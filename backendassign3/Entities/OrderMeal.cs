using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign3.Entities;

public class OrderMeal
{
    public int MealId { get; set; }
    public int OrderId { get; set; }
    
    public int Rating { get; set; }

    public int Quantity { get; set; }

  
    public CustomerOrder CustomerOrder { get; set; }

    public Meal Meal { get; set; }
    
    

}