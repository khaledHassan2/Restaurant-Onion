using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Context
{
    public class RestaurantDbContext:IdentityDbContext<IdentityCustomer>
    {
       public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options): base(options)
        {
        }

        public DbSet<IdentityCustomer> IdentityCustomers { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MenuCategory>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<MenuItem>().HasQueryFilter(i => !i.IsDeleted);
            builder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
            builder.Entity<OrderItem>().HasQueryFilter(o => !o.IsDeleted);
            builder.Entity<Order>()
                   .HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);

            builder.Entity<MenuCategory>().HasData(
                new MenuCategory { Id = 1, Name = "Main Dishes", IsDeleted = false },
                new MenuCategory { Id = 2, Name = "Drinks", IsDeleted = false },
                new MenuCategory { Id = 3, Name = "Desserts", IsDeleted = false }
            );

            // 🔹 Seed Menu Items
            builder.Entity<MenuItem>().HasData
            (
                new MenuItem
                {
                    Id = 1,
                    Name = "Grilled Chicken",
                    Price = 120.00m,
                    IsAvailable = true,
                    PreparationTime = 20,
                    DailyOrderCount = 0,
                    ImageUrl = "/images/chicken.jpg",
                    CategoryId = 1,
                    IsDeleted = false
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Beef Burger",
                    Price = 95.00m,
                    IsAvailable = true,
                    PreparationTime = 15,
                    DailyOrderCount = 0,
                    ImageUrl = "/images/burger.jpg",
                    CategoryId = 1,
                    IsDeleted = false
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Orange Juice",
                    Price = 30.00m,
                    IsAvailable = true,
                    PreparationTime = 5,
                    DailyOrderCount = 0,
                    ImageUrl = "/images/orange-juice.jpg",
                    CategoryId = 2,
                    IsDeleted = false
                },
                new MenuItem
                {
                    Id = 4,
                    Name = "Chocolate Cake",
                    Price = 50.00m,
                    IsAvailable = true,
                    PreparationTime = 10,
                    DailyOrderCount = 0,
                    ImageUrl = "/images/cake.jpg",
                    CategoryId = 3,
                    IsDeleted = false
                }
            );
                
        }
        
    }
}
