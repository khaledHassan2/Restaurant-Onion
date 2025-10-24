using Restaurant.Application.Contracts;
using Restaurant.DTOs.MenuItemDTOs;
using Restaurant.DTOs.OrderDTOs;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAll(string? searchString = null);
        Task<Order> GetById(int id);
        Task Create(CreateOrderDTO dto);
        Task Update(CreateOrderDTO entity);
        Task Delete(int id);
        Task<int> SaveChangesAsync();
    }
}
