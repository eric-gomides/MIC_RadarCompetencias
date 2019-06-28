using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RadarCompetencias.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace RadarCompetencias
{
    public static class Inicializar
    {
        /// <summary>
        /// Cria no banco de dados as tabelas necessárias, adiciona as roles default e o usuário admin
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userManager"></param>
        public static async Task InitAsync(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            await SeedDB(configuration);
            await SeedRoles(configuration);
            await SeedUsers(userManager);
        }
        
        /// <summary>
        /// Cria as tabelas necessárias no banco de dados caso estas não estejam criadas
        /// </summary>
        private static async Task SeedDB(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                //Verifica se a tabela ApplicationUser já está criada
                var userExists = await connection.ExecuteScalarAsync<bool>(@"SELECT CASE WHEN EXISTS (
                                                                            SELECT 1
                                                                            FROM INFORMATION_SCHEMA.TABLES
                                                                            WHERE TABLE_SCHEMA = 'dbo'
	                                                                        AND  TABLE_NAME = 'ApplicationUser'
                                                                            )
                                                                            THEN CAST(1 AS BIT)
                                                                            ELSE CAST(0 AS BIT) END");
                //Cria a tabela ApplicationUser caso ela já não esteja criada
                if (!userExists)
                {
                    const string createApplicationUser = @"CREATE TABLE[dbo].[ApplicationUser]
                                                (
                                                    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
                                                    [UserName] NVARCHAR(256) NOT NULL,
                                                    [NormalizedUserName] NVARCHAR(256) NOT NULL,
                                                    [Name] NVARCHAR(256) NULL,
                                                    [Email] NVARCHAR(256) NULL,
                                                    [NormalizedEmail] NVARCHAR(256) NULL,
                                                    [PasswordHash] NVARCHAR(MAX) NULL,
                                                )";
                    const string index1 = @"CREATE INDEX[IX_ApplicationUser_NormalizedUserName] ON[dbo].[ApplicationUser]
                                                        ([NormalizedUserName])";
                    const string index2 = @"CREATE INDEX[IX_ApplicationUser_NormalizedEmail] ON[dbo].[ApplicationUser]
                                                        ([NormalizedEmail])";

                    await connection.ExecuteAsync(createApplicationUser);
                    await connection.ExecuteAsync(index1);
                    await connection.ExecuteAsync(index2);
                }
                //Verifica se a tabela ApplicationRole já está criada
                var rolesExists = await connection.ExecuteScalarAsync<bool>(@"SELECT CASE WHEN EXISTS (
                                                                             SELECT 1
                                                                             FROM INFORMATION_SCHEMA.TABLES
                                                                             WHERE TABLE_SCHEMA = 'dbo'
	                                                                         AND  TABLE_NAME = 'ApplicationRole'
                                                                             )
                                                                             THEN CAST(1 AS BIT)
                                                                             ELSE CAST(0 AS BIT) END");
                //Cria a tabela ApplicationRole caso ela já não esteja criada
                if (!rolesExists)
                {
                    const string createApplicationRole = @"CREATE TABLE [dbo].[ApplicationRole]
                                                          (
                                                              [Id] INT NOT NULL PRIMARY KEY IDENTITY,
                                                              [Name] NVARCHAR(256) NOT NULL,
                                                              [NormalizedName] NVARCHAR(256) NOT NULL
                                                          )";
                    const string index = @"CREATE INDEX [IX_ApplicationRole_NormalizedName]
                                           ON [dbo].[ApplicationRole] ([NormalizedName])";
                    await connection.ExecuteAsync(createApplicationRole);
                    await connection.ExecuteAsync(index);
                }
                //Verifica se a tabela ApplicationUserRole já está criada
                var userRoleExists = await connection.ExecuteScalarAsync<bool>(@"SELECT CASE WHEN EXISTS (
                                                                                SELECT 1
                                                                                FROM INFORMATION_SCHEMA.TABLES
                                                                                WHERE TABLE_SCHEMA = 'dbo'
	                                                                            AND  TABLE_NAME = 'ApplicationUserRole'
                                                                                )
                                                                                THEN CAST(1 AS BIT)
                                                                                ELSE CAST(0 AS BIT) END");
                //Cria a tabela ApplicationUserRole caso ela já não esteja criada
                if (!userRoleExists)
                {
                    const string createApplicationUserRole = @"CREATE TABLE [dbo].[ApplicationUserRole]
                                                          (
                                                              [UserId] INT NOT NULL,
                                                              [RoleId] INT NOT NULL
                                                              PRIMARY KEY ([UserId], [RoleId]),
                                                              CONSTRAINT [FK_ApplicationUserRole_User] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser]([Id]),
                                                              CONSTRAINT [FK_ApplicationUserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [ApplicationRole]([Id])
                                                          )";
                    await connection.ExecuteAsync(createApplicationUserRole);
                }
            }
        }
        
        /// <summary>
        /// Adiciona as roles "admin" e "user" no banco de dados
        /// </summary>
        private static async Task SeedRoles(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                if (await connection.ExecuteScalarAsync<string>($@"SELECT 1 FROM [ApplicationRole]") == null)
                {
                    await connection.QueryAsync($@"INSERT [dbo].[ApplicationRole] ([Name],[NormalizedName])
                                               VALUES ('user', 'USER'), ('admin', 'ADMIN')");
                }
            }
        }
        
        /// <summary>
        /// Adiciona o usuário admin
        /// </summary>
        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByNameAsync("admin@admin") == null)
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
