using AutoMapper;
using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
using HG.Entities.Entities;
using HG.Entities.Entities.DanhMuc;
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
            return View(new HoSoModels());
        }    
        
        public async Task<IActionResult> LayLuongThanhPhanByMaTTHC(string ma_thu_tuc)
        {
            var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/LayLuongThanhPhanByMaTTHC.cshtml", lstObj);
            return Content(result);
        }
        [HttpPost]
        public IActionResult TiepNhanHoSoMoi(HoSoModels item, TP_1 tp1, TP_2 tp2, TP_3 tp3, TP_4 tp4, TP_5 tp5, string type_view)
        {
            EAContext db = new EAContext();
            var counths = db.Ho_So.Count();
            item.CreatedDateUtc = DateTime.Now;
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.ma_khach_hang = "0000000" + (counths + 1);
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<HoSoModels, Ho_So>(); });
            var des = config.CreateMapper().Map<HoSoModels, Ho_So>(item);
            var hosoId = 0; 
            if (type_view == StatusAction.View.ToString())
            {
                //lọc insert các thành phần theo thủ tục sau đó insert hồ sơ
                des.trang_thai = 1; //đang tiếp nhận
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;
            }
            else if (type_view == StatusAction.Add.ToString())
            {
                des.trang_thai = 1; //đang tiếp nhận
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;
            }
            else if (type_view == StatusAction.SaveAndTranfer.ToString()) //lưu và chuyển
            {
                des.trang_thai = 2;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;//hs đã chuyển và Đang chờ xử lý và Chuyển chưa xử lý
            }
            else if (type_view == StatusAction.TraKQ.ToString())
            {
                des.trang_thai = 3; //hs đã trả kết quả
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;
            }
            else
            {
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
                ViewBag.LstThuTuc = _danhmucDao.DanhSachThuTuc();
                ViewBag.LstLuongXL = db.Dm_Luong_Xu_Ly.Where(n => n.Deleted != 1).ToList();
                ViewBag.LstNguoiDung = db.AspNetUsers.Where(n => n.Deleted != 1).ToList();
                return View(item);
            }
            //insert thành phần
            if (tp1.ThanhPhan_1 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                dm_thanh_phan obj = new dm_thanh_phan();
                obj.ma_thu_tuc = des.ma_thu_tuc_hc;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp1.ThanhPhan_1;
                obj.ban_goc = tp1.BanChinh1;
                obj.ban_sao = tp1.BanSao1;
                obj.ban_pho_to = tp1.BanPhoto1;
                obj.Status = 0;
                obj.file_dinh_kem = tp1.file_dinh_kem1;
                obj.CreatedDateUtc = DateTime.Now;
                obj.CreatedUid = des.CreatedUid;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_thanh_phan> _role = db.dm_thanh_phan.Add(obj);
                db.SaveChanges();
                //var roleid = _role.Entity.Id;
            };
            if (tp2.ThanhPhan_2 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                dm_thanh_phan obj = new dm_thanh_phan();
                obj.ma_thu_tuc = des.ma_thu_tuc_hc;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp2.ThanhPhan_2;
                obj.ban_goc = tp2.BanChinh2;
                obj.ban_sao = tp2.BanSao2;
                obj.ban_pho_to = tp2.BanPhoto2;
                obj.Status = 0;
                obj.file_dinh_kem = tp2.file_dinh_kem2;
                obj.CreatedDateUtc = DateTime.Now;
                obj.CreatedUid = des.CreatedUid;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_thanh_phan> _role = db.dm_thanh_phan.Add(obj);
                db.SaveChanges();
            };
            if (tp3.ThanhPhan_3 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                dm_thanh_phan obj = new dm_thanh_phan();
                obj.ma_thu_tuc = des.ma_thu_tuc_hc;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp3.ThanhPhan_3;
                obj.ban_goc = tp3.BanChinh3;
                obj.ban_sao = tp3.BanSao3;
                obj.ban_pho_to = tp3.BanPhoto3;
                obj.file_dinh_kem = tp3.file_dinh_kem3;
                obj.CreatedDateUtc = DateTime.Now;
                obj.CreatedUid = des.CreatedUid;
                obj.Status = 0;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_thanh_phan> _role = db.dm_thanh_phan.Add(obj);
                db.SaveChanges();
            };
            if (tp4.ThanhPhan_4 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                dm_thanh_phan obj = new dm_thanh_phan();
                obj.ma_thu_tuc = des.ma_thu_tuc_hc;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp4.ThanhPhan_4;
                obj.ban_goc = tp4.BanChinh4;
                obj.ban_sao = tp4.BanSao4;
                obj.ban_pho_to = tp4.BanPhoto4;
                obj.file_dinh_kem = tp4.file_dinh_kem4;
                obj.CreatedDateUtc = DateTime.Now;
                obj.CreatedUid = des.CreatedUid;
                obj.Status = 0;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_thanh_phan> _role = db.dm_thanh_phan.Add(obj);
                db.SaveChanges();
            };
            if (tp5.ThanhPhan_5 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                dm_thanh_phan obj = new dm_thanh_phan();
                obj.ma_thu_tuc = des.ma_thu_tuc_hc;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp5.ThanhPhan_5;
                obj.ban_goc = tp5.BanChinh5;
                obj.ban_sao = tp5.BanSao5;
                obj.ban_pho_to = tp5.BanPhoto5;
                obj.file_dinh_kem = tp5.file_dinh_kem5;
                obj.CreatedDateUtc = DateTime.Now;
                obj.CreatedUid = des.CreatedUid;
                obj.Status = 0;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_thanh_phan> _role = db.dm_thanh_phan.Add(obj);
                db.SaveChanges();
            };

            if (type_view == StatusAction.View.ToString())
            {
               
                return RedirectToAction("SuaHoSo", new { Id = hosoId });
            }
            else if (type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("TiepNhanHoSoMoi");
            }
            else if (type_view == StatusAction.SaveAndTranfer.ToString()) //lưu và chuyển
            {
                //chuyển hồ sơ về danh sách hồ sơ chờ tiếp nhận
                return RedirectToAction("DanhSachHoSo");
            }
            else if (type_view == StatusAction.TraKQ.ToString())
            {
              //chuyển hồ sơ về danh sách hồ sơ đã trã kết quả
            }
            return RedirectToAction("DanhSachHoSo");
        }




        [HttpGet]
        public IActionResult DanhSachHoSo(string txtSearch = "", int pageSize = 25)
        {
            var currentPage = 1;
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                using (var db = new EAContext())
                {
                    var ds = db.Ho_So.Where(n => n.Deleted == 0 && (n.ten_ho_so ?? "").Contains(txtSearch)).ToList();
                    hs = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    var ds = db.Ho_So.Where(n => n.Deleted != 1).ToList();
                    hs = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                }
            }
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            return View(hs);
        }

        [HttpGet]
        public IActionResult HoSoMoiTiepNhan(Ho_So item, string type_view)
        {
           
            return View(new Ho_So());
        }

        [HttpGet]
        public IActionResult SuaHoSo(int code)
        {
            EAContext db = new EAContext();
            ViewBag.type_view = StatusAction.View.ToString();
            var obj = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            return View(obj);
        }
    }
}
