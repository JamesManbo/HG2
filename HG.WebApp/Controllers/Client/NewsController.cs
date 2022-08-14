using HG.WebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers.Client
{
    public class NewsController : Controller
    {
        public readonly static EAContext db = new EAContext();
        public IActionResult News()
        {
            return View();
        }

        public IActionResult GetListNews()
        {
            return PartialView(db.Tin_Tuc.Take(20).ToList());
        }
        public IActionResult OtherNews()
        {
            return PartialView(db.Tin_Tuc.Take(20).ToList());
        }
        public IActionResult XemChiTiet(int id)
        {
            return View();
        }
    }
}
