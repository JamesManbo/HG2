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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
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
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/PhongBan" });
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
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/LinhVuc" });
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

        #region Chức vụ
        public IActionResult ChucVu()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var linhvuc = eAContext.Dm_Chuc_Vu.Where(n => n.Deleted == 0).ToList();
            return View("~/Views/DanhMuc/ChucVu/ChucVu.cshtml", linhvuc);

        }

        public IActionResult ThemChucVu()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            return View("~/Views/DanhMuc/ChucVu/ThemChucVu.cshtml");
        }

        [HttpPost]
        public IActionResult ThemChucVu(Dm_Chuc_Vu item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuChucVu(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo chức vụ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return View("~/Views/DanhMuc/ChucVu/ThemChucVu.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaChucVu", "DanhMuc", new { code = item.ma_chuc_vu, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaChucVu(string code, string type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var chucvu = eAContext.Dm_Chuc_Vu.Where(n => n.Deleted == 0 && n.ma_chuc_vu == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/ChucVu/SuaChucVu.cshtml", chucvu);
        }

        [HttpPost]
        public IActionResult SuaChucVu(string code, string type, Dm_Chuc_Vu item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuChucVu(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật chức vụ!";
                return PartialView("~/Views/DanhMuc/ChucVu/SuaChucVu.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemChucVu", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaChucVu", "DanhMuc", new { code = item.ma_chuc_vu, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaChucVu(string code)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaChucVu(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/ChucVu" });
        }

        public async Task<IActionResult> RenderViewChucVu()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var chucvu = eAContext.Dm_Chuc_Vu.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/ChucVu/ViewChucVu.cshtml", chucvu);
            return Content(result);
        }

        #endregion

        #region Giấy tờ hợp lệ
        public IActionResult GiayTo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var giayto = eAContext.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).ToList();
            return View("~/Views/DanhMuc/GiayTo/GiayTo.cshtml", giayto);

        }

        public IActionResult ThemGiayTo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            return View("~/Views/DanhMuc/GiayTo/ThemGiayTo.cshtml");
        }

        [HttpPost]
        public IActionResult ThemGiayTo(Dm_Giay_To_Hop_Le item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuGiayTo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo giấy tờ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return View("~/Views/DanhMuc/GiayTo/ThemGiayTo.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaGiayTo", "DanhMuc", new { code = item.ma_giay_to, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaGiayTo(string code, string type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var chucvu = eAContext.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0 && n.ma_giay_to == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/GiayTo/SuaGiayTo.cshtml", chucvu);
        }

        [HttpPost]
        public IActionResult SuaGiayTo(string code, string type, Dm_Giay_To_Hop_Le item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuGiayTo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập giấy tờ lỗi!";
                return PartialView("~/Views/DanhMuc/GiayTo/SuaGiayTo.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemGiayTo", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaGiayTo", "DanhMuc", new { code = item.ma_giay_to, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaGiayTo(string code)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaGiayTo(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/GiayTo" });
        }

        public async Task<IActionResult> RenderViewGiayTo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var giayto = eAContext.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/GiayTo/ViewGiayTo.cshtml", giayto);
            return Content(result);
        }

        #endregion

        #region Sổ hồ sơ
        public IActionResult SoHoSo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var giayto = eAContext.Dm_So_Ho_So.Where(n => n.Deleted == 0).ToList();
            return View("~/Views/DanhMuc/SoHoSo/SoHoSo.cshtml", giayto);

        }

        public IActionResult ThemSoHoSo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.LinhVuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            return View("~/Views/DanhMuc/SoHoSo/ThemSoHoSo.cshtml");
        }

        [HttpPost]
        public IActionResult ThemSoHoSo(Dm_So_Ho_So item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuSoHoSo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo sổ hồ sơ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                ViewBag.LinhVuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                return View("~/Views/DanhMuc/SoHoSo/ThemSoHoSo.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaSoHoSo", "DanhMuc", new { code = item.ma_so, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaSoHoSo(string code, string type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var hoso = eAContext.Dm_So_Ho_So.Where(n => n.Deleted == 0 && n.ma_so == code).FirstOrDefault();
            ViewBag.type_view = type;
            ViewBag.LinhVuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            return View("~/Views/DanhMuc/SoHoSo/SuaSoHoSo.cshtml", hoso);
        }

        [HttpPost]
        public IActionResult SuaSoHoSo(string code, string type, Dm_So_Ho_So item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuSoHoSo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập giấy tờ lỗi!";
                ViewBag.LinhVuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                return PartialView("~/Views/DanhMuc/SoHoSo/SuaSoHoSo.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemSoHoSo", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaSoHoSo", "DanhMuc", new { code = item.ma_so, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaSoHoSo(string code)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaSoHoSo(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/SoHoSo" });
        }

        public async Task<IActionResult> RenderViewSoHoSo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var hoso = eAContext.Dm_So_Ho_So.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/SoHoSo/ViewSoHoSo.cshtml", hoso);
            return Content(result);
        }

        #endregion

    }
}