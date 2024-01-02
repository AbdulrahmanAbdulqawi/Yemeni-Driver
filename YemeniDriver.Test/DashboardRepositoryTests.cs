using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using System;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.Repository;

namespace YemeniDriver.Test
{
    public class DashboardRepositoryTests : IClassFixture<WebApplicationFactory<TestStartup>>, IDisposable
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ApplicationDbContext _dbContextMock;
        private readonly List<ApplicationUser> _users;

        public DashboardRepositoryTests()
        {
            var serviceProvider = new ServiceCollection()
               .AddEntityFrameworkInMemoryDatabase()
               .AddDbContext<ApplicationDbContext>(options =>
                   options.UseInMemoryDatabase("TestDatabase"))
               .BuildServiceProvider();


            _userManagerMock = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object,
                null, null, null, null, null, null, null, null);

            _dbContextMock = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _dbContextMock.Database.EnsureDeleted(); // Ensure the database is deleted before each test
            _dbContextMock.Database.EnsureCreated();

            _users = AddTestData();

            _userRepositoryMock = new UserRepository(_dbContextMock, _userManagerMock.Object);
            //_dashboardRepository = new DashboardRepository(_dbContextMock, _userManagerMock.Object, _userRepositoryMock);


        }

        [Fact]
        public async Task GetDriverByIdAsync_ReturnsDriver_WhenDriverExists()
        {
            // Arrange
            var driverId = "userId1";

            // Act
            var result = await _dashboardRepository.GetDriverByIdAsync(driverId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(driverId, result.Id);
        }

        [Fact]
        public async Task GetDriverByIdAsync_ThrowsException_WhenDriverNotFound()
        {
            // Arrange
            var driverId = "2";
            var result = _dashboardRepository.GetDriverByIdAsync(driverId);
            // Act & Assert
            await Should.ThrowAsync<Exception>(async () => await result);
        }

        [Fact]
        public async Task GetDrivers_ReturnsListOfDrivers_WhenDriversExist()
        {
            // Arrange
            _userManagerMock.Setup(x => x.GetUsersInRoleAsync(Roles.Driver.ToString()))
            .ReturnsAsync(_users);
            // Act
            var result = await _dashboardRepository.GetDrivers();

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldBe(_users);
        }

        //[Fact]
        //public async Task GetDrivers_ThrowsException_WhenDriversNotExist()
        //{

        //    _userManagerMock.Setup(x => x.GetUsersInRoleAsync(Roles.Driver.ToString()))
        //    .ReturnsAsync(_users);
        //    // Act
        //    var result = _dashboardRepository.GetDrivers();

        //    // Assert
        //    await Should.ThrowAsync<Exception>(async () => await result);

        //}


        public List<ApplicationUser> AddTestData()
        {
            _dbContextMock.Users.Add(new ApplicationUser
            {
                Id = "userId1",
                UserName = "testuser1",
                Email = "testuser1@example.com",
                Roles = Roles.Driver
                // Add other properties as needed
            });

            _dbContextMock.Users.Add(new ApplicationUser
            {
                Id = "userId2",
                UserName = "testuser2",
                Email = "testuser2@example.com",
                Roles = Roles.Passenger
                // Add other properties as needed
            });

            // Save changes to the in-memory database
            _dbContextMock.SaveChanges();
            return _dbContextMock.Users.ToList();
        }

        public void Dispose()
        {
            _dbContextMock.Dispose(); // Dispose the DbContext after all tests
        }
    }
}