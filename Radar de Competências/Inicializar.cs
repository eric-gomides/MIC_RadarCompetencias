using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RadarCompetencias.Data;
using RadarCompetencias.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace RadarCompetencias
{
    public static class Inicializar
    {
        public static async Task SeedDB(IConfiguration configuration)
        {

        }

        /// <summary>
        /// Adiciona as roles "admin" e "user" no banco de dados
        /// </summary>
        public static async Task SeedRoles(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                if (await connection.QueryFirstOrDefaultAsync<string>($@"SELECT 1 FROM [ApplicationRole]") == null)
                {
                    await connection.QueryAsync($@"INSERT [dbo].[ApplicationRole] ([Name],[NormalizedName])
                                               VALUES ('user', 'USER'), ('admin', 'ADMIN')");
                }
            }
        }
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if(await userManager.FindByNameAsync("admin@admin") == null)
            {
                var user = new ApplicationUser
                {
                    Email = "admin@admin",
                    UserName = "admin",
                    Name = "Administrador",
                    NormalizedEmail = "ADMIN@ADMIN"
                };

                await userManager.CreateAsync(
                    user: user,
                    password: "@dmin123"
                    );
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
