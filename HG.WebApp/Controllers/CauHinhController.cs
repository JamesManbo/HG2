using HG.Data.Business.CauHinh;
using HG.Data.Business.DanhMuc;
using HG.Data.Business.NguoiDung;
using HG.Entities.CauHinh;
using HG.Entities.Entities.Model;
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
        private readonly LuongXuLyDao _danhmucDao;
        private readonly DanhMucDao _dmDao;
        private readonly NguoiDungDao _nguoiDungDao;
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
            _danhmucDao = new LuongXuLyDao(DbProvider);
            _dmDao = new DanhMucDao(DbProvider);
            _nguoiDungDao = new NguoiDungDao(DbProvider);
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
            var UserId = Guid.Parse(userManager.GetUserId(User));
            ViewBag.DanhSachNguoiDung =  _nguoiDungDao.DanhSachNguoiDung(UserId);

            return PartialView();
        }
        
        public async Task<IActionResult> XoaNguoiCTXLList(string UserName = "")
        {
            ViewBag.UserName = UserName;
            if (!string.IsNullOrEmpty(UserName))
            {
                var fullname = "";
                var lstObj = _cauHinhDao.LayDsNhomPhanTrang(UserName);
                var _user = await userManager.FindByNameAsync(UserName);
                fullname = _user != null ? _user.ho_dem + " " + _user.ten : "";
                ViewBag.UserName = UserName;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/CauHinh/XoaNguoiCTXLList.cshtml", lstObj);
                return Content(result);
            }
            return Content("");
        }
        public string XoaNguoiXuLy(string username = "")
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return "Mã người dùng không tồn tại!";
            }
            else
            {
                var UserId = Guid.Parse(userManager.GetUserId(User));
                return _cauHinhDao.XoaNguoiXL(username, UserId);
            }
        }
        #endregion
        #region thay thế người xử lý
        public IActionResult DoiNguoiXLPartial()
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            ViewBag.DanhSachNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            return PartialView();
        }

        public async Task<IActionResult> ThayTheNguoiXL(string ma_nguoi_dung_hien_tai = "", string ma_nguoi_dung_thay_the = "")
        {
            if (!string.IsNullOrEmpty(ma_nguoi_dung_hien_tai) && !string.IsNullOrEmpty(ma_nguoi_dung_thay_the))
            {
                var UserId = Guid.Parse(userManager.GetUserId(User));
                var lstObj = _cauHinhDao.ThayTheNguoiXL(ma_nguoi_dung_hien_tai, ma_nguoi_dung_thay_the, UserId);
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/CauHinh/DoiNguoiXLList.cshtml", lstObj);
                return Content(result);
            }
            return Content("");
        }

        #endregion
        #region đổi người xử lý theo luồng
        public IActionResult DoiNguoiXLTLPartial()
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            ViewBag.DanhSachNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            return PartialView();
        }
        public async Task<IActionResult> DoiNguoiXLTLList(string ma_nguoi_dung = "", string ma_nguoi_dung_thay_the = "")
        {
            ViewBag.ma_nguoi_dung = ma_nguoi_dung; 
            ViewBag.ma_nguoi_dung_thay_the = ma_nguoi_dung_thay_the;
            if (!string.IsNullOrEmpty(ma_nguoi_dung))
            {
                var lstObj = _cauHinhDao.LayDsNhomPhanTrang(ma_nguoi_dung);
                ViewBag.ma_nguoi_dung = ma_nguoi_dung;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/CauHinh/DoiNguoiXLTLList.cshtml", lstObj);
                return Content(result);
            }
            return Content("");
        }
        public async Task<IActionResult> LayCacBuocXuLy(string ma_luong, string ma_nguoi_dung = "", string ma_nguoi_dung_thay_the = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            QuyTrinhModel nhomSearchItem = new QuyTrinhModel() { CurrentPage = 1, ma_luong = ma_luong, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhQuyTrinhXuLy(nhomSearchItem);
            ds.ma_luong = ma_luong;
            var user = _dmDao.DanhSachNguoiDung("0");
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            ViewBag.NguoiHienTai = "";
            ViewBag.NguoiThayThe = "";
            if (!string.IsNullOrEmpty(ma_nguoi_dung))
            {
                EAContext db = new EAContext();
                var objNguoihientai = db.AspNetUsers.Where(n => n.UserName == ma_nguoi_dung).FirstOrDefault();
                ViewBag.NguoiHienTai = objNguoihientai.ho_dem + " " + objNguoihientai.ten;
            }
            if (!string.IsNullOrEmpty(ma_nguoi_dung_thay_the))
            {
                EAContext db = new EAContext();
                var objNguoihientai = db.AspNetUsers.Where(n => n.UserName == ma_nguoi_dung).FirstOrDefault();
                ViewBag.NguoiThayThe = objNguoihientai.ho_dem + " " + objNguoihientai.ten;
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/CauHinh/LayCacBuocXuLy.cshtml", ds);
            return Content(result);
        }

        public IActionResult ThayDoiNguoiXLTL(string ma_buoc = "", string ma_nguoi_dung = "", string ma_nguoi_dung_thay_the ="")
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var result = "Thay đổi người thay thế không thành công!";
            foreach (var item in ma_buoc.Split(""))
            {
                if(item != "on")
                {
                    result = _cauHinhDao.ThayDoiXLTL(item, Guid.Parse(ma_nguoi_dung), Guid.Parse(ma_nguoi_dung_thay_the), UserId);
                }
            }
            return Content(result);
        }
        public IActionResult XoaNguoiXLTL(string ma_buoc = "", string ma_nguoi_dung = "", string ma_nguoi_dung_thay_the = "")
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var result = "Xóa người thay thế không thành công!";
            foreach (var item in ma_buoc.Split(""))
            {
                if (item != "on")
                {
                    result = _cauHinhDao.XoaNguoiXLTL(item, UserId);
                }
            }
            return Content(result);
        }
        #endregion
        #region Thêm người xl vào bước
        public ActionResult ThemNguoiXLVBPartial()
        {
            var db = new EAContext();
            var UserId = Guid.Parse(userManager.GetUserId(User));
            ViewBag.DanhSachNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.ListLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            return PartialView();
        }
        public async Task<IActionResult> ThemNguoiXLVBList(string ma_linh_vuc = "", string nguoi_phoi_hop = "")
        {
            ViewBag.ma_linh_vuc = ma_linh_vuc;
            ViewBag.nguoi_phoi_hop = nguoi_phoi_hop;
            if (!string.IsNullOrEmpty(ma_linh_vuc))
            {
                var lstObj = _cauHinhDao.LayDanhSachLuongBoiLinhVuc(ma_linh_vuc);
                ViewBag.ma_nguoi_dung = ma_linh_vuc;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/CauHinh/ThemNguoiXLVBList.cshtml", lstObj);
                return Content(result);
            }
            return Content("");
        }
        public async Task<IActionResult> LayCacBuocXuLyCuaLuong(string ma_luong, string ma_nguoi_dung)
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            QuyTrinhModel nhomSearchItem = new QuyTrinhModel() { CurrentPage = 1, ma_luong = ma_luong, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _danhmucDao.DanhSanhQuyTrinhXuLy(nhomSearchItem);
            ds.ma_luong = ma_luong;
            var user = _dmDao.DanhSachNguoiDung("0");
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            ViewBag.NguoiHienTai = "";
            ViewBag.NguoiThayThe = "";
            if (!string.IsNullOrEmpty(ma_nguoi_dung))
            {
                EAContext db = new EAContext();
                var objNguoihientai = db.AspNetUsers.Where(n => n.UserName == ma_nguoi_dung).FirstOrDefault();
                ViewBag.NguoiThemVaoBuoc = objNguoihientai.ho_dem + " " + objNguoihientai.ten;
            }
            
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/CauHinh/LayCacBuocXuLyCuaLuong.cshtml", ds);
            return Content(result);
        }

        public IActionResult ThemNguoiXLVB(string ma_buoc = "", string ma_nguoi_dung = "")
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            //var result = "Thay đổi người thay thế không thành công!";
            //foreach (var item in ma_buoc.Split(""))
            //{
            //    if (item != "on")
            //    {
            //        result = _cauHinhDao.ThayDoiXLTL(item, Guid.Parse(ma_nguoi_dung), Guid.Parse(ma_nguoi_dung_thay_the), UserId);
            //    }
            //}
            return Content("Thêm người xử lý vào bước thành công!");
        }
        #endregion

    }
}
