using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Services.MenuItemServices;
using Restaurant.Application.Services.OrderServices;
using Restaurant.DTOs.OrderDTOs;
using Restaurant.Models;
using System.Security.Claims;
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
            var orders = await _orderService.GetAll();
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
           var items= await _menuItemService.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid || orderDTO == null)
                return View(orderDTO);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            orderDTO.CustomerId = userId;

            await _orderService.Create(orderDTO);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var order = await _orderService.GetById(id);
            var orderdto = order.Adapt<CreateOrderDTO>();
            return View(orderdto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid || orderDTO == null)
                return View(orderDTO);

            await _orderService.Update(orderDTO);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var customerId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                TempData["ErrorMessage"] = "Please log in to checkout.";
                return RedirectToAction("Login", "Account");
            }

            // جلب الـCart الحالي (Status = Pending)
            var cart = await _orderService.GetPendingOrderWithItemsAsync(customerId);

            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                TempData["InfoMessage"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            // إنشاء Order جديد
            var newOrder = new Order
            {
                CustomerId = customerId,
                Status = OrderStatus.Ready, // أو Pending لو عايز تاكد الدفع بعدين
                Type = cart.Type,
                Discount = cart.Discount,
                TaxPercent = cart.TaxPercent,
                DeliveryAddress = cart.DeliveryAddress,
                LastStatusChange = DateTime.Now
            };

            await _orderService.Create(newOrder);

            // نسخ العناصر من الكارت إلى الـOrder الجديد
            newOrder.Items = cart.Items.Select(i => new OrderItem
            {
                MenuItemId = i.MenuItemId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                OrderId = newOrder.Id
            }).ToList();

            await _orderService.Update(newOrder);

            // مسح الكارت الحالي بعد الشيك اوت
            //await _orderService.ClearCart(customerId); // تأكد ان عندك method دي

            TempData["SuccessMessage"] = "Checkout successful! Your cart is now empty.";
            return RedirectToAction("Index");
        }

    }
}
