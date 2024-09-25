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
    public DbSet<TripAddresses> TripAddresses { get; set; }
    public DbSet<TripDetails> TripDetails { get; set; }
    public DbSet<Cook> Cooks { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<OrderMeal> OrderMeals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure composite keys
        modelBuilder.Entity<Meal>()
            .HasKey(m => new { m.CookCPR, m.Dish });

        modelBuilder.Entity<OrderMeal>()
            .HasKey(om => new { om.OrderID, om.CookCPR, om.Dish });

        modelBuilder.Entity<TripDetails>()
            .HasKey(td => new { td.TripID, td.AddressID });

        base.OnModelCreating(modelBuilder);
    }
}