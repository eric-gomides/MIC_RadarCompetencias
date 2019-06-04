using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radar_de_Competências.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
    }
}