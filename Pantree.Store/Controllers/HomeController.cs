using Microsoft.AspNetCore.Mvc;
using Pantree.Helpers;
using Pantree.Store.Models;
using System.Diagnostics;

namespace Pantree.Store.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Cookies()
        {
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