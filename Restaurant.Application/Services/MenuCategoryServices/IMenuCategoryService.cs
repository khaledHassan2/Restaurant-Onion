using Restaurant.DTOs.MenuCategoryDTOs;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services.MenuCategoryServices
{
    public interface IMenuCategoryService
    {
        Task<IEnumerable<GetAllMenuCategoryDTO>> GetAll(string? searchString = null);
        public Task<MenuCategory> GetById(int id);
        public Task Create(CreateMenuCategoryDTO entity);
        public Task Update(CreateMenuCategoryDTO entity);
        public Task Delete(int id);
    }
}
