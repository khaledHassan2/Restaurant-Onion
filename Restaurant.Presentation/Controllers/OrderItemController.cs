using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Services.MenuItemServices;
using Restaurant.Application.Services.OrderItemServices;
using Restaurant.DTOs.OrderDTOs;
using Restaurant.Models;

namespace Restaurant.Presentation.Controllers
{
    [Authorize]
    public class OrderItemController : Controller
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IMenuItemService _menuItemService;
        private readonly UserManager<IdentityCustomer> _userManager;

        public OrderItemController(
            IOrderItemService orderItemService,
            IMenuItemService menuItemService,
            UserManager<IdentityCustomer> userManager)
        {
            _orderItemService = orderItemService;
            _menuItemService = menuItemService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int menuItemId)
        {
            var customerId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                TempData["ErrorMessage"] = "Please log in to add items to your cart.";
                return RedirectToAction("Login", "Account");
            }

            var result = await _orderItemService.AddToCartAsync(customerId, menuItemId);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index", "MenuItem");
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Cart()
        {
            var customerId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                TempData["ErrorMessage"] = "Please log in to view your cart.";
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _orderItemService.GetCartItemsAsync(customerId);
            if (cartItems == null || !cartItems.Any())
            {
                TempData["InfoMessage"] = "Your cart is empty.";
                return View(new List<CartItemDTO>());
            }

            return View(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int orderItemId)
        {
            var customerId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                TempData["ErrorMessage"] = "Please log in to modify your cart.";
                return RedirectToAction("Login", "Account");
            }

            // مجرد استدعاء Delete من السيرفيس
            await _orderItemService.Delete(orderItemId);
            await _orderItemService.SaveChangesAsync();

            TempData["SuccessMessage"] = "Item removed from cart!";
            return RedirectToAction("Cart");
        }

    }
}
