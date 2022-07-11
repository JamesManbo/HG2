using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
using HG.Entities;
using HG.Entities.DanhMuc.dm_bieu_mau;
using HG.Entities.Entities.Model;
using HG.Entities.Entities.ThuTuc;
using HG.WebApp.Data;
using HG.WebApp.Helper;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
                ViewBag.lstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();
            }
            ViewBag.BieuMau = tabbieumau;
            return View("~/Views/ThuTuc/QuanLy.cshtml", ds);
        }

        public async Task<IActionResult> QuanLyPaging(int currentPage = 0, int pageSize = 0, string ma_pb = "", string ma_lv = "", string tu_khoa = "")
        {
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = currentPage, ma_pb = ma_pb, ma_lv = ma_lv, tu_khoa = tu_khoa, RecordsPerPage = pageSize };
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
        public IActionResult ThemThuTuc(string code = "", int currentPage = 0, int pageSize = 20)
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
            ViewBag.code = code;
            return View("~/Views/ThuTuc/ThemThuTuc.cshtml", ds);
        }
        [HttpPost]
        public IActionResult ThemThuTuc(DmThuTuc item)
        {
            item.ma_thu_tuc = HelperString.CreateCode(item.ma_thu_tuc);
            item.ma_quoc_gia = HelperString.CreateCode(item.ma_quoc_gia);
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
            return RedirectToAction("SuaThuTuc", "ThuTuc", new { code = item.ma_thu_tuc, type = StatusAction.View.ToString(), active = ActionThuTuc.ThuTuc.ToString() });
        }


        public IActionResult SuaThuTuc(string code, string type, string active = "", int currentPage = 0, int pageSize = 20)
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

        [HttpPost]
        public JsonResult CheckMaTTHC(string code)
        {
            var ds = _thuTucDao.CheckMaThuTuc(code, ActionThuTuc.ThuTuc.ToString());
            if (ds == 0)
            {
                return Json(new { error = 0, href = "/ThuTuc/ThemThuTuc?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "" });
        }

        #region Thành phần

        public IActionResult ThemThanhPhan(string code, string code_tp = "", string type = "", int currentPage = 1, int pageSize = 20)
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
            if (string.IsNullOrEmpty(code_tp))
            {
                ViewBag.code_tp = code + "-" + (ds.thuTuc.count_tp + 1).ToString();
            }
            else
            {
                ViewBag.code_tp = code_tp;
            }
            ViewBag.type_view = type;

            return View("~/Views/ThuTuc/ThanhPhan/ThemThanhPhan.cshtml", ds);
        }

        [HttpPost]
        public IActionResult ThemThanhPhan(ThanhPhan item)
        {
            item.ma_thanh_phan = HelperString.CreateCode(item.ma_thanh_phan);
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
            var _pb = _thuTucDao.XoaThanhPhan(code_tp, uid, type);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/ThuTuc/SuaThuTuc?code=" + code + "&type=" + type + "&active=" + ActionThuTuc.ThanhPhan.ToString() });
        }

        [HttpPost]
        public IActionResult XoaFileThanhPhan(string code, string code_tp, string type)
        {
            if (String.IsNullOrEmpty(code_tp))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _thuTucDao.XoaThanhPhan(code_tp, uid, "file");
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/ThuTuc/SuaThanhPhan?code=" + code + "&code_tp=" + code_tp + "&type=" + type + "&type_tp=" + StatusAction.Edit.ToString() });

        }

        public async Task<IActionResult> RenderViewThanhPhan(string code = "")
        {
            var ds = _thuTucDao.DanhSachThanhPhan(code);
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThanhPhan/ViewThanhPhan.cshtml", ds);
            return Content(result);
        }

        [HttpPost]
        public JsonResult CheckMaTP(string code, string code_tp, string type)
        {
            var ds = _thuTucDao.CheckMaThuTuc(code, ActionThuTuc.ThanhPhan.ToString());
            if (ds == 0)
            {
                return Json(new { error = 0, href = "/ThuTuc/ThemThanhPhan?code=" + code.ToUpper() + "&code_tp=" + code_tp + "&type=" + type });
            }
            return Json(new { error = 1, href = "" });
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
                var obj = eAContext.dm_bieu_mau.Where(n => n.ten_bieu_mau == item.ten_bieu_mau).FirstOrDefault();
                if (obj != null)
                {
                    //is update
                    obj.UpdatedDateUtc = DateTime.Now;
                    obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    obj.mo_ta = item.mo_ta;
                    eAContext.Entry(obj).State = EntityState.Modified;
                    eAContext.SaveChanges();
                    //return "Có thểm thêm thông báo ở đây !";
                    return RedirectToAction("QuanLy", new { tabbieumau = "1" });
                };
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_bieu_mau> _role = eAContext.dm_bieu_mau.Add(item);
                eAContext.SaveChanges();
                int id = item.Id;
                if (id > 0)
                {
                    return RedirectToAction("QuanLy", new { tabbieumau = 1 });
                }
                else
                {
                    return RedirectToAction("QuanLy", new { tabbieumau = 1 });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ThemBieuMau", new { tabbieumau = 1 });
            }
        }
        public IActionResult ViewBieuMau(int id, string type = "")
        {
            if (type == StatusAction.View.ToString())
            {
                EAContext db = new EAContext();
                ViewBag.Template = db.dm_bieu_mau.Where(n => n.Id == id).FirstOrDefault().mo_ta;
                return View(db.dm_bieu_mau.Where(n => n.Id == id).FirstOrDefault());
            }
            else
            {
                EAContext db = new EAContext();
                var obj = db.dm_bieu_mau.Where(n => n.Id == id).FirstOrDefault();

                if (obj != null)
                {
                    var abc = obj.mo_ta;
                    foreach (var item in HG.WebApp.Helper.HelperString.ListTemplate())
                    {
                        if (abc.Contains(item.TempFormat))
                        {
                            abc = abc.Replace(item.TempFormat, item.TempElement);
                        }
                    }
                    ViewBag.Template = abc;
                }
                return View(obj);
            }

        }
        public string XoaBieuMau(int id)
        {
            EAContext db = new EAContext();
            var obj = db.dm_bieu_mau.Find(id);
            if (obj != null)
            {
                db.dm_bieu_mau.Attach(obj);
                db.dm_bieu_mau.Remove(obj);
                db.SaveChanges();
                return "Xóa biểu mẫu thành công !!!";
            }
            return "Xóa biểu mẫu không thành công !!!";
        }
        public async Task<IActionResult> SuaBieuMau(int id = 0)
        {
            EAContext db = new EAContext();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/SuaBieuMau.cshtml", db.dm_bieu_mau.Where(n => n.Id == id).FirstOrDefault());
            return Content(result);
        }

        #region nhom văn bản liên quan 
        public async Task<IActionResult> ThemVBLQ(string ma_thu_tuc = "")
        {
            EAContext db = new EAContext();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemVBLQ.cshtml", new VanBanLQ() { ma_thu_tuc = ma_thu_tuc });
            return Content(result);
        }
        [HttpPost]
        public string SaveVBLQ(string Id = "", string ma_thu_tuc = "", string ten_van_ban = "", string mo_ta = "", string file_dinh_kem = "", string stt = "0")
        {
            try
            {
                EAContext db = new EAContext();
                if (!string.IsNullOrEmpty(Id) && Id != "0")
                {
                    var objVBLQ = db.VanBanLQ.Where(n => n.Id == Convert.ToInt32(Id)).FirstOrDefault();
                    objVBLQ.UpdatedDateUtc = DateTime.Now;
                    objVBLQ.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    objVBLQ.ten_van_ban = ten_van_ban;
                    objVBLQ.mo_ta = mo_ta;
                    objVBLQ.file_dinh_kem = file_dinh_kem;
                    objVBLQ.stt = Convert.ToInt32(stt);
                    db.Entry(objVBLQ).State = EntityState.Modified;
                    db.SaveChanges();
                    return "Sửa văn bản liên quan thành công!!!";
                }

                var obj = db.VanBanLQ.Where(n => n.ten_van_ban == ten_van_ban).Count();
                VanBanLQ item = new VanBanLQ();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                item.CreatedDateUtc = DateTime.Now;
                item.Deleted = 0;
                item.ten_van_ban = ten_van_ban;
                item.ma_thu_tuc = ma_thu_tuc;
                item.ma_van_ban = ma_thu_tuc;
                item.file_dinh_kem = file_dinh_kem;
                item.mo_ta = mo_ta;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<VanBanLQ> _role = db.VanBanLQ.Add(item);
                db.SaveChanges();
                int id = item.Id;
                if (id > 0)
                {
                    return "Thêm văn bản liên quan thành công!!!";
                }
                else
                {
                    return "Thêm văn bản liên quan thất bại!!!";
                }
            }
            catch (Exception e)
            {
                return "Lỗi dữ liệu !!!";
            }

        }
        public async Task<IActionResult> SaveAndAddVBLQ(string Id = "", string ma_thu_tuc = "", string ten_van_ban = "", string mo_ta = "", string file_dinh_kem = "", string stt = "")
        {
            try
            {
                EAContext db = new EAContext();
                if (!string.IsNullOrEmpty(Id) && Id != "0")
                {
                    var obj = db.VanBanLQ.Where(n => n.Id == Convert.ToInt32(Id)).FirstOrDefault();
                    obj.UpdatedDateUtc = DateTime.Now;
                    obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    obj.ten_van_ban = ten_van_ban;
                    obj.mo_ta = mo_ta;
                    obj.file_dinh_kem = file_dinh_kem;
                    obj.stt = Convert.ToInt32(stt);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemVBLQ.cshtml", new VanBanLQ() { ma_thu_tuc = ma_thu_tuc });
                    return Content(result);
                }
                VanBanLQ item = new VanBanLQ();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                item.Deleted = 0;
                item.ten_van_ban = ten_van_ban;
                item.ma_thu_tuc = ma_thu_tuc;
                item.ma_van_ban = ma_thu_tuc;
                item.file_dinh_kem = file_dinh_kem;
                item.mo_ta = mo_ta;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<VanBanLQ> _role = db.VanBanLQ.Add(item);
                db.SaveChanges();
                int id = item.Id;
                if (id > 0)
                {
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemVBLQ.cshtml", new VanBanLQ() { ma_thu_tuc = ma_thu_tuc });
                    return Content(result);
                }
                else
                {
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemVBLQ.cshtml", new VanBanLQ() { ma_thu_tuc = ma_thu_tuc });
                    return Content(result);
                }
            }
            catch (Exception e)
            {
                ViewBag.MaThuTuc = ma_thu_tuc;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemVBLQ.cshtml", new VanBanLQ() { ma_thu_tuc = ma_thu_tuc });
                return Content(result);

            }

        }
        public string XoaVBLQ(string id = "")
        {
            try
            {
                EAContext db = new EAContext();
                var obj = db.VanBanLQ.Find(id);
                if (obj != null)
                {
                    db.VanBanLQ.Attach(obj);
                    db.VanBanLQ.Remove(obj);
                    db.SaveChanges();
                    return "Xóa văn bản liên quan thành công !!!";
                }
                return "Xóa văn bản liên quan không thành công !!!";
            }
            catch (Exception e)
            {
                return "Xóa văn bản liên quan không thành công !!!";
            }

        }
        public async Task<IActionResult> SuaVBLQ(string id = "", string type = "")
        {
            if (type == StatusAction.View.ToString())
            {
                EAContext db = new EAContext();
                ViewBag.type_view = StatusAction.View.ToString();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemVBLQ.cshtml", db.VanBanLQ.FirstOrDefault(n => n.Id == Convert.ToInt32(id)));
                return Content(result);
            }
            else
            {
                EAContext db = new EAContext();
                ViewBag.type_view = StatusAction.Edit.ToString();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemVBLQ.cshtml", db.VanBanLQ.FirstOrDefault(n => n.Id == Convert.ToInt32(id)));
                return Content(result);
            }
        }
        #endregion
        #region nhóm thành phần
        public async Task<IActionResult> ThemNhomThanhPhan(string ma_thu_tuc = "")
        {
            EAContext db = new EAContext();
            var obj = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemNhomThanhPhan.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "-nhom" + (obj + 1).ToString() });
            return Content(result);
        }
        public string SaveNhomTP(string Id = "", string ma_thu_tuc = "", string ma_nhom = "", string ten_nhom = "", string mo_ta = "", string Stt = "0")
        {
            try
            {
                EAContext db = new EAContext();
                if (!string.IsNullOrEmpty(Id))
                {
                    var objVBLQ = db.dm_nhom_tp.Where(n => n.Id == Convert.ToInt32(Id)).FirstOrDefault();
                    objVBLQ.UpdatedDateUtc = DateTime.Now;
                    objVBLQ.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    objVBLQ.ma_nhom = ma_nhom;
                    objVBLQ.ten_nhom = ten_nhom;
                    objVBLQ.mo_ta = mo_ta;
                    objVBLQ.Stt = Convert.ToInt32(Stt);
                    db.Entry(objVBLQ).State = EntityState.Modified;
                    db.SaveChanges();
                    return "Sửa nhóm thành phần thành công!!!";
                }
                dm_nhom_tp item = new dm_nhom_tp();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                item.CreatedDateUtc = DateTime.Now;
                item.Deleted = 0;
                item.ma_nhom = ma_nhom;
                item.ma_thu_tuc = ma_thu_tuc;
                item.ten_nhom = ten_nhom;
                item.Stt = Convert.ToInt32(Stt);
                item.mo_ta = mo_ta;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_nhom_tp> _role = db.dm_nhom_tp.Add(item);
                db.SaveChanges();
                int id = item.Id;
                if (id > 0)
                {
                    return "Thêm nhóm thành phần thành công!!!";
                }
                else
                {
                    return "Thêm nhóm thành phần thất bại!!!";
                }
            }
            catch (Exception e)
            {
                return "Lỗi dữ liệu !!!";
            }

        }
        public async Task<IActionResult> SaveAddNhomTP(string Id = "", string ma_thu_tuc = "", string ma_nhom = "", string ten_nhom = "", string mo_ta = "", string Stt = "0")
        {
            try
            {
                EAContext db = new EAContext();
                if (!string.IsNullOrEmpty(Id) && Id != "0")
                {
                    var objVBLQ = db.dm_nhom_tp.Find(Id);
                    objVBLQ.UpdatedDateUtc = DateTime.Now;
                    objVBLQ.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    objVBLQ.ma_nhom = ma_nhom;
                    objVBLQ.ten_nhom = ten_nhom;
                    objVBLQ.mo_ta = mo_ta;
                    objVBLQ.Stt = Convert.ToInt32(Stt);
                    db.Entry(objVBLQ).State = EntityState.Modified;
                    db.SaveChanges();
                    var demNhomTP = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemNhomThanhPhan.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "-nhom" + (demNhomTP + 1).ToString() });
                    return Content(result);
                }
                dm_nhom_tp item = new dm_nhom_tp();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                item.CreatedDateUtc = DateTime.Now;
                item.Deleted = 0;
                item.ma_nhom = ma_nhom;
                item.ma_thu_tuc = ma_thu_tuc;
                item.ten_nhom = ten_nhom;
                item.Stt = Convert.ToInt32(Stt);
                item.mo_ta = mo_ta;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_nhom_tp> _role = db.dm_nhom_tp.Add(item);
                db.SaveChanges();
                int id = item.Id;
                if (id > 0)
                {
                    var demNhomTP = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemNhomThanhPhan.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "-nhom" + (demNhomTP + 1).ToString() });
                    return Content(result);
                }
                else
                {
                    var demNhomTP = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemNhomThanhPhan.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "nhom" + (demNhomTP + 1).ToString() });
                    return Content(result);
                }
            }
            catch (Exception e)
            {
                EAContext db = new EAContext();
                var demNhomTP = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemNhomThanhPhan.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "-nhom" + (demNhomTP + 1).ToString() });
                return Content(result);

            }

        }
        public string XoaNhomTP(string id = "")
        {
            try
            {
                EAContext db = new EAContext();
                var obj = db.dm_nhom_tp.Find(id);
                if (obj != null)
                {
                    db.dm_nhom_tp.Attach(obj);
                    db.dm_nhom_tp.Remove(obj);
                    db.SaveChanges();
                    return "Xóa nhóm thành phần thành công !!!";
                }
                return "Xóa nhóm thành phần không thành công !!!";
            }
            catch (Exception e)
            {
                return "Xóa nhóm thành phần không thành công !!!";
            }

        }
        public async Task<IActionResult> SuaNhomTP(string id = "", string type = "")
        {
            if (type == StatusAction.View.ToString())
            {
                EAContext db = new EAContext();
                ViewBag.type_view = StatusAction.View.ToString();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemNhomThanhPhan.cshtml", db.dm_nhom_tp.FirstOrDefault(n => n.Id == Convert.ToInt32(id)));
                return Content(result);
            }
            else
            {
                EAContext db = new EAContext();
                ViewBag.type_view = StatusAction.Edit.ToString();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemNhomThanhPhan.cshtml", db.dm_nhom_tp.FirstOrDefault(n => n.Id == Convert.ToInt32(id)));
                return Content(result);
            }
        }

        #endregion
        #region nhóm thành phần KQXL
        public async Task<IActionResult> ThemTPKQXL(string ma_thu_tuc = "")
        {
            EAContext db = new EAContext();
            var obj = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemTPKQXL.cshtml", new dm_thanh_phan_kqxl() { ma_thu_tuc = ma_thu_tuc, ma_tp_kq = ma_thu_tuc + "-kqxl" + (obj + 1).ToString() });
            return Content(result);
        }
        public string SaveTPKQXL(string Id = "", string ma_thu_tuc = "", string ma_nhom = "", string ten_nhom = "", string mo_ta = "", string Stt = "0")
        {
            try
            {
                EAContext db = new EAContext();
                if (!string.IsNullOrEmpty(Id))
                {
                    var objVBLQ = db.dm_nhom_tp.Where(n => n.Id == Convert.ToInt32(Id)).FirstOrDefault();
                    objVBLQ.UpdatedDateUtc = DateTime.Now;
                    objVBLQ.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    objVBLQ.ma_nhom = ma_nhom;
                    objVBLQ.ten_nhom = ten_nhom;
                    objVBLQ.mo_ta = mo_ta;
                    objVBLQ.Stt = Convert.ToInt32(Stt);
                    db.Entry(objVBLQ).State = EntityState.Modified;
                    db.SaveChanges();
                    return "Sửa nhóm thành phần thành công!!!";
                }
                dm_nhom_tp item = new dm_nhom_tp();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                item.CreatedDateUtc = DateTime.Now;
                item.Deleted = 0;
                item.ma_nhom = ma_nhom;
                item.ma_thu_tuc = ma_thu_tuc;
                item.ten_nhom = ten_nhom;
                item.Stt = Convert.ToInt32(Stt);
                item.mo_ta = mo_ta;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_nhom_tp> _role = db.dm_nhom_tp.Add(item);
                db.SaveChanges();
                int id = item.Id;
                if (id > 0)
                {
                    return "Thêm nhóm thành phần thành công!!!";
                }
                else
                {
                    return "Thêm nhóm thành phần thất bại!!!";
                }
            }
            catch (Exception e)
            {
                return "Lỗi dữ liệu !!!";
            }

        }
        public async Task<IActionResult> SaveAddTPKQXL(string Id = "", string ma_thu_tuc = "", string ma_nhom = "", string ten_nhom = "", string mo_ta = "", string Stt = "0")
        {
            try
            {
                EAContext db = new EAContext();
                if (!string.IsNullOrEmpty(Id) && Id != "0")
                {
                    var objVBLQ = db.dm_nhom_tp.Find(Id);
                    objVBLQ.UpdatedDateUtc = DateTime.Now;
                    objVBLQ.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    objVBLQ.ma_nhom = ma_nhom;
                    objVBLQ.ten_nhom = ten_nhom;
                    objVBLQ.mo_ta = mo_ta;
                    objVBLQ.Stt = Convert.ToInt32(Stt);
                    db.Entry(objVBLQ).State = EntityState.Modified;
                    db.SaveChanges();
                    var demNhomTP = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemNhomThanhPhan.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "-nhom" + (demNhomTP + 1).ToString() });
                    return Content(result);
                }
                dm_nhom_tp item = new dm_nhom_tp();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                item.CreatedDateUtc = DateTime.Now;
                item.Deleted = 0;
                item.ma_nhom = ma_nhom;
                item.ma_thu_tuc = ma_thu_tuc;
                item.ten_nhom = ten_nhom;
                item.Stt = Convert.ToInt32(Stt);
                item.mo_ta = mo_ta;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_nhom_tp> _role = db.dm_nhom_tp.Add(item);
                db.SaveChanges();
                int id = item.Id;
                if (id > 0)
                {
                    var demNhomTP = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemTPKQXL.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "-nhom" + (demNhomTP + 1).ToString() });
                    return Content(result);
                }
                else
                {
                    var demNhomTP = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemTPKQXL.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "nhom" + (demNhomTP + 1).ToString() });
                    return Content(result);
                }
            }
            catch (Exception e)
            {
                EAContext db = new EAContext();
                var demNhomTP = db.dm_nhom_tp.Where(n => n.ma_thu_tuc == ma_thu_tuc).Count();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemTPKQXL.cshtml", new dm_nhom_tp() { ma_thu_tuc = ma_thu_tuc, ma_nhom = ma_thu_tuc + "-nhom" + (demNhomTP + 1).ToString() });
                return Content(result);

            }

        }
        public string XoaTPKQXL(string id = "")
        {
            try
            {
                EAContext db = new EAContext();
                var obj = db.dm_nhom_tp.Find(id);
                if (obj != null)
                {
                    db.dm_nhom_tp.Attach(obj);
                    db.dm_nhom_tp.Remove(obj);
                    db.SaveChanges();
                    return "Xóa nhóm thành phần thành công !!!";
                }
                return "Xóa nhóm thành phần không thành công !!!";
            }
            catch (Exception e)
            {
                return "Xóa nhóm thành phần không thành công !!!";
            }

        }
        public async Task<IActionResult> SuaPKQXL(string id = "", string type = "")
        {
            if (type == StatusAction.View.ToString())
            {
                EAContext db = new EAContext();
                ViewBag.type_view = StatusAction.View.ToString();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemTPKQXL.cshtml", db.dm_nhom_tp.FirstOrDefault(n => n.Id == Convert.ToInt32(id)));
                return Content(result);
            }
            else
            {
                EAContext db = new EAContext();
                ViewBag.type_view = StatusAction.Edit.ToString();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/ThemTPKQXL.cshtml", db.dm_nhom_tp.FirstOrDefault(n => n.Id == Convert.ToInt32(id)));
                return Content(result);
            }
        }

        #endregion
    }
}
