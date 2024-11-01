using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebQuanLyNhaKhoa.Models; // Đảm bảo rằng bạn đã import đúng namespace cho ApplicationUser
using WebQuanLyNhaKhoa.Areas.Identity.Pages.Account;
using WebQuanLyNhaKhoa.Data;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.LoginModel; // Import namespace chứa InputModel

namespace WebQuanLyNhaKhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(SignInManager<ApplicationUser> signInManager, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] InputModel input) // Sử dụng InputModel từ Identity
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                var user = await _signInManager.UserManager.FindByEmailAsync(input.Email);
                var token = GenerateJwtToken(user);

                return Ok(new { Token = token });
            }
            else if (result.RequiresTwoFactor)
            {
                return BadRequest(new { Error = "Requires two-factor authentication." });
            }
            else if (result.IsLockedOut)
            {
                return BadRequest(new { Error = "User account locked out." });
            }
            else
            {
                return BadRequest(new { Error = "Invalid login attempt." });
            }
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
