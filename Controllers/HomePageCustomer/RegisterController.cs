using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Models;

namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
	public class RegisterController : Controller
	{
		private readonly QlnhaKhoaContext _context;

		private readonly IMapper _mapper;
		public RegisterController(QlnhaKhoaContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var taiKhoan = _mapper.Map<TaiKhoan>(model);
				_context.Add(taiKhoan);
				_context.SaveChanges();
				return RedirectToAction("/Home/Index");
			}
			return View();
		}
	}
}
