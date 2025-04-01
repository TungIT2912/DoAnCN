using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using WebQuanLyNhaKhoa.Data;

namespace BookingTest.Controllers
{
    public class TestBase
    {
        protected readonly DbContextOptions<ApplicationDbContext> _options;
        protected readonly Mock<IWebHostEnvironment> _mockWebHost;

        public TestBase()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockWebHost = new Mock<IWebHostEnvironment>();
            _mockWebHost.Setup(m => m.WebRootPath).Returns(Path.GetTempPath());
        }
    }
}
