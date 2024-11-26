using Microsoft.AspNetCore.Mvc;
using simple_seller_app.Contexts;
using simple_seller_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace simple_seller_app.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginUser([FromBody] login req)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(req.username);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(req.username, req.password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return new JsonResult(new { status = true, message = "Welcome, " + req.username.ToLower() });
                    }
                }
                return new JsonResult(new { status = false, message = "Incorrect username or password" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> checkUsername(string? username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user != null)
                {
                    return Json(new { status = false, message = "Username is already used" });
                }
                else
                {
                    return Json(new { status = true, message = "Username is available" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CreateNewUser([FromBody] register req)
        {
            try
            {
                if (await _userManager.FindByNameAsync(req.username.ToLower()) != null)
                {
                    return new JsonResult(new { status = false, message = "Username is already used" });
                }

                var user = new IdentityUser
                {
                    UserName = req.username.ToLower(),
                };

                var result = await _userManager.CreateAsync(user, req.password);
                if (result.Succeeded)
                {
                    string role = await _userManager.Users.CountAsync() <= 1 ? "ADMIN" : "KASIR";

                    var roleExist = await _roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    await _userManager.AddToRoleAsync(user, role);

                    return new JsonResult(new { status = true, message = "Your account has been created successfully." });
                }

                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new JsonResult(new { status = false, message = $"An error occurred: {errors}" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();

                HttpContext.Session.Clear();

                return Json(new { status = true, message = "Logged out." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

    }
}