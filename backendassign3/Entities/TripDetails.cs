using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign3.Entities;

public class TripDetails
{
    [Key]
    public int DetailId { get; set; }

    public DateTime TripDate { get; set; }

    public int? Rating { get; set; }
    
    public string Address { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }
    
    public Trip Trip { get; set; }

}