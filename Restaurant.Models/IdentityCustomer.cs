using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class IdentityCustomer : IdentityUser
    {
        

        [Phone]
        public string? Phone { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }

}
