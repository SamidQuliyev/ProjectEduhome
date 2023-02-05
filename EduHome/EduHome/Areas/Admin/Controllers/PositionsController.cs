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
    public class PositionsController : Controller
    {
        private readonly AppDbContext _db;
        public PositionsController(AppDbContext db)
        {

            _db = db;

        }

        public IActionResult Index()
        {
            List<Position> positions = _db.Positions.ToList();
            return View(positions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Positions.AnyAsync(s => s.Name == position.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This Position is already exist");
                return View();
            }
            await _db.Positions.AddAsync(position);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(s => s.Id == id);
            if (dbPosition == null)
            {
                return Ok();
            }

            return View(dbPosition);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(s => s.Id == id);
            if (dbPosition == null)
            {
                return Ok();
            }

            return View(dbPosition);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id, Position Position)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(s => s.Id == id);
            if (dbPosition == null)
            {
                return Ok();
            }
            bool isExist = await _db.Positions.AnyAsync(s => s.Name == Position.Name && s.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This Position is already exist");
                return View();
            }
            dbPosition.Name = Position.Name;
          
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(s => s.Id == id);
            if (dbPosition == null)
            {
                return Ok();
            }

            return View(dbPosition);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(s => s.Id == id);
            if (dbPosition == null)
            {
                return Ok();
            }
            dbPosition.IsDeactive = true;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(s => s.Id == id);
            if (dbPosition == null)
            {
                return Ok();
            }
            if (dbPosition.IsDeactive)
            {
                dbPosition.IsDeactive = false;
            }
            else
            {
                dbPosition.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}


