using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.Models;
using Xunit;

namespace RaceDay.Tests
{
    public class AuthTests
    {
        private RaceDayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task Register_NewUser_ReturnsSuccess()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var request = new AuthController.RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                Password = "Password123",
                Role = "Participant"
            };

            // Act
            var result = await controller.Register(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, context.Users.Count());
        }
    }
}