using Mapster;
using Restaurant.DTOs.MenuCategoryDTOs;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Mapster
{
    public class MapsterConfig
    {
        public static void Configure()
        {
            // MenuCategory Mappings
            TypeAdapterConfig<MenuCategory, GetAllMenuCategoryDTO>.NewConfig().TwoWays();
              //  .Map(dest => dest.Name, src => $"Category Name: {src.Name}");
            //    .Map(dest => dest.ParentName_En, src => src.ParentCategory.Name_En);
            //TypeAdapterConfig<Category, CreateCategoryDTO>.NewConfig().TwoWays();

            //// Product Mappings
            //TypeAdapterConfig<Product, ProductDTO>.NewConfig().TwoWays();
            //TypeAdapterConfig<CreateProductDTO, Product>.NewConfig().TwoWays();

        }
    }
}
