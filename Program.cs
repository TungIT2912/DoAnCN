using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.wwwroot.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QlnhaKhoaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NhaKhoa"));
});

builder.Services.AddScoped<EmailService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


var app = builder.Build();

// Middleware ordering
app.UseRouting();  // Ensures routes are prepared

app.UseStatusCodePagesWithReExecute("/Home/HandleError", "?statusCode={0}");

app.UseStaticFiles(); // Allows serving static files after checking routes

// Error handling in non-development environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS redirection
app.UseHttpsRedirection();

// Authorization
app.UseAuthorization();

// Mapping controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();



















// var app = builder.Build();
// app.UseStaticFiles();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseStatusCodePagesWithReExecute("/Home/HandleError", "?statusCode={0}");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

//     // if (app.IsDevelopment())
//     // {
//     //     app.UseDeveloperExceptionPage();
//     // }
//     // else
//     // {
//     //     app.UseExceptionHandler("/Home/Error");
//     //     app.UseStatusCodePagesWithReExecute("/Home/HandleError", "?statusCode={0}");
//     // }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}");

// app.Run();
