using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities.Model;
using HG.Entities.HoSo;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HG.Entities.Search;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace HG.WebApp.Controllers
{
    public class TraCuuHoSoController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LuongXuLyDao _danhmucDao;
        private readonly ThuTucDao _thuTucDao;
        private readonly HG.Data.Business.HoSo.HoSoDao _hoso;
        public TraCuuHoSoController(IWebHostEnvironment _environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new LuongXuLyDao(DbProvider);
            _thuTucDao = new ThuTucDao(DbProvider);
            _hoso = new HG.Data.Business.HoSo.HoSoDao(DbProvider);

        }
        public IActionResult DanhSachToanBoHoSo(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 120 };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public async Task<IActionResult> DanhSachToanBoHoSoPaging(int currentPage = 1, string tu_khoa = "", string ma_thu_tuc = "", int tat_ca = 1, int dung_han = 0, int qua_han = 0, int trang_thai = 120, int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = currentPage, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = dung_han, qua_han = qua_han, RecordsPerPage = pageSize, trang_thai_hs = 120 };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = tu_khoa;
            ViewBag.MaThuTuc = ma_thu_tuc;
            ViewBag.trang_thai = trang_thai;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TraCuuHoSo/Paging/DanhSachToanBoHoSoPaging.cshtml", hs);
            return Content(result);
        }
        public IActionResult ViewTraCuHoSo(int code, string type)
        {
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = db.AspNetUsers.ToList();
            ViewBag.type_view = StatusAction.View.ToString();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            ViewBag.LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstLichSu = db.Ho_So_History.Where(N => N.Id == code).ToList();
            return View(hoso);
        }
        public IActionResult DanhSachHoSoTheoTieuChi(int currentPage = 1, string txtSearch = "", int pageSize = 25, HoSoFilter hoSoFilter = null) 
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var nd = new List<AspNetUsers>();
            var pb = new List<Dm_Phong_Ban>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = "", tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 120 };
            hs = _hoso.TimKiemHoSoTheoTieuChi(hoSoPaging, hoSoFilter, out totalRecored);
            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
                nd = db.AspNetUsers.ToList();
                pb = db.Dm_Phong_Ban.ToList();
            };
            ViewBag.LstNguoiDung = nd;
            ViewBag.ListPhongBan = pb;
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            

            //handle các data đã filter ở đây
            ViewBag.ma_ho_so = hoSoFilter.ma_ho_so;
            ViewBag.ten_nguoi_nop = hoSoFilter.ten_nguoi_nop;
            ViewBag.can_bo_tiep_nhan = hoSoFilter.can_bo_tiep_nhan;
            ViewBag.ma_phong_ban = hoSoFilter.ma_phong_ban;
            ViewBag.can_bo_xu_ly = hoSoFilter.can_bo_xu_ly;
            ViewBag.ma_trang_thai = hoSoFilter.ma_trang_thai;
            ViewBag.tu_ngay = hoSoFilter.tu_ngay;
            ViewBag.den_ngay = hoSoFilter.den_ngay;
            ViewBag.tinh_trang = hoSoFilter.tinh_trang;
            ViewBag.MaLinhVuc = hoSoFilter.ma_linh_vuc;
            ViewBag.MaThuTuc = hoSoFilter.ma_thu_tuc_hc;
            return View(hs);
        }


        public IActionResult TimKiemHoSoLienThong(int trang_thai = 0, string tu_khoa = "", string Oid = "")
        {
            EAContext db = new EAContext();
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var lstQuery = db.Ho_So.Where(n => n.Id != 0 && n.trang_thai == (int)StatusXuLyHoSo.HoSoLienThong);
            var id = 0;
            var tab = 1;
            if (Oid != "")
            {
                id = Convert.ToInt32(Oid.Split("-")[0]);
                tab = Convert.ToInt32(Oid.Split("-")[1]);
            }
            if(trang_thai != 0)
            {
                lstQuery = lstQuery.Where(n => n.id_trang_thai_xl_detail == trang_thai);
            }
            if (!string.IsNullOrEmpty(tu_khoa))
            {
                lstQuery = lstQuery.Where(n => n.ten_ho_so.Contains(tu_khoa));
            }
           
            //ViewBag.type_view = type;
            ViewBag.TotalPage = (lstQuery.Count() / pageSize) + ((lstQuery.Count() % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.PageSize = 0;
            ViewBag.TotalRecored = lstQuery.Count();
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? lstQuery.Count() : pageSize;
            ViewBag.active = tab;
            //href = "/CauHinhHeThong/CauHinh?don_vi=&type=@StatusAction.Edit.ToString()&Oid=@(item.id.ToString()+" - 2")
            return View(lstQuery.ToList());
        }
        public async Task<IActionResult> TimKiemHoSoLienThongPaging(int currentPage = 1, string tu_khoa = "", string ma_thu_tuc = "", int tat_ca = 1, int dung_han = 0, int qua_han = 0, int trang_thai = 120, int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = currentPage, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = dung_han, qua_han = qua_han, RecordsPerPage = pageSize, trang_thai_hs = 120 };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = tu_khoa;
            ViewBag.MaThuTuc = ma_thu_tuc;
            ViewBag.trang_thai = trang_thai;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TraCuuHoSo/Paging/TimKiemHoSoLienThongPaging.cshtml", hs);
            return Content(result);
        }      
        public async Task<IActionResult> GetNguoiDungByPB(string ma_phong_ban = "")
        {
            EAContext db = new EAContext();
            var lstUser = new List<AspNetUsers>();
            lstUser = db.AspNetUsers.Where(s => s.ma_phong_ban == ma_phong_ban).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/LayNguoiDungByPB.cshtml", lstUser);
            return Content(result);
        }
        public async Task<IActionResult> GetLinhVucByPB(string ma_phong_ban = "")
        {
            EAContext db = new EAContext();
            var lstlinhvuc = new List<Dm_Linh_Vuc>();
            var lstPB = db.Dm_Phong_Ban.Where(s => s.ma_phong_ban == ma_phong_ban).FirstOrDefault();
            lstlinhvuc = db.Dm_Linh_Vuc.Where(s => s.ma_don_vi == lstPB.ma_don_vi).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/LayLinhVucByPB.cshtml", lstlinhvuc);
            return Content(result);
        }
    }
}
