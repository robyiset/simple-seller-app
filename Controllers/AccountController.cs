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
        private readonly DbSellerContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(DbSellerContext _db, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            db = _db;
            userManager = _userManager;
            signInManager = _signInManager;
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
                // login with identity first
                var user = await userManager.FindByNameAsync(req.username.ToLower());
                if (user != null)
                {
                    // Check if the password matches
                    var result = await signInManager.PasswordSignInAsync(user, req.password, false, false);
                    if (result.Succeeded)
                    {
                        return Json(new { status = true, message = "[IDENTITY] Welcome, " + req.username.ToLower() });
                    }
                }


                var db_user = db.u_user.Where(f => f.username == req.username.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(req.password, db_user.password))
                    {
                        return Json(new { status = true, message = "[DATABASE] Welcome, " + req.username.ToLower() });
                    }
                }
                return Json(new { status = true, message = "Incorrect username or password" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult checkUsername(string? username)
        {
            try
            {
                if (db.u_user.Where(f => f.username == username).FirstOrDefault() != null)
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
                if (db.u_user.Where(f => f.username == req.username.ToLower()).FirstOrDefault() != null)
                {
                    return Json(new { status = false, message = "Username is already used" });
                }
                else
                {
                    string role = db.u_user.Count() == 0 ? "ADMIN" : "KASIR";
                    db.u_user.Add(new u_user
                    {
                        id = Guid.NewGuid().ToString(),
                        full_name = req.full_name,
                        username = req.username.ToLower(),
                        password = BCrypt.Net.BCrypt.HashPassword(req.password),
                        role = role,
                        register_date = DateTime.UtcNow,
                    });
                    db.SaveChanges();

                    //create user identity
                    role = (await userManager.Users.CountAsync()) == 0 ? "ADMIN" : "KASIR";

                    var user = new ApplicationUser
                    {
                        UserName = req.username.ToLower(),
                        FullName = req.full_name,
                        Role = role,
                        RegisterDate = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, req.password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);  // Assign role

                        return Json(new { status = true, message = "Your account has been successfully registered" });
                    }

                    return Json(new { status = false, message = "Failed to create user" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize] // Only authorized users can log out
        public async Task<JsonResult> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Json(new { status = true, message = "Logged out." });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}