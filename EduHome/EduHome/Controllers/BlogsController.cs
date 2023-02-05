using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppDbContext _db;
        public BlogsController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int page=1)
        {
            ViewBag.SelectedPage = page;
          List<Blogs> blogs = _db.Blogs.OrderByDescending(x=>x.Id).Skip((page-1)*6).Take(6).ToList();
            ViewBag.PageCount = Math.Ceiling((decimal)_db.Blogs.Count() / 6);

            return View(blogs);
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
