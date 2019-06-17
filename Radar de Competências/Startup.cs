using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RadarCompetencias.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RadarCompetencias.Models;


namespace RadarCompetencias
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Adiciona classes de usuários e roles para o serviço de injeção de dependências.
            services.AddTransient<IUserStore<ApplicationUser>, UserContext>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleContext>();
            services.AddSingleton<UserContext>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Alteração nos requerimentos de senha 
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

                //Tentativas máximas de login malsucedidas
                options.Lockout.MaxFailedAccessAttempts = 10;
            });

            //Alteração nas propriedades do cookie
            services.ConfigureApplicationCookie(options =>
            {
                //Cookie expira em 5 dias
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
                //Renova o cookie quando o usuário acessa com seu cookie próximo de expirar
                options.SlidingExpiration = true;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Users}/{action=Login}/{id?}");
            });
        }
    }
}
