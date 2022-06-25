using HG.Data.Business.DanhMuc;
using HG.Entities;
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
    public class DanhMucChungController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly DanhMucChungDao _danhmucDao;

        //extend sys identity
        public DanhMucChungController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new DanhMucChungDao(DbProvider);
        }

        #region Quốc tịch
        public IActionResult QuocTich()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var quoc_tich = new List<Dm_Quoc_Tich>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Quoc_Tich.Where(n => n.Deleted == 0).ToList();
                quoc_tich = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMucChung/QuocTich/QuocTich.cshtml", quoc_tich);

        }

        public async Task<IActionResult> QuocTichPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Quoc_Tich.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMucChung/QuocTich/QuocTichPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemQuocTich()
        {
            return View("~/Views/DanhMucChung/QuocTich/ThemQuocTich.cshtml");
        }

        [HttpPost]
        public IActionResult ThemQuocTich(Dm_Quoc_Tich item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuQuocTich(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo quốc tịch lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || _pb > 0)
            {
                return View("~/Views/DanhMucChung/QuocTich/ThemQuocTich.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaQuocTich", "DanhMucChung", new { code = item.ma_quoc_tich, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaQuocTich(string code, string type)
        {
            var quoc_tich = new Dm_Quoc_Tich();
            using (var db = new EAContext())
            {
                quoc_tich = db.Dm_Quoc_Tich.Where(n => n.Deleted == 0 && n.ma_quoc_tich == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/DanhMucChung/QuocTich/SuaQuocTich.cshtml", quoc_tich);
        }

        [HttpPost]
        public IActionResult SuaQuocTich(string code, string type, Dm_Quoc_Tich item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuQuocTich(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật quốc tịch lỗi";
                return PartialView("~/Views/DanhMucChung/QuocTich/SuaQuocTich.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemQuocTich", "DanhMucChung");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaQuocTich", "DanhMucChung", new { code = item.ma_quoc_tich, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaQuocTich(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaQuocTich(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMucChung/QuocTich" });
        }

        public async Task<IActionResult> RenderViewQuocTich()
        {
            var lst = new List<Dm_Quoc_Tich>();
            using (var db = new EAContext())
            {
                lst = db.Dm_Quoc_Tich.Where(n => n.Deleted == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMucChung/QuocTich/ViewQuocTich.cshtml", lst);
            return Content(result);
        }

        #endregion

        #region Giới Tính
        public IActionResult GioiTinh()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var gioi_tinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Gioi_Tinh.Where(n => n.Deleted == 0).ToList();
                gioi_tinh = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMucChung/GioiTinh/GioiTinh.cshtml", gioi_tinh);

        }

        public IActionResult ThemGioiTinh()
        {
            return View("~/Views/DanhMucChung/GioiTinh/ThemGioiTinh.cshtml");
        }

        [HttpPost]
        public IActionResult ThemGioiTinh(Dm_Gioi_Tinh item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuGioiTinh(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo giới tính lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || _pb > 0)
            {
                return View("~/Views/DanhMucChung/GioiTinh/ThemGioiTinh.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaGioiTinh", "DanhMucChung", new { code = item.ma_gioi_tinh, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaGioiTinh(string code, string type)
        {
            var quoc_tich = new Dm_Gioi_Tinh();
            using (var db = new EAContext())
            {
                quoc_tich = db.Dm_Gioi_Tinh.Where(n => n.Deleted == 0 && n.ma_gioi_tinh == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/DanhMucChung/GioiTinh/SuaGioiTinh.cshtml", quoc_tich);
        }

        [HttpPost]
        public IActionResult SuaGioiTinh(string code, string type, Dm_Gioi_Tinh item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuGioiTinh(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật giới tính lỗi";
                return PartialView("~/Views/DanhMucChung/GioiTinh/SuaGioiTinh.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemGioiTinh", "DanhMucChung");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaGioiTinh", "DanhMucChung", new { code = item.ma_gioi_tinh, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaGioiTinh(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaGioiTinh(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMucChung/GioiTinh" });
        }

        public async Task<IActionResult> RenderViewGioiTinh()
        {
            var lst = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                lst = db.Dm_Gioi_Tinh.Where(n => n.Deleted == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMucChung/GioiTinh/ViewGioiTinh.cshtml", lst);
            return Content(result);
        }

        #endregion

        #region Tôn giáo
        public IActionResult TonGiao()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var ton_giao = new List<Dm_Ton_Giao>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Ton_Giao.Where(n => n.Deleted == 0).ToList();
                ton_giao = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMucChung/TonGiao/TonGiao.cshtml", ton_giao);

        }

        public async Task<IActionResult> TonGiaoPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Ton_Giao.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMucChung/TonGiao/TonGiaoPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemTonGiao()
        {
            return View("~/Views/DanhMucChung/TonGiao/ThemTonGiao.cshtml");
        }

        [HttpPost]
        public IActionResult ThemTonGiao(Dm_Ton_Giao item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuTonGiao(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo tôn giáo lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || _pb > 0)
            {
                return View("~/Views/DanhMucChung/TonGiao/ThemTonGiao.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaTonGiao", "DanhMucChung", new { code = item.ma_ton_giao, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaTonGiao(string code, string type)
        {
            var quoc_tich = new Dm_Ton_Giao();
            using (var db = new EAContext())
            {
                quoc_tich = db.Dm_Ton_Giao.Where(n => n.Deleted == 0 && n.ma_ton_giao == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/DanhMucChung/TonGiao/SuaTonGiao.cshtml", quoc_tich);
        }

        [HttpPost]
        public IActionResult SuaTonGiao(string code, string type, Dm_Ton_Giao item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuTonGiao(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật tôn giáo lỗi";
                return PartialView("~/Views/DanhMucChung/TonGiao/SuaTonGiao.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemTonGiao", "DanhMucChung");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaTonGiao", "DanhMucChung", new { code = item.ma_ton_giao, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaTonGiao(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaTonGiao(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMucChung/TonGiao" });
        }

        public async Task<IActionResult> RenderViewTonGiao()
        {
            var lst = new List<Dm_Ton_Giao>();
            using (var db = new EAContext())
            {
                lst = db.Dm_Ton_Giao.Where(n => n.Deleted == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMucChung/TonGiao/ViewTonGiao.cshtml", lst);
            return Content(result);
        }

        #endregion

        #region Dân tộc
        public IActionResult DanToc()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var dan_toc = new List<Dm_Dan_Toc>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Dan_Toc.Where(n => n.Deleted == 0).ToList();
                dan_toc = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMucChung/DanToc/DanToc.cshtml", dan_toc);

        }

        public async Task<IActionResult> DanTocPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Dan_Toc.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMucChung/DanToc/DanTocPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemDanToc()
        {
            return View("~/Views/DanhMucChung/DanToc/ThemDanToc.cshtml");
        }

        [HttpPost]
        public IActionResult ThemDanToc(Dm_Dan_Toc item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuDanToc(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo dân tộc lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || _pb > 0)
            {
                return View("~/Views/DanhMucChung/DanToc/ThemDanToc.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaDanToc", "DanhMucChung", new { code = item.ma_dan_toc, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaDanToc(string code, string type)
        {
            var quoc_tich = new Dm_Dan_Toc();
            using (var db = new EAContext())
            {
                quoc_tich = db.Dm_Dan_Toc.Where(n => n.Deleted == 0 && n.ma_dan_toc == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/DanhMucChung/DanToc/SuaDanToc.cshtml", quoc_tich);
        }

        [HttpPost]
        public IActionResult SuaDanToc(string code, string type, Dm_Dan_Toc item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuDanToc(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật dân tộc lỗi";
                return PartialView("~/Views/DanhMucChung/DanToc/SuaDanToc.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemDanToc", "DanhMucChung");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaDanToc", "DanhMucChung", new { code = item.ma_dan_toc, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaDanToc(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaDanToc(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMucChung/DanToc" });
        }

        public async Task<IActionResult> RenderViewDanToc()
        {
            var lst = new List<Dm_Ton_Giao>();
            using (var db = new EAContext())
            {
                lst = db.Dm_Ton_Giao.Where(n => n.Deleted == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMucChung/DanToc/ViewDanToc.cshtml", lst);
            return Content(result);
        }

        #endregion
    }
}