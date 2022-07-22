using AutoMapper;
using HG.Data.Business.DanhMuc;
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
    public class TiepNhanController : BaseController
    {
        
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LuongXuLyDao _danhmucDao;
        private readonly ThuTucDao _thuTucDao;
        private readonly HG.Data.Business.HoSo.HoSoDao _hoso;
        public TiepNhanController(IWebHostEnvironment _environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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

        public async Task<IActionResult> LayLuongThanhPhanByMaTTHC(string ma_thu_tuc, string type, string lstThanhPhan = "", string ma_luong = "")
        {
            var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            ViewBag.view_type = type;
            ViewBag.lstThanhPhan = lstThanhPhan;
            ViewBag.ma_luong = ma_luong;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/LayLuongThanhPhanByMaTTHC.cshtml", lstObj);
            return Content(result);
        } 
        public JsonResult GetNguoiXLNguoiPH(string ma_luong, string ten_buoc = "")
        {
            var lstNguoiXl = _danhmucDao.LayNguoiXLNguoiPHXLByMaLuong(ma_luong, "Tiếp nhận hồ sơ");
            //var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/LayLuongThanhPhanByMaTTHC.cshtml", lstObj);
            return Json(lstNguoiXl);
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
                return RedirectToAction("HoSoDangTiepNhan");
            }
            else if (type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("TiepNhanHoSoMoi");
            }
            else if (type_view == StatusAction.SaveAndTranfer.ToString()) //lưu và chuyển
            {
                //chuyển hồ sơ về danh sách hồ sơ chờ tiếp nhận
                return RedirectToAction("HoSoChuyenChuaXL");
            }
            else if (type_view == StatusAction.TraKQ.ToString())
            {
              //chuyển hồ sơ về danh sách hồ sơ đã trã kết quả
            }
            return RedirectToAction("DanhSachHoSo");
        }

        public async Task<IActionResult> HoSoPaging( int currentPage = 1, string tu_khoa = "", string ma_thu_tuc = "", int tat_ca = 1, int dung_han = 0, int qua_han = 0, int trang_thai = 1, int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = currentPage, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = dung_han, qua_han = qua_han, RecordsPerPage = pageSize, trang_thai_hs = trang_thai };
            hs = _hoso.HoSoPaging(hoSoPaging,out totalRecored);
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
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/HoSoPaging.cshtml", hs);
            return Content(result);
        }
        public IActionResult HoSoDangTiepNhan(string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var currentPage = 1;
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                using (var db = new EAContext())
                {
                    var ds = db.Ho_So.Where(n => n.Deleted != 1 && (n.ten_ho_so ?? "").Contains(txtSearch) && n.trang_thai == 1).ToList();
                    hs = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
                    totalRecored = ds.Count();
                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    var ds = db.Ho_So.Where(n => n.Deleted != 1 && n.trang_thai == 1).ToList();
                    hs = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
                    totalRecored = ds.Count();
                }
            }
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

        public IActionResult HoSoChuyenChuaXL(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1,tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 2 };
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
        public IActionResult HoSoTrucTuyen(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoTrucTuyen };
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
        public IActionResult SuaHoSo(int code, string type)
        {
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = db.AspNetUsers.ToList();
            if (type == StatusAction.View.ToString())
            {
                ViewBag.type_view = StatusAction.View.ToString();
                var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
                //Lấy thủ tục bởi mã lv
                ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
                ViewBag.LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
                return View(hoso);
            }
           
            ViewBag.type_view = StatusAction.Edit.ToString();
            var obj = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            return View(obj);
        }
        public IActionResult ViewHoSo(int code, string type)
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

            return View(hoso);
        }
        public async Task<IActionResult> LayThuTucByLinhVuc(string tu_khoa = "", string ma_linh_vuc = "", int pageSize = 25, string LaTimKiem = "")
        {
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = ma_linh_vuc, tu_khoa = tu_khoa, RecordsPerPage = pageSize };
            var LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
            ViewBag.LaTimKiem = LaTimKiem;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/LayThuTucByLinhVuc.cshtml", LstThuTuc);
            return Content(result);
        }  
        public string TiepNhanHoSoOnline(int code)
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            if (obj != null)
            {
                obj.trang_thai = (int)StatusTiepNhanHoSo.HoSoDangTiepNhan;
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(code, "Tiếp nhận từ hồ sơ online", (int)StatusTiepNhanHoSo.HoSoTrucTuyen, (int)StatusTiepNhanHoSo.HoSoDangTiepNhan, Guid.Parse(userManager.GetUserId(User)));
                return "Tiếp nhận hồ sơ thành công!";
            }
            else
            {
                return "Tiếp nhận hồ sơ lỗi !";
            }
        }
        public string ChuyenCanBoXuLy(int Id)
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == Id).FirstOrDefault();
            if (obj != null)
            {
                obj.trang_thai = (int)StatusTiepNhanHoSo.HoSoChoBoSung;
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(Id, "Chuyển đến người xử lý", (int)StatusTiepNhanHoSo.HoSoTrucTuyen, (int)StatusTiepNhanHoSo.HoSoChoBoSung, Guid.Parse(userManager.GetUserId(User)));
                return "Chuyển đến người xử lý!";
            }
            else
            {
                return "Chuyển đến người xử lý lỗi !";
            }
        }
        public string LoaiHoSo(int code)
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            if (obj != null)
            {
                obj.trang_thai = (int)StatusTiepNhanHoSo.HoSoBiThuHoi;
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(code, "Loại Hồ Sơ", (int)StatusTiepNhanHoSo.HoSoTrucTuyen, (int)StatusTiepNhanHoSo.HoSoBiThuHoi, Guid.Parse(userManager.GetUserId(User)));
                return "Loại hồ sơ thành công!";
            }
            else
            {
                return "Loại hồ sơ lỗi !";
            }
        }
        public IActionResult HoSoChoBoSung(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChoBoSung };
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
        public IActionResult HoSoChoTiepNhanChuaChinhThuc(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChoTiepNhanChuaChinhThuc };
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
        public IActionResult HoSoChuyenDaXL(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChuyenDaXL };
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

        public IActionResult HoSoLienThong(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoLienThong };
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
        public IActionResult HoSoChoTiepNhanGD2(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChoTiepNhanGD2 };
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
        //public IActionResult HoSoTraKQ(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        //{
        //    var totalRecored = 0;
        //    var hs = new List<Ho_So>();
        //    var lv = new List<Dm_Linh_Vuc>();
        //    HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.Tra };
        //    hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
        //    using (var db = new EAContext())
        //    {
        //        lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
        //    };
        //    ViewBag.LstLinhVuc = lv;
        //    ViewBag.CurrentPage = 1;
        //    ViewBag.TotalRecored = totalRecored;
        //    ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
        //    ViewBag.CurrentPage = currentPage;
        //    ViewBag.RecoredFrom = 1;
        //    ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
        //    ViewBag.txtSearch = txtSearch;
        //    ViewBag.MaLinhVuc = ma_linh_vuc;
        //    ViewBag.MaThuTuc = ma_thu_tuc;
        //    return View(hs);
        //}
    }
}
