using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.ServicesPay;
using WebQuanLyNhaKhoa.wwwroot.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure the DbContext with connection string from configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
		.AddDefaultTokenProviders()
		.AddDefaultUI()
		.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();


builder.Services.AddSingleton<IVnPayService,VnPayService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddAuthentication();




builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=DanhSachKhams}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();
app.Run();
