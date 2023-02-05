using EduHome.DAL;
using EduHome.Helpers;
using EduHome.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SlidersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _db.Sliders.ToList();
            return View(sliders);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (dbSlider == null)
            {
                return Ok();
            }

            return View(dbSlider);
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]



        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null");
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image");
                return View();
            }
            if (slider.Photo.OlderOneMb())
            {
                ModelState.AddModelError("Photo", "Image max 3mb");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "img", "slider");
            slider.Image = await slider.Photo.SaveFileAsync(path);
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (dbSlider == null)
            {
                return Ok();
            }

            return View(dbSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (id == null)
            {
                return NotFound();
            }

            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbSlider);
            }
            if (slider.Photo != null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image");
                    return View(dbSlider);
                }
                if (slider.Photo.OlderOneMb())
                {
                    ModelState.AddModelError("Photo", "Image max 3mb");
                    return View(dbSlider);
                }
                string path = Path.Combine(_env.WebRootPath, "img", "slider");

                if (System.IO.File.Exists(Path.Combine(path, dbSlider.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, dbSlider.Image));
                }
                dbSlider.Image = await slider.Photo.SaveFileAsync(path);
            }

            dbSlider.Title = slider.Title;
            dbSlider.Subtitle = slider.Subtitle;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



    }


}
