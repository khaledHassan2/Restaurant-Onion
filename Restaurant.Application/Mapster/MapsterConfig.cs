using Mapster;
using Restaurant.DTOs.MenuCategoryDTOs;
using Restaurant.DTOs.MenuItemDTOs;
using Restaurant.DTOs.OrderDTOs;
using Restaurant.DTOs.UsersDTOs;
using Restaurant.Models;

namespace Restaurant.Application.Mapster
{
    public class MapsterConfig
    {
        public static void Configure()
        {
            // ✅ MenuCategory Mappings
            TypeAdapterConfig<MenuCategory, GetAllMenuCategoryDTO>.NewConfig().TwoWays();

            // ✅ MenuItem Mappings
            TypeAdapterConfig<MenuItem, CreateMenuItemDTO>.NewConfig().TwoWays();

            // ✅ Order Mappings
            TypeAdapterConfig<Order, CreateOrderDTO>.NewConfig().TwoWays()
                .Map(dest => dest.CustomerUserName, src => src.Customer != null ? src.Customer.UserName : null)
                .Map(dest => dest.Items, src => src.Items.Select(i => new OrderItemDTO
                {
                    MenuItemId = i.MenuItemId,
                    MenuItemName = i.MenuItem != null ? i.MenuItem.Name : null,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }));

            // ✅ IdentityCustomer Mappings
            TypeAdapterConfig<IdentityCustomer, RegisterUserDTO>.NewConfig().TwoWays();
        }
    }
}
