using EduHome.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace EduHome.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Blogs> Blogs{ get; set; }
        public DbSet<Bio> Bios { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Teacher> Teachers { get; set; }




    }

}

