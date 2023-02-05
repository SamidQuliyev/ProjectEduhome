using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EduHome.Helpers.Helper;

namespace EduHome.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;




        public UserController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;


        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Username = user.UserName,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()

                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }

        public async Task<IActionResult> Activation(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser dbAppUser = await _userManager.FindByIdAsync(id);
            if (dbAppUser == null)
            {
                return Ok();
            }
            if (dbAppUser.IsDeactive)
            {
                dbAppUser.IsDeactive = false;
            }
            else
            {
                dbAppUser.IsDeactive = true;
            }
            await _userManager.UpdateAsync(dbAppUser);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ChangePassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser dbAppUser = await _userManager.FindByIdAsync(id);
            if (dbAppUser == null)
            {
                return BadRequest();
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ChangePassword(string id, ChangePasswordVM changePassword)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser dbAppUser = await _userManager.FindByIdAsync(id);
            if (dbAppUser == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)

            {
                return View();
            }

            bool isCorrect = await _userManager.CheckPasswordAsync(dbAppUser, changePassword.OldPassword);

            if (!isCorrect)
            {
                ModelState.AddModelError("OldPassword", "Duzgun Parolu yaz");
                return View();
            }
            IdentityResult identityResult = await _userManager.ChangePasswordAsync(dbAppUser, changePassword.OldPassword, changePassword.NewPassword);
            if (!identityResult.Succeeded)

            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("NewPassword", error.Description);
                }
                return View();


            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser dbAppUser = await _userManager.FindByIdAsync(id);
            if (dbAppUser == null)
            {
                return BadRequest();
            }
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            string oldRole = (await _userManager.GetRolesAsync(dbAppUser)).FirstOrDefault();
            ViewBag.Role = oldRole;
            UpdateVM update = new UpdateVM
            {
                FullName = dbAppUser.FullName,
                Email = dbAppUser.Email,
                Username = dbAppUser.UserName

            };
            return View(update);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateVM updateVM, string newRole)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser dbAppUser = await _userManager.FindByIdAsync(id);
            if (dbAppUser == null)
            {
                return BadRequest();
            }
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            string oldRole = (await _userManager.GetRolesAsync(dbAppUser)).FirstOrDefault();
            ViewBag.Role = oldRole;
            if (oldRole != newRole)
            {
                IdentityResult removeResult = await _userManager.RemoveFromRoleAsync(dbAppUser, oldRole);
                if (!removeResult.Succeeded)
                {
                    foreach (IdentityError error in removeResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();

                }
                IdentityResult addResult = await _userManager.AddToRoleAsync(dbAppUser, newRole);
                if (!addResult.Succeeded)
                {
                    foreach (IdentityError error in addResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();

                }



            }
            dbAppUser.FullName = updateVM.FullName;
            dbAppUser.Email = updateVM.Email;
            dbAppUser.UserName = updateVM.Username;
            await _userManager.UpdateAsync(dbAppUser);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM createVM)
        {
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser dbAppUser = new AppUser
            {
                FullName = createVM.FullName,
                UserName = createVM.Username,
                Email = createVM.Email


            };
            IdentityResult identityResult = await _userManager.CreateAsync(dbAppUser, createVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View();

            }

            await _userManager.AddToRoleAsync(dbAppUser, Roles.Member.ToString());

            return RedirectToAction("Index");
        }


    }
}


