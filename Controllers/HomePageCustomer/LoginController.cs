using Microsoft.AspNetCore.Mvc;

namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
    public class LoginController : Controller
    {
        public LoginController() { }
                
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
