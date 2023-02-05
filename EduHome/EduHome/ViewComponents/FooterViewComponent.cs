using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.ViewComponents
{
    public class FooterViewComponent: ViewComponent
    {

        private readonly AppDbContext _db;
        public FooterViewComponent(AppDbContext db)
        {

            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Bio bio = await _db.Bios.FirstOrDefaultAsync();
            return View(bio);
        }
    }
}
