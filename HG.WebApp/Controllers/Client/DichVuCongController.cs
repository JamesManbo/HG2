using HG.Entities;
using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.DichVuCong;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HG.WebApp.Controllers.Client
{
    public class DichVuCongController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        public DichVuCongController(IWebHostEnvironment environment, ILogger<UserController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager)
        : base(configuration, httpContextAccessor)
        {
            _environment = environment;
            _logger = logger;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public IActionResult GopY()
        {
            var listDV = new List<dm_don_vi>();
            using (var db = new EAContext())
            {
                listDV = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
            }
            ViewBag.Message = ""; 
            ViewBag.LstDonVi = listDV;
            return View( new GopYDanhGia());
        }
        [HttpPost]
        public IActionResult GopY(GopYDanhGia item)
        {
            var successed = 0;
            var UserId = Guid.Parse(userManager.GetUserId(User));
            using (var db = new EAContext())
            {
                item.CreatedDateUtc = DateTime.Now;
                item.CreatedUid = UserId;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<GopYDanhGia> _gopy = db.GopYDanhGia.Add(item);
                db.SaveChanges();
                successed = _gopy.Entity.Id;
            };

            if (string.IsNullOrEmpty(successed.ToString()))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Thêm góp ý không thành công!";
                return View(item);
            }
            else
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Thêm góp ý không thành công!";
                var listDV = new List<dm_don_vi>();
                using (var db = new EAContext())
                {
                    listDV = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                }
                ViewBag.LstDonVi = listDV;
                return View();
            }
        }
        public IActionResult DanhGia()
        {
            return View();
        }
        public IActionResult TraCuuHoSo()
        {
            return View();
        }
    }
}
