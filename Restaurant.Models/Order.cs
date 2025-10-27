using System.ComponentModel.DataAnnotations.Schema;
namespace Restaurant.Models
{
    public enum OrderType { DineIn, Takeout, Delivery }
    public enum OrderStatus
    {
        Pending,      // الـ Cart
        Confirmed,    // بعد الـ Checkout
        Processing,
        Ready,
        Delivered,
        Cancelled
    }

    public class Order : ModelBase
    {
        public OrderType Type { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string? DeliveryAddress { get; set; }

        public decimal TaxPercent { get; set; } = 8.5m;

        public decimal Discount { get; set; } = 0m;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public DateTime? LastStatusChange { get; set; }


        [ForeignKey(nameof(Customer))]
        public string? CustomerId { get; set; }
        public IdentityCustomer? Customer { get; set; }

        [NotMapped]
        public decimal Subtotal => Items?.Sum(i => i.Subtotal) ?? 0;

        [NotMapped]
        public decimal Tax => Math.Round(Subtotal * (TaxPercent / 100), 2);

        [NotMapped]
        public decimal Total => Math.Round(Subtotal + Tax - Discount, 2);
        [NotMapped]
        public decimal? TotalAmount { get; set; }

    }

}
