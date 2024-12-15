using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Hubs;
using WebQuanLyNhaKhoa.Models;
using WebQuanLyNhaKhoa.ServicesPay;
using WebQuanLyNhaKhoa.wwwroot.AutoMapper;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    // Allow getting token from cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwt_token"];
            return Task.CompletedTask;
        }
    };
});

// Configure Kestrel to use specific ports
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7101, listenOptions =>
    {
        listenOptions.UseHttps(); // Bind to HTTPS
    });
    options.ListenLocalhost(7100); // Optional HTTP
});

// Configure services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers(options =>
{
    // Add the custom TimeSpan binder provider
    options.ModelBinderProviders.Insert(0, new TimeSpanBinderProvider());
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSignalR();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var hangfireConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHangfire(x => x.UseSqlServerStorage(hangfireConnectionString));
builder.Services.AddHangfireServer();


// Dependency Injection setup
builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/login";
        options.AccessDeniedPath = "/Home/404";
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Apply CORS policy
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Home/HandleError", "?statusCode={0}");

app.UseHangfireDashboard();
app.UseHangfireServer();


// Map Controller Routes
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=NhanViens}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllers();
app.MapRazorPages();

// Map SignalR hub
app.MapHub<ChatHub>("/chatHub");

app.UseCors("AllowAll");

// Run the app
app.Run();
