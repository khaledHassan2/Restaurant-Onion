using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
       
            public int MenuItemId { get; set; }
            public string? MenuItemName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }

            [NotMapped]
            public decimal Subtotal => Quantity * UnitPrice;
    }
    
}
