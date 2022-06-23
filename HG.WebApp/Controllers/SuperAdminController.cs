using Microsoft.AspNetCore.Mvc;
using HG.Entities.Entities.Nhom;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using HG.Data.Business.DanhMuc;
using HG.WebApp.Data;
using HG.Entities.SearchModels;
using HG.Data.Business.Nhom;

namespace HG.WebApp.Controllers
{
    public class SuperAdminController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly DanhMucDao _danhmucDao;
        private readonly NhomDao _nhomDao;

        //extend sys identity
        public SuperAdminController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new DanhMucDao(DbProvider);
            _nhomDao = new NhomDao(DbProvider);
        }


        #region Nhom
        [HttpGet]
        public IActionResult ViewNhom(NhomSearchItem item)
        {
            EAContext eAContext = new EAContext();
            ViewBag.TotalPage = eAContext.Asp_nhom.Where(n => n.Deleted == 0).Count()/10;
            ViewBag.CurrentPage = 1;
            return View(eAContext.Asp_nhom.Where(n=>n.Deleted == 0).ToList());
        }

        [HttpGet]
        public IActionResult SuaNhom(string code, string type)
        {
            return View();
        }
        [HttpPost]
        public IActionResult SuaNhom(Asp_nhom item)
        {
            return View();
        }

        [HttpGet]
        public IActionResult ThemNhom()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ThemNhom(Asp_nhom item)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            return View();
        }

        public IActionResult XoaNhom(string code, string type, Asp_nhom item)
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
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/SuperAdmin/ViewNhom" });
        }

        public async Task<IActionResult> NhomPaging(int currentPage = 0, string tu_khoa = "")
        {
            NhomSearchItem nhomSearchItem = new NhomSearchItem() { CurrentPage = currentPage, tu_khoa = tu_khoa, RecordsPerPage = 10 };
            var ds = _nhomDao.LayDsNhomPhanTrang(nhomSearchItem);
            ds.Pagelist.CurrentPage = currentPage;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/nhomPaging.cshtml", ds);
            return Content(result);
        }
        #endregion

        public IActionResult Quyen()
        {
            return View();
        }
        
    }
}
