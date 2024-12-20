﻿////// Licensed to the .NET Foundation under one or more agreements.
////// The .NET Foundation licenses this file to you under the MIT license.
////#nullable disable

////using System;
////using System.Collections.Generic;
////using System.ComponentModel.DataAnnotations;
////using System.Linq;
////using System.Text;
////using System.Text.Encodings.Web;
////using System.Threading;
////using System.Threading.Tasks;
////using Microsoft.AspNetCore.Authentication;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Identity.UI.Services;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
////using Microsoft.AspNetCore.Mvc.RazorPages;
////using Microsoft.AspNetCore.Mvc.Rendering;
////using Microsoft.AspNetCore.WebUtilities;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.Extensions.Logging;
////using WebQuanLyNhaKhoa.Data;

////namespace WebQuanLyNhaKhoa.Areas.Identity.Pages.Account
////{
////    public class RegisterModel : PageModel
////    {
////        private readonly ApplicationDbContext _context;
////        private readonly SignInManager<ApplicationUser> _signInManager;
////        private readonly RoleManager<IdentityRole> _roleManager;
////        private readonly UserManager<ApplicationUser> _userManager;
////        private readonly IUserStore<ApplicationUser> _userStore;
////        private readonly IUserEmailStore<ApplicationUser> _emailStore;
////        private readonly ILogger<RegisterModel> _logger;
////        private readonly IEmailSender _emailSender;

////        public RegisterModel(
////            UserManager<ApplicationUser> userManager,
////            RoleManager<IdentityRole> roleManager,
////            IUserStore<ApplicationUser> userStore,
////            SignInManager<ApplicationUser> signInManager,
////            ILogger<RegisterModel> logger,
////            IEmailSender emailSender)
////        {
////            _roleManager = roleManager;
////            _userManager = userManager;
////            _userStore = userStore;
////            _emailStore = GetEmailStore();
////            _signInManager = signInManager;
////            _logger = logger;
////            _emailSender = emailSender;
////        }

////        /// <summary>
////        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////        ///     directly from your code. This API may change or be removed in future releases.
////        /// </summary>
////        [BindProperty]
////        public InputModel Input { get; set; }

////        /// <summary>
////        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////        ///     directly from your code. This API may change or be removed in future releases.
////        /// </summary>
////        public string ReturnUrl { get; set; }

////        /// <summary>
////        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////        ///     directly from your code. This API may change or be removed in future releases.
////        /// </summary>
////        public IList<AuthenticationScheme> ExternalLogins { get; set; }

////        /// <summary>
////        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////        ///     directly from your code. This API may change or be removed in future releases.
////        /// </summary>
////        public class InputModel
////        {
////            [Required]
////            [StringLength(100, ErrorMessage = "Tên phải dài ít nhất 5 kí tự.", MinimumLength = 5)]
////            public string FullName { get; set; }
////            /// <summary>
////            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////            ///     directly from your code. This API may change or be removed in future releases.
////            /// </summary>
////            /// [Required]
////            [Display(Name = "Địa chỉ")]
////            public string Address { get; set; }
////            /// <summary>
////            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////            ///     directly from your code. This API may change or be removed in future releases.
////            /// </summary>
////            [Required]
////            [EmailAddress]
////            [Display(Name = "Email")]
////            public string Email { get; set; }

////            /// <summary>
////            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////            ///     directly from your code. This API may change or be removed in future releases.
////            /// </summary>
////            [Required]
////            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
////            [DataType(DataType.Password)]
////            [Display(Name = "Password")]
////            public string Password { get; set; }

////            /// <summary>
////            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
////            ///     directly from your code. This API may change or be removed in future releases.
////            /// </summary>
////            [DataType(DataType.Password)]
////            [Display(Name = "Confirm password")]
////            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
////            public string ConfirmPassword { get; set; }

////            public string? Role { get; set; }
////            //[ValidateNever]
////            //public IEnumerable<SelectListItem> RoleList { get; set; }
////        }
////        private async Task<string> SaveImage(IFormFile image)
////        {
////            if (image == null)
////            {
////                // Sử dụng hình ảnh mặc định nếu không có ảnh tải lên
////                return "/images/anonymous.png";
////            }

////            var savePath = Path.Combine("wwwroot/images", image.FileName);
////            using (var fileStream = new FileStream(savePath, FileMode.Create))
////            {
////                await image.CopyToAsync(fileStream);
////            }

////            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
////        }

////        public async Task OnGetAsync(string returnUrl = null)
////        {
////            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
////            {
////                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
////                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
////                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
////            }
////            Input = new()
////            {
////                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
////                {
////                    Text = i,
////                    Value = i
////                })
////            };
////            ReturnUrl = returnUrl;
////            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
////        }

////        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
////        {
////            returnUrl ??= Url.Content("~/");
////            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
////            if (ModelState.IsValid)
////            {
////                var user = CreateUser();
////                user.FullName = Input.FullName;
////                user.Address = Input.Address;
////                if (!string.IsNullOrEmpty(Input.Role))
////                {
////                    user.ChucVu = Input.Role;
////                }
////                else
////                {

////                    user.ChucVu = "Khách hàng";
////                }
////                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
////                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
////                //var imagePath = await SaveImage(Input.Avatar);
////                //user.AvatarPath = imagePath;

////                var result = await _userManager.CreateAsync(user, Input.Password);

////                if (result.Succeeded)
////                {
////                    _logger.LogInformation("User created a new account with password.");

////                    if (!String.IsNullOrEmpty(Input.Role))
////                    {
////                        await _userManager.AddToRoleAsync(user, Input.Role); ;
////                    }
////                    else
////                    {
////                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
////                    }
////                    var customer = new BenhNhan
////                    {
////                        UserId = user.Id,
////                        HoTen = Input.FullName

