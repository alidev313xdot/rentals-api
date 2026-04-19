namespace Rentals.API.Models
{
    public class Property
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public decimal PricePerMonth { get; set; }
        public int Bedrooms { get; set; }
        public bool IsFurnished { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsVerified { get; set; } = false; // Only admin can set it true/false 
        public string ContactNumber { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

/*
 Every property here becomes a column in data base later. 
 IsVarified is your admin approval Admin Flag 
 */ 

/*
 My Questions 

1. 
 
 */ 
