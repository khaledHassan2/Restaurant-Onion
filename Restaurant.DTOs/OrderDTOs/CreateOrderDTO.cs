using Restaurant.Models;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        public int Id { get; set; }

        [Required]
        public OrderType Type { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [MaxLength(250)]
        public string? DeliveryAddress { get; set; }

        [Range(0, 100)]
        public decimal TaxPercent { get; set; } = 8.5m;

        [Range(0, 100)]
        public decimal Discount { get; set; } = 0m;

        public ICollection<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();

        public DateTime? LastStatusChange { get; set; }

        public string? CustomerId { get; set; }

        public string? CustomerUserName { get; set; }
    }
}
