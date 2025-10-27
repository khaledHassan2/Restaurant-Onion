using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.Application.Services.MenuCategoryServices;
using Restaurant.Application.Services.MenuItemServices;
using Restaurant.Application.Services.OrderItemServices;
using Restaurant.Application.Services.OrderServices;
using Restaurant.Context;
using Restaurant.Infrastructure;
using Restaurant.Models;
using Restaurant.Presentation.Middlewares;
using System;

namespace Restaurant.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            //builder.Services.AddDbContext()
            var conn = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<RestaurantDbContext>(options =>
            {
                options.UseSqlServer(conn);
            });
            builder.Services.AddIdentity<IdentityCustomer, IdentityRole>().AddEntityFrameworkStores<RestaurantDbContext>();
            //builder.Services.AddIdentity<IdentityCustomer, IdentityRole>(options =>
            //{
            //    //options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireConfirmedEmail");
            //    options.Password.RequireDigit = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireDigit");
            //    options.Password.RequiredLength = builder.Configuration.GetValue<int>("PasswordRequirements:MinimumLength");
            //    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireSpecialCharacter");
            //    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireUppercase");
            //    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireLowercase");
            //    options.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireUniqueEmail");

            //})
            //.AddRoles<IdentityRole>()
            //.AddEntityFrameworkStores<RestaurantDbContext>();

            // Add services to the container.
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMenuCategoryService, MenuCategoryService>();
            builder.Services.AddScoped<IMenuItemService, MenuItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();




            builder.Services.AddControllersWithViews();
            builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAntiforgery(); // لو مش موجود

            builder.Services.AddSession();

            var app = builder.Build();
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<Totalpudgetof_mounth>();
          //  app.UseMiddleware<TimeOut>();


            app.UseAuthentication();  
           


            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
