using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backendassign2.Entities;

    public class Cook
    {

    [Key] // Marks the cookCPR as the primary key
    [MaxLength(15)] // Equivalent to VARCHAR(15)
    public string CookCPR { get; set; }

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
    }

    public class DeliveryDriver
    {
        [Key]
        public int CyclistID { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; }

        [MaxLength(15)]
        public string PhoneNo { get; set; }

        [MaxLength(50)]
        public string BikeType { get; set; }
    }
    
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

        [MaxLength(50)]
        public string PaymentOption { get; set; }
    }

    public class CustomerOrder
    {
        [Key] 
        public int OrderID { get; set; }

        public int CustomerID { get; set; }
        
        [Column(TypeName = "numeric(2,0)")]
        public decimal? Rating { get; set; } // Numeric(2,0) can be represented as decimal
        public DateTime Timestamp { get; set; }

        // Navigation property
        public Customer Customer { get; set; }

        // Foreign key relationship
        [ForeignKey("CustomerID")] 
        public int CustomerForeignKey { get; set; }
    }
    
    public class TripAddresses
    {
        [Key]
        public int AddressID { get; set; }

        [MaxLength(255)]
        public string StreetName { get; set; }

        public int Zipcode { get; set; }

        public int HouseNumber { get; set; }

        [MaxLength(255)]
        public string City { get; set; }
    }
    
    public class TripDetails
    {
        [Key]
        public int TripID { get; set; }

    public int OrderID { get; set; }

    public int CyclistID { get; set; }

    public DateTime TripDate { get; set; }

    public TimeSpan Time { get; set; }

    public int? Rating { get; set; }
    
    [NotMapped]
    public string Address { get; set; }
    public int AddressID { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }

    // Relationships
    public DeliveryDriver DeliveryDriver { get; set; }

    public CustomerOrder CustomerOrder { get; set; }

    public TripAddresses TripAddresses { get; set; }

    // Foreign keys
    [ForeignKey("CyclistID")]
    public int DeliveryDriverForeignKey { get; set; }

    [ForeignKey("OrderID")]
    public int OrderForeignKey { get; set; }

    [ForeignKey("AddressID")]
    public int AddressForeignKey { get; set; }
    }
    
    public class Meal
    {
        [Key, Column(Order = 1)]
        [MaxLength(15)]
        public string CookCPR { get; set; }

        [Key, Column(Order = 2)]
        [MaxLength(255)]
        public string Dish { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        // Foreign key
        public Cook Cook { get; set; }

        [ForeignKey("CookCPR")]
        public string CookForeignKey { get; set; }
    }
    
    public class OrderMeal
    {
        [Key, Column(Order = 1)]
        public int OrderID { get; set; }

        [Key, Column(Order = 2)]
        [MaxLength(15)]
        public string CookCPR { get; set; }

        [Key, Column(Order = 3)]
        [MaxLength(255)]
        public string Dish { get; set; }

        public int Quantity { get; set; }

        // Foreign key relationships
        public CustomerOrder CustomerOrder { get; set; }

        public Meal Meal { get; set; }

        [ForeignKey("OrderID")]
        public int OrderForeignKey { get; set; }

        [ForeignKey("CookCPR, Dish")]
        public string MealForeignKey { get; set; }
    }

