using HG.Entities;
using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.DichVuCong;
using HG.Entities.HoSo;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HG.WebApp.Controllers.Client
{
    [Authorize]
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
                item.Type = 0;
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
        public IActionResult DanhGiaView()
        {
            return View();
        }
        public IActionResult DanhGiaCanBo()
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var listDV = new List<dm_don_vi>();
            var ObjUser = new AspNetUsers();
            var lstHS = new List<Ho_So>();
            using (var db = new EAContext())
            {
                listDV = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                ObjUser = db.AspNetUsers.Where(n => n.Id == UserId).FirstOrDefault();
                lstHS = db.Ho_So.Where(n => n.CreatedUid == UserId).ToList();
            }
            ViewBag.Message = "";
            ViewBag.LstDonVi = listDV;
            ViewBag.Profile = ObjUser;
            ViewBag.LstHoSo = lstHS;
            return View();
        }
        [HttpPost]
        public IActionResult DanhGiaCanBo(GopYDanhGia item)
        {
            var successed = 0;
            var UserId = Guid.Parse(userManager.GetUserId(User));
            using (var db = new EAContext())
            {
                item.Type = 1;
                item.CreatedDateUtc = DateTime.Now;
                item.CreatedUid = UserId;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<GopYDanhGia> _gopy = db.GopYDanhGia.Add(item);
                db.SaveChanges();
                successed = _gopy.Entity.Id;
            };

            if (string.IsNullOrEmpty(successed.ToString()))
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Thêm đánh giá không thành công!";
                return View(item);
            }
            else
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Thêm đánh giá thành công!";
                var listDV = new List<dm_don_vi>();
                var ObjUser = new AspNetUsers();
                var lstHS = new List<Ho_So>();
                using (var db = new EAContext())
                {
                    listDV = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                    ObjUser = db.AspNetUsers.Where(n => n.Id == UserId).FirstOrDefault();
                    lstHS = db.Ho_So.Where(n => n.CreatedUid == UserId).ToList();
                }
                ViewBag.LstDonVi = listDV;
                ViewBag.Profile = ObjUser;
                ViewBag.LstHoSo = lstHS;
                return View();
            }
        }
        public IActionResult XemDanhGiaCanBo()
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var listDV = new List<dm_don_vi>();

            using (var db = new EAContext())
            {
                listDV = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();

            }
            ViewBag.LstDonVi = listDV;
            return View();
        }


        public IActionResult TraCuuHoSo()
        {
            return View();
        }
        public async  Task<IActionResult> LayPhongBanBoiDonVi(string ma_don_vi)
        {
            var listPB = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                listPB = db.Dm_Phong_Ban.Where(n => n.Deleted != 1 && n.ma_don_vi == ma_don_vi).ToList();
            }
            ViewBag.Message = "";
            ViewBag.LstPB = listPB;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DichVuCong/Ajax/LayPhongBanBoiDonVi.cshtml", listPB);
            return Content(result);
        }
        public async Task<IActionResult> LayNguoiDungBoiPhongBan(string ma_phong_ban)
        {
            var listCB = new List<AspNetUsers>();
            using (var db = new EAContext())
            {
                listCB = db.AspNetUsers.Where(n => n.Deleted != 1 && n.ma_phong_ban == ma_phong_ban).ToList();
            }
            ViewBag.Message = "";
            ViewBag.listCB = listCB;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DichVuCong/Ajax/LayNguoiDungBoiPhongBan.cshtml", listCB);
            return Content(result);
        }
        public async Task<IActionResult> AjaxXemDanhGiaCanBo(string ma_phong_ban)
        {
            var listCB = new List<AspNetUsers>();
            using (var db = new EAContext())
            {
                listCB = db.AspNetUsers.Where(n => n.Deleted != 1 && n.ma_phong_ban == ma_phong_ban).ToList();
                //listCB = db.AspNetUsers.Where(n => n.Deleted != 1).ToList();
            }
            ViewBag.Message = "";
            ViewBag.listCB = listCB;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DichVuCong/Ajax/AjaxXemDanhGiaCanBo.cshtml", listCB);
            return Content(result);
        }


    }
}
