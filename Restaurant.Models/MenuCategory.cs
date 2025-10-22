using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class MenuCategory:ModelBase
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
