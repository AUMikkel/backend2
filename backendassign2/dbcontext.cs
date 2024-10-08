using Microsoft.EntityFrameworkCore;
using backendassign2.Entities;
namespace backendassign2;

public class dbcontext : DbContext
{
    public dbcontext(DbContextOptions<dbcontext> options) : base(options)
    {
    }
    
    public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerOrder> CustomerOrders { get; set; }
    public DbSet<TripDetails> TripDetails { get; set; }
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<OrderMeal> OrderMeals { get; set; }
    public DbSet<PaymentOption> PaymentOptions { get; set; }
    public DbSet<BikeType> BikeType { get; set; }
    public DbSet<Trip> Trip { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure composite keys
        // modelBuilder.Entity<Meal>()
        //     .HasKey(m => new { m.CookCPR, m.Dish });

        modelBuilder.Entity<OrderMeal>()
            .HasKey(om => new { om.OrderId, om.MealId });

        modelBuilder.Entity<OrderMeal>()
            .HasOne(om => om.CustomerOrder)
            .WithMany(co => co.OrderMeal);
        modelBuilder.Entity<OrderMeal>()
            .HasOne(m => m.Meal)
            .WithMany(b => b.OrderMeal);
        
            
        
        // modelBuilder.Entity<CustomerOrder>()
        //     .HasMany(c => c.Meal)
        //     .WithMany(m => m.CustomerOrder)
        //     .UsingEntity(x => x.ToTable("OrderMeal").Property<int>("CustomersQuantity"))
        //     .Property<int>("Rating");
            
        
        base.OnModelCreating(modelBuilder);
    }
    
