using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        private readonly AppDbContext _db;
        public ServicesController(AppDbContext db)
        {

            _db = db;

        }

        public IActionResult Index()
        {
            List<Service> services = _db.Services.ToList();
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Services.AnyAsync(s => s.Name == service.Name);
            if(isExist)
            {
                ModelState.AddModelError("Name", "This service is already exist");
                return View();
            }
            await _db.Services.AddAsync(service);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            Service dbService = await _db.Services.FirstOrDefaultAsync(s => s.Id==id);
            if (dbService == null)
            {
                return Ok();
            }

            return View(dbService);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service dbService = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (dbService == null)
            {
                return Ok();
            }

            return View(dbService);
        }
        [HttpPost]
        
        public async Task<IActionResult> Update(int? id, Service service)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service dbService = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (dbService == null)
            {
                return Ok();
            }
            bool isExist = await _db.Services.AnyAsync(s => s.Name == service.Name&& s.Id!=id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This service is already exist");
                return View();
            }
            dbService.Name = service.Name;
            dbService.Description = service.Description;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service dbService = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (dbService == null)
            {
                return Ok();
            }

            return View(dbService);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service dbService = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (dbService == null)
            {
                return Ok();
            }
            dbService.IsDeactive = true;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service dbService = await _db.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (dbService == null)
            {
                return Ok();
            }
            if (dbService.IsDeactive)
            {
                dbService.IsDeactive = false;
            }
            else
            {
                dbService.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
    }

   

