using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QApp.Models.Entities;


namespace QApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = @"Server=tcp:qapp.database.windows.net,1433;Initial Catalog=Milljas;Persist Security Info=False;User ID=milljas;Password=KronanWhite90;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddDbContext<MilljasContext>(
                options => options.UseSqlServer(connString));

            // EF från VS till SQL -- Måste denna vara aktiv??
            services.AddDbContext<IdentityDbContext>(
                options => options.UseSqlServer(connString));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(240);
                options.CookieHttpOnly = true;
            });

            

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Cookies.ApplicationCookie.LoginPath = "/teller/login";
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            
            services.AddMvc();
            services.AddMemoryCache();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/error");
            //}

            app.UseDeveloperExceptionPage();

            app.UseIdentity();

            app.UseStaticFiles();

            app.UseSession();
      

            app.UseMvcWithDefaultRoute();

            

            
        }
    }
}
