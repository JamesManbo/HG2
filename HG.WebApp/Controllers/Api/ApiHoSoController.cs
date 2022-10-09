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
            var user = userManager.GetUserId(User);
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = tu_khoa, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChoBoSung, userid = user };
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
                obj.trang_thai = (int)StatusXuLyHoSo.HoSoChoXuLy;
                obj.id_trang_thai_xl = 0; //chưa tiếp nhận
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(ho_so_id, "Hồ sơ chuyển qua chờ xử lý", (int)StatusTiepNhanHoSo.HoSoDangTiepNhan, (int)StatusXuLyHoSo.HoSoChoXuLy, Guid.Parse(userManager.GetUserId(User)));
                return "Chuyển xử lý hồ sơ thành công!";
            }
            else
            {
                return "Chuyển xử lý hồ sơ lỗi !";
            }
           
        } 
        public string TiepNhanChinhThuc(int ho_so_id)
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == ho_so_id).FirstOrDefault();
            if (obj != null)
            {
                obj.trang_thai = (int)StatusTiepNhanHoSo.HoSoDangTiepNhan;
                obj.id_trang_thai_xl = 0; //chưa tiếp nhận
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                obj.ho_so_chua_du_dk_tiep_nhan_chinh_thuc = 0;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(ho_so_id, "Hồ sơ chuyển qua đang tiếp nhận", (int)StatusTiepNhanHoSo.HoSoChoTiepNhanChuaChinhThuc, (int)StatusTiepNhanHoSo.HoSoDangTiepNhan, Guid.Parse(userManager.GetUserId(User)));
                return "Chuyển đang tiếp nhận hồ sơ thành công!";
            }
            else
            {
                return "Chuyển đang tiếp nhận hồ sơ lỗi !";
            }
           
        }
        public string TraKetQua(int ho_so_id,  string NguoiKy, int LePhi = 0 , int DaThuPhi = 0, string filedinhkem = "", string YKien = "")
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == ho_so_id).FirstOrDefault();
            if (obj != null)
            {
                obj.da_thu_phi = DaThuPhi;
                obj.le_phi = LePhi;
                obj.id_file_dinh_kem = filedinhkem;
                obj.y_kien = YKien;
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
