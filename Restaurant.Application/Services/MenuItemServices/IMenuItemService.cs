using Restaurant.DTOs.MenuItemDTOs;
using Restaurant.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Services.MenuItemServices
{
    public interface IMenuItemService
    {
        Task<IEnumerable<IdentityCustomers>> GetAll(string? searchString = null);
        Task<MenuItem> GetById(int id);
        Task Create(CreateMenuItemDTO dto);
        Task Update(MenuItem entity);
        Task Delete(int id);
    }
}
