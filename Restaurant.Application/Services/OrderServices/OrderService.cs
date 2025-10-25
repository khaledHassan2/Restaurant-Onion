using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.DTOs.OrderDTOs;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _menuItemRepo;

        public OrderService(IGenericRepository<Order> menuItemRepo)
        {
            _menuItemRepo = menuItemRepo;
        }



        public async Task<IEnumerable<Order>> GetAll(string? searchString = null)
        {
            return await _menuItemRepo.GetAll().ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _menuItemRepo.GetById(id);
        }

        public async Task Create(CreateOrderDTO dto)
        {
            var order = dto.Adapt<Order>();
          await _menuItemRepo.Create(order);
            await _menuItemRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
           await _menuItemRepo.Delete(id);
            await _menuItemRepo.SaveChangesAsync();
        }

        public async Task Update(CreateOrderDTO entity)
        {
           var order= entity.Adapt<Order>();
           _menuItemRepo.Update(order);
            await _menuItemRepo.SaveChangesAsync();
        }
        public async Task Update(Order entity)
        {
            _menuItemRepo.Update(entity);
            await _menuItemRepo.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _menuItemRepo.SaveChangesAsync();
        }
        public async Task<Order?> GetPendingOrderWithItemsAsync(string customerId)
        {
            return await _menuItemRepo.GetPendingOrderWithItemsAsync(customerId);
        }

        public async Task Create(Order dto)
        {
            await _menuItemRepo.Create(dto);
            await _menuItemRepo.SaveChangesAsync();
        }
    }
}
