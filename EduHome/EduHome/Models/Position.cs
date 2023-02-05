using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.Models
{
    public class Position
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeactive { get; set; }

        public List<Teacher> Teachers { get; set; }
    }
}
