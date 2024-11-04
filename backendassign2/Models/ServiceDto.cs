using System.ComponentModel.DataAnnotations;
using backendassign2.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backendassign2.Models;

public class ServiceDto
{
    public class CookDto
    {
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public int CookId { get; set; }
        public bool HasPassedFoodSafetyCourse { get; set; }
    }

    public class MealDto
    {
        public string Dish { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    

    public class AddMealDto
    {
        public string Dish { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //Price must be above 0
        [PriceValidation]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CookId { get; set; }
        
    }

  
    public class OrderMealDto
    {
        
        public string Dish { get; set; }
        public int Quantity { get; set; }
        
        public string FullName { get; set; }
        
    }
    
    public class UpdateQuantityDto
    {
        public int Quantity { get; set; }
        public int mealId { get; set; }
    }

    public class TripDto
    {
        public string address { get; set; }
        public TimeOnly tripDate { get; set; }
        public string type { get; set; }
    }
    public class Log
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Timestamp")]  // Ensure the name matches the MongoDB field name if it's different
        public DateTime Timestamp { get; set; }
        
        
        public string Level { get; set; }
        public string MessageTemplate { get; set; }
        public string RenderedMessage { get; set; }

        public Properties Properties { get; set; }
    }

    public class Properties
    {
        public string EnvName { get; set; }
        public string SourceContext { get; set; }
        public string UtcTimestamp { get; set; }
        public string address { get; set; }  // Added to match "address" field from MongoDB

        public EventId EventId { get; set; }  // Define EventId as a nested object
    }

    public class EventId
    {
        public int Id { get; set; }     // Matches the "Id" field in EventId object
        public string Name { get; set; } // Matches the "Name" field in EventId object
    }
    

   
    
}