using Restaurant.DTOs.OrderDTOs;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services.OrderItemServices
{
    public interface IOrderItemService
    {
        public Task<(bool IsSuccess, string Message)> AddToCartAsync(string customerId, int menuItemId);
        public Task<List<CartItemDTO>> GetCartItemsAsync(string customerId);
        Task<IEnumerable<OrderItem>> GetAll(string? searchString = null);
        Task<OrderItem> GetById(int id);
        Task Create(OrderItem dto);
        Task Update(OrderItem entity);
        Task Delete(int id);
        Task<int> SaveChangesAsync();
     
    }
}
