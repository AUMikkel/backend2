using Microsoft.EntityFrameworkCore;
using backendassign3.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
namespace backendassign3;

public class dbcontext : IdentityDbContext<ApiUser>
{
    public dbcontext(DbContextOptions<dbcontext> options) : base(options)
    {
    }
    
    public DbSet<CustomerOrder> CustomerOrders { get; set; }
    public DbSet<TripDetails> TripDetails { get; set; }
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<OrderMeal> OrderMeals { get; set; }
    public DbSet<Trip> Trip { get; set; }
    public DbSet<ApiUser> ApiUsers { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        

        modelBuilder.Entity<OrderMeal>()
            .HasKey(om => new { om.OrderId, om.MealId });

        modelBuilder.Entity<OrderMeal>()
            .HasOne(om => om.CustomerOrder)
            .WithMany(co => co.OrderMeal);
        modelBuilder.Entity<OrderMeal>()
            .HasOne(m => m.Meal)
            .WithMany(b => b.OrderMeal);
        modelBuilder.Entity<Meal>()
            .HasOne(m => m.Cook)       // Each Meal has one Cook
            .WithMany(u => u.Meals)   // Each ApiUser (Cook) can have many Meals
            .HasForeignKey(m => m.CookId)  // Foreign key in Meal
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<CustomerOrder>()
            .HasOne(co => co.Customer)
            .WithMany(u => u.CustomerOrder)
            .OnDelete(DeleteBehavior.NoAction);
        
        base.OnModelCreating(modelBuilder);
    }
    
    // Seed data
    /*
    public void Seed()
    {
        
        ApiUser customer1 = new ApiUser()
        {
            FullName = "John Doe",
            phoneNo = "12345678",
            Address = "Main Street 1, 1234 City"
        };
       
        ApiUser customer2 = new ApiUser()
        {
            FullName = "Jane Doe", 
            phoneNo = "87654321",
            Address = "Third Street 3, 4323 City"
        };
        ApiUsers.AddRange(customer1, customer2);
        
        ApiUser cook1 = new ApiUser()
        {
            FullName = "John Cook",
            phoneNo = "12345678",
            Address = "Main Street 1, 1234 City"
        };
        ApiUser cook2 = new ApiUser()
        {
            FullName = "Jane Cook",
            phoneNo = "87654321",
            Address = "Second Street 2, 4321 City"
        };
        ApiUsers.AddRange(cook1, cook2);
        
        Meal meal1 = new Meal
        {
            Cook = cook1,
            Dish = "Pizza",
            Quantity = 1,
            Price = 100,
            StartTime = new DateTime(2024,08,12, 12,0,0),
            EndTime = new DateTime(2024,08,12, 18,0,0)
            
        };
        
        Meal meal2 = new Meal
        {
            Cook = cook2,
            Dish = "Pasta",
            Quantity = 2,
            Price = 200,
            StartTime = new DateTime(2024,08,12, 12,0,0),
            EndTime = new DateTime(2024,08,12, 18,0,0)
        };
        Meal meal3 = new Meal
        {
            Cook = cook2,
            Dish = "Panini",
            Quantity = 6,
            Price = 150,
            StartTime = new DateTime(2024,08,12, 12,0,0),
            EndTime = new DateTime(2024,08,12, 18,0,0)
        };
        
        
        ApiUser driver1 = new ApiUser()
        {
            FullName = "John Driver",
            phoneNo = "12345678"
        };
        ApiUser driver2 = new ApiUser()
        {
            FullName = "Jane Driver",
            phoneNo = "87654321",
        };
        ApiUsers.AddRange(driver1, driver2);
        
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
            Driver = driver1,
            rating = 5
        };
        
        Trip trip2 = new Trip
        {
            Driver = driver1,
            rating = 4
        };
        
        
        Trip trip3 = new Trip
        {
            Driver = driver1,
            rating = 2
        };
        
        TripDetails tripdetails1 = new TripDetails
        {
            Address = cook1.Address,
            TripDate = new DateTime(2024,08,12, 12,0,0),
            Type = "Pickup",
            Trip = trip1
        };
        TripDetails tripdetails2 = new TripDetails
        {
            Address = customer1.Address,
            TripDate = new DateTime(2024,08,12, 13,0,0),
            Type = "Delivery",
            Trip = trip1
        };
        
        TripDetails tripdetails3= new TripDetails
        {
            Address = cook2.Address,
            TripDate = new DateTime(2024,09,12, 13,0,0),
            Type = "Pickup",
            Trip = trip2
        };
        
        TripDetails tripdetails4 = new TripDetails
        {
            Address = customer2.Address,
            TripDate = new DateTime(2024,09,12, 13,45,0),
            Type = "Delivery",
            Trip = trip2
        };
        
        TripDetails tripdetails5 = new TripDetails
        {
            Address = cook2.Address,
            TripDate = new DateTime(2024,08,12, 15,0,0),
            Type = "Pickup",
            Trip = trip3
        };
        
        TripDetails tripdetails6 = new TripDetails
        {
            Address = customer2.Address,
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
    */
    
}