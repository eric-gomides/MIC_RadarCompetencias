using Microsoft.AspNetCore.Identity;

namespace Radar_de_Competências.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string PasswordHash { get; set; }

    }
}