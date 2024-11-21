using System.ComponentModel.DataAnnotations;
using backendassign2.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backendassign2.DTOs;

public class ServiceDto
{
    public class CookDto
    {
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string CookId { get; set; }
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
    
    [BsonIgnoreExtraElements]
    public class LogDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("Level")]
        public string Level { get; set; } = "";

        [BsonElement("Properties")] 
        public LogProperties Properties { get; set; } = new LogProperties();

    }
    [BsonIgnoreExtraElements]
    public class LogProperties
    {
        
        [BsonElement("LogInfo")]
        public LogInfo LogInfo { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class LogInfo
    {
        [BsonElement("Operation")]
        public string Operation { get; set; } = "";
        
        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }
        
        [BsonElement("User")]
        public string User { get; set; } = "";
    }

   
    
}