namespace backendassign2.Models;

public class ServiceDto
{
    public class OrderMealDto
    {
        public int MealId { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; }
        public string Dish { get; set; }
        public int Quantity { get; set; }
    }
}