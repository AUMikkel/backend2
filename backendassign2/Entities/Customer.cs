using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendassign2.Entities;

public class Customer
{
    [Key]
    public int CustomerID { get; set; }

    [MaxLength(255)]
    public string FullName { get; set; }

    [MaxLength(15)]
    public string PhoneNo { get; set; }

    [MaxLength(255)]
    public string StreetName { get; set; }

    public int Zipcode { get; set; }

    [MaxLength(255)]
    public string City { get; set; }

    public int HouseNumber { get; set; }

    public PaymentOption PaymentOption { get; set; }
    
    public ICollection<CustomerOrder> CustomerOrder { get; set; }
    
}