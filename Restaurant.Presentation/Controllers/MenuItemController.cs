using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Application.Services.MenuItemServices;
using Restaurant.Application.Services.MenuCategoryServices;
using Restaurant.DTOs.MenuItemDTOs;
using Restaurant.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Mapster;

namespace Restaurant.Presentation.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IMenuCategoryService _menuCategoryService;

        public MenuItemController(IMenuItemService menuItemService, IMenuCategoryService menuCategoryService)
        {
            _menuItemService = menuItemService;
            _menuCategoryService = menuCategoryService;
        }

       
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var items = await _menuItemService.GetAll(searchString);
            return View(items);
        }


        public async Task<IActionResult> Create()
        {
            var cats = await _menuCategoryService.GetAll();
            var model = new CreateMenuItemDTO
            {
                Categories = new SelectList(cats, "Id", "Name")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMenuItemDTO item)
        {
            if (!ModelState.IsValid)
            {
                var cats = await _menuCategoryService.GetAll();
                item.Categories = new SelectList(cats, "Id", "Name");
                return View(item);
            }

            if (item.ImageFile != null && item.ImageFile.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(item.ImageFile.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    await item.ImageFile.CopyToAsync(stream);

                item.ImageUrl = "/images/" + fileName;
            }

            await _menuItemService.Create(item);
            TempData["SuccessMessage"] = "Menu item created successfully!";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _menuItemService.GetById(id);
            if (item == null)
                return NotFound();

            
            var model = item.Adapt<GetAllMenuItemDTO>();

            
            var cats = await _menuCategoryService.GetAll();
            model.Categories = new SelectList(cats, "Id", "Name", item.CategoryId);

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(GetAllMenuItemDTO item)
        {
            if (!ModelState.IsValid)
            {
                var cats = await _menuCategoryService.GetAll();
                item.Categories = new SelectList(cats, "Id", "Name");
                return View(item);
            }

            var oldItem = await _menuItemService.GetById(item.Id);
            if (oldItem == null)
                return NotFound();

            // تحديث الخصائص العادية
            oldItem.Name = item.Name;
            oldItem.Price = item.Price;
            oldItem.PreparationTime = item.PreparationTime;
            oldItem.DailyOrderCount = item.DailyOrderCount;
            oldItem.CategoryId = item.CategoryId;

            // لو في صورة جديدة
            if (item.ImageFile != null && item.ImageFile.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(item.ImageFile.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    await item.ImageFile.CopyToAsync(stream);

                // حذف القديمة لو موجودة
                if (!string.IsNullOrEmpty(oldItem.ImageUrl))
                {
                    string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldItem.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                oldItem.ImageUrl = "/images/" + fileName;
            }

            await _menuItemService.Update(oldItem);
            TempData["SuccessMessage"] = "Menu item updated successfully!";
            return RedirectToAction("Index");
        }


        // ✅ حذف (Soft Delete)
        public async Task<IActionResult> Delete(int id)
        {
            await _menuItemService.Delete(id);
            TempData["SuccessMessage"] = "Menu item deleted successfully!";
            return RedirectToAction("Index");
        }

        // ✅ تفاصيل عنصر
        public async Task<IActionResult> Details(int id)
        {
            var item = await _menuItemService.GetById(id);
            if (item == null)
            {
                TempData["ErrorMessage"] = "Item not found.";
                return RedirectToAction("Index");
            }

            return View(item);
        }
    }
}
