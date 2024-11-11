using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class DeliveryDriver
{
    [Key]
    public int CyclistID { get; set; }

    [MaxLength(255)]
    public string FullName { get; set; }

    [MaxLength(15)]
    public string PhoneNo { get; set; }
    
    public BikeType BikeType { get; set; }
    public ICollection<Trip> Trip { get; set; }

    public string UserId { get; set; }
}