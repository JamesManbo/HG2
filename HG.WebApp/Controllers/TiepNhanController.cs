using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
using HG.Entities.Entities;
using HG.Entities.HoSo;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
{
    public class TiepNhanController : BaseController
    {
        
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LuongXuLyDao _danhmucDao;
        public TiepNhanController(IWebHostEnvironment _environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new LuongXuLyDao(DbProvider);
        }
        [HttpGet]
        public async Task<IActionResult> TiepNhanHoSoMoi()
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var UserCreated = await userManager.FindByIdAsync(uid.ToString());
            ViewBag.UserCreated = UserCreated.ho_dem + " " + UserCreated.ten;
            ViewBag.NgayTiepNhan = DateTime.Now;
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstThuTuc = _danhmucDao.DanhSachThuTuc();
            ViewBag.LstLuongXL = db.Dm_Luong_Xu_Ly.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = db.AspNetUsers.Where(n => n.Deleted != 1).ToList();
            return View(new Ho_So());
        }    
        
        public async Task<IActionResult> LayLuongThanhPhanByMaTTHC(string ma_thu_tuc)
        {
            var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/LayLuongThanhPhanByMaTTHC.cshtml", lstObj);
            return Content(result);
        }
        [HttpPost]
        public IActionResult TiepNhanHoSoMoi(HoSoModels item, string type_view)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            if(type_view == StatusAction.View.ToString())
            {
                //lọc insert các thành phần theo thủ tục sau đó insert hồ sơ

                item.trang_thai = 1; //đang tiếp nhận



            }else if(type_view == StatusAction.Add.ToString()) 
            {
                item.trang_thai = 1; //đang tiếp nhận
            }else if (type_view == StatusAction.SaveAndTranfer.ToString()) //lưu và chuyển
            {
                item.trang_thai = 2; //hs đã chuyển và Đang chờ xử lý và Chuyển chưa xử lý
            }
            else if (type_view == StatusAction.TraKQ.ToString())
            {
                item.trang_thai = 3; //hs đã trả kết quả
            }
            else
            {
                return View(item);
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult HoSoMoiTiepNhan(Ho_So item, string type_view)
        {
           
            return View(new Ho_So());
        }
    }
}
