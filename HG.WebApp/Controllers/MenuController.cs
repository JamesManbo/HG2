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

        #region Menu cấp 1
        public IActionResult ChuyenMuc(string txtSearch = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            MenuModel nhomSearchItem = new MenuModel() { CurrentPage = 1, level = 0, tu_khoa = txtSearch, RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhMenu(nhomSearchItem);
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            return View("~/Views/Menu/MenuChinh/Menu.cshtml", ds);
        }
        public async Task<IActionResult> ChuyenMucPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            MenuModel nhomSearchItem = new MenuModel() { CurrentPage = currentPage, level = 0, tu_khoa = tu_khoa, RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhMenu(nhomSearchItem);
            ds.Pagelist.CurrentPage = currentPage;
            ViewBag.Stt = (currentPage - 1) * pageSize;
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? ds.Pagelist.TotalRecords : currentPage * pageSize;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Menu/MenuChinh/MenuPaging.cshtml", ds);
            return Content(result);
        }
        public IActionResult ThemChuyenMuc()
        {
            return View("~/Views/Menu/MenuChinh/ThemMenu.cshtml");
        }

        [HttpPost]
        public IActionResult ThemChuyenMuc(Dm_menu item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuMenu(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo menu lỗi";
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
                return RedirectToAction("SuaChuyenMuc", "Menu", new { code = item.ma_trang, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaChuyenMuc(string code, string type)
        {
            var menu = new Dm_menu();
            using (var db = new EAContext())
            {
                menu = db.Dm_menu.Where(n => n.Deleted == 0 && n.ma_trang == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/Menu/MenuChinh/SuaMenu.cshtml", menu);
        }

        [HttpPost]
        public IActionResult SuaChuyenMuc(string code, string type, Dm_menu item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuMenu(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật trang chính lỗi!";
                return PartialView("~/Views/Menu/MenuChinh/SuaMenu.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemChuyenMuc", "Menu");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaChuyenMuc", "Menu", new { code = item.ma_trang, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaChuyenMuc(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaMenu(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/Menu/ChuyenMuc" });
        }

        public async Task<IActionResult> RenderViewChuyenMuc()
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

        #region Menu cấp 2
        public IActionResult ChuyenMucCap2(string txtSearch = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            MenuModel nhomSearchItem = new MenuModel() { CurrentPage = 1, level = 1, tu_khoa = txtSearch, RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhMenu(nhomSearchItem);
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            ViewBag.txtSearch = txtSearch;
            return View("~/Views/Menu/MenuCap2/Menu.cshtml", ds);
        }
        public async Task<IActionResult> ChuyenMucCap2Paging(int currentPage = 0, int pageSize = 0, string txtSearch = "")
        {           
            MenuModel nhomSearchItem = new MenuModel() { CurrentPage = currentPage, level = 1, tu_khoa = txtSearch, RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhMenu(nhomSearchItem);
            ds.Pagelist.CurrentPage = currentPage;
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? ds.Pagelist.TotalRecords : currentPage * pageSize;
            ViewBag.Stt = (currentPage - 1) * pageSize;
            ViewBag.txtSearch = txtSearch;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Menu/MenuCap2/MenuPaging.cshtml", ds);
            return Content(result);
        }
        public IActionResult ThemChuyenMucCap2()
        {
            using (var db = new EAContext())
            {
                ViewBag.MenuCha = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
                ViewBag.MenuCon = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 1).ToList();
            }

            return View("~/Views/Menu/MenuCap2/ThemMenu.cshtml");
        }

        [HttpPost]
        public IActionResult ThemChuyenMucCap2(Dm_menu item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            //check mã trang cha
            if ((String.IsNullOrEmpty(item.ma_trang_level1) && !String.IsNullOrEmpty(item.ma_trang_level2))
                || (String.IsNullOrEmpty(item.ma_trang_level1) && !String.IsNullOrEmpty(item.ma_trang_level2)))
            {
                ViewBag.error = 2;
                ViewBag.msg = "Chọn danh mục cha không hợp lệ !";
            }
            if (!String.IsNullOrEmpty(item.ma_trang_level1))
            {
                item.ma_trang_cha = item.ma_trang_level1;
                item.level = 1;
            }
            if (!String.IsNullOrEmpty(item.ma_trang_level2))
            {
                item.ma_trang_cha = item.ma_trang_level2;
                item.level = 2;
            }

            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuMenu(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo menu lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || ViewBag.error > 0)
            {
                using (var db = new EAContext())
                {
                    ViewBag.MenuCha = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
                    ViewBag.MenuCon = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 1).ToList();
                }

                return View("~/Views/Menu/MenuCap2/ThemMenu.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaChuyenMucCap2", "Menu", new { code = item.ma_trang, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaChuyenMucCap2(string code, string type)
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
                ViewBag.MenuCha = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
                ViewBag.MenuCon = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 1).ToList();
            }
            return View("~/Views/Menu/MenuCap2/SuaMenu.cshtml", menu);
        }

        [HttpPost]
        public IActionResult SuaChuyenMucCap2(string code, string type, Dm_menu item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            //check mã trang cha
            if ((String.IsNullOrEmpty(item.ma_trang_level1) && !String.IsNullOrEmpty(item.ma_trang_level2))
                || (String.IsNullOrEmpty(item.ma_trang_level1) && !String.IsNullOrEmpty(item.ma_trang_level2)))
            {
                ViewBag.error = 2;
                ViewBag.msg = "Chọn danh mục cha không hợp lệ !";
            }
            if (!String.IsNullOrEmpty(item.ma_trang_level1))
            {
                item.ma_trang_cha = item.ma_trang_level1;
                item.level = 1;
            }
            if (!String.IsNullOrEmpty(item.ma_trang_level2))
            {
                item.ma_trang_cha = item.ma_trang_level2;
                item.level = 2;
            }
            var response = _danhmucDao.LuuMenu(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật trang chính lỗi!";
                using (var db = new EAContext())
                {
                    ViewBag.MenuCha = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
                    ViewBag.MenuCon = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 1).ToList();
                }
                return PartialView("~/Views/Menu/MenuCap2/SuaMenu.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemChuyenMucCap2", "Menu");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaChuyenMucCap2", "Menu", new { code = item.ma_trang, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaChuyenMucCap2(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaMenu(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/Menu/ChuyenMucCap2" });
        }

        public async Task<IActionResult> RenderViewChuyenMucCap2()
        {
            var lstpb = new List<Dm_menu>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_menu.Where(n => n.Deleted == 0 && n.level == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/Menu/MenuCap2/ViewMenu.cshtml", lstpb);
            return Content(result);
        }


        #endregion


    }
}