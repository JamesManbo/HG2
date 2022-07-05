using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
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
        public IActionResult QuanLy()
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
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            }
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
            ds.PagelistThanhPhan.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistThanhPhan.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistThanhPhan.TotalRecords : currentPage * pageSize;
            // Văn bản liên quan
            ds.PagelistVanBanLQ.CurrentPage = currentPage;
            ds.PagelistVanBanLQ.TotalPage = (ds.PagelistVanBanLQ.TotalRecords / pageSize) + ((ds.PagelistVanBanLQ.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistVanBanLQ.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistVanBanLQ.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistVanBanLQ.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelistNhomTP.CurrentPage = currentPage;
            ds.PagelistNhomTP.TotalPage = (ds.PagelistNhomTP.TotalRecords / pageSize) + ((ds.PagelistNhomTP.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistNhomTP.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistNhomTP.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistNhomTP.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelisthanhPhanKQXL.CurrentPage = currentPage;
            ds.PagelisthanhPhanKQXL.TotalPage = (ds.PagelisthanhPhanKQXL.TotalRecords / pageSize) + ((ds.PagelisthanhPhanKQXL.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelisthanhPhanKQXL.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelisthanhPhanKQXL.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelisthanhPhanKQXL.TotalRecords : currentPage * pageSize;
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


        public IActionResult SuaThuTuc(string code, string type, int currentPage = 0, int pageSize = 20)
        {
            var thuTuc = new ThuTucModel();
            var ds = _thuTucDao.ThuTuc(code, "", "", "", "", currentPage, pageSize);
            //Thành phần
            ds.PagelistThanhPhan.CurrentPage = currentPage;
            ds.PagelistThanhPhan.TotalPage = (ds.PagelistThanhPhan.TotalRecords / pageSize) + ((ds.PagelistThanhPhan.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistThanhPhan.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistThanhPhan.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistThanhPhan.TotalRecords : currentPage * pageSize;
            // Văn bản liên quan
            ds.PagelistVanBanLQ.CurrentPage = currentPage;
            ds.PagelistVanBanLQ.TotalPage = (ds.PagelistVanBanLQ.TotalRecords / pageSize) + ((ds.PagelistVanBanLQ.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistVanBanLQ.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistVanBanLQ.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistVanBanLQ.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelistNhomTP.CurrentPage = currentPage;
            ds.PagelistNhomTP.TotalPage = (ds.PagelistNhomTP.TotalRecords / pageSize) + ((ds.PagelistNhomTP.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistNhomTP.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistNhomTP.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistNhomTP.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelisthanhPhanKQXL.CurrentPage = currentPage;
            ds.PagelisthanhPhanKQXL.TotalPage = (ds.PagelisthanhPhanKQXL.TotalRecords / pageSize) + ((ds.PagelisthanhPhanKQXL.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelisthanhPhanKQXL.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelisthanhPhanKQXL.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelisthanhPhanKQXL.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstDonViLienThong = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstMucDoThucHien = db.Dm_Muc_Do_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.type_view = type;
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
        public IActionResult ThemThanhPhan(string code, int currentPage = 0, int pageSize = 20)
        {
            var ds = _thuTucDao.ThuTuc(code, "", "", "", "", currentPage, pageSize);
            //Thành phần
            ds.PagelistThanhPhan.CurrentPage = currentPage;
            ds.PagelistThanhPhan.TotalPage = (ds.PagelistThanhPhan.TotalRecords / pageSize) + ((ds.PagelistThanhPhan.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistThanhPhan.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistThanhPhan.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistThanhPhan.TotalRecords : currentPage * pageSize;
            // Văn bản liên quan
            ds.PagelistVanBanLQ.CurrentPage = currentPage;
            ds.PagelistVanBanLQ.TotalPage = (ds.PagelistVanBanLQ.TotalRecords / pageSize) + ((ds.PagelistVanBanLQ.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistVanBanLQ.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistVanBanLQ.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistVanBanLQ.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelistNhomTP.CurrentPage = currentPage;
            ds.PagelistNhomTP.TotalPage = (ds.PagelistNhomTP.TotalRecords / pageSize) + ((ds.PagelistNhomTP.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistNhomTP.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistNhomTP.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistNhomTP.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelisthanhPhanKQXL.CurrentPage = currentPage;
            ds.PagelisthanhPhanKQXL.TotalPage = (ds.PagelisthanhPhanKQXL.TotalRecords / pageSize) + ((ds.PagelisthanhPhanKQXL.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelisthanhPhanKQXL.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelisthanhPhanKQXL.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelisthanhPhanKQXL.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstDonViLienThong = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstMucDoThucHien = db.Dm_Muc_Do_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
            }
            return View("~/Views/ThuTuc/ThanhPhan/ThemThanhPhan.cshtml", ds);
        }

        [HttpPost]
        public IActionResult ThemThanhPhan(ThanhPhan item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            item.file_dinh_kem = item.url_file.Split('\\').LastOrDefault();
            var _pb = _thuTucDao.LuuThanhPhan(item);
            if (_pb > 0)
            {
                return Json(new { errorCode = 1, msg = "Lỗi hệ thống" });
            }
            return Json(new { errorCode = 0, msg = "M" });
            //return RedirectToAction("SuaThanhPhan", "ThuTuc", new { code = item.ma_thu_tuc, code_tp = item.ma_thanh_phan, type = StatusAction.View.ToString() });
        }

        public IActionResult SuaThanhPhan(string code, string code_tp, string type, int currentPage = 0, int pageSize = 20)
        {
            var thuTuc = new ThuTucModel();
            var ds = _thuTucDao.ThuTuc(code, code_tp, "", "", "", currentPage, pageSize);
            //Thành phần
            ds.PagelistThanhPhan.CurrentPage = currentPage;
            ds.PagelistThanhPhan.TotalPage = (ds.PagelistThanhPhan.TotalRecords / pageSize) + ((ds.PagelistThanhPhan.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistThanhPhan.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistThanhPhan.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistThanhPhan.TotalRecords : currentPage * pageSize;
            // Văn bản liên quan
            ds.PagelistVanBanLQ.CurrentPage = currentPage;
            ds.PagelistVanBanLQ.TotalPage = (ds.PagelistVanBanLQ.TotalRecords / pageSize) + ((ds.PagelistVanBanLQ.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistVanBanLQ.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistVanBanLQ.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistVanBanLQ.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelistNhomTP.CurrentPage = currentPage;
            ds.PagelistNhomTP.TotalPage = (ds.PagelistNhomTP.TotalRecords / pageSize) + ((ds.PagelistNhomTP.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelistNhomTP.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelistNhomTP.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelistNhomTP.TotalRecords : currentPage * pageSize;
            // Nhóm TP
            ds.PagelisthanhPhanKQXL.CurrentPage = currentPage;
            ds.PagelisthanhPhanKQXL.TotalPage = (ds.PagelisthanhPhanKQXL.TotalRecords / pageSize) + ((ds.PagelisthanhPhanKQXL.TotalRecords % pageSize) > 0 ? 1 : 0);
            ds.PagelisthanhPhanKQXL.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ds.PagelisthanhPhanKQXL.RecoredTo = ViewBag.TotalPage == currentPage ? ds.PagelisthanhPhanKQXL.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstDonViLienThong = db.Dm_Don_Vi_Lien_Thong.Where(n => n.Deleted == 0).ToList();
                ViewBag.LstMucDoThucHien = db.Dm_Muc_Do_Thuc_Hien.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.type_view = type;
            return View("~/Views/ThuTuc/ThanhPhan/SuaThanhPhan.cshtml", ds);
        }
        #endregion

        [HttpPost]
        public IActionResult ThemBieuMau(string bieu_mau, string ten_file)
        {
            return null;
        }
    }
}
