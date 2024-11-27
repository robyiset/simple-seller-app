
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simple_seller_app.Contexts;
using simple_seller_app.Contexts.Tables;
using simple_seller_app.Models;

namespace simple_seller_app.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class MasterProductController : Controller
    {
        private readonly DbSellerContext db;

        public MasterProductController(DbSellerContext _db)
        {
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

        [HttpGet]
        public async Task<JsonResult> getProductDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { status = false, message = "Invalid product ID." });
            }
            try
            {
                var data = await db.m_product.Where(f => f.id == id).FirstOrDefaultAsync();
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message, data = new m_product() });
            }
        }

        [HttpPost]
        public async Task<JsonResult> createNewProduct([FromBody] product req)
        {
            try
            {
                await db.m_product.AddAsync(new m_product
                {
                    id = Guid.NewGuid().ToString(),
                    product_code = req.product_code,
                    product_name = req.product_name,
                    price = req.price,
                    created_date = DateTime.Now,
                    created_by = User.Identity.Name
                });

                await db.SaveChangesAsync();

                return Json(new { status = true, message = "Product added successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> editProduct([FromBody] product req)
        {
            if (string.IsNullOrEmpty(req.id))
            {
                return Json(new { status = false, message = "Invalid product ID" });
            }
            try
            {
                var data = await db.m_product.Where(f => f.id == req.id).FirstOrDefaultAsync();
                data.product_code = req.product_code;
                data.product_name = req.product_name;
                data.price = req.price;
                data.updated_date = DateTime.Now;
                data.updated_by = User.Identity.Name;
                db.m_product.Update(data);
                await db.SaveChangesAsync();

                return Json(new { status = true, message = "Product updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> deleteProduct([FromBody] product req)
        {
            if (string.IsNullOrEmpty(req.id))
            {
                return Json(new { status = false, message = "Failed to delete product" });
            }
            try
            {
                var data = await db.m_product.Where(f => f.id == req.id).FirstOrDefaultAsync();
                data.deleted_date = DateTime.Now;
                data.deleted_by = User.Identity.Name;
                db.m_product.Update(data);
                await db.SaveChangesAsync();

                return Json(new { status = true, message = "Product deleted!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex.Message });
            }
        }
    }
}