using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using simple_seller_app.Contexts;
using simple_seller_app.Contexts.Tables;
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
        private readonly DbSellerContext db;

        public AccountController(DbSellerContext _db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            db = _db;
            _userManager = userManager;
            _signInManager = signInManager;
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
                // Use UserManager to check if the username already exists
                var user = await _userManager.FindByNameAsync(username);

                if (user != null)
                {
                    // Username already exists
                    return Json(new { status = false, message = "Username is already used" });
                }
                else
                {
                    // Username is available
                    return Json(new { status = true, message = "Username is available" });
                }
            }
            catch (Exception ex)
            {
                // Catch any exception and return the message
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CreateNewUser([FromBody] register req)
        {
            try
            {
                // Check if username already exists
                if (await _userManager.FindByNameAsync(req.username.ToLower()) != null)
                {
                    return new JsonResult(new { status = false, message = "Username is already used" });
                }

                // Create a new user
                var user = new IdentityUser
                {
                    UserName = req.username.ToLower(),
                };

                var result = await _userManager.CreateAsync(user, req.password);
                if (result.Succeeded)
                {
                    // Assign default role
                    string role = await _userManager.Users.CountAsync() <= 1 ? "ADMIN" : "KASIR";
                    await _userManager.AddToRoleAsync(user, role);

                    return new JsonResult(new { status = true, message = "Your account has been created successfully." });
                }

                // Handle errors
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new JsonResult(new { status = false, message = $"An error occurred: {errors}" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, message = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpPost]
        [Authorize] // Only authorized users can log out
        public async Task<JsonResult> Logout()
        {
            try
            {
                return Json(new { status = true, message = "Logged out." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}