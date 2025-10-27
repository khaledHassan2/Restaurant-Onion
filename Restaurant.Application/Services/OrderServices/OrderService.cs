using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.DTOs.OrderDTOs;
using Restaurant.Models;

namespace Restaurant.Application.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order>  _genericRepository;

        public OrderService(IGenericRepository<Order> genericRepository)
        {
            _genericRepository = genericRepository;
        }



        public async Task<IEnumerable<Order>> GetAll(string? searchString = null)
        {
            return await _genericRepository.GetAll()
       .Include(o => o.Customer)
       .Include(o => o.Items)
           .ThenInclude(oi => oi.MenuItem)
       .ToListAsync();
        }


        public async Task<Order?> GetById(int id)
        {
            return await _genericRepository.GetById(id,q => q.Include(o => o.Customer)
           .Include(o => o.Items)
           .ThenInclude(i => i.MenuItem)
             );

        }


        public async Task Create(CreateOrderDTO dto)
        {
            var order = dto.Adapt<Order>();
            await _genericRepository.Create(order);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _genericRepository.Delete(id);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task Update(CreateOrderDTO entity)
        {
            var order = entity.Adapt<Order>();
            _genericRepository.Update(order);
            await _genericRepository.SaveChangesAsync();
        }
        public async Task Update(Order entity)
        {   
            _genericRepository.Update(entity);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _genericRepository.SaveChangesAsync();
        }
        public async Task<Order?> GetPendingOrderWithItemsAsync(string customerId)
        {
            return await _genericRepository.GetPendingOrderWithItemsAsync(customerId);
        }

        public async Task Create(Order dto)
        {
            await _genericRepository.Create(dto);
            await _genericRepository.SaveChangesAsync();
        }
    }
}