////                    };
////                    _context.BenhNhans.Add(customer);
////                    await _context.SaveChangesAsync();
////                    var userId = await _userManager.GetUserIdAsync(user);
////                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
////                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
////                    var callbackUrl = Url.Page(
////                        "/Account/ConfirmEmail",
////                        pageHandler: null,
////                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
////                        protocol: Request.Scheme);

////                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
////                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

////                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
////                    {
////                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
////                    }
////                    else
////                    {
////                        await _signInManager.SignInAsync(user, isPersistent: false);
////                        return LocalRedirect(returnUrl);
////                    }
////                }
////                foreach (var error in result.Errors)
////                {
////                    ModelState.AddModelError(string.Empty, error.Description);
////                }
////            }

////            // If we got this far, something failed, redisplay form
////            return Page();
////        }

////        private ApplicationUser CreateUser()
////        {
////            try
////            {
////                return Activator.CreateInstance<ApplicationUser>();
////            }
////            catch
////            {
////                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
////                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
////                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
////            }
////        }

////        private IUserEmailStore<ApplicationUser> GetEmailStore()
////        {
////            if (!_userManager.SupportsUserEmail)
////            {
////                throw new NotSupportedException("The default UI requires a user store with email support.");
////            }
////            return (IUserEmailStore<ApplicationUser>)_userStore;
////        }
////    }
////}


//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using WebQuanLyNhaKhoa.Data;

//namespace WebQuanLyNhaKhoa.Areas.Identity.Pages.Account
//{
//    public class RegisterModel : PageModel
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly IUserStore<ApplicationUser> _userStore;
//        private readonly IUserEmailStore<ApplicationUser> _emailStore;
//        private readonly ILogger<RegisterModel> _logger;
//        private readonly IEmailSender _emailSender;

//        public RegisterModel(
//            UserManager<ApplicationUser> userManager,
//            RoleManager<IdentityRole> roleManager,
//            IUserStore<ApplicationUser> userStore,
//            SignInManager<ApplicationUser> signInManager,
//            ILogger<RegisterModel> logger,
//            IEmailSender emailSender)
//        {
//            _roleManager = roleManager;
//            _userManager = userManager;
//            _userStore = userStore;
//            _emailStore = GetEmailStore();
//            _signInManager = signInManager;
//            _logger = logger;
//            _emailSender = emailSender;
//        }

//        [BindProperty]
//        public InputModel Input { get; set; }

//        public string ReturnUrl { get; set; }

//        public IList<AuthenticationScheme> ExternalLogins { get; set; }

//        public class InputModel
//        {
//            [Required]
//            [StringLength(100, ErrorMessage = "Tên phải dài ít nhất 5 kí tự.", MinimumLength = 5)]
//            public string FullName { get; set; }

//            [Display(Name = "Địa chỉ")]
//            public string Address { get; set; }

//            [Required]
//            [EmailAddress]
//            [Display(Name = "Email")]
//            public string Email { get; set; }

//            [Required]
//            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
//            [DataType(DataType.Password)]
//            [Display(Name = "Password")]
//            public string Password { get; set; }

//            [DataType(DataType.Password)]
//            [Display(Name = "Confirm password")]
//            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//            public string ConfirmPassword { get; set; }

//            public string? Role { get; set; }
//        }

//        public async Task OnGetAsync(string returnUrl = null)
//        {
//            ReturnUrl = returnUrl;
//            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
//        }

//        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
//        {
//            returnUrl ??= Url.Content("~/");
//            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

//            if (ModelState.IsValid)
//            {
//                var user = new ApplicationUser
//                {
//                    FullName = Input.FullName,
//                    Address = Input.Address,
//                    Email = Input.Email,
//                    UserName = Input.Email,
//                    ChucVu = Input.Role
//                };  

//                var result = await _userManager.CreateAsync(user, Input.Password);

//                if (result.Succeeded)
//                {
//                    _logger.LogInformation("User created a new account with password.");

//                    // Assign role based on user input or default to Customer
//                    var assignedRole = Input.Role ?? "Customer";

//                    // Ensure the role exists before assigning
//                    if (await _roleManager.RoleExistsAsync(assignedRole))
//                    {
//                        await _userManager.AddToRoleAsync(user, assignedRole);

//                        // Example policy-based logic (optional): Redirect based on role
//                        if (assignedRole == "Admin" && User.IsInRole("Admin"))
//                        {
//                            return LocalRedirect(Url.Page("/Admin/Dashboard"));
//                        }
//                        else if (assignedRole == "Customer")
//                        {
//                            return LocalRedirect(Url.Page("/Customer/Home"));
//                        }
//                        else if (assignedRole == "Staff" && User.IsInRole("Staff"))
//                        {
//                            return LocalRedirect(Url.Page("/Staff/Tasks"));
//                        }
//                    }
//                    else
//                    {
//                        ModelState.AddModelError(string.Empty, "Invalid role specified.");
//                    }

//                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
//                    var callbackUrl = Url.Page(
//                        "/Account/ConfirmEmail",
//                        pageHandler: null,
//                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
//                        protocol: Request.Scheme);

//                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
//                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

//                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
//                    {
//                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
//                    }
//                    else
//                    {
//                        await _signInManager.SignInAsync(user, isPersistent: false);
//                        return LocalRedirect(returnUrl);
//                    }
//                }
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//            }

//            // If we got this far, something failed, redisplay form
//            return Page();
//        }

//        private IUserEmailStore<ApplicationUser> GetEmailStore()
//        {
//            if (!_userManager.SupportsUserEmail)
//            {
//                throw new NotSupportedException("The default UI requires a user store with email support.");
//            }
//            return (IUserEmailStore<ApplicationUser>)_userStore;
//        }
//    }
//}
