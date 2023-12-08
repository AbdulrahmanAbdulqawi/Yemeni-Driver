using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemeniDriver.Data;
using YemeniDriver.Models;

namespace YemeniDriver.Test
{
    public static class DbContextMocker
    {

      
        public static void Initialize(ApplicationDbContext dbContext)
        {
            // Add test data to the in-memory database
            dbContext.Users.Add(new ApplicationUser
            {
                Id = "userId1",
                UserName = "testuser1",
                Email = "testuser1@example.com",
                // Add other properties as needed
            });

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "userId2",
                UserName = "testuser2",
                Email = "testuser2@example.com",
                // Add other properties as needed
            });

            // Save changes to the in-memory database
            dbContext.SaveChanges();
        }
       
    }

    public static class UserManagerMocker
    {
        public static Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "someDriverId", UserName = "driver@example.com", Email = "driver@example.com"},
                // Add more test users if needed
            };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => users.Find(u => u.Id == id));

            userManager.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()));
                

            // Add more setup for other UserManager methods if needed

            return userManager;
        }
    }
}
