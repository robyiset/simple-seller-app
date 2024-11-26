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

namespace simple_seller_app.Controllers
{
    public class AccountController : Controller
    {
        private DbSellerContext db;

        public AccountController(DbSellerContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginUser([FromBody] login req)
        {
            try
            {
                var user = db.u_user.Where(f => f.username == req.username.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(req.password, user.password))
                    {
                        return Json(new { status = true, message = "Welcome, " + req.username.ToLower() });
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
        public JsonResult CreateNewUser([FromBody] register req)
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

                    return Json(new { status = true, message = "Your account has been successfully registered" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}