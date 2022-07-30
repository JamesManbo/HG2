using HG.Data.Business.DanhMuc;
using HG.Entities;
using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Model;
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
    public class DanhMucController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly DanhMucDao _danhmucDao;

        //extend sys identity
        public DanhMucController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new DanhMucDao(DbProvider);
        }

        #region Phong Ban
        public IActionResult PhongBan()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var phongBan = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                phongBan = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.lst_phong_ban = phongBan;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/Phongban/PhongBan.cshtml", phongBan);

        }

        public async Task<IActionResult> PhongBanPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ma_phong_ban.ToUpper().Contains(tu_khoa.ToUpper()) || n.ten_phong_ban.ToUpper().Contains(tu_khoa.ToUpper())).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/Phongban/PhongBanPaging.cshtml", datapage);

            }
            return Content(result);
        }
        [HttpPost]
        public JsonResult CheckMaPb(string code)
        {
            var lstpb = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0 && n.ma_phong_ban.ToUpper() == code.ToUpper()).ToList();
            }
            if (lstpb.Count() == 0)
            {
                return Json(new { error = 0, href = "/Danhmuc/ThemPhongBan?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "/Danhmuc/SuaPhongBan?code=" + code.ToUpper() + "&type=" + StatusAction.Edit.ToString() });
        }

        public IActionResult ThemPhongBan(string code = "")
        {
            var modal = new Dm_Phong_Ban();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstpb = new List<Dm_Phong_Ban>();
            var lstdv = new List<dm_don_vi>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
            }
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_phong_ban = lstpb;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.code = code;
            return View("~/Views/DanhMuc/Phongban/ThemPhongBan.cshtml", modal);
        }

        [HttpPost]
        public IActionResult ThemPhongBan(Dm_Phong_Ban pb)
        {
            pb.ma_phong_ban = HelperString.CreateCode(pb.ma_phong_ban);
            pb.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            pb.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuPhongBan(pb);
            if (_pb.ErrorCode > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = _pb.ErrorMsg;
            }
            if (pb.type_view == StatusAction.Add.ToString() || _pb.ErrorCode > 0)
            {
                var user = _danhmucDao.DanhSachNguoiDung("");
                var lstpb = new List<Dm_Phong_Ban>();
                using (var db = new EAContext())
                {
                    lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
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
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstpb = new List<Dm_Phong_Ban>();
            var lstdv = new List<dm_don_vi>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
            }
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_phong_ban = lstpb;
            ViewBag.lst_don_vi = lstdv;
            var pb = lstpb.Where(n => n.ma_phong_ban == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/Phongban/SuaPhongBan.cshtml", pb);
        }

        [HttpPost]
        public IActionResult SuaPhongBan(string code, string type, Dm_Phong_Ban item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuPhongBan(item);
            if (_pb.ErrorCode > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = _pb.ErrorMsg;
                var user = _danhmucDao.DanhSachNguoiDung("");
                var lstpb = new List<Dm_Phong_Ban>();
                using (var db = new EAContext())
                {
                    lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
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
        public IActionResult XoaPhongBan(string code = "")
        {
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
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
            var lstpb = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/Phongban/ViewPhongBan.cshtml", lstpb);
            return Content(result);
        }

        [HttpPost]
        public async Task<IActionResult> RenderViewNguoiDung(string ma_phong_ban)
        {
            var ds_nguoi_dung = _danhmucDao.DanhSachNguoiDung(ma_phong_ban);
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/ViewNguoiDung.cshtml", ds_nguoi_dung);
            return Content(result);
        }


        #endregion

        #region Lĩnh vực
        public IActionResult LinhVuc()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var linhvuc = new List<Dm_Linh_Vuc>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                linhvuc = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/LinhVuc/LinhVuc.cshtml", linhvuc);
        }

        public async Task<IActionResult> LinhVucPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ma_linh_vuc.ToUpper().Contains(tu_khoa.ToUpper()) || n.ten_linh_vuc.ToUpper().Contains(tu_khoa.ToUpper())).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/LinhVuc/LinhVucPaging.cshtml", datapage);

            }
            return Content(result);
        }
        public IActionResult ThemLinhVuc(string code = "")
        {
            ViewBag.code = code;
            return View("~/Views/DanhMuc/LinhVuc/ThemLinhVuc.cshtml");
        }

        [HttpPost]
        public IActionResult ThemLinhVuc(Dm_Linh_Vuc item)
        {
            item.ma_linh_vuc = HelperString.CreateCode(item.ma_linh_vuc);
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuLinhVuc(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo phòng ban lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
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
            var linhvuc = new Dm_Linh_Vuc();
            using (var db = new EAContext())
            {
                linhvuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc == code).FirstOrDefault();
            }
            //var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/LinhVuc/SuaLinhVuc.cshtml", linhvuc);
        }

        [HttpPost]
        public IActionResult SuaLinhVuc(string code, string type, Dm_Linh_Vuc item)
        {
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
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
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
            var linhvuc = new List<Dm_Linh_Vuc>();
            using (var db = new EAContext())
            {
                linhvuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            //var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/LinhVuc/ViewLinhVuc.cshtml", linhvuc);
            return Content(result);
        }

        [HttpPost]
        public JsonResult CheckMaLv(string code)
        {
            var lstpb = new List<Dm_Linh_Vuc>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc.ToUpper() == code.ToUpper()).ToList();
            }
            if (lstpb.Count() == 0)
            {
                return Json(new { error = 0, href = "/Danhmuc/ThemLinhVuc?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "" });
        }

        #endregion


        #region ------ Danh mục kênh tin
        public IActionResult KenhTin()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var kenhtin = new List<Dm_Kenh_Tin>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Kenh_Tin.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                kenhtin = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/KenhTin/KenhTin.cshtml", kenhtin);
        }

        public async Task<IActionResult> KenhTinPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Kenh_Tin.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ma_kenh_tin.ToUpper().Contains(tu_khoa.ToUpper()) || n.ma_kenh_tin.ToUpper().Contains(tu_khoa.ToUpper())).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/KenhTin/KenhTinPaging.cshtml", datapage);

            }
            return Content(result);
        }
        public IActionResult ThemKenhTin(string code = "")
        {
            ViewBag.code = code;
            return View("~/Views/DanhMuc/KenhTin/ThemKenhTin.cshtml");
        }

        [HttpPost]
        public IActionResult ThemKenhTin(Dm_Kenh_Tin item)
        {
            item.ma_kenh_tin = HelperString.CreateCode(item.ma_kenh_tin);
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuKenhTin(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo kênh tin lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                return View("~/Views/DanhMuc/KenhTin/ThemKenhTin.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaKenhTin", "DanhMuc", new { code = item.ma_kenh_tin, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaKenhTin(string code, string type)
        {
            var kenhtin = new Dm_Kenh_Tin();
            using (var db = new EAContext())
            {
                kenhtin = db.Dm_Kenh_Tin.Where(n => n.Deleted == 0 && n.ma_kenh_tin == code).FirstOrDefault();
            }
            //var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/KenhTin/SuaKenhTin.cshtml", kenhtin);
        }

        [HttpPost]
        public IActionResult SuaKenhTin(string code, string type, Dm_Kenh_Tin item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuKenhTin(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật kênh tin lỗi";
                return PartialView("~/Views/DanhMuc/KenhTin/SuaKenhTin.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemKenhTin", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaKenhTin", "DanhMuc", new { code = item.ma_kenh_tin, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaKenhTin(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaKenhTin(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/KenhTin" });
        }

        public async Task<IActionResult> RenderViewKenhTin()
        {
            var kenhtin = new List<Dm_Kenh_Tin>();
            using (var db = new EAContext())
            {
                kenhtin = db.Dm_Kenh_Tin.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            //var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/KenhTin/ViewKenhTin.cshtml", kenhtin);
            return Content(result);
        }

        [HttpPost]
        public JsonResult CheckMaKT(string code)
        {
            var lstpb = new List<Dm_Kenh_Tin>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Kenh_Tin.Where(n => n.Deleted == 0 && n.ma_kenh_tin.ToUpper() == code.ToUpper()).ToList();
            }
            if (lstpb.Count() == 0)
            {
                return Json(new { error = 0, href = "/Danhmuc/ThemKenhTin?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "" });
        }

        #endregion


        #region Chức vụ
        public IActionResult ChucVu()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var chucvu = new List<Dm_Chuc_Vu>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                chucvu = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/ChucVu/ChucVu.cshtml", chucvu);

        }
        public async Task<IActionResult> ChucVuPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ma_chuc_vu.ToUpper().Contains(tu_khoa.ToUpper()) || n.ten_chuc_vu.ToUpper().Contains(tu_khoa.ToUpper())).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/ChucVu/ChucVuPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemChucVu(string code = "")
        {
            ViewBag.code = code;
            return View("~/Views/DanhMuc/ChucVu/ThemChucVu.cshtml");
        }

        [HttpPost]
        public IActionResult ThemChucVu(Dm_Chuc_Vu item)
        {
            item.ma_chuc_vu = HelperString.CreateCode(item.ma_chuc_vu);
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuChucVu(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo chức vụ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
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
            var chucvu = new Dm_Chuc_Vu();
            using (var db = new EAContext())
            {
                chucvu = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0 && n.ma_chuc_vu == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/ChucVu/SuaChucVu.cshtml", chucvu);
        }

        [HttpPost]
        public IActionResult SuaChucVu(string code, string type, Dm_Chuc_Vu item)
        {
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
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
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
            var chucvu = new List<Dm_Chuc_Vu>();
            using (var db = new EAContext())
            {
                chucvu = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            // var chucvu = eAContext.Dm_Chuc_Vu.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/ChucVu/ViewChucVu.cshtml", chucvu);
            return Content(result);
        }

        [HttpPost]
        public JsonResult CheckMaCv(string code)
        {
            var lstpb = new List<Dm_Chuc_Vu>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0 && n.ma_chuc_vu.ToUpper() == code.ToUpper()).ToList();
            }
            if (lstpb.Count() == 0)
            {
                return Json(new { error = 0, href = "/Danhmuc/ThemChucVu?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "" });
        }
        #endregion

        #region Giấy tờ hợp lệ
        public IActionResult GiayTo()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var giayto = new List<Dm_Giay_To_Hop_Le>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                giayto = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/GiayTo/GiayTo.cshtml", giayto);

        }

        public async Task<IActionResult> GiayToPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ma_giay_to.ToUpper().Contains(tu_khoa.ToUpper()) || n.ten_giay_to.ToUpper().Contains(tu_khoa.ToUpper())).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/GiayTo/GiayToPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemGiayTo(string code = "")
        {
            ViewBag.code = code;
            return View("~/Views/DanhMuc/GiayTo/ThemGiayTo.cshtml");
        }

        [HttpPost]
        public IActionResult ThemGiayTo(Dm_Giay_To_Hop_Le item)
        {
            item.ma_giay_to = HelperString.CreateCode(item.ma_giay_to);
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuGiayTo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo giấy tờ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
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
            var giayto = new Dm_Giay_To_Hop_Le();
            using (var db = new EAContext())
            {
                giayto = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0 && n.ma_giay_to == code).FirstOrDefault();
            }
            //var chucvu = eAContext.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0 && n.ma_giay_to == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/GiayTo/SuaGiayTo.cshtml", giayto);
        }

        [HttpPost]
        public IActionResult SuaGiayTo(string code, string type, Dm_Giay_To_Hop_Le item)
        {
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
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
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
            var giayto = new List<Dm_Giay_To_Hop_Le>();
            using (var db = new EAContext())
            {
                giayto = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            //var giayto = eAContext.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/GiayTo/ViewGiayTo.cshtml", giayto);
            return Content(result);
        }

        [HttpPost]
        public JsonResult CheckMaGt(string code)
        {
            var lstpb = new List<Dm_Giay_To_Hop_Le>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0 && n.ma_giay_to.ToUpper() == code.ToUpper()).ToList();
            }
            if (lstpb.Count() == 0)
            {
                return Json(new { error = 0, href = "/Danhmuc/ThemGiayTo?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "" });
        }
        #endregion

        #region Sổ hồ sơ
        public IActionResult SoHoSo()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var hoso = new List<Dm_So_Ho_So>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_So_Ho_So.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                hoso = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/SoHoSo/SoHoSo.cshtml", hoso);

        }
        public async Task<IActionResult> SoHoSoPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_So_Ho_So.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ten_so.ToUpper().Contains(tu_khoa.ToUpper())).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/SoHoSo/SoHoSoPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemSoHoSo(string code = "")
        {
            ViewBag.code = code;
            using (var db = new EAContext())
            {
                ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            // ViewBag.LinhVuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            return View("~/Views/DanhMuc/SoHoSo/ThemSoHoSo.cshtml");
        }

        [HttpPost]
        public IActionResult ThemSoHoSo(Dm_So_Ho_So item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuSoHoSo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo sổ hồ sơ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                using (var db = new EAContext())
                {
                    ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
                // ViewBag.LinhVuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
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
            var hoso = new Dm_So_Ho_So();
            using (var db = new EAContext())
            {
                hoso = db.Dm_So_Ho_So.Where(n => n.Deleted == 0 && n.ma_so == code).FirstOrDefault();
            }
            //var hoso = eAContext.Dm_So_Ho_So.Where(n => n.Deleted == 0 && n.ma_so == code).FirstOrDefault();
            ViewBag.type_view = type;
            using (var db = new EAContext())
            {
                ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            return View("~/Views/DanhMuc/SoHoSo/SuaSoHoSo.cshtml", hoso);
        }

        [HttpPost]
        public IActionResult SuaSoHoSo(string code, string type, Dm_So_Ho_So item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuSoHoSo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập giấy tờ lỗi!";
                using (var db = new EAContext())
                {
                    ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
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
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
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
            var hoso = new List<Dm_So_Ho_So>();
            using (var db = new EAContext())
            {
                hoso = db.Dm_So_Ho_So.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/SoHoSo/ViewSoHoSo.cshtml", hoso);
            return Content(result);
        }

        [HttpPost]
        public JsonResult CheckMaShs(string code)
        {
            var lstpb = new List<Dm_So_Ho_So>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_So_Ho_So.Where(n => n.Deleted == 0 && n.ma_so.ToUpper() == code.ToUpper()).ToList();
            }
            if (lstpb.Count() == 0)
            {
                return Json(new { error = 0, href = "/Danhmuc/ThemSoHoSo?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "" });
        }

        #endregion

        #region Ngày nghỉ
        public IActionResult NgayNghi(string type = "View")
        {
            var ngay_nghi = new Dm_Ngay_Nghi();
            using (var db = new EAContext())
            {
                ngay_nghi = db.Dm_Ngay_Nghi.Where(n => n.Deleted == 0).OrderByDescending(n => n.nam).FirstOrDefault();
                ViewBag.type_view = type;
            }
            return View("~/Views/DanhMuc/NgayNghi/NgayNghi.cshtml", ngay_nghi);

        }
        [HttpPost]
        public IActionResult NgayNghi(Dm_Ngay_Nghi item, string type)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuNgayNghi(item);
            ViewBag.type_view = item.type_view;
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Cập nhật ngày nghỉ lỗi!";
            }
            return RedirectToAction("NgayNghi", "DanhMuc");
            // return PartialView("~/Views/DanhMuc/NgayNghi/NgayNghi.cshtml", item);
        }
        #endregion

        #region ------ Đơn vị liên thông
        public IActionResult DonViLienThong()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var kenhtin = new List<Dm_Don_Vi_Lien_Thong>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                kenhtin = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/DonViLienThong/DonViLienThong.cshtml", kenhtin);
        }

        public async Task<IActionResult> DonViLienThongPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ma_don_vi.ToUpper().Contains(tu_khoa.ToUpper()) || n.ten_don_vi.ToUpper().Contains(tu_khoa.ToUpper())).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/DonViLienThong/DonViLienThongPaging.cshtml", datapage);

            }
            return Content(result);
        }
        public IActionResult ThemDonViLienThong(string code = "")
        {
            ViewBag.code = code;
            return View("~/Views/DanhMuc/DonViLienThong/ThemDonViLienThong.cshtml");
        }

        [HttpPost]
        public IActionResult ThemDonViLienThong(Dm_Don_Vi_Lien_Thong item)
        {
            item.ma_don_vi = HelperString.CreateCode(item.ma_don_vi);
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuDonViLienThong(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo kênh tin lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                return View("~/Views/DanhMuc/DonViLienThong/ThemDonViLienThong.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaDonViLienThong", "DanhMuc", new { code = item.ma_don_vi, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaDonViLienThong(string code, string type)
        {
            var kenhtin = new Dm_Don_Vi_Lien_Thong();
            using (var db = new EAContext())
            {
                kenhtin = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0 && n.ma_don_vi == code).FirstOrDefault();
            }
            //var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/DonViLienThong/SuaDonViLienThong.cshtml", kenhtin);
        }

        [HttpPost]
        public IActionResult SuaDonViLienThong(string code, string type, Dm_Don_Vi_Lien_Thong item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuDonViLienThong(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật kênh tin lỗi";
                return PartialView("~/Views/DanhMuc/DonViLienThong/SuaDonViLienThong.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemKenhTin", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaKenhTin", "DanhMuc", new { code = item.ma_don_vi, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaDonViLienThong(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaDonViLienThong(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/DonViLienThong" });
        }

        public async Task<IActionResult> RenderViewDonViLienThong()
        {
            var kenhtin = new List<Dm_Kenh_Tin>();
            using (var db = new EAContext())
            {
                kenhtin = db.Dm_Kenh_Tin.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            //var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/DonViLienThong/ViewDonViLienThong.cshtml", kenhtin);
            return Content(result);
        }

        [HttpPost]
        public JsonResult CheckMaDVLT(string code)
        {
            var lstpb = new List<Dm_Don_Vi_Lien_Thong>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0 && n.ma_don_vi.ToUpper() == code.ToUpper()).ToList();
            }
            if (lstpb.Count() == 0)
            {
                return Json(new { error = 0, href = "/Danhmuc/ThemDonViLienThong?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "" });
        }

        #endregion
    }
}