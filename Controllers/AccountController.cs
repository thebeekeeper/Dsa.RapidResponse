using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using Dsa.RapidResponse.Services;
using Dsa.RapidResponse.Implementations;

// this mostly came from http://www.blinkingcaret.com/2016/11/30/asp-net-identity-core-from-scratch/
namespace Dsa.RapidResponse
{
    public class AccountController : Controller
    {
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailService emailService, ComradeDbContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._messageService = emailService;
            _context = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var u = await _userManager.GetUserAsync(HttpContext.User);  
            return View(u);
        }

        //public async Task<IActionResult> UpdateSettings(IdentityUser user)
        public async Task<IActionResult> UpdateSettings(string phoneNumber)
        {
            var u = await _userManager.GetUserAsync(HttpContext.User);  
            u.PhoneNumber = phoneNumber; 
            var ir = await _userManager.UpdateAsync(u);
            //var written = _context.SaveChanges();
            return Redirect("Index");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                return View();
            }
            /*if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Confirm your email first");
                return View();
            }*/

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, password, isPersistent: rememberMe, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                return View();
            }

            return Redirect("~/");
        }

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string repassword)
        {
            if (password != repassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords don't match");
                return View();
            }

            var newUser = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var userCreationResult = await _userManager.CreateAsync(newUser, password);
            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View();
            }

            //await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, "Administrator"));

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var tokenVerificationUrl = Url.Action("VerifyEmail", "Account", new { id = newUser.Id, token = emailConfirmationToken }, Request.Scheme);

            _messageService.Send(email, $"Click <a href=\"{tokenVerificationUrl}\">here</a> to verify your email");

            //return Content("Check your email for a verification link");
            return Redirect("~/Account/Login");
        }

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _messageService;
        private readonly ComradeDbContext _context;
    }
}
