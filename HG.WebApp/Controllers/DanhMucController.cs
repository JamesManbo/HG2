using HG.Data.Business.DanhMuc;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.WebApp.Data;
using HG.WebApp.Entities;
using HG.WebApp.Helper;
using HG.WebApp.Models.DanhMuc;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
{
    public class DanhMucController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly DanhMucDao _danhmucDao;

        //extend sys identity
        public DanhMucController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new DanhMucDao(DbProvider);
        }

        #region Phong Ban
        public IActionResult PhongBan()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var phongBan = eAContext.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_phong_ban = phongBan;
            return View("~/Views/DanhMuc/Phongban/PhongBan.cshtml", phongBan);

        }

        public IActionResult ThemPhongBan()
        {
            var modal = new Dm_Phong_Ban();
            var user = _danhmucDao.DanhSachNguoiDung("0");
            var lstpb = eAContext.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_phong_ban = lstpb;
            return View("~/Views/DanhMuc/Phongban/ThemPhongBan.cshtml", modal);
        }

        [HttpPost]
        public IActionResult ThemPhongBan(Dm_Phong_Ban pb)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            pb.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            pb.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuPhongBan(pb);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo phòng ban lỗi";
            }
            if (pb.type_view == StatusAction.Add.ToString())
            {
                var user = _danhmucDao.DanhSachNguoiDung("0");
                var lstpb = eAContext.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.lst_nguoi_dung = user;
                ViewBag.lst_phong_ban = lstpb;
                return View("~/Views/DanhMuc/Phongban/ThemPhongBan.cshtml", pb);
            }
            if (pb.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaPhongBan", "DanhMuc", new { code = pb.ma_phong_ban, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaPhongBan(string code, string type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var user = _danhmucDao.DanhSachNguoiDung("0");
            var lstpb = eAContext.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
            var pb = lstpb.Where(n => n.ma_phong_ban == code).FirstOrDefault();
            ViewBag.lst_phong_ban = lstpb;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/Phongban/SuaPhongBan.cshtml", pb);
        }

        [HttpPost]
        public IActionResult SuaPhongBan(string code, string type, Dm_Phong_Ban item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuPhongBan(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật phòng ban lỗi";
                var user = _danhmucDao.DanhSachNguoiDung("0");
                var lstpb = eAContext.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.lst_phong_ban = lstpb;
                ViewBag.lst_nguoi_dung = user;
                return PartialView("~/Views/DanhMuc/Phongban/SuaPhongBan.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemPhongBan", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaPhongBan", "DanhMuc", new { code = item.ma_phong_ban, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaPhongBan(string code)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaPhongBan(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!" });
        }

        public async Task<IActionResult> RenderViewPhongBan()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var phongBan = eAContext.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/Phongban/ViewPhongBan.cshtml", phongBan);
            return Content(result);
        }

        [HttpPost]
        public async Task<IActionResult> RenderViewNguoiDung(string ma_phong_ban)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var ds_nguoi_dung = _danhmucDao.DanhSachNguoiDung(ma_phong_ban);
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/Phongban/ViewNguoiDung.cshtml", ds_nguoi_dung);
            return Content(result);
        }
        #endregion

        #region Lĩnh vực
        public IActionResult LinhVuc()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            return View("~/Views/DanhMuc/LinhVuc/LinhVuc.cshtml", linhvuc);

        }

        public IActionResult ThemLinhVuc()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            return View("~/Views/DanhMuc/LinhVuc/ThemLinhVuc.cshtml");
        }

        [HttpPost]
        public IActionResult ThemLinhVuc(Dm_Linh_Vuc item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuLinhVuc(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo phòng ban lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                var lstpb = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                return View("~/Views/DanhMuc/LinhVuc/ThemLinhVuc.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaLinhVuc", "DanhMuc", new { code = item.ma_linh_vuc, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaLinhVuc(string code, string type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/LinhVuc/SuaLinhVuc.cshtml", linhvuc);
        }

        [HttpPost]
        public IActionResult SuaLinhVuc(string code, string type, Dm_Linh_Vuc item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuLinhVuc(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật phòng ban lỗi";
                var lstpb = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc == code).FirstOrDefault();
                return PartialView("~/Views/DanhMuc/LinhVuc/SuaLinhVuc.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemLinhVuc", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaLinhVuc", "DanhMuc", new { code = item.ma_linh_vuc, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaLinhVuc(string code)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaLinhVuc(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!" });
        }

        public async Task<IActionResult> RenderViewLinhVuc()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/LinhVuc/ViewLinhVuc.cshtml", linhvuc);
            return Content(result);
        }

        #endregion


    }
}