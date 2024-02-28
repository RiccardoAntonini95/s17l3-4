using Microsoft.AspNetCore.Mvc;
using TeZenNis.Models;

namespace TeZenNis.Controllers
{
    public class AddProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name, double price, string description, string coverImg)
        {
            var product = StaticDb.Add(name, price, description, coverImg);
            return RedirectToAction("Index", "Home");
        }
    }
}
