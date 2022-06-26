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
    public class LuongController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly LuongXuLyDao _danhmucDao;

        //extend sys identity
        public LuongController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new LuongXuLyDao(DbProvider);
        }

        #region Quốc tịch
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

    }
}