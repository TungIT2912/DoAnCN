using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebQuanLyNhaKhoa.Areas.Employee.Controllers;
using WebQuanLyNhaKhoa.Controllers.AdminController;
using WebQuanLyNhaKhoa.Controllers.ApiConrtroller;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace BookingTest.Controllers
{
    public class BookAppointmentControllerTests : TestBase
    {
        [Fact]
        public async Task PostBenhNhan_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var context = new ApplicationDbContext(_options);
            var emailService = new Mock<EmailService>().Object;
            var controller = new BookAppointmentController(context, emailService);

            var dto = new BenhNhanDTO
            {
                HoTen = "AA Patient",
                time = "10:00",
                NgayKhamDau = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                EmailBn = "testing@gmail.com"
            };

            // Act
            var result = await controller.PostBenhNhan(dto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedDto = Assert.IsType<BenhNhanDTO>(createdAtActionResult.Value);

            Assert.Equal(dto.HoTen, returnedDto.HoTen);

            // Verify database entries
            var benhNhanInDb = await context.BenhNhans.FirstOrDefaultAsync();
            Assert.NotNull(benhNhanInDb);
            Assert.Equal(dto.HoTen, benhNhanInDb.HoTen);
        }

        [Fact]
        public async Task PostBenhNhan_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var context = new ApplicationDbContext(_options);
            var emailService = new Mock<EmailService>().Object; 
            var controller = new BookAppointmentController(context, emailService);

            controller.ModelState.AddModelError("HoTen", "Required");

            // Act
            var result = await controller.PostBenhNhan(new BenhNhanDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetBenhNhan_ExistingId_ReturnsOk()
        {
            // Arrange
            var context = new ApplicationDbContext(_options);
            var emailService = new Mock<EmailService>().Object; 
            var controller = new BookAppointmentController(context, emailService);

            var benhNhan = new BenhNhan { HoTen = "AA" };
            context.BenhNhans.Add(benhNhan);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.GetBenhhNhans();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDto = (okResult.Value as IEnumerable<BenhNhanDTO>).FirstOrDefault(b => b.IdbenhNhan == benhNhan.IdbenhNhan);
            Assert.NotNull(returnedDto);
            Assert.Equal(benhNhan.HoTen, returnedDto.HoTen);
        }

        [Fact]
        public async Task PostBenhNhan_DbSaveFails_RollsBack()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(_options);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            var emailService = new Mock<EmailService>().Object;
            // Use the mocked context here instead of a real one
            var controller = new BookAppointmentController(mockContext.Object, emailService);
            var dto = new BenhNhanDTO { /* valid data */ };

            // Act
            var result = await controller.PostBenhNhan(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            mockContext.Verify(c => c.Database.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
