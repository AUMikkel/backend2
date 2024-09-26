using System.ComponentModel.DataAnnotations;

namespace backendassign2.Entities;

public class PaymentOption
{
    [Key]
    public string Type { get; set; }
    
    public ICollection<Customer> Customer { get; set; }
}