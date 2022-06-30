using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers.Client
{
    public class NewsController : Controller
    {
        public IActionResult News()
        {
            return View();
        }

        public IActionResult AddNews()
        {
            return View();
        }
    }
}
