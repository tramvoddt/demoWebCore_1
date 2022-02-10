using demoWebCore_1.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()

        {

            TempData["type"] = "nav1";

            return View();
        }

        public IActionResult Contact()
        {
            TempData["type"] = "nav2";
             BCrypt.Net.BCrypt.HashPassword("123456");

            TempData["aaa"] = BCrypt.Net.BCrypt.Verify("Pa$$w0rd", "$2a$11$GOXvcuJUAhq0rcN.5dzhauG0pOqIgd3OlYgKGKmjRq68Kw1Wq/1AG");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
