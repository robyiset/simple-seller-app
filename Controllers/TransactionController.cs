
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simple_seller_app.Contexts;
using simple_seller_app.Contexts.Tables;
using simple_seller_app.Models;
using simple_seller_app.Services;

namespace simple_seller_app.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly CalculatorService calculatorService;
        private readonly DbSellerContext db;

        public TransactionController(DbSellerContext _db, CalculatorService _calculatorService)
        {
            calculatorService = _calculatorService;
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> getData()
        {
            try
            {
                var data = await db.m_product
                                .Where(p => p.deleted_date == null)
                                .Select(p => new
                                {
                                    p.id,
                                    p.product_code,
                                    p.product_name,
                                    p.price
                                })
                                .ToListAsync();

                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message, data = new List<m_product>() });
            }
        }

        [HttpPost]
        public async Task<JsonResult> addTransaction([FromBody] List<transactions> req)
        {
            try
            {
                List<t_transaction> data = new List<t_transaction>();
                foreach (var item in req)
                {
                    data.Add(new t_transaction
                    {
                        id = Guid.NewGuid().ToString(),
                        transaction_code = "TO/" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        product_code = item.product_code,
                        quantity = item.quantity,
                        created_date = DateTime.Now,
                        created_by = User.Identity.Name
                    });
                }
                await db.t_transaction.AddRangeAsync(data);
                await db.SaveChangesAsync();

                return Json(new { status = true, message = "Success create new orders" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Add([FromBody] calculate req)
        {
            try
            {
                var sss = await calculatorService.Add(req.a, req.b);
                return Json(new { status = true, data = await calculatorService.Add(req.a, req.b) });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpPost]
        public async Task<JsonResult> Subtract([FromBody] calculate req)
        {
            try
            {
                return Json(new { status = true, data = await calculatorService.Subtract(req.a, req.b) });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpPost]
        public async Task<JsonResult> Multiply([FromBody] calculate req)
        {
            try
            {
                return Json(new { status = true, data = await calculatorService.Multiply(req.a, req.b) });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpPost]
        public async Task<JsonResult> Divide([FromBody] calculate req)
        {
            try
            {
                return Json(new { status = true, data = await calculatorService.Divide(req.a, req.b) });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message, data = string.Empty });
            }
        }

    }

}