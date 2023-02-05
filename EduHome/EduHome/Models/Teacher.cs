using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Models
{
    public class Teacher
    {
        public int Id { get; set; }


        public string FullName { get; set; }
        public string Image { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]

        public IFormFile Photo { get; set; }

        public Position Position { get; set; }

        public int PositionId { get; set; }


    }
}
