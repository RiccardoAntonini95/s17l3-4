using Microsoft.AspNetCore.Mvc;
using TeZenNis.Models;

namespace TeZenNis.Controllers
{
    public class DetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index([FromRoute] int id)
        {
            var product = StaticDb.GetById(id);
            if (product is null)
            {
               return View("Error");
            }
             return View(product);
        }
    }
}
