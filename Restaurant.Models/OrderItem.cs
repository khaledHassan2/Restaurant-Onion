using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Restaurant.Models
{
    public class OrderItem : ModelBase
    {
        public int OrderId { get; set; }
        [JsonIgnore]
        [System.Runtime.Serialization.IgnoreDataMember]
        public Order Order { get; set; }

        public int MenuItemId { get; set; }
        [JsonIgnore]
        public MenuItem MenuItem { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal Subtotal => UnitPrice * Quantity;
    }

}
