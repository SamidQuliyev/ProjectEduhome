using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="This area can not be empty")/*,DataType(DataType.EmailAddress)*/]

        public string Name { get; set; }
        [Required(ErrorMessage = "This area can not be empty")]
        public string Description { get; set; }
        public bool IsDeactive { get; set; }

    }
}
