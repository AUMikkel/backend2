using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign3.Entities
{
    public class Meal
    {
        [Key]
        public int mealId { get; set; } // Changed casing for consistency
        
        [MaxLength(255)]
        public string Dish { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        // Foreign key for ApiUser (Cook)
        public string CookId { get; set; }

        // Navigation property for Cook
        public ApiUser Cook { get; set; }

        // Other relationships
        public ICollection<CustomerOrder> CustomerOrder { get; set; }
        
        public ICollection<OrderMeal> OrderMeal { get; set; }
    }
}