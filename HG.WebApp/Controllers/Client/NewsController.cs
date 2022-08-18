using HG.Data.Business.DanhMuc;
using HG.Data.Business.HoSo;
using HG.Data.Business.ThuTuc;
using HG.Data.Business.User;
using HG.Data.SqlService;
using HG.Entities.Entities;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HG.WebApp.Controllers.Client
{
    public class NewsController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        public NewsController(IWebHostEnvironment environment, ILogger<UserController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager)
        : base(configuration, httpContextAccessor)
        {
            _environment = environment;
            _logger = logger;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public readonly static EAContext db = new EAContext();
        public IActionResult News()
        {
            return View(db.Tin_Tuc.Where(n=>n.is_hien_thi == true).FirstOrDefault());
        }

        public IActionResult GetListNews()
        {
            return PartialView(db.Tin_Tuc.Take(20).ToList());
        }
        public IActionResult OtherNews(int id)
        {
            if(id == 0)
            {
                return PartialView(db.Tin_Tuc.Where(n => n.is_hien_thi == true).Take(20).ToList());
            }
            return PartialView(db.Tin_Tuc.Where(n=>n.Id != id && n.is_hien_thi == true).Take(20).ToList());
        }
        public IActionResult XemChiTiet(int id)
        {
            return View(db.Tin_Tuc.Where(n => n.Id == id).FirstOrDefault());
        }
        public IActionResult HDKySoHoSoDVC()
        {
            string ReportURL = @"" + this._environment.WebRootPath + "/HDSD/HDKySoHoSoDVC.pdf";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }
        public IActionResult HDSD_TrangChung()
        {
            string ReportURL = @"" + this._environment.WebRootPath + "/HDSD/HDSD_TrangChung.pdf";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }
        public IActionResult HuongDanThanhToanTrucTuyen()
        {
            string ReportURL = @"" + this._environment.WebRootPath + "/HDSD/HuongDanThanhToanTrucTuyen.pdf";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }
        public IActionResult HuongDanNhanThongBaoZalo()
        {
            string ReportURL = @"" + this._environment.WebRootPath + "/HDSD/notification_Zalo.pdf";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }

        public async Task<string> Login(string txtTaiKhoan, string txtMatKhau)
        {
            EAContext db = new EAContext();
            var obj = db.AspNetUsers.Where(n => n.UserName == txtTaiKhoan && n.mat_khau == txtMatKhau && (n.khoa_tai_khoan == 1 || n.Deleted == 1)).Count();
            if (obj > 0)
            {
                return "false";
            }
            ViewBag.Msg = "";
            var q = await userManager.Users.CountAsync();
            var checkUser = await userManager.FindByNameAsync(txtTaiKhoan);
            var user = userManager.Users;
            if (checkUser == null)
            {
                return "false";
            };
            var result = await signInManager.PasswordSignInAsync(checkUser, txtMatKhau, true, true);
            if (!result.Succeeded)
            {
                 return "false";
            }
            else
            {
                return "true";
            }
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            ViewBag.DanhSachTinhTP = db.dm_dia_ban.Where(n=>n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
            ViewBag.Verify = HG.WebApp.Helper.HelperString.RandomCodeVerify();
            return View(new NguoiDungCongDan());
        }
        [HttpPost]
        public async Task<IActionResult> DangKy(NguoiDungCongDan item)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            AspNetUsers user = new AspNetUsers();
            user.UserName = item.UserName;
            user.PhoneNumber = item.PhoneNumber;
            user.Email = item.Email;
            user.mat_khau = "1";
            user.ma_chuc_vu = item.ma_chuc_vu;
            user.ma_phong_ban = item.ma_phong_ban;
            user.ho_dem = item.ho_dem;
            user.ten = item.ten;
            user.ngay_sinh = item.ngay_sinh;
            user.khoa_tai_khoan = 0;
            user.IsAdministrator = 0;
            var result = await userManager.CreateAsync(user, user.mat_khau);
            var db = new EAContext();
            
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            if (result.Succeeded)
            {
                //tiep tuc them vao bang mapping
                return View("/DichVuCong");
            }
            else
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Thêm người dùng không thành công!";
                return View(item);
            }  
        }
        public async Task<IActionResult> LayHuyenTheoTinh(string ma_dia_ban_cha)
        {
            EAContext db = new EAContext();
            var LstDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_dia_ban_cha == ma_dia_ban_cha).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/News/Ajax/LayHuyenTheoTinh.cshtml", LstDiaBan);
            return Content(result);
        }
        public async Task<IActionResult> LayXaTheoHuyen(string ma_dia_ban_con)
        {
            EAContext db = new EAContext();
            var LstDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_dia_ban_con == ma_dia_ban_con).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/News/Ajax/LayXaTheoHuyen.cshtml", LstDiaBan);
            return Content(result);
        }
    }
}
