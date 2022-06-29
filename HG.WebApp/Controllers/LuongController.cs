using HG.Data.Business.DanhMuc;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Luong;
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
    public class LuongController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly LuongXuLyDao _danhmucDao;
        private readonly DanhMucDao _dmDao;

        //extend sys identity
        public LuongController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new LuongXuLyDao(DbProvider);
            _dmDao = new DanhMucDao(DbProvider);
        }

        #region Khai báo luồng xử lý 
        public IActionResult LuongXuLy()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            DanhMucModel nhomSearchItem = new DanhMucModel() { CurrentPage = 1, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhLuongXuLy(nhomSearchItem);
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            return View("~/Views/Luong/LuongXuLy/LuongXuLy.cshtml", ds.lstLuongXuLy);

        }

        public async Task<IActionResult> ChuyenMucPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            DanhMucModel nhomSearchItem = new DanhMucModel() { CurrentPage = currentPage, tu_khoa = tu_khoa, RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhLuongXuLy(nhomSearchItem);
            ds.Pagelist.CurrentPage = currentPage;
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? ds.Pagelist.TotalRecords : currentPage * pageSize;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Luong/LuongXuLy/LuongXuLy.cshtml", ds);
            return Content(result);
        }

        public IActionResult ThemLuongXuLy()
        {
            ViewBag.ThuTuc = _danhmucDao.DanhSachThuTuc();
            return View("~/Views/Luong/LuongXuLy/ThemLuongXuLy.cshtml");
        }

        [HttpPost]
        public IActionResult ThemLuongXuLy(Dm_Luong_Xu_Ly item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuLuongXuLy(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo luồng xử lý lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || _pb > 0)
            {
                return View("~/Views/Luong/LuongXuLy/ThemLuongXuLy.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaLuongXuLy", "Luong", new { code = item.ma_luong, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaLuongXuLy(string code, string type)
        {
            var luong_xu_ly = new Dm_Luong_Xu_Ly();
            ViewBag.ThuTuc = _danhmucDao.DanhSachThuTuc();
            using (var db = new EAContext())
            {
                luong_xu_ly = db.Dm_Luong_Xu_Ly.Where(n => n.Deleted == 0 && n.ma_luong == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/Luong/LuongXuLy/SuaLuongXuLy.cshtml", luong_xu_ly);
        }

        [HttpPost]
        public IActionResult SuaLuongXuLy(string code, string type, Dm_Luong_Xu_Ly item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuLuongXuLy(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật quốc tịch lỗi";
                return PartialView("~/Views/Luong/LuongXuLy/SuaLuongXuLy.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemLuongXuLy", "Luong");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaLuongXuLy", "Luong", new { code = item.ma_luong, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaQuocTich(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaLuongXuLy(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/Luong/LuongXuLy" });
        }

        public async Task<IActionResult> RenderViewQuocTich()
        {
            var lst = new List<Dm_Luong_Xu_Ly>();
            using (var db = new EAContext())
            {
                lst = db.Dm_Luong_Xu_Ly.Where(n => n.Deleted == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Luong/LuongXuLy/ViewLuongXuLy.cshtml", lst);
            return Content(result);
        }

        #endregion

        #region Bước xử lý       
        public IActionResult BuocXuLy()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var buocXuLy = new List<Dm_Buoc_Xu_Ly>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Buoc_Xu_Ly.Where(n => n.Deleted == 0).ToList();
                buocXuLy = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/Luong/BuocXuLy/BuocXuLy.cshtml", buocXuLy);
        }

        public async Task<IActionResult> BuocXuLyPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Buoc_Xu_Ly.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Luong/BuocXuLy/BuocXuLyPaging.cshtml", datapage);

            }
            return Content(result);
        }
        public IActionResult ThemBuocXuLy()
        {
            return View("~/Views/Luong/BuocXuLy/ThemBuocXuLy.cshtml");
        }

        [HttpPost]
        public IActionResult ThemBuocXuLy(Dm_Buoc_Xu_Ly item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuBuocXuLy(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo bước xử lý lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                return View("~/Views/Luong/BuocXuLy/ThemBuocXuLy.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaBuocXuLy", "Luong", new { code = item.ma_buoc_xu_ly, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaBuocXuLy(string code, string type)
        {
            var buocXuLy = new Dm_Buoc_Xu_Ly();
            using (var db = new EAContext())
            {
                buocXuLy = db.Dm_Buoc_Xu_Ly.Where(n => n.Deleted == 0 && n.ma_buoc_xu_ly == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/Luong/BuocXuLy/SuaBuocXuLy.cshtml", buocXuLy);
        }

        [HttpPost]
        public IActionResult SuaBuocXuLy(string code, string type, Dm_Buoc_Xu_Ly item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuBuocXuLy(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật bước xử lý lỗi";
                return PartialView("~/Views/Luong/BuocXuLy/SuaBuocXuLy.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemBuocXuLy", "Luong");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaBuocXuLy", "Luong", new { code = item.ma_buoc_xu_ly, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaBuocXuLy(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaBuocXuLy(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/luong/BuocXuLy" });
        }

        public async Task<IActionResult> RenderViewBuocXuLy(int type = 0)
        {

            var buocXuLy = new List<Dm_Buoc_Xu_Ly>();
            using (var db = new EAContext())
            {
                buocXuLy = db.Dm_Buoc_Xu_Ly.Where(n => n.Deleted == 0).ToList();
            }
            if (type == 1)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Luong/QuyTrinh/ViewBuocXuLy.cshtml", buocXuLy);
                return Content(result);
            }
            else
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Luong/BuocXuLy/ViewBuocXuLy.cshtml", buocXuLy);
                return Content(result);
            }
        }

        #endregion

        #region Bước xử lý       
        public IActionResult BuocThucHien()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var buocXuLy = new List<Dm_Buoc_Thuc_Hien>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Buoc_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
                buocXuLy = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/Luong/BuocThucHien/BuocThucHien.cshtml", buocXuLy);
        }

        public async Task<IActionResult> BuocThucHienPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Buoc_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Luong/BuocThucHien/BuocThucHienPaging.cshtml", datapage);

            }
            return Content(result);
        }
        public IActionResult ThemBuocThucHien()
        {
            return View("~/Views/Luong/BuocThucHien/ThemBuocThucHien.cshtml");
        }

        [HttpPost]
        public IActionResult ThemBuocThucHien(Dm_Buoc_Thuc_Hien item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuBuocThucHien(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo bước thực hiện lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                return View("~/Views/Luong/BuocThucHien/ThemBuocThucHien.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaBuocThucHien", "Luong", new { code = item.ma_buoc, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaBuocThucHien(string code, string type)
        {
            var buocThucHien = new Dm_Buoc_Thuc_Hien();
            using (var db = new EAContext())
            {
                buocThucHien = db.Dm_Buoc_Thuc_Hien.Where(n => n.Deleted == 0 && n.ma_buoc == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/Luong/BuocThucHien/SuaBuocThucHien.cshtml", buocThucHien);
        }

        [HttpPost]
        public IActionResult SuaBuocThucHien(string code, string type, Dm_Buoc_Thuc_Hien item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuBuocThucHien(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Cập nhật bước thực hiện lỗi";
                return PartialView("~/Views/Luong/BuocThucHien/SuaBuocThucHien.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemBuocThucHien", "Luong");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaBuocThucHien", "Luong", new { code = item.ma_buoc, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaBuocThucHien(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaBuocThucHien(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/luong/BuocThucHien" });
        }

        public async Task<IActionResult> RenderViewBuocThucHien(int type = 0)
        {

            var buocThucHien = new List<Dm_Buoc_Thuc_Hien>();
            var Views = "";
            using (var db = new EAContext())
            {
                buocThucHien = db.Dm_Buoc_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
            }
            if (type == 1)
            {
                Views = "~/Views/Luong/QuyTrinh/ViewBuocThucHien.cshtml";
            }
            else
            {
                Views = "~/Views/Luong/BuocThucHien/ViewBuocThucHien.cshtml";
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, Views, buocThucHien);
            return Content(result);
        }

        #endregion

        #region Quy trình xử lý 
        public IActionResult QuyTrinhXuLy(string code)
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            QuyTrinhModel nhomSearchItem = new QuyTrinhModel() { CurrentPage = 1, ma_luong = code, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhQuyTrinhXuLy(nhomSearchItem);
            ds.ma_luong = code;
            var user = _dmDao.DanhSachNguoiDung("0");
            var lstpb = new List<Dm_Phong_Ban>();
            var nhanhXuLy = new List<Dm_Nhanh_Xu_Ly>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                nhanhXuLy = db.Dm_Nhanh_Xu_Ly.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.NguoiDung = user;
            ViewBag.NhanhXuLy = nhanhXuLy;
            ViewBag.PhongBan = lstpb;
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            return View("~/Views/Luong/QuyTrinh/QuyTrinhXuLy.cshtml", ds);
        }

        public IActionResult SuaQuyTrinhXuLy(string code, string step)
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            QuyTrinhModel nhomSearchItem = new QuyTrinhModel() { CurrentPage = 1, ma_luong = code, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhQuyTrinhXuLy(nhomSearchItem);
            ds.ma_luong = code;
            ds.quyTrinhXuLy = ds.lstQuyTrinhXuLy.Where(n => n.ma_buoc == step).FirstOrDefault();
            var user = _dmDao.DanhSachNguoiDung("0");
            var lstpb = new List<Dm_Phong_Ban>();
            var nhanhXuLy = new List<Dm_Nhanh_Xu_Ly>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                nhanhXuLy = db.Dm_Nhanh_Xu_Ly.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.NguoiDung = user;
            ViewBag.NhanhXuLy = nhanhXuLy;
            ViewBag.PhongBan = lstpb;
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            return View("~/Views/Luong/QuyTrinh/QuyTrinhXuLy.cshtml", ds);
        }

        public IActionResult ThemQuyTrinhXuLy()
        {
            return View("~/Views/Luong/QuyTrinh/ThemQuyTrinhXuLy.cshtml");
        }

        [HttpPost]
        public IActionResult ThemQuyTrinhXuLy(QuyTrinhXuLy item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuQuyTrinhXuLy(item);
            if (response.ErrorCode > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = response.ErrorMsg;
            }
            if (response.ErrorCode > 0)
            {
                return View("~/Views/Luong/QuyTrinh/ThemQuyTrinhXuLy.cshtml", item);
            }
            else
            {
                return RedirectToAction("QuyTrinhXuLy", "Luong", new { code = item.ma_luong });
            }
        }
        #endregion
    }
}