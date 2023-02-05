using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;


        public CoursesController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        { 
            ViewBag.CourseCount = _db.Courses.Count();
            List<Courses> courses = _db.Courses.OrderByDescending(x=>x.Id).Take(6).ToList();
            return View(courses);
        }
        public IActionResult Details()
        {
            return View();
        }
        [HttpPost]

        public IActionResult LoadCourses(int skip)
        {
            int count = _db.Courses.Count();
            if(count<=skip)
            {
                return Content("redd ol");

            }
            List<Courses> courses = _db.Courses.OrderByDescending(x => x.Id).Skip(skip).Take(6).ToList();
            return Json(courses);
        }
    }
}
