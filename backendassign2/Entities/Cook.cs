using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class Cook
{
    //[Key]
    //[MaxLength(15)] // Equivalent to VARCHAR(15)
    //public string CookCPR { get; set; }
    
    [Key]
    public int CookId { get; set; }
    
    [MaxLength(255)] // Equivalent to VARCHAR(255)
    public string FullName { get; set; }
    
    [NotMapped]
    public string Address { get; set; }

    [MaxLength(255)] // Equivalent to VARCHAR(255)
    public string StreetName { get; set; }

    public int Zipcode { get; set; } // Equivalent to INT

    public int HouseNumber { get; set; } // Equivalent to INT

    [MaxLength(255)] // Equivalent to VARCHAR(255)
    public string City { get; set; }

    [MaxLength(15)] // Equivalent to VARCHAR(15)
    public string PhoneNo { get; set; }
    
    public bool HasPassedFoodSafetyCourse { get; set; } 
}