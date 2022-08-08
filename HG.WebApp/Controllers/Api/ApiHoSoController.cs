using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
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
