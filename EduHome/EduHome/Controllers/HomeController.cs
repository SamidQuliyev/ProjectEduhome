using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            AppUser appUser = new AppUser();


            HomeVM homeVM = new HomeVM
            {
                Sliders = _db.Sliders.ToList(),
                About = _db.About.FirstOrDefault(),
                Courses = _db.Courses.ToList(),
                Video = _db.Video.FirstOrDefault(),
                Services = _db.Services.Where(s => !s.IsDeactive).ToList(),
                Blogs = _db.Blogs.ToList(),



            };
            return View(homeVM);
        }

        public IActionResult GlobalSearch(string key)
        {
            SearchVM searchVM = new SearchVM
            {
                Courses = _db.Courses.Where(x=>x.Title.Contains(key)).OrderByDescending(x=>x.Id).Take(3).ToList(),
                Teachers = _db.Teachers.Where(x => x.FullName.Contains(key)).OrderByDescending(x => x.Id).ToList(),
                Blogs = _db.Blogs.Where(x => x.Title.Contains(key)).OrderByDescending(x => x.Id).ToList(),
            };
            return PartialView("_GlobalSearchPartial", searchVM);

        
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
