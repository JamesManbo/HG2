using AutoMapper;
using HG.Data.Business.DanhMuc;
using HG.Data.Business.HoSo;
using HG.Data.Business.ThuTuc;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Model;
using HG.Entities.HoSo;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HG.WebApp.Controllers
{
    public class XuLyHoSoController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LuongXuLyDao _danhmucDao;
        private readonly ThuTucDao _thuTucDao;
        private readonly XuLyHoSoDao _xulyhsDao;
        private readonly HG.Data.Business.HoSo.HoSoDao _hoso;
        public XuLyHoSoController(IWebHostEnvironment _environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new LuongXuLyDao(DbProvider);
            _thuTucDao = new ThuTucDao(DbProvider);
            _xulyhsDao = new XuLyHoSoDao(DbProvider);
            _hoso = new HG.Data.Business.HoSo.HoSoDao(DbProvider);

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HoSoChoXuLy(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 18 };
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
        public IActionResult ViewHoSo(int code, string type)
        {
            
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = db.AspNetUsers.ToList();
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            ViewBag.LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public async Task<IActionResult> LayThongTinXuLyHoSo(string ma_ho_so, string type)
        {
            //var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            QuaTrinhXuLyModels quaTrinhXuLyModels = new QuaTrinhXuLyModels();
            ViewBag.view_type = type;
            //ViewBag.lstThanhPhan = lstThanhPhan;
            //ViewBag.ma_luong = ma_luong;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/ThongTinXuLy.cshtml", quaTrinhXuLyModels);
            return Content(result);
        }
        [HttpPost]
        public async Task<IActionResult> LuuThongTinHoSo(string Id, string type,int trangthai)
        {
            //var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            EAContext db = new EAContext();
            var hoso = db.Ho_So.Where(n => n.Id == Int32.Parse(Id)).FirstOrDefault();
            
            hoso.trang_thai = trangthai;
            switch (trangthai)
            {
                case 19:
                    hoso.id_trang_thai_xl = 1;
                    break;
                case 22:
                    hoso.id_trang_thai_xl = 2;
                    break;
                case 6:
                    hoso.id_trang_thai_xl = 4;
                    break;
                case 15:
                    hoso.id_trang_thai_xl = 6;
                    break;
                case 14:
                    hoso.id_trang_thai_xl = 3;
                    break;
                case 25:
                    hoso.id_trang_thai_xl = 10;
                    break;
                case 24:
                    hoso.id_trang_thai_xl = 1;
                    break;
                case 12:
                    hoso.id_trang_thai_xl = 5;
                    break;               
                case 99:
                    hoso.id_trang_thai_xl = 9;
                    break;
                case 98:
                    hoso.id_trang_thai_xl = 11;
                    break;
            }
            db.Update(hoso);
            db.SaveChangesAsync();
            ViewBag.view_type = type;
            //ViewBag.lstThanhPhan = lstThanhPhan;
            //ViewBag.ma_luong = ma_luong;
           
            return RedirectToAction("XuLyHoSo", "HoSoChoXuLy", new {  currentPage = 1,  txtSearch = "",  ma_linh_vuc = "",  ma_thu_tuc = "",  pageSize = 25 });
           // var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoChoXuLy.cshtml",hs);
           // return Content(result);
        }
    }
}
