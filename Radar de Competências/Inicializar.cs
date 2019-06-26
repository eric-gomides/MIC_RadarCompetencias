using Microsoft.AspNetCore.Identity;
using RadarCompetencias.Data;
using RadarCompetencias.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RadarCompetencias
{
    public static class Inicializar
    {
        public static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {

        }
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if(await userManager.FindByNameAsync("admin@admin") == null)
            {
                var user = new ApplicationUser();
                user.Email = "admin@admin";
                user.UserName = "admin";
                user.Name = "Administrador";
                user.NormalizedEmail = "ADMIN@ADMIN";

                await userManager.CreateAsync(
                    user: user,
                    password: "@dmin123"
                    );
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
