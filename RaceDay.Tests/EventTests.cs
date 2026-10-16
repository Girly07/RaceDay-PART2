using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.Models;
using System.Threading.Tasks;
using Xunit;

namespace RaceDay.Tests
{
    public class EventsTests
    {
        private RaceDayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task CreateEvent_OrganiserRole_ReturnsCreated()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new EventsController(context);

            // Mock the Session to simulate an Organiser logged in
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            httpContext.Session.SetInt32("UserID", 1);
            httpContext.Session.SetString("UserRole", "Organiser");
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var newEvent = new Event { Name = "Test Race", Description = "Test", Location = "Test", EventDate = System.DateTime.Now };

            // Act
            var result = await controller.CreateEvent(newEvent);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(1, context.Events.Count());
        }
    }

    // Helper class to mock ISession
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _storage = new Dictionary<string, byte[]>();
        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => _storage.Keys;

        public void Clear() => _storage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _storage.Remove(key);
        public void Set(string key, byte[] value) => _storage[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _storage.TryGetValue(key, out value);
    }
}