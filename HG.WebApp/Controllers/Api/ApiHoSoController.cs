using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
using HG.Entities;
using HG.Entities.HoSo;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HG.WebApp.Controllers
{
    public class ApiHoSoController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LuongXuLyDao _danhmucDao;
        private readonly ThuTucDao _thuTucDao;
        private readonly HG.Data.Business.HoSo.HoSoDao _hoso;
        public ApiHoSoController(IWebHostEnvironment _environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        public async Task<IActionResult> HoSoBoSungPaging(int currentPage = 1, string tu_khoa = "", string ma_thu_tuc = "", int tat_ca = 1, int dung_han = 0, int qua_han = 0, int trang_thai = 1, int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = currentPage, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = dung_han, qua_han = qua_han, RecordsPerPage = pageSize, trang_thai_hs = trang_thai };
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
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/HoSoBoSungPaging.cshtml", hs);
            return Content(result);
        }
        public string ChuyenXuLy(int ho_so_id)
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == ho_so_id).FirstOrDefault();
            if (obj != null)
            {
                obj.trang_thai = (int)StatusTiepNhanHoSo.HoSoChoXL;
                obj.id_trang_thai_xl = 0; //chưa tiếp nhận
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(ho_so_id, "Hồ sơ chuyển qua chờ xử lý", (int)StatusTiepNhanHoSo.HoSoDangTiepNhan, (int)StatusTiepNhanHoSo.HoSoChoXL, Guid.Parse(userManager.GetUserId(User)));
                return "Chuyển xử lý hồ sơ thành công!";
            }
            else
            {
                return "Chuyển xử lý hồ sơ lỗi !";
            }
           
        }
        public string TraKetQua(int ho_so_id, string le_phi, string nguoiky, int da_thanh_toan,string filedinhkem )
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == ho_so_id).FirstOrDefault();
            if (obj != null)
            {
                //update cac thong tin thanh toan, nguoi ky ... vao ho so
                obj.trang_thai = (int)StatusTraKetQua.HoSoDaTraKQ;
                //obj.id_trang_thai_xl = 0; //chưa tiếp nhận
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(ho_so_id, "Hồ sơ chuyển qua đã trả kết quả", (int)StatusTiepNhanHoSo.HoSoDangTiepNhan, (int)StatusTraKetQua.HoSoDaTraKQ, Guid.Parse(userManager.GetUserId(User)));
                return "Chuyển trả kết quả hồ sơ thành công!";
            }
            else
            {
                return "Chuyển trả kết quả hồ sơ lỗi !";
            }
        }


    }
}
