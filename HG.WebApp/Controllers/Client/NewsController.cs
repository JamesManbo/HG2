using HG.WebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers.Client
{
    public class NewsController : Controller
    {
        public IActionResult News()
        {
            return View();
        }

        public IActionResult GetListNews()
        {
            var db = new EAContext();
            return PartialView(db.Tin_Tuc.Take(20).ToList());
        }
    }
}
