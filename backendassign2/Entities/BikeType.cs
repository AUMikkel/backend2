using System.ComponentModel.DataAnnotations;

namespace backendassign2.Entities;

public class BikeType
{
    [Key]
    public string Type { get; set; }
    
    public ICollection<DeliveryDriver> DeliveryDriver { get; set; }
}