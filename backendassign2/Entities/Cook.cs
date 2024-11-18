using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class Cook
{
    
    [Key]
    public string CookId { get; set; }
    
    public ApiUser ApiUser { get; set; }
    
    public bool HasPassedFoodSafetyCourse { get; set; } 

}