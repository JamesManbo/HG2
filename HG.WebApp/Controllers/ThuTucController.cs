using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
{
    public class ThuTucController : Controller
    {
        public IActionResult QuanLy()
        {
            return View();
        }

        public IActionResult ThemThuTuc()
        {
            return View();
        }
    }
}
