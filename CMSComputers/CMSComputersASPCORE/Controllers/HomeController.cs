using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMSComputersASPCORE.Models;


namespace CMSComputersASPCORE.Controllers
{
    public class HomeController : Controller
    {
        
        public int active = 0;

        private readonly ILogger<HomeController> _logger;

        //public int Active { get => active; set => active = value; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.ID = 1;            
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.ID = 2;
            return View();
        }

        public IActionResult ContactUs()
        {
            ViewBag.ID = 3;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
