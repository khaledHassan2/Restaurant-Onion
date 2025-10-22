using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Restaurant.Context
{
   
        public class RestaurantDbContextFactory : IDesignTimeDbContextFactory<RestaurantDbContext>
        {
            public RestaurantDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<RestaurantDbContext>();

                optionsBuilder.UseSqlServer(
                    "Data Source=MY-DESKTOP;Initial Catalog=RestaurantDBOnion;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
                );

                return new RestaurantDbContext(optionsBuilder.Options);
            }
        }
    
}
