using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForexMasters_site.Models;

namespace ForexMasters_site.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signals()
        {
            return View();
        }
        public IActionResult Resources()
        {
            return View();
        }
        public IActionResult Payments()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Disclaimer()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
