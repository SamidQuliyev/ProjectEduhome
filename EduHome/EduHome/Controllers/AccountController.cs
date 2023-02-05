using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EduHome.Helpers.Helper;

namespace EduHome.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;




        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;


        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");

            }
                
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
           AppUser appUser= await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser == null) 
            {
                ModelState.AddModelError("Email", "Istifadeci tapilmadi");
                return View();
            }
            if(appUser.IsDeactive)
            {
                ModelState.AddModelError("Email", "Siz block olmusunuz");
                return View();

            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult=   await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true) ;
            if(signInResult.IsLockedOut)
            {
                ModelState.AddModelError("Email", "Siz 1 deqiqelik blocklanmisiniz");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Email", "Sizin sifreniz yanlisdir");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }




        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email


            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View();

            }

            await _userManager.AddToRoleAsync(appUser, Roles.Member.ToString());
            await _signInManager.SignInAsync(appUser, true);
            return RedirectToAction("Index", "Home");
        }
        public async Task CreateRoles()
        {

            if (!await _roleManager.RoleExistsAsync(Roles.Member.ToString()))

            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
            }
            if (!await _roleManager.RoleExistsAsync(Roles.Member.ToString()))

            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
            }
        }
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");

            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
