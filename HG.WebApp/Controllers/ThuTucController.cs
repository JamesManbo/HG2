using HG.Data.Business.ThuTuc;
using HG.Entities.Entities.Model;
using HG.Entities.Entities.ThuTuc;
using HG.WebApp.Data;
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

        //extend sys identity
        public ThuTucController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _thuTucDao = new ThuTucDao(DbProvider);
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
            }
            return View("~/Views/ThuTuc/QuanLy.cshtml", ds);
        }

        public IActionResult QuanLyPaging(int currentPage = 0, int pageSize = 0, string ma_pb = "", string tu_khoa = "")
        {
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = ma_pb, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
            ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? ds.Pagelist.TotalRecords : currentPage * pageSize;
            using (var db = new EAContext())
            {
                ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
            }
            return View("~/Views/ThuTuc/QuanLyPaging.cshtml", ds);
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
            }
            return View("~/Views/ThuTuc/ThemThuTuc.cshtml", ds);
        }
        [HttpPost]
        public IActionResult ThemThuTuc(DmThuTuc item)
        {
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
            }
            ViewBag.type_view = type;
            return View("~/Views/ThuTuc/SuaThuTuc.cshtml", ds);
        }
    }
}
