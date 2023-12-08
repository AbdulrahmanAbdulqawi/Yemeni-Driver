using Microsoft.AspNetCore.Identity;
using System.Net;
using YemeniDriver.Models;

namespace YemeniDriver.Data
{
    public class Seed
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                if (!await roleManager.RoleExistsAsync(Roles.Passenger.ToString()))
                    await roleManager.CreateAsync(new IdentityRole(Roles.Passenger.ToString()));
                if (!await roleManager.RoleExistsAsync(Roles.Driver.ToString()))
                    await roleManager.CreateAsync(new IdentityRole(Roles.Driver.ToString()));

                ////Users
                //var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                //string adminUserEmail = "Abdulrahman.Abdulqawi@gmail.com";

                //var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                //if (adminUser == null)
                //{
                //    var newAdminUser = new ApplicationUser()
                //    {
                //        UserName = "AbdulrahmanAbdulqawi",
                //        Email = adminUserEmail,
                //        EmailConfirmed = true,
                      
                //    };
                //    await userManager.CreateAsync(newAdminUser, "123456aA@");
                //    await userManager.AddToRoleAsync(newAdminUser, Roles.Admin.ToString());
                //}

                //string appPassngerUserEmail = "passenger@gmail.com";

                //var appPassengerUser = await userManager.FindByEmailAsync(appPassngerUserEmail);
                //if (appPassengerUser == null)
                //{
                //    var newAppUser = new ApplicationUser()
                //    {
                //        UserName = "passenger-user",
                //        Email = appPassngerUserEmail,
                //        EmailConfirmed = true,
                    
                //    };
                //    await userManager.CreateAsync(newAppUser, "123456aA@");
                //    await userManager.AddToRoleAsync(newAppUser, Roles.Passenger.ToString());
                //}

                //string appDriverUserEmail = "driver@gmail.com";

                //var appDriverUser = await userManager.FindByEmailAsync(appDriverUserEmail);
                //if (appDriverUser == null)
                //{
                //    var newAppUser = new ApplicationUser()
                //    {
                //        UserName = "driver-user",
                //        Email = appDriverUserEmail,
                //        EmailConfirmed = true,
                //        VehicleId = "123AA"
                //    };
                //    await userManager.CreateAsync(newAppUser, "123456aA@");
                //    await userManager.AddToRoleAsync(newAppUser, Roles.Driver.ToString());
                    
                //}
            }
        }
    }
}
