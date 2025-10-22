using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.Application.Services.MenuCategoryServices;
using Restaurant.Context;
using Restaurant.Infrastructure;
using Restaurant.Models;

namespace Restaurant.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Add services to the container.
            //builder.Services.AddDbContext()
            var conn = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<RestaurantDbContext>(options =>
            {
                options.UseSqlServer(conn);
            });

            builder.Services.AddIdentity<IdentityCustomer, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireConfirmedEmail");
                options.Password.RequireDigit = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireDigit");
                options.Password.RequiredLength = builder.Configuration.GetValue<int>("PasswordRequirements:MinimumLength");
                options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireSpecialCharacter");
                options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireUppercase");
                options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireLowercase");
                options.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("PasswordRequirements:RequireUniqueEmail");

            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RestaurantDbContext>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMenuCategoryService, MenuCategoryService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
          

            app.UseAuthentication();  // 👈 أضف السطر ده قبل Authorization
           


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
