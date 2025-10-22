using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class MenuItem:ModelBase
    {
        [Required, StringLength(150)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public int PreparationTime { get; set; } = 10;

        public int DailyOrderCount { get; set; } = 0;

        [StringLength(250)]
        public string? ImageUrl { get; set; } = "/images/default-item.jpg"; // default fallback

        // Foreign Key
        public int CategoryId { get; set; }
        public MenuCategory? Category { get; set; }
    }
}
