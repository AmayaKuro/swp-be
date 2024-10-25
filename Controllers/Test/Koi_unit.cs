using NUnit.Framework;
using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using swp_be.Controllers;
using swp_be.Models;
using swp_be.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Allure.NUnit;
using Microsoft.AspNetCore.Routing;
using NuGet.ContentModel;

namespace swp_be.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureEpic("Koi Management")]
    [AllureFeature("Koi CRUD Operations")]
    public class KoiControllerTests
    {
        private KoiController _controller;
        private ApplicationDBContext _context;

        [SetUp]
        [AllureBefore("Setup test database")]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: $"TestKoiDatabase_{TestContext.CurrentContext.Test.Name}")
                .Options;
            _context = new ApplicationDBContext(options);
            _controller = new KoiController(_context);
            SeedTestData();
        }

        private void SeedTestData()
        {
            _context.Kois.AddRange(new List<Koi>
            {
                new Koi { Name = "Tancho", Color = "Red and White", Age = 2, Price = 1000, Species = "Kohaku" },
                new Koi { Name = "Showa", Color = "Black, Red, and White", Age = 3, Price = 1500, Species = "Showa" }
            });
            _context.SaveChanges();
        }

        [Test]
        [AllureTag("GetKoi")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Get all Koi")]
        public async Task GetKoi_ReturnsAllKoi()
        {
            // Act
            var result = await _controller.GetKoi();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<List<Koi>>());
            var kois = okResult.Value as List<Koi>;
            Assert.That(kois.Count, Is.EqualTo(2));
        }

        [Test]
        [AllureTag("GetKoiById")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Get Koi by ID")]
        public async Task GetKoi_WithValidId_ReturnsKoi()
        {
            // Arrange
            var kois = await _context.Kois.ToListAsync();
            var firstKoi = kois.First();

            // Act
            var result = await _controller.GetKoi(firstKoi.KoiID);

            // Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.KoiID, Is.EqualTo(firstKoi.KoiID));
            Assert.That(result.Value.Name, Is.EqualTo("Tancho"));
        }

        [Test]
        [AllureTag("GetKoiById")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Get Koi by Invalid ID")]
        public async Task GetKoi_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetKoi(999);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [AllureTag("CreateKoi")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Create new Koi")]
        public async Task PostKoi_CreatesNewKoi()
        {
            // Arrange
            var newKoi = new Koi { Name = "Kohaku", Color = "Red and White", Age = 1, Price = 800, Species = "Kohaku" };

            // Act
            var result = await _controller.PostKoi(newKoi);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.That(createdResult.Value, Is.InstanceOf<Koi>());
            var createdKoi = createdResult.Value as Koi;
            Assert.That(createdKoi.Name, Is.EqualTo("Kohaku"));
        }

        [Test]
        [AllureTag("UpdateKoi")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Update existing Koi")]
        public async Task PutKoi_UpdatesExistingKoi()
        {
            // Arrange
            var kois = await _context.Kois.ToListAsync();
            var koiToUpdate = kois.First();
            koiToUpdate.Name = "Updated Tancho";
            koiToUpdate.Price = 1200;

            // Act
            var result = await _controller.PutKoi(koiToUpdate.KoiID, koiToUpdate);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            var updatedKoi = await _context.Kois.FindAsync(koiToUpdate.KoiID);
            Assert.That(updatedKoi.Name, Is.EqualTo("Updated Tancho"));
            Assert.That(updatedKoi.Price, Is.EqualTo(1200));
        }

        [Test]
        [AllureTag("UpdateKoi")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Update non-existing Koi")]
        public async Task PutKoi_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingKoi = new Koi { KoiID = 999, Name = "Non-existing", Color = "Blue", Age = 5, Price = 2000, Species = "Unknown" };

            // Act
            var result = await _controller.PutKoi(999, nonExistingKoi);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [AllureTag("DeleteKoi")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStory("Delete existing Koi")]
        public async Task DeleteKoi_RemovesExistingKoi()
        {
            // Arrange
            var kois = await _context.Kois.ToListAsync();
            var koiToDelete = kois.First();

            // Act
            var result = await _controller.DeleteKoi(koiToDelete.KoiID);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            var deletedKoi = await _context.Kois.FindAsync(koiToDelete.KoiID);
            Assert.That(deletedKoi, Is.Null);
        }

        [Test]
        [AllureTag("DeleteKoi")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStory("Delete non-existing Koi")]
        public async Task DeleteKoi_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteKoi(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [TearDown]
        [AllureAfter("Cleanup test database")]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}

