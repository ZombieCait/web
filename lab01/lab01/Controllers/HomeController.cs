using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace lab01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Report()
        {
            return Report();
        }
        public IActionResult Contact()
        {
            return Contact();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
