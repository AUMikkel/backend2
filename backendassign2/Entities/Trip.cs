using System.ComponentModel.DataAnnotations;

namespace backendassign2.Entities;

public class Trip
{
    [Key]
    public int TripId { get; set; }
    public int rating { get; set; }
    
    public ICollection<CustomerOrder> CustomerOrder { get; set; }
    public ICollection<TripDetails> TripDetails { get; set; }
    
    public ApiUser Driver { get; set; }
}