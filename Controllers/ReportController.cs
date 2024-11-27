
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using simple_seller_app.Contexts;
using simple_seller_app.Contexts.StoreProcedures;
using simple_seller_app.Models;
namespace simple_seller_app.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly DbSellerContext db;

        public ReportController(DbSellerContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> getReport(string month)
        {
            if (string.IsNullOrEmpty(month))
            {
                return Json(new { status = true, data = new List<GetTransactionByMonth>() });
            }
            try
            {
                var parameter = new SqlParameter("@Month", month);
                var data = await db.Set<GetTransactionByMonth>().FromSqlRaw(
                    "EXEC dbo.GetTransactionByMonth @Month", parameter).ToListAsync();

                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message, data = new List<GetTransactionByMonth>() });
            }
        }
    }
}