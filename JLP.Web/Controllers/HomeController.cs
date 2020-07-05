using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DockerWeb.Models;
using JLP.DB;

namespace JLP.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IProductDAL ProductDAL { get; }

        public HomeController(ILogger<HomeController> logger
            , IProductDAL productDAL
            )
        {
            _logger = logger;
            this.ProductDAL = productDAL;
        }

        public IActionResult Index()
        {
            var products = this.ProductDAL.Query(1, 5);
            this.ViewData["Products"] = products.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
