using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.DTOs.MenuCategoryDTOs;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services.MenuCategoryServices
{
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IGenericRepository<MenuCategory> _genericRepository;

        public MenuCategoryService(IGenericRepository<MenuCategory> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        //public async Task<IEnumerable<GetAllMenuCategoryDTO>> GetAll()
        //{
        //    var res = await (await _genericRepository.GetAll()).ToListAsync();
        //    var cats = res.Adapt<IEnumerable<GetAllMenuCategoryDTO>>();
        //    return cats;
        //}


        public async Task Create(CreateMenuCategoryDTO entity)
        {
            var newMenuCategory = entity.Adapt<MenuCategory>();
            newMenuCategory.CreatedBy = "System";
            await _genericRepository.Create(newMenuCategory);
        }

        public async Task Delete(int id)
        {
           var item= await _genericRepository.GetById(id);
            item.IsDeleted = true;
            await _genericRepository.Update(item);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<MenuCategory> GetById(int id)
        {
            return await _genericRepository.GetById(id,query => query.Include(c => c.MenuItems));
        }


        public async Task Update(CreateMenuCategoryDTO entity)
        {
            var item= entity.Adapt<MenuCategory>();
            await _genericRepository.Update(item);
        }

        public async Task<IEnumerable<GetAllMenuCategoryDTO>> GetAll(string? searchString = null)
        {
            var query = await _genericRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(c => c.Name.Contains(searchString));
            }

            var result = await query
                .ProjectToType<GetAllMenuCategoryDTO>()  // باستخدام Mapster
                .ToListAsync();

            return result;
        }

    }
}
