using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace backendassign2.Entities
{
    public class ApiUser : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; }
        
        public string Address { get; set; }

        public string PhoneNo { get; set; }

        // Navigation properties
        public ICollection<Trip> Trip { get; set; }
        public ICollection<CustomerOrder> CustomerOrder { get; set; }

        // Relationship with Meals
        public ICollection<Meal> Meals { get; set; } // Cooked meals
        
        
    }
}