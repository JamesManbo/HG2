using HG.Data.Business.DanhMuc;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.WebApp.Data;
using HG.WebApp.Entities;
using HG.WebApp.Helper;
using HG.WebApp.Models.DanhMuc;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
{
    [Authorize]
    public class MenuController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly DanhMucMenuDao _danhmucDao;

        //extend sys identity
        public MenuController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new DanhMucMenuDao(DbProvider);
        }

        #region Menu
        public IActionResult TrangChinh()
        {
            var menu = new List<Dm_menu>();
            using (var db = new EAContext())
            {
                menu = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
            }
            return View("~/Views/Menu/MenuChinh/Menu.cshtml", menu);

        }

        public IActionResult ThemTrangChinh()
        {
            using (var db = new EAContext())
            {
                ViewBag.Menu = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
            }
            return View("~/Views/Menu/MenuChinh/ThemMenu.cshtml");
        }

        [HttpPost]
        public IActionResult ThemTrangChinh(Dm_menu item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuMenu(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo phòng ban lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || _pb > 0)
            {
                using (var db = new EAContext())
                {
                    ViewBag.Menu = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
                }

                return View("~/Views/Menu/MenuChinh/ThemMenu.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaTrangChinh", "Menu", new { code = item.ma_trang, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaTrangChinh(string code, string type)
        {
            var menu = new Dm_menu();
            using (var db = new EAContext())
            {
                menu = db.Dm_menu.Where(n => n.Deleted == 0 && n.ma_trang == code).FirstOrDefault();
            }
            //var hoso = eAContext.Dm_So_Ho_So.Where(n => n.Deleted == 0 && n.ma_so == code).FirstOrDefault();
            ViewBag.type_view = type;
            using (var db = new EAContext())
            {
                ViewBag.Menu = db.Dm_menu.Where(n => n.Deleted == 0).ToList();
            }
            return View("~/Views/Menu/MenuChinh/SuaMenu.cshtml", menu);
        }

        [HttpPost]
        public IActionResult SuaTrangChinh(string code, string type, Dm_menu item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuMenu(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật trang chính lỗi!";
                using (var db = new EAContext())
                {
                    ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                }
                return PartialView("~/Views/Menu/MenuChinh/SuaMenu.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemTrangChinh", "Menu");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaTrangChinh", "Menu", new { code = item.ma_trang, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaTrangChinh(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaMenu(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/Menu/TrangChinh" });
        }

        public async Task<IActionResult> RenderViewTrangChinh()
        {
            var lstpb = new List<Dm_menu>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Menu/MenuChinh/ViewMenu.cshtml", lstpb);
            return Content(result);
        }


        #endregion


    }
}