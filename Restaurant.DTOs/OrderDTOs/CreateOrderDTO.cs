using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
       
        public DateTime? LastStatusChange { get; set; }

        public string? CustomerId { get; set; } = null;
        public IdentityCustomer? Customer { get; set; }
    }
}