    // Seed data
    public void Seed()
    {
        
        PaymentOption cash = new PaymentOption { Option ="Cash" };
        PaymentOption creditCard = new PaymentOption { Option = "Credit Card" };
        PaymentOption mobilePay = new PaymentOption { Option = "MobilePay" };
        
        BikeType mountainBike = new BikeType { Bike = "mountainBike" };
        BikeType roadBike = new BikeType { Bike = "roadBike" };
        BikeType cityBike = new BikeType { Bike = "cityBike" };
        BikeType electricBike = new BikeType { Bike = "electricBike" };
        
        BikeType.AddRange(mountainBike, roadBike, cityBike, electricBike);
        PaymentOptions.AddRange(cash, creditCard, mobilePay);
        
        Customer customer1 = new Customer
        {
            FullName = "John Doe",
            PhoneNo = "12345678",
            StreetName = "Main Street",
            Zipcode = 1234,
            HouseNumber = 1,
            City = "City",
            PaymentOption = cash
        };
        //customer1.PaymentOption = cash;
        Customer customer2 = new Customer
        {
            FullName = "Jane Doe",
            PhoneNo = "87654321",
            StreetName = "Third Street",
            Zipcode = 4323,
            HouseNumber = 3,
            City = "City",
            PaymentOption = creditCard
        };
       //customer2.PaymentOption = creditCard;
        Customer customer3 = new Customer
        {
            FullName = "Alice Doe",
            PhoneNo = "12348765",
            StreetName = "Third Street",
            Zipcode = 5678,
            HouseNumber = 3,
            City = "City",
            PaymentOption = mobilePay
        };
        //customer3.PaymentOption = mobilePay;
        Customers.AddRange(customer1, customer2, customer3);
        
        Cook cook1 = new Cook
        {
            FullName = "John Cook",
            PhoneNo = "12345678",
            StreetName = "Main Street",
            Zipcode = 1234,
            HouseNumber = 1,
            City = "City",
            //CookCPR = "1234567890"
        };
        Cook cook2 = new Cook
        {
            FullName = "Jane Cook",
            PhoneNo = "87654321",
            StreetName = "Second Street",
            Zipcode = 4321,
            HouseNumber = 2,
            City = "City",
            //CookCPR = "0987654321"
        };
        Cooks.AddRange(cook1, cook2);
        
        Meal meal1 = new Meal
        {
            Cook = cook1,
            Dish = "Pizza",
            Quantity = 1,
            Price = 100,
            //StartTime = new TimeOnly(12, 0),
            //EndTime = new TimeOnly(13,0)
        };
        
        Meal meal2 = new Meal
        {
            Cook = cook2,
            Dish = "Pasta",
            Quantity = 2,
            Price = 200,
            //StartTime = new TimeOnly(14, 0),
            //EndTime = new TimeOnly(15,0)
        };
        Meal meal3 = new Meal
        {
            Cook = cook2,
            Dish = "Panini",
            Quantity = 6,
            Price = 150,
           // StartTime = new TimeOnly(17, 0),
           // EndTime = new TimeOnly(18,30)
        };
        
        
        DeliveryDriver driver1 = new DeliveryDriver
        {
            FullName = "John Driver",
            PhoneNo = "12345678",
            BikeType = mountainBike
        };
        DeliveryDriver driver2 = new DeliveryDriver
        {
            FullName = "Jane Driver",
            PhoneNo = "87654321",
            BikeType = roadBike
        };
        DeliveryDrivers.AddRange(driver1, driver2);
        
        CustomerOrder order1 = new CustomerOrder
        {
            Customer = customer1,
            Timestamp = new DateTime(2024,08,12, 12,0,0),
            Price = 100,
        };
        CustomerOrder order2 = new CustomerOrder
        {
            Customer = customer2,
            Timestamp = new DateTime(2024,08,12, 14,0,0),
            Price = 200
        };
        
        CustomerOrder order3 = new CustomerOrder
        {
            Customer = customer2,
            Timestamp = new DateTime(2024,08,12, 15,0,0),
            Price = 275
        };
        CustomerOrder order4 = new CustomerOrder
        {
            Customer = customer2,
            Timestamp = new DateTime(2024,08,12, 16,0,0),
            Price = 220
        };
        
        CustomerOrder order5 = new CustomerOrder
        {
            Customer = customer1,
            Timestamp = new DateTime(2024,08,12, 17,0,0),
            Price = 225
        };
        
        
        OrderMeal orderMeal1 = new OrderMeal
        {
            CustomerOrder = order1,
            Meal = meal1,
            Quantity = 1,
            Rating = 4
        };
        OrderMeal orderMeal2 = new OrderMeal
        {
            CustomerOrder = order2,
            Meal = meal2,
            Quantity = 2,
            Rating = 5
        };
        
        OrderMeal orderMeal3 = new OrderMeal
        {
            CustomerOrder = order3,
            Meal = meal3,
            Quantity = 1,
            Rating = 2
        };
        
        OrderMeal orderMeal4 = new OrderMeal
        {
            CustomerOrder = order4,
            Meal = meal3,
            Quantity = 1,
            Rating = 2
        };
        
        OrderMeal orderMeal5 = new OrderMeal
        {
            CustomerOrder = order5,
            Meal = meal3,
            Quantity = 1,
            Rating = 2
        };
        
        

        Trip trip1 = new Trip
        {
            DeliveryDriver = driver1,
            rating = 5
        };
        
        Trip trip2 = new Trip
        {
            DeliveryDriver = driver1,
            rating = 4
        };
        
        
        Trip trip3 = new Trip
        {
            DeliveryDriver = driver1,
            rating = 2
        };
        
        TripDetails tripdetails1 = new TripDetails
        {
            Address = cook1.StreetName + " " + cook1.HouseNumber + ", " + cook1.Zipcode + " " + cook1.City,
            TripDate = new DateTime(2024,08,12, 12,0,0),
            Type = "Pickup",
            Trip = trip1
        };
        TripDetails tripdetails2 = new TripDetails
        {
            Address = customer2.StreetName + " " + customer2.HouseNumber + ", " + customer2.Zipcode + " " + customer2.City,
            TripDate = new DateTime(2024,08,12, 13,0,0),
            Type = "Delivery",
            Trip = trip1
        };
        
        TripDetails tripdetails3= new TripDetails
        {
            Address = cook2.StreetName + " " + cook2.HouseNumber + ", " + cook2.Zipcode + " " + cook2.City,
            TripDate = new DateTime(2024,09,12, 13,0,0),
            Type = "Pickup",
            Trip = trip2
        };
        
        TripDetails tripdetails4 = new TripDetails
        {
            Address = customer2.StreetName + " " + customer2.HouseNumber + ", " + customer2.Zipcode + " " + customer2.City,
            TripDate = new DateTime(2024,09,12, 13,45,0),
            Type = "Delivery",
            Trip = trip2
        };
        
        TripDetails tripdetails5 = new TripDetails
        {
            Address = cook2.StreetName + " " + cook2.HouseNumber + ", " + cook2.Zipcode + " " + cook2.City,
            TripDate = new DateTime(2024,08,12, 15,0,0),
            Type = "Pickup",
            Trip = trip3
        };
        
        TripDetails tripdetails6 = new TripDetails
        {
            Address = customer2.StreetName + " " + customer2.HouseNumber + ", " + customer2.Zipcode + " " + customer2.City,
            TripDate = new DateTime(2024,08,12, 16,0,0),
            Type = "Delivery",
            Trip = trip3
        };
        
        Meals.AddRange(meal1, meal2, meal3);
        CustomerOrders.AddRange(order1, order2, order3, order4, order5);
        OrderMeals.AddRange(orderMeal1, orderMeal2, orderMeal3, orderMeal4, orderMeal5);
        Trip.AddRange(trip1,trip2,trip3);
        order1.Trip = trip1;
        order2.Trip = trip1;
        order3.Trip = trip2;
        order4.Trip = trip3;
        order5.Trip = trip3;
        
        TripDetails.AddRange(tripdetails1, tripdetails2, tripdetails3, tripdetails4, tripdetails5, tripdetails6);
        
        SaveChanges();
        
        
        
    }
    
}