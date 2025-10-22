using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.DTOs.MenuItemDTOs;
using Restaurant.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Application.Services.MenuItemServices
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IGenericRepository<MenuItem> _menuItemRepo;

        public MenuItemService(IGenericRepository<MenuItem> menuItemRepo)
        {
            _menuItemRepo = menuItemRepo;
        }

        public async Task<IEnumerable<GetAllMenuItemDTO>> GetAll(string? search = null)
        {
            var query =  _menuItemRepo.GetAll(); 
            query = query.Include(m => m.Category); 

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(m => m.Name.Contains(search));

            return await query.ProjectToType<GetAllMenuItemDTO>().ToListAsync();
        }

        public async Task<MenuItem> GetById(int id)
        {
            return await _menuItemRepo.GetById(id);
         
        }


        public async Task Create(CreateMenuItemDTO dto)
        {
            var newItem = dto.Adapt<MenuItem>();
            newItem.CreatedBy = "System";
            newItem.IsDeleted = false;

            await _menuItemRepo.Create(newItem);
            await _menuItemRepo.SaveChangesAsync();
        }

        public async Task Update(MenuItem entity)
        {
            _menuItemRepo.Update(entity);
            await _menuItemRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
           
                await  _menuItemRepo.Delete(id);
                await _menuItemRepo.SaveChangesAsync();
            
        }

    }
}
