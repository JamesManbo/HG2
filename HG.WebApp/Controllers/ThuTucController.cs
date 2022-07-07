using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
using HG.Entities.DanhMuc.dm_bieu_mau;
using HG.Entities.Entities.Model;
using HG.Entities.Entities.ThuTuc;
using HG.WebApp.Data;
using HG.WebApp.Helper;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
{
    [Authorize]
    public class ThuTucController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly ThuTucDao _thuTucDao;
        private readonly LuongXuLyDao _luongDao;
        private IWebHostEnvironment Environment;

        //extend sys identity
        public ThuTucController(IWebHostEnvironment _environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            Environment = _environment;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _thuTucDao = new ThuTucDao(DbProvider);
            _luongDao = new LuongXuLyDao(DbProvider);
        }
        public IActionResult QuanLy(int tabbieumau = 0)
        {
            
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted != 1).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted !=  1).ToList();
                ViewBag.lstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();
            }
            ViewBag.BieuMau = tabbieumau;
            return View("~/Views/ThuTuc/QuanLy.cshtml", ds);
        }

        public async Task<IActionResult> QuanLyPaging(int currentPage = 0, int pageSize = 0, string ma_pb = "", string ma_lv = "", string tu_khoa = "")
        {
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = ma_pb, ma_lv = ma_lv, tu_khoa = tu_khoa, RecordsPerPage = pageSize };
            var ds = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? ds.Pagelist.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/QuanLyPaging.cshtml", ds);
            return Content(result);
        }
        public IActionResult ThemThuTuc(int currentPage = 0, int pageSize = 20)
        {
            var ds = _thuTucDao.ThuTuc("", "", "", "", "", currentPage, pageSize);
            //Thành phần
            ds.PagelistThanhPhan.CurrentPage = currentPage;
            ds.PagelistThanhPhan.TotalPage = (ds.PagelistThanhPhan.TotalRecords / pageSize) + ((ds.PagelistThanhPhan.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistThanhPhan.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistThanhPhan.RecoredTo = ds.PagelistThanhPhan.TotalPage == currentPage ? ds.PagelistThanhPhan.TotalRecords : currentPage * pageSize;
            // Văn bản liên quan
            ds.PagelistVanBanLQ.CurrentPage = currentPage;
            ds.PagelistVanBanLQ.TotalPage = (ds.PagelistVanBanLQ.TotalRecords / pageSize) + ((ds.PagelistVanBanLQ.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistVanBanLQ.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistVanBanLQ.RecoredTo = ds.PagelistVanBanLQ.TotalPage == currentPage ? ds.PagelistVanBanLQ.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelistNhomTP.CurrentPage = currentPage;
            ds.PagelistNhomTP.TotalPage = (ds.PagelistNhomTP.TotalRecords / pageSize) + ((ds.PagelistNhomTP.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistNhomTP.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistNhomTP.RecoredTo = ds.PagelistNhomTP.TotalPage == currentPage ? ds.PagelistNhomTP.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelisthanhPhanKQXL.CurrentPage = currentPage;
            ds.PagelisthanhPhanKQXL.TotalPage = (ds.PagelisthanhPhanKQXL.TotalRecords / pageSize) + ((ds.PagelisthanhPhanKQXL.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelisthanhPhanKQXL.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelisthanhPhanKQXL.RecoredTo = ds.PagelisthanhPhanKQXL.TotalPage == currentPage ? ds.PagelisthanhPhanKQXL.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstDonViLienThong = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstMucDoThucHien = db.Dm_Muc_Do_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
            }
            return View("~/Views/ThuTuc/ThemThuTuc.cshtml", ds);
        }
        [HttpPost]
        public IActionResult ThemThuTuc(DmThuTuc item)
        {
            item.ma_thu_tuc = String.Concat(HelperString.RemoveSign4VietnameseString(item.ma_thu_tuc).ToUpper().Where(c => !Char.IsWhiteSpace(c)));
            item.ma_quoc_gia = String.Concat(HelperString.RemoveSign4VietnameseString(item.ma_quoc_gia).ToUpper().Where(c => !Char.IsWhiteSpace(c)));
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _thuTucDao.LuuThuTuc(item);
            if (_pb.ErrorCode > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = _pb.ErrorMsg;
            }
            if (_pb.ErrorCode > 0)
            {
                return RedirectToAction("ThemThuTuc", "ThuTuc", new { code = item.ma_thu_tuc });
            }
            return RedirectToAction("SuaThuTuc", "ThuTuc", new { code = item.ma_thu_tuc, type = StatusAction.View.ToString() });
        }


        public IActionResult SuaThuTuc(string code, string type, string active = "ThuTuc", int currentPage = 0, int pageSize = 20)
        {
            var thuTuc = new ThuTucModel();
            var ds = _thuTucDao.ThuTuc(code, "", "", "", "", currentPage, pageSize);
            //Thành phần
            ds.PagelistThanhPhan.CurrentPage = currentPage;
            ds.PagelistThanhPhan.TotalPage = (ds.PagelistThanhPhan.TotalRecords / pageSize) + ((ds.PagelistThanhPhan.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistThanhPhan.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistThanhPhan.RecoredTo = (ds.PagelistThanhPhan.TotalPage == currentPage || currentPage == 0) ? ds.PagelistThanhPhan.TotalRecords : currentPage * pageSize;
            // Văn bản liên quan
            ds.PagelistVanBanLQ.CurrentPage = currentPage;
            ds.PagelistVanBanLQ.TotalPage = (ds.PagelistVanBanLQ.TotalRecords / pageSize) + ((ds.PagelistVanBanLQ.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistVanBanLQ.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistVanBanLQ.RecoredTo = (ds.PagelistVanBanLQ.TotalPage == currentPage || currentPage == 0) ? ds.PagelistVanBanLQ.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelistNhomTP.CurrentPage = currentPage;
            ds.PagelistNhomTP.TotalPage = (ds.PagelistNhomTP.TotalRecords / pageSize) + ((ds.PagelistNhomTP.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistNhomTP.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistNhomTP.RecoredTo = (ds.PagelistNhomTP.TotalPage == currentPage || currentPage == 0) ? ds.PagelistNhomTP.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelisthanhPhanKQXL.CurrentPage = currentPage;
            ds.PagelisthanhPhanKQXL.TotalPage = (ds.PagelisthanhPhanKQXL.TotalRecords / pageSize) + ((ds.PagelisthanhPhanKQXL.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelisthanhPhanKQXL.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelisthanhPhanKQXL.RecoredTo = (ds.PagelisthanhPhanKQXL.TotalPage == currentPage || currentPage == 0) ? ds.PagelisthanhPhanKQXL.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstDonViLienThong = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstMucDoThucHien = db.Dm_Muc_Do_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.type_view = type;
            ViewBag.active = active;
            return View("~/Views/ThuTuc/SuaThuTuc.cshtml", ds);
        }

        public async Task<IActionResult> RenderViewThuTuc(int type = 0)
        {
            var ds = _luongDao.DanhSachThuTuc();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ViewThuTuc.cshtml", ds);
            return Content(result);
        }

        [HttpPost]
        public IActionResult XoaThuTuc(string code, int type)
        {
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _thuTucDao.XoaThuTuc(code, type, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/ThuTuc/QuanLy" });
        }

        #region Thành phần

        public IActionResult ThemThanhPhan(string code, string type = "", int currentPage = 0, int pageSize = 20)
        {
            var ds = _thuTucDao.ThuTuc(code, "", "", "", "", currentPage, pageSize);
            //Thành phần
            ds.PagelistThanhPhan.CurrentPage = currentPage;
            ds.PagelistThanhPhan.TotalPage = (ds.PagelistThanhPhan.TotalRecords / pageSize) + ((ds.PagelistThanhPhan.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistThanhPhan.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistThanhPhan.RecoredTo = (ds.PagelistThanhPhan.TotalPage == currentPage || currentPage == 0) ? ds.PagelistThanhPhan.TotalRecords : currentPage * pageSize;
            // Văn bản liên quan
            ds.PagelistVanBanLQ.CurrentPage = currentPage;
            ds.PagelistVanBanLQ.TotalPage = (ds.PagelistVanBanLQ.TotalRecords / pageSize) + ((ds.PagelistVanBanLQ.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistVanBanLQ.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistVanBanLQ.RecoredTo = (ds.PagelistVanBanLQ.TotalPage == currentPage || currentPage == 0) ? ds.PagelistVanBanLQ.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelistNhomTP.CurrentPage = currentPage;
            ds.PagelistNhomTP.TotalPage = (ds.PagelistNhomTP.TotalRecords / pageSize) + ((ds.PagelistNhomTP.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistNhomTP.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistNhomTP.RecoredTo = (ds.PagelistNhomTP.TotalPage == currentPage || currentPage == 0) ? ds.PagelistNhomTP.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelisthanhPhanKQXL.CurrentPage = currentPage;
            ds.PagelisthanhPhanKQXL.TotalPage = (ds.PagelisthanhPhanKQXL.TotalRecords / pageSize) + ((ds.PagelisthanhPhanKQXL.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelisthanhPhanKQXL.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelisthanhPhanKQXL.RecoredTo = (ds.PagelisthanhPhanKQXL.TotalPage == currentPage || currentPage == 0) ? ds.PagelisthanhPhanKQXL.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstDonViLienThong = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstMucDoThucHien = db.Dm_Muc_Do_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.type_view = type;
            return View("~/Views/ThuTuc/ThanhPhan/ThemThanhPhan.cshtml", ds);
        }

        [HttpPost]
        public IActionResult ThemThanhPhan(ThanhPhan item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            if (item.url_file != null)
            {
                item.file_dinh_kem = item.url_file.Split('\\').LastOrDefault();
            }
            var _pb = _thuTucDao.LuuThanhPhan(item);
            if (_pb > 0)
            {
                return Json(new { errorCode = 1, msg = "Lỗi hệ thống" });
            }
            if (item.type_view_tt == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemThanhPhan", "ThuTuc", new { code = item.ma_thu_tuc, type = item.type_view });
            }
            else if (item.type_view_tt == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaThanhPhan", "ThuTuc", new { code = item.ma_thu_tuc, code_tp = item.ma_thanh_phan, type = item.type_view, type_tp = item.type_view_tt });
            }
            return RedirectToAction("SuaThuTuc", "ThuTuc", new { code = item.ma_thu_tuc, type = item.type_view, active = ActionThuTuc.ThanhPhan.ToString() });
        }

        public IActionResult SuaThanhPhan(string code, string code_tp, string type, string type_tp, int currentPage = 0, int pageSize = 20)
        {
            var thuTuc = new ThuTucModel();
            var ds = _thuTucDao.ThuTuc(code, code_tp, "", "", "", currentPage, pageSize);
            //Thành phần
            ds.PagelistThanhPhan.CurrentPage = currentPage;
            ds.PagelistThanhPhan.TotalPage = (ds.PagelistThanhPhan.TotalRecords / pageSize) + ((ds.PagelistThanhPhan.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistThanhPhan.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistThanhPhan.RecoredTo = (ds.PagelistThanhPhan.TotalPage == currentPage || currentPage == 0) ? ds.PagelistThanhPhan.TotalRecords : currentPage * pageSize;
            // Văn bản liên quan
            ds.PagelistVanBanLQ.CurrentPage = currentPage;
            ds.PagelistVanBanLQ.TotalPage = (ds.PagelistVanBanLQ.TotalRecords / pageSize) + ((ds.PagelistVanBanLQ.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistVanBanLQ.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistVanBanLQ.RecoredTo = (ds.PagelistVanBanLQ.TotalPage == currentPage || currentPage == 0) ? ds.PagelistVanBanLQ.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelistNhomTP.CurrentPage = currentPage;
            ds.PagelistNhomTP.TotalPage = (ds.PagelistNhomTP.TotalRecords / pageSize) + ((ds.PagelistNhomTP.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistNhomTP.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistNhomTP.RecoredTo = (ds.PagelistNhomTP.TotalPage == currentPage || currentPage == 0) ? ds.PagelistNhomTP.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelisthanhPhanKQXL.CurrentPage = currentPage;
            ds.PagelisthanhPhanKQXL.TotalPage = (ds.PagelisthanhPhanKQXL.TotalRecords / pageSize) + ((ds.PagelisthanhPhanKQXL.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelisthanhPhanKQXL.RecoredFrom = (currentPage - 1) * pageSize < 1 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelisthanhPhanKQXL.RecoredTo = (ds.PagelisthanhPhanKQXL.TotalPage == currentPage || currentPage == 0) ? ds.PagelisthanhPhanKQXL.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstDonViLienThong = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstMucDoThucHien = db.Dm_Muc_Do_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.type_view = type;
            ViewBag.type_view_tp = type_tp;
            return View("~/Views/ThuTuc/ThanhPhan/SuaThanhPhan.cshtml", ds);
        }

        [HttpPost]
        public IActionResult XoaThanhPhan(string code, string code_tp, string type)
        {
            if (String.IsNullOrEmpty(code_tp))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _thuTucDao.XoaThanhPhan(code_tp, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/ThuTuc/SuaThuTuc?code=" + code + "&type=" + type + "&action=" + ActionThuTuc.ThanhPhan.ToString() });
        }

        public async Task<IActionResult> RenderViewThanhPhan(string code = "")
        {
            var ds = _thuTucDao.DanhSachThanhPhan(code);
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThanhPhan/ViewThanhPhan.cshtml", ds);
            return Content(result);
        }
        #endregion
        [HttpPost]
        public IActionResult ThemBieuMau(string ckedit1, string ten_bieu_mau)
        {
            try
            {
                EAContext eAContext = new EAContext();
                dm_bieu_mau item = new dm_bieu_mau();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                ViewBag.UidName = User.Identity.Name;
                item.Deleted = 0;
                item.CreatedDateUtc = DateTime.Now;
                item.ten_bieu_mau = ten_bieu_mau;
                item.mo_ta = ckedit1;
                item.url_bieu_mau = "ThuTuc/ViewBieuMau?Id=" + ten_bieu_mau;
                item.ma_thu_tuc = "";

                var obj = eAContext.dm_bieu_mau.Where(n => n.ten_bieu_mau == item.ten_bieu_mau).Count();
                if (obj > 0)
                {
                    //return "Có thểm thêm thông báo ở đây !";
                    return RedirectToAction("QuanLy", new { tabbieumau  = "1"});
                };
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_bieu_mau> _role = eAContext.dm_bieu_mau.Add(item);
                eAContext.SaveChanges();
                int id = item.Id;
                if(id > 0)
                {
                    return RedirectToAction("QuanLy", new { tabbieumau = 1 });
                }
                else
                {
                    return RedirectToAction("QuanLy", new { tabbieumau = 1 });
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("ThemBieuMau", new { tabbieumau = 1 });
            }
        }
        public IActionResult ViewBieuMau(int id, string type = "")
        {
            EAContext db = new EAContext();
            return View(db.dm_bieu_mau.Where(n=>n.Id == id).FirstOrDefault());
        }
    }
}
