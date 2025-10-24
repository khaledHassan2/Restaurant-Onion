using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.DTOs.UsersDTOs;
using Restaurant.Models;

namespace Restaurant.Presentation.Controllers
{
    public class AccountController : Controller
    {
       
            private readonly UserManager<IdentityCustomer> _userManager;

            private readonly SignInManager<IdentityCustomer> _signInManager;

            public AccountController(SignInManager<IdentityCustomer> signInManager, UserManager<IdentityCustomer> userManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public IActionResult Register()
            {
                return View();
            }
            [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityCustomer
                {
                    UserName = dto.UserName,
                    Email = dto.UserEmail
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                Console.WriteLine($"UserName: '{dto.UserName}'");


                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(dto);
        }

        public IActionResult Login()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> Login(LoginUserDTO vm)
            {
                if (ModelState.IsValid)
                {
                    var res = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, true);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", res.IsNotAllowed.ToString());
                }

                return View(vm);
            }
            public async Task<IActionResult> LogOut()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }
        }
    }
