using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }


        public string FullName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsDeactive { get; set; }



    }
}
