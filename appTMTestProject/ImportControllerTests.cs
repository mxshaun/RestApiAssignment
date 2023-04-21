using appTM.Controllers;
using appTM.Data;
using appTM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appTM.Tests
{
    public class ImportControllerTests
    {
        private readonly ImportController controller;
        private readonly MusicDbContext context;

        public ImportControllerTests()
        {
            var options = new DbContextOptionsBuilder<MusicDbContext>()
                .UseInMemoryDatabase(databaseName: "MyRockingMusicDB")
                .Options;

            context = new MusicDbContext(options);

            controller = new ImportController(context);
        }

        [Fact]
        public async Task Create_ShouldReturnNoContent_WhenSuccessful()
        {
            // Act
            var result = await controller.Create();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Create_ShouldRemoveDuplicates_WhenSuccessful()
        {
            // Arrange
            // Add some data to the context for testing
            context.Artists.Add(new Artist { Id = 20000, Name = "Metallica" });
            context.Songs.Add(new Song { Id = 20000, Name = "Enter Sandman", Year = 1990, Artist = "Metallica", Shortname = "Mtl", Genre = "Rock" });
            await context.SaveChangesAsync();

            // Act
            await controller.Create();

            // Assert
            Assert.Equal(888, await context.Artists.CountAsync()); // No duplicate artist added from the import service
            Assert.Equal(252, await context.Songs.CountAsync()); // One duplicate song removed from the import service
        }
    }
}