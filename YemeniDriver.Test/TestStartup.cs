using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemeniDriver.Data;
using YemeniDriver.Helpers;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.Repository;

namespace YemeniDriver.Test
{
    public class TestStartup
    {
        public IConfiguration Configuration { get; }

        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure services here
            services.AddControllersWithViews();
            string uniqueDatabaseName = "TestDatabase_" + Guid.NewGuid().ToString();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(uniqueDatabaseName));


            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IUserRepository, UserRepository>(); // Add other repositories...

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            // Configure your test-specific settings if needed
            // For example, setting up a different configuration section
            services.Configure<CloudinarySettings>(Configuration.GetSection("TestCloudinarySettings"));
            // Add other services...
        }



        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
