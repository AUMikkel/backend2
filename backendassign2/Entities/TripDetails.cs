using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class TripDetails
{
    [Key]
    public int DetailId { get; set; }

   // public int OrderID { get; set; }

    //public int CyclistID { get; set; }

    public DateTime TripDate { get; set; }

    //public TimeOnly Time { get; set; }

    public int? Rating { get; set; }
    
    public string Address { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }

    // Relationships
    
    public Trip Trip { get; set; }

}