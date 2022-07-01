using HG.Data.Business.CauHinh;
using HG.Entities.CauHinh;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace HG.WebApp.Controllers
{
    public class CauHinhController :BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CauHinhDao _cauHinhDao;
        //extend sys identity
        public CauHinhController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _cauHinhDao = new CauHinhDao(DbProvider);
        }
        [HttpGet]
        public IActionResult CauHinh()
        {
            var db = new EAContext();
            var obj = db.Asp_cau_hinh.Where(a => a.Deleted != 1).FirstOrDefault();
            if(obj != null)
            {
                ViewBag.type_view = StatusAction.View.ToString();
                return View(obj);
            }
            ViewBag.type_view = StatusAction.Add.ToString();
            return View(new Asp_cau_hinh());
        }
        [HttpPost]
        public IActionResult CauHinh(Asp_cau_hinh item, string type_view )
        {
            var db = new EAContext();
            var obj = db.Asp_cau_hinh.Where(a => a.Deleted != 1 && a.Id == item.Id).AsNoTracking().FirstOrDefault();
            if (type_view == StatusAction.Edit.ToString())
            {
                ViewBag.type_view = StatusAction.Edit.ToString();
                return View(obj);
            }
            else
            {
                if (obj != null)
                {
                    item.UpdatedDateUtc = DateTime.Now;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.type_view = StatusAction.View.ToString();
                    return View(item);
                }
                else
                {
                    ViewBag.type_view = StatusAction.View.ToString();
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Asp_cau_hinh> _role = db.Asp_cau_hinh.Add(item);
                    db.SaveChanges();
                    ViewBag.type_view = StatusAction.View.ToString();
                    return View(item);
                }
            }
            
        }

        public string CheckHostMail()
        {
            try
            {
                var db = new EAContext();
                var obj = db.Asp_cau_hinh.Where(a => a.Deleted != 1).AsNoTracking().FirstOrDefault();
                if (obj != null && obj.tai_khoan_dang_nhap != null)
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress(obj.tai_khoan_dang_nhap);
                    message.To.Add(new MailAddress("anhtuan02399@gmail.com"));
                    message.Subject = "Test";
                    message.IsBodyHtml = true; //to make message body as html  
                    message.Body = "test check host";
                    smtp.Port = Convert.ToInt32(obj.cong_smtp);
                    smtp.Host = "smtp.gmail.com"; //for gmail host  
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(obj.tai_khoan_dang_nhap, obj.mat_khau_dang_nhap);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                   
                }
                return "SMTP, tài khoản email và đã được xác thực";
            }
            catch (Exception) {
                return "Không thành công! Kiểm tra lại thông tin SMTP, tài khoản email và nhập lại mật khẩu email.";
            }
        }
        #region xóa người có thể xử lý
        public IActionResult XoaNguoiCTXLPartial()
        {
            var db = new EAContext();
            ViewBag.DanhSachNguoiDung = db.AspNetUsers.Where(n => n.Deleted != 1).ToList();
         
            return PartialView();
        }
        
        public async Task<IActionResult> XoaNguoiCTXLList(string ma_nguoi_dung = "")
        {
            ViewBag.ma_nguoi_dung = ma_nguoi_dung;
            if (!string.IsNullOrEmpty(ma_nguoi_dung))
            {
                var fullname = "";
                var lstObj = _cauHinhDao.LayDsNhomPhanTrang(Guid.Parse(ma_nguoi_dung));
                var _user = await userManager.FindByIdAsync(ma_nguoi_dung);
                fullname = _user != null ? _user.ho_dem + " " + _user.ten : "";
                ViewBag.ma_nguoi_dung = ma_nguoi_dung;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/CauHinh/XoaNguoiCTXLList.cshtml", lstObj);
                return Content(result);
            }
            return Content("");
        }
        public string XoaNguoiXuLy(string ma_nguoi_dung = "")
        {
            if (string.IsNullOrWhiteSpace(ma_nguoi_dung))
            {
                return "Mã người dùng không tồn tại!";
            }
            else
            {
                var UserId = Guid.Parse(userManager.GetUserId(User));
                return _cauHinhDao.XoaNguoiXL(Guid.Parse(ma_nguoi_dung), UserId);
            }
        }
        #endregion
        #region thay thế người xử lý
        public IActionResult DoiNguoiXLPartial()
        {
            var db = new EAContext();
            ViewBag.DanhSachNguoiDung = db.AspNetUsers.Where(n => n.Deleted != 1).ToList();

            return PartialView();
        }
        #endregion
       
    }
}
