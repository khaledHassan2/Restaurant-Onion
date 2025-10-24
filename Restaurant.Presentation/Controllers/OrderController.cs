using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Services.MenuItemServices;
using Restaurant.Application.Services.OrderServices;
using Restaurant.DTOs.OrderDTOs;
using Restaurant.Models;
using System.Threading.Tasks;

namespace Restaurant.Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMenuItemService _menuItemService;
        private readonly UserManager<IdentityCustomer> _userManager;

        private readonly SignInManager<IdentityCustomer> _signInManager;

        public OrderController(IOrderService orderService, IMenuItemService menuItemService, SignInManager<IdentityCustomer> signInManager, UserManager<IdentityCustomer> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _orderService = orderService;
            _menuItemService = menuItemService;
        }

        public async Task<IActionResult> Index()
        {
            var orders=await _orderService.GetAll();
            return View(orders);
        }
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDTO orderDTO)
        {
            if(!ModelState.IsValid)
            {
                return View(orderDTO);
            }

            if(orderDTO == null)
                return View(orderDTO);
           await _orderService.Create(orderDTO);
   
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
          var order=await  _orderService.GetById(id);
            var orderdto=order.Adapt<CreateOrderDTO>();
            return View(orderdto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(orderDTO);
            }

            if (orderDTO == null)
                return View(orderDTO);
            await _orderService.Update(orderDTO);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await  _orderService.Delete(id);
            return RedirectToAction("Index");

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int menuItemId)
        {
            var customerId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                TempData["ErrorMessage"] = "Please log in to add items to your cart.";
                return RedirectToAction("Login", "Account");
            }

            // جلب المستخدم الحالي مع Orders و Items و MenuItem
            var customer = await _userManager.Users
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Items)
                        .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index", "MenuItem");
            }

            // جلب Pending Order أو إنشاء جديد
            var pendingOrder = customer.Orders.FirstOrDefault(o => o.Status == OrderStatus.Pending);
            if (pendingOrder == null)
            {
                pendingOrder = new Order
                {
                    CustomerId = customerId,
                    Status = OrderStatus.Pending,
                    Type = OrderType.DineIn,
                    Discount = 0,
                    TaxPercent = 8.5m
                };

                customer.Orders.Add(pendingOrder);
                await _orderService.SaveChangesAsync();
            }

            // إضافة أو زيادة كمية الـ MenuItem
            var existingItem = pendingOrder.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                var menuItem = await _menuItemService.GetById(menuItemId);
                if (menuItem == null)
                {
                    TempData["ErrorMessage"] = "Item not found.";
                    return RedirectToAction("Cart");
                }

                pendingOrder.Items.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    MenuItem = menuItem,
                    Quantity = 1,
                    UnitPrice = menuItem.Price
                });
            }

            await _orderService.SaveChangesAsync();

            TempData["SuccessMessage"] = "Item added to cart!";
            return RedirectToAction("Cart");
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var customerId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                TempData["ErrorMessage"] = "Please log in to view your cart.";
                return RedirectToAction("Login", "Account");
            }

            // جلب المستخدم مع الأوردرز و Items و MenuItem
            var customer = await _userManager.Users
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Items)
                        .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index", "MenuItem");
            }

            // جلب الـ Pending Order الخاص بالمستخدم
            var pendingOrder = customer.Orders.FirstOrDefault(o => o.Status == OrderStatus.Pending);

            if (pendingOrder == null || pendingOrder.Items == null || !pendingOrder.Items.Any())
            {
                TempData["InfoMessage"] = "Your cart is empty.";
                return View(new List<CartItemDTO>());
            }

            // تحويل العناصر لـ DTO للعرض
            var cartItems = pendingOrder.Items.Select(i => new CartItemDTO
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = i.MenuItem.Name,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList();

            return View(cartItems);
        }




    }
}
