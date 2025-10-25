using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Contracts;
using Restaurant.Application.Services.MenuItemServices;
using Restaurant.DTOs.OrderDTOs;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services.OrderItemServices
{
    public class OrderItemService : IOrderItemService
    {
        //--------------------------------------
            private readonly IGenericRepository<OrderItem> _genericRepository;
            private readonly IGenericRepository<Order> _orderRepo;
            private readonly IMenuItemService _menuItemService;

            public OrderItemService(
                IGenericRepository<OrderItem> genericRepository,
                IGenericRepository<Order> orderRepo,
                IMenuItemService menuItemService)
            {
                _genericRepository = genericRepository;
                _orderRepo = orderRepo;
                _menuItemService = menuItemService;
            }

public async Task<(bool IsSuccess, string Message)> AddToCartAsync(string customerId, int menuItemId)
    {
        var pendingOrder = _orderRepo.GetAll()
            .Include(o => o.Items)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefault(o => o.CustomerId == customerId && o.Status == OrderStatus.Pending);

        if (pendingOrder == null)
        {
            pendingOrder = new Order
            {

                CustomerId = customerId,
                Status = OrderStatus.Pending,
                Type = OrderType.DineIn,
                Discount = 0,
                TaxPercent = 8.5m,
                Items = new List<OrderItem>()
            };
            await _orderRepo.Create(pendingOrder);
            await _orderRepo.SaveChangesAsync();
        }

        var existingItem = pendingOrder.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
        if (existingItem != null)
        {
            existingItem.Quantity += 1;
            _genericRepository.Update(existingItem);
        }
        else
        {
            var menuItem = await _menuItemService.GetById(menuItemId);
            if (menuItem == null)
                return (false, "Item not found.");

            var orderItem = new OrderItem
            {

                MenuItemId = menuItem.Id,
                MenuItem = menuItem,
                Quantity = 1,
                UnitPrice = menuItem.Price,
                Order = pendingOrder
            };
            await _genericRepository.Create(orderItem);
        }

        await _genericRepository.SaveChangesAsync();
        return (true, "Item added to cart!");
    }


    //public async Task<(bool IsSuccess, string Message)> AddToCartAsync(string customerId, int menuItemId)
    //{
    //    var pendingOrder = _orderRepo.GetAll()
    //        .FirstOrDefault(o => o.CustomerId == customerId && o.Status == OrderStatus.Pending);

    //    if (pendingOrder == null)
    //    {
    //        pendingOrder = new Order
    //        {
    //            CustomerId = customerId,
    //            Status = OrderStatus.Pending,
    //            Type = OrderType.DineIn,
    //            Discount = 0,
    //            TaxPercent = 8.5m,
    //            Items = new List<OrderItem>()
    //        };
    //        await _orderRepo.Create(pendingOrder);
    //        await _orderRepo.SaveChangesAsync();
    //    }

    //    var existingItem = pendingOrder.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
    //    if (existingItem != null)
    //    {
    //        existingItem.Quantity += 1;
    //    _genericRepository.Update(existingItem);
    //    }
    //    else
    //    {
    //        var menuItem = await _menuItemService.GetById(menuItemId);
    //        if (menuItem == null)
    //            return (false, "Item not found.");

    //        var orderItem = new OrderItem
    //        {
    //            MenuItemId = menuItem.Id,
    //            MenuItem = menuItem,
    //            Quantity = 1,
    //            UnitPrice = menuItem.Price,
    //            Order = pendingOrder
    //        };
    //        await _genericRepository.Create(orderItem);
    //    }

    //    await _genericRepository.SaveChangesAsync();
    //    return (true, "Item added to cart!");
    //}

    public async Task<List<CartItemDTO>> GetCartItemsAsync(string customerId)
    {
        var pendingOrder = _orderRepo.GetAll()
            .Include(o => o.Items)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefault(o => o.CustomerId == customerId && o.Status == OrderStatus.Pending);

        if (pendingOrder == null || pendingOrder.Items == null || !pendingOrder.Items.Any())
            return new List<CartItemDTO>();

        return pendingOrder.Items.Select(i => new CartItemDTO
        {
            OrderItemId = i.Id,      // <-- استخدم Id الخاص بالـ OrderItem
            MenuItemId = i.MenuItemId,
            MenuItemName = i.MenuItem.Name,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity
        }).ToList();
    }
       
        public async Task Create(OrderItem dto)
        {
           await _genericRepository.Create(dto);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _genericRepository.Delete(id);
        }

        public async Task<IEnumerable<OrderItem>> GetAll(string? searchString = null)
        {
            return  _genericRepository.GetAll();
        }

        public async Task<OrderItem> GetById(int id)
        {
         return  await _genericRepository.GetById(id);
        }

        public async Task<int> SaveChangesAsync()
        {
          return await  _genericRepository.SaveChangesAsync();
        }

        public async Task Update(OrderItem entity)
        {
            _genericRepository.Update(entity);
            await _genericRepository.SaveChangesAsync();
        }

      
    }
}
