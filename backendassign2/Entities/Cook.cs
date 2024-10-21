using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class Cook
{
    
    [Key]
    public int CookId { get; set; }
    
    [MaxLength(255)] 
    public string FullName { get; set; }
    
    [NotMapped]
    public string Address { get; set; }

    [MaxLength(255)] 
    public string StreetName { get; set; }

    public int Zipcode { get; set; } 

    public int HouseNumber { get; set; }

    [MaxLength(255)] 
    public string City { get; set; }

    [MaxLength(15)] 
    public string PhoneNo { get; set; }
    
    public bool HasPassedFoodSafetyCourse { get; set; } 
}