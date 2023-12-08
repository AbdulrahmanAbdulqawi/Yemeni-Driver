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
    public class DashboardRepositoryTests : IClassFixture<WebApplicationFactory<TestStartup>>
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ApplicationDbContext _dbContextMock;

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
            DbContextMocker.Initialize(_dbContextMock);

            _userRepositoryMock = new UserRepository(_dbContextMock, _userManagerMock.Object);
            _dashboardRepository = new DashboardRepository(_dbContextMock, _userManagerMock.Object, _userRepositoryMock);



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
            var expectedDrivers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "userId1", Email = "testuser1@example.com" },
                new ApplicationUser { Id = "userId2", Email = "testusr2@example.com" },
            };

            _userManagerMock.Setup(x => x.GetUsersInRoleAsync(Roles.Driver.ToString()))
            .ReturnsAsync(expectedDrivers);
            // Act
            var result = await _dashboardRepository.GetDrivers();

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldBe(expectedDrivers);
        }


    }
}