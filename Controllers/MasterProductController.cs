using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace simple_seller_app.Controllers
{
    public class MasterProductController : Controller
    {
        private readonly ILogger<MasterProductController> _logger;

        public MasterProductController(ILogger<MasterProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}