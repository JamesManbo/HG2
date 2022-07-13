using HG.Data.Business.DanhMuc;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Luong;
using HG.Entities.Entities.Model;
using HG.WebApp.Data;
using HG.WebApp.Entities;
using HG.WebApp.Helper;
using HG.WebApp.Models;
using HG.WebApp.Models.DanhMuc;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Xml.Serialization;

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
            var ds = _danhmucDao.DanhSachLuongKey();
            ViewBag.ThuTuc = _danhmucDao.DanhSachThuTuc();
            ViewBag.LuongKey = ds;
            return View("~/Views/Luong/LuongXuLy/ThemLuongXuLy.cshtml");
        }

        [HttpPost]
        public IActionResult ThemLuongXuLy(Dm_Luong_Xu_Ly item)
        {
            item.ma_luong = HelperString.CreateCode(item.ma_luong);
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
            if (luong_xu_ly != null && luong_xu_ly.tt_hai_gd)
            {
                var ds = _danhmucDao.DanhSachLuongKey();
                ViewBag.LuongKey = ds;
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
        public IActionResult XoaLuongXuLy(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaLuongXuLy(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/Luong/LuongXuLy" });
        }

        public async Task<IActionResult> RenderViewLuongXuLy()
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

        #region Bước thực hiện
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
        public IActionResult QuyTrinhXuLy(string code, string views = "View")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            QuyTrinhModel nhomSearchItem = new QuyTrinhModel() { CurrentPage = 1, ma_luong = code, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhQuyTrinhXuLy(nhomSearchItem);
            ds.ma_luong = code;
            var user = _dmDao.DanhSachNguoiDung("");
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
            ViewBag.Views = views;
            return View("~/Views/Luong/QuyTrinh/QuyTrinhXuLy.cshtml", ds);
        }

        public IActionResult SuaQuyTrinhXuLy(string code, int step)
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            QuyTrinhModel nhomSearchItem = new QuyTrinhModel() { CurrentPage = 1, ma_luong = code, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhQuyTrinhXuLy(nhomSearchItem);
            ds.ma_luong = code;
            ds.quyTrinhXuLy = ds.lstQuyTrinhXuLy.FirstOrDefault(n => n.Id == step);
            var user = _dmDao.DanhSachNguoiDung("");
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
            return View("~/Views/Luong/QuyTrinh/SuaQuyTrinhXuLy.cshtml", ds);
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

        public IActionResult XoaQuyTrinhXuLy(int id, string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaQuyTrinhXuLy(id, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/luong/QuyTrinhXuLy?code=" + code });
        }
        #endregion         

        [HttpPost]
        public async Task<int> ReadFileExcel(IFormFile file)
        {
            var code = 0;
            var list = new List<string>();
            var listdata = new List<DataLuong>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var columnCount = worksheet.Dimension.Columns;
                    for (int i = 2; i < rowCount + 1; i++)
                    {
                        var luong = new DataLuong();
                        for (int j = 1; j < columnCount + 1; j++)
                        {

                            if (j == 1)
                            {
                                luong.ma_luong = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 2)
                            {
                                luong.ten_luong = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 3)
                            {
                                luong.mo_ta = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 4)
                            {
                                var matt = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";

                                luong.ma_thu_tuc = matt;
                            }
                            else if (j == 5)
                            {
                                luong.ten_buoc = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 6)
                            {
                                luong.nguoi_xl_mac_dinh = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 7)
                            {
                                luong.nguoi_co_the_xl = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 8)
                            {
                                luong.nguoi_phoi_hop_xl = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 9)
                            {
                                luong.so_ngay_xl = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 10)
                            {
                                luong.stt = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString()) ?? "";
                            }
                            else if (j == 11)
                            {
                                luong.buoc_xl_chinh = worksheet.Cells[i, j].Value == null ? false : worksheet.Cells[i, j].Value.ToString() == "X" ? true : false;
                            }
                            else
                            {
                                if (j == 12)
                                {
                                    luong.chuc_nang = (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "KYHS," : "") ?? "";
                                }
                                else if (j == 13)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "YCBSHS," : "") ?? "";
                                }
                                else if (j == 14)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "HSKDK," : "") ?? "";
                                }
                                else if (j == 15)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "THUHOIHS," : "") ?? "";
                                }
                                else if (j == 16)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "TRALAIHS," : "") ?? "";
                                }
                                else if (j == 17)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "CMC," : "") ?? "";
                                }
                                else if (j == 18)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "GUIHSLT," : "") ?? "";
                                }
                                else if (j == 19)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "KTXL," : "") ?? "";
                                }
                                else if (j == 20)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "INPHOIKQ," : "") ?? "";
                                }
                                else if (j == 21)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "GUIKQLT," : "") ?? "";
                                }
                                else if (j == 22)
                                {
                                    luong.chuc_nang += (worksheet.Cells[i, j].Value == null ? "" : worksheet.Cells[i, j].Value.ToString() == "X" ? "THONGBAOCD," : "") ?? "";
                                }
                            }
                        }
                        if (luong.chuc_nang.Length > 0)
                        {
                            luong.chuc_nang = luong.chuc_nang.Remove(luong.chuc_nang.Length - 1, 1);
                        }
                        listdata.Add(luong);
                    }
                    var item = new Dm_Luong_Xu_Ly();
                    // Insert luồng
                    if (listdata.FirstOrDefault() != null)
                    {
                        item.ma_luong = listdata.FirstOrDefault().ma_luong;
                        item.ten_luong = listdata.FirstOrDefault().ten_luong;
                        item.mo_ta = listdata.FirstOrDefault().mo_ta;
                        item.tt_hai_gd = false;
                        item.ma_thu_tuc = listdata.FirstOrDefault().ma_thu_tuc;
                        item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                        item.UidName = User.Identity.Name;
                        var _pb = _danhmucDao.LuuLuongXuLy(item);
                        if (_pb == 0)
                        {
                            // Insert quy trình
                            foreach (var itemlist in listdata)
                            {
                                var item2 = new QuyTrinhXuLy();
                                item2.ma_luong = itemlist.ma_luong;
                                item2.ten_buoc = itemlist.ten_buoc;
                                item2.so_ngay_xl = float.Parse(itemlist.so_ngay_xl);
                                item2.buoc_xl_chinh = itemlist.buoc_xl_chinh;
                                item2.chuc_nang = itemlist.chuc_nang;
                                item2.nguoi_xl = itemlist.nguoi_xl_mac_dinh;
                                item2.nguoi_co_the_xl = itemlist.nguoi_co_the_xl;
                                item2.nguoi_phoi_hop_xl = itemlist.nguoi_phoi_hop_xl;
                                item2.Stt = Convert.ToInt32(itemlist.stt);
                                item2.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                                item2.UidName = User.Identity.Name;
                                var _pb2 = _danhmucDao.LuuQuyTrinhXuLyExcel(item2);
                                if (_pb2.ErrorCode == 0)
                                {
                                    code = 1;
                                }
                            }
                        }
                    }
                }
            }
            return code;
        }


        public async Task<IActionResult> ExportLuong(string code)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();
            var list = _danhmucDao.LuongExcel(code);
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                var modelTable = workSheet.Cells;
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                workSheet.Cells[1, 1].Value = "Mã luồng";
                workSheet.Cells[1, 2].Value = "Tên luồng";
                workSheet.Cells[1, 3].Value = "Mô tả";
                workSheet.Cells[1, 4].Value = "Mã TTHC";
                workSheet.Cells[1, 5].Value = "Tên bước thực hiện";
                workSheet.Cells[1, 6].Value = "Người XL";
                workSheet.Cells[1, 7].Value = "Người có thể XL";
                workSheet.Cells[1, 8].Value = "Người phối hợp";
                workSheet.Cells[1, 9].Value = "Số ngày";
                workSheet.Cells[1, 10].Value = "Thứ tự";
                workSheet.Cells[1, 11].Value = "Xử lý chính";
                workSheet.Cells[1, 12].Value = "Ký duyệt";
                workSheet.Cells[1, 13].Value = "Yêu cầu bổ sung";
                workSheet.Cells[1, 14].Value = "Không đủ điều kiện";
                workSheet.Cells[1, 15].Value = "Thu hồi";
                workSheet.Cells[1, 16].Value = "Trả lại";
                workSheet.Cells[1, 17].Value = "Chuyển một cửa";
                workSheet.Cells[1, 18].Value = "Gửi liên thông";
                workSheet.Cells[1, 19].Value = "Không phải xử lý";
                workSheet.Cells[1, 20].Value = "In phôi";
                workSheet.Cells[1, 21].Value = "Hồi đáp KQ Liên thông";
                workSheet.Cells[1, 22].Value = "Thông báo Email";
                using (var range = workSheet.Cells["A1:V1"])
                {
                    // Set PatternType
                    range.Style.Font.Bold = true;
                }

                // Đỗ dữ liệu từ list vào 
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    workSheet.Cells[i + 2, 1].Value = item.ma_luong;
                    workSheet.Cells[i + 2, 2].Value = item.ten_luong;
                    workSheet.Cells[i + 2, 3].Value = item.mo_ta;
                    workSheet.Cells[i + 2, 4].Value = item.ma_thu_tuc;
                    workSheet.Cells[i + 2, 5].Value = item.ten_buoc;
                    workSheet.Cells[i + 2, 6].Value = item.nguoi_xl_mac_dinh;
                    workSheet.Cells[i + 2, 7].Value = item.nguoi_co_the_xl;
                    workSheet.Cells[i + 2, 8].Value = item.nguoi_phoi_hop_xl;
                    workSheet.Cells[i + 2, 9].Value = item.so_ngay_xl;
                    workSheet.Cells[i + 2, 10].Value = item.stt;
                    workSheet.Cells[i + 2, 11].Value = item.buoc_xl_chinh ? "X" : "";
                    workSheet.Cells[i + 2, 12].Value = item.chuc_nang.Contains("KYHS") ? "X" : "";
                    workSheet.Cells[i + 2, 13].Value = item.chuc_nang.Contains("YCBSHS") ? "X" : "";
                    workSheet.Cells[i + 2, 14].Value = item.chuc_nang.Contains("HSKDK") ? "X" : "";
                    workSheet.Cells[i + 2, 15].Value = item.chuc_nang.Contains("THUHOIHS") ? "X" : "";
                    workSheet.Cells[i + 2, 16].Value = item.chuc_nang.Contains("TRALAIHS") ? "X" : "";
                    workSheet.Cells[i + 2, 17].Value = item.chuc_nang.Contains("CMC") ? "X" : "";
                    workSheet.Cells[i + 2, 18].Value = item.chuc_nang.Contains("GUIHSLT") ? "X" : "";
                    workSheet.Cells[i + 2, 19].Value = item.chuc_nang.Contains("KTXL") ? "X" : "";
                    workSheet.Cells[i + 2, 20].Value = item.chuc_nang.Contains("INPHOIKQ") ? "X" : "";
                    workSheet.Cells[i + 2, 21].Value = item.chuc_nang.Contains("GUIKQLT") ? "X" : "";
                    workSheet.Cells[i + 2, 22].Value = item.chuc_nang.Contains("THONGBAOCD") ? "X" : "";


                }

                // workSheet.Cells.LoadFromCollection(list, true);
                package.Save();

            }

            stream.Position = 0;
            string excelName = code + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
      

    }
}