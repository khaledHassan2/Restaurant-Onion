using Mapster;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Contracts;
using Restaurant.Application.Services.MenuCategoryServices;
using Restaurant.DTOs.MenuCategoryDTOs;
using Restaurant.Models;
using System;

namespace Restaurant.Presentation.Controllers
{
    public class MenuCategoryController : Controller
    {
            private readonly IMenuCategoryService _menuCategoryService;
        private readonly IGenericRepository<MenuCategory> _genericRepository;


        public MenuCategoryController(IMenuCategoryService menuCategoryService,IGenericRepository<MenuCategory> genericRepository)
            {
               _menuCategoryService = menuCategoryService;
              _genericRepository = genericRepository;
            }

        public async Task<IActionResult> Index()
        {
            var cat = await _menuCategoryService.GetAll();

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(cat);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMenuCategoryDTO menu)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //var cats = await _menuCategoryService.GetAll();
            //foreach (var c in cats)
            //{
            //    if (c.Name == menu.Name)
            //    {
            //        return View();
            //    }

            //}

            await _menuCategoryService.Create(menu);
           await _genericRepository.SaveChangesAsync();
          
            return RedirectToAction("Index");
        }
        //[AcceptVerbs("GET", "POST")]
        //public async Task<IActionResult> IsUniqueName(string name)
        //{
        //    if (await _context.MenuCategories.AnyAsync(e => e.Name == name))
        //    {
        //        return Json($"Name {name} is already in use.");
        //    }

        //    return Json(true);
        //}
        public async Task<IActionResult> Update(int id)
        {
            var cat = await _menuCategoryService.GetById(id); 
            if (cat == null)
                return NotFound("Category not found");


            return View(cat); 
        }

        [HttpPost]
        public async Task<IActionResult> Update(MenuCategory entity)
        {

            var old = await _genericRepository.GetById(entity.Id);
            if (old == null)
                return NotFound("Category not found");

            old.Name = entity.Name;
            old.Description = entity.Description;


             _genericRepository.Update(old);
            await _genericRepository.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        //public async Task<IActionResult> Delete(int id)
        //{
        //   var cat= await _menuCategoryService.GetById(id);
        //    if (!cat.MenuItems.Any())
        //    {
        //        await _menuCategoryService.Delete(id);
        //    return RedirectToAction("Index");
        //    }
        //    ModelState.AddModelError("","Can't Deleted this category Becaous Has MenuItem");
        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _menuCategoryService.GetById(id);

            if (cat.MenuItems == null || !cat.MenuItems.Any())
            {
                await _menuCategoryService.Delete(id);

                TempData["SuccessMessage"] = "Category deleted successfully.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = $"Can't delete this category because it has related MenuItems.Count ={cat.MenuItems.Count}";
            return RedirectToAction("Index");
        }


    }
}
