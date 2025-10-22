using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Services.MenuCategoryServices;
using Restaurant.Application.Services.MenuItemServices;
using Restaurant.Presentation.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Restaurant.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMenuItemService _menuItemService ;

        public HomeController(ILogger<HomeController> logger, IMenuItemService menuItem )
        {
            _logger = logger;
           _menuItemService = menuItem;
        }

        public async Task<IActionResult> Index(string? searchString)
        {
            ViewData["CurrentFilter"] = searchString; // ⁄·‘«‰ ‰Õ«›Ÿ ⁄·Ï ﬁÌ„… «·»ÕÀ ›Ì «·‹ View
            var res = await _menuItemService.GetAll(searchString);
            return View(res);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
