using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebQuanLyNhaKhoa.Controllers.ApiConrtroller;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace BookingTest.Mocks
{
    namespace BookingTest.Mocks
    {
        public class MockFileSystemService : BookAppointmentController
        {
            public MockFileSystemService(ApplicationDbContext context, EmailService emailService, IWebHostEnvironment env)
                : base(context, emailService) { }
        }
    }

}
