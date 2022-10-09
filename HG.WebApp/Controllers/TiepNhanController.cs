using AutoMapper;
using HG.Data.Business.DanhMuc;
using HG.Data.Business.NguoiDung;
using HG.Data.Business.ThuTuc;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Model;
using HG.Entities.Entities.ThuTuc;
using HG.Entities.HoSo;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Drawing.Printing;

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
        private readonly NguoiDungDao _nguoiDungDao;
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
            _nguoiDungDao = new NguoiDungDao(DbProvider);
        }
        [HttpGet]
        public async Task<IActionResult> TiepNhanHoSoMoi(string code = "", int id = 0)
        {
            if(id == 0)
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
                ViewBag.LstNguoiDung = _hoso.DanhSachNguoiDung();
                return View(new HoSoModels() { ma_khach_hang = code });
            }
            else
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
                ViewBag.LstNguoiDung = _hoso.DanhSachNguoiDung();
                var obj = db.Ho_So.Where(n => n.Id == id).FirstOrDefault();
                if(obj != null)
                {
                    return View(new HoSoModels() { ma_khach_hang = obj.ma_khach_hang, ho_ten = obj.ho_ten, so_giay_to = obj.so_giay_to, dia_chi = obj.dia_chi, ngay_sinh = obj.ngay_sinh, email = obj.email, dien_thoai = obj.dien_thoai, ten_to_chuc = obj.ten_to_chuc });
                }
                else
                {
                    return View(new HoSoModels() { ma_khach_hang = code });
                }
                
            }
            
        }
        public bool KiemTraMaKH(string code = "")
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(m => m.ma_khach_hang == code).FirstOrDefault();
            return obj == null ? false : true;
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
        public IActionResult GetTPByTTHCInPartial(string ma_thu_tuc, string type, string lstThanhPhan = "", string ma_luong = "",int hosoid = 0)
        {
            EAContext db = new EAContext();
            var lstTP = new List<dm_thanh_phan>();
            var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            //Lấy thêm thành phần hồ sơ đã add tay và view ra ở đây
            var lsttp2 = db.ho_so_thanh_phan.Where(n => n.ho_so_id == hosoid).ToList();
            if(lsttp2 != null)
            {
                foreach(var item in lsttp2)
                {
                    lstTP.Add(new dm_thanh_phan { ma_thanh_phan = item.ma_thanh_phan, ma_thu_tuc = ma_thu_tuc, file_dinh_kem = item.file_dinh_kem, ban_goc = item.ban_goc, ban_sao = item.ban_sao, ban_pho_to = item.ban_pho_to, ten_thanh_phan = item.ten_thanh_phan, id_ho_so_thanh_phan = item.Id });
                };
                lstObj.dm_thanh_phan.AddRange(lstTP);
            }
            ViewBag.view_type = type;
            ViewBag.lstThanhPhan = lstThanhPhan;
            ViewBag.ma_luong = ma_luong;
            return PartialView(lstObj);
        }
        public DmThuTuc GetThuTucByCode(string ma_thu_tuc)
        {
            var lstObj = _danhmucDao.GetThuTucByCode(ma_thu_tuc);

            return lstObj;
        }
        public JsonResult GetNguoiXLNguoiPH(string ma_luong, string ten_buoc = "")
        {
            var lstNguoiXl = _danhmucDao.LayNguoiXLNguoiPHXLByMaLuong(ma_luong, "Tiếp nhận hồ sơ");
            //var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/LayLuongThanhPhanByMaTTHC.cshtml", lstObj);
            return Json(lstNguoiXl == null ? "" : lstNguoiXl);
        }
        [HttpPost]
        public IActionResult TiepNhanHoSoMoi(HoSoModels item, TP_1 tp1, TP_2 tp2, TP_3 tp3, TP_4 tp4, TP_5 tp5, TP_6 tp6, TP_7 tp7, TP_8 tp8, string type_view)
        {
            EAContext db = new EAContext();
            var counths = db.Ho_So.Count();
            item.CreatedDateUtc = DateTime.Now;
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.ma_khach_hang = Helper.HelperString.CheckMaKH((counths + 1).ToString());
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<HoSoModels, Ho_So>(); });
            var des = config.CreateMapper().Map<HoSoModels, Ho_So>(item);
            var hosoId = 0; 
            if (type_view == StatusAction.View.ToString())
            {
                //lọc insert các thành phần theo thủ tục sau đó insert hồ sơ
                des.trang_thai = 1; //đang tiếp nhận
                if (des.ho_so_chua_du_dk_tiep_nhan_chinh_thuc == 1)
                {
                    des.trang_thai = 11;
                }
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;
            }
            else if (type_view == StatusAction.Add.ToString())
            {
                des.trang_thai = 1; //đang tiếp nhận
                if (des.ho_so_chua_du_dk_tiep_nhan_chinh_thuc == 1)
                {
                    des.trang_thai = 11;
                }
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;
            }
            else if (type_view == StatusAction.SaveAndTranfer.ToString()) //lưu và chuyển
            {
                des.trang_thai = 2;
                if (des.ho_so_chua_du_dk_tiep_nhan_chinh_thuc == 1)
                {
                    des.trang_thai = 11;
                }
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;//hs đã chuyển và Đang chờ xử lý và Chuyển chưa xử lý
            }
            else if (type_view == StatusAction.TraKQ.ToString())
            {
                des.trang_thai = 12; //hs đã trả kết quả
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;
            } else if (type_view == StatusAction.Edit.ToString())
            {
                // truong hop la chiunh sua ho so
                des.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                des.UpdatedDateUtc = DateTime.Now;
                db.Entry(des).State = EntityState.Modified;
                db.SaveChanges();
                hosoId = des.Id;
            }
            else
            {
                ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
                ViewBag.LstThuTuc = _danhmucDao.DanhSachThuTuc();
                ViewBag.LstLuongXL = db.Dm_Luong_Xu_Ly.Where(n => n.Deleted != 1).ToList();
                ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(Guid.Parse(userManager.GetUserId(User)));
                return View(item);
            }
            //insert thành phần
            if (tp1.ThanhPhan_1 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                ho_so_thanh_phan obj = new ho_so_thanh_phan();
                obj.ho_so_id = hosoId;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp1.ThanhPhan_1;
                obj.ban_goc = tp1.BanChinh1;
                obj.ban_sao = tp1.BanSao1;
                obj.ban_pho_to = tp1.BanPhoto1;
                obj.Deleted = 0;
                obj.file_dinh_kem = tp1.file_dinh_kem1;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ho_so_thanh_phan> _role = db.ho_so_thanh_phan.Add(obj);
                db.SaveChanges();
                //var roleid = _role.Entity.Id;
            };
            if (tp2.ThanhPhan_2 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                ho_so_thanh_phan obj = new ho_so_thanh_phan();
                obj.ho_so_id = hosoId;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp2.ThanhPhan_2;
                obj.ban_goc = tp2.BanChinh2;
                obj.ban_sao = tp2.BanSao2;
                obj.ban_pho_to = tp2.BanPhoto2;
                obj.Deleted = 0;
                obj.file_dinh_kem = tp2.file_dinh_kem2;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ho_so_thanh_phan> _role = db.ho_so_thanh_phan.Add(obj);
                db.SaveChanges();
            };
            if (tp3.ThanhPhan_3 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                ho_so_thanh_phan obj = new ho_so_thanh_phan();
                obj.ho_so_id = hosoId;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp3.ThanhPhan_3;
                obj.ban_goc = tp3.BanChinh3;
                obj.ban_sao = tp3.BanSao3;
                obj.ban_pho_to = tp3.BanPhoto3;
                obj.file_dinh_kem = tp3.file_dinh_kem3;
                obj.Deleted = 0;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ho_so_thanh_phan> _role = db.ho_so_thanh_phan.Add(obj);
                db.SaveChanges();
            };
            if (tp4.ThanhPhan_4 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                ho_so_thanh_phan obj = new ho_so_thanh_phan();
                obj.ho_so_id = hosoId;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp4.ThanhPhan_4;
                obj.ban_goc = tp4.BanChinh4;
                obj.ban_sao = tp4.BanSao4;
                obj.ban_pho_to = tp4.BanPhoto4;
                obj.file_dinh_kem = tp4.file_dinh_kem4;
                obj.Deleted = 0;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ho_so_thanh_phan> _role = db.ho_so_thanh_phan.Add(obj);
                db.SaveChanges();
            };
            if (tp5.ThanhPhan_5 != null && hosoId != 0)
            {
                var totalrc = db.dm_thanh_phan.Count();
                ho_so_thanh_phan obj = new ho_so_thanh_phan();
                obj.ho_so_id = hosoId;
                obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                obj.ten_thanh_phan = tp5.ThanhPhan_5;
                obj.ban_goc = tp5.BanChinh5;
                obj.ban_sao = tp5.BanSao5;
                obj.ban_pho_to = tp5.BanPhoto5;
                obj.file_dinh_kem = tp5.file_dinh_kem5;
                obj.Deleted = 0;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ho_so_thanh_phan> _role = db.ho_so_thanh_phan.Add(obj);
                db.SaveChanges();
            };
            if (tp6.ThanhPhan_6 != null && hosoId != 0)
            {
                var tp = db.ho_so_thanh_phan.Where(n => n.Id == tp6.id_ho_so_thanh_phan).FirstOrDefault();
                if(tp == null)
                {
                    var totalrc = db.dm_thanh_phan.Count();
                    ho_so_thanh_phan obj = new ho_so_thanh_phan();
                    obj.ho_so_id = hosoId;
                    obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                    obj.ten_thanh_phan = tp6.ThanhPhan_6;
                    obj.ban_goc = tp6.BanChinh6;
                    obj.ban_sao = tp6.BanSao6;
                    obj.ban_pho_to = tp6.BanPhoto6;
                    obj.file_dinh_kem = tp6.file_dinh_kem6;
                    obj.Deleted = 0;
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ho_so_thanh_phan> _role = db.ho_so_thanh_phan.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    tp.ten_thanh_phan = tp6.ThanhPhan_6;
                    tp.ban_goc = tp6.BanChinh6;
                    tp.ban_sao = tp6.BanSao6;
                    tp.ban_pho_to = tp6.BanPhoto6;
                    tp.file_dinh_kem = tp6.file_dinh_kem6;
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
            };
            if (tp7.ThanhPhan_7 != null && hosoId != 0)
            {
                var tp = db.ho_so_thanh_phan.Where(n => n.Id == tp7.id_ho_so_thanh_phan).FirstOrDefault();
                if (tp == null)
                {
                    var totalrc = db.dm_thanh_phan.Count();
                    ho_so_thanh_phan obj = new ho_so_thanh_phan();
                    obj.ho_so_id = hosoId;
                    obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                    obj.ten_thanh_phan = tp7.ThanhPhan_7;
                    obj.ban_goc = tp7.BanChinh7;
                    obj.ban_sao = tp7.BanSao7;
                    obj.ban_pho_to = tp7.BanPhoto7;
                    obj.file_dinh_kem = tp7.file_dinh_kem7;
                    obj.Deleted = 0;
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ho_so_thanh_phan> _role = db.ho_so_thanh_phan.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    tp.ten_thanh_phan = tp7.ThanhPhan_7;
                    tp.ban_goc = tp7.BanChinh7;
                    tp.ban_sao = tp7.BanSao7;
                    tp.ban_pho_to = tp7.BanPhoto7;
                    tp.file_dinh_kem = tp7.file_dinh_kem7;
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();
                }
            };
            if (tp8.ThanhPhan_8 != null && hosoId != 0)
            {
                var tp = db.ho_so_thanh_phan.Where(n => n.Id == tp8.id_ho_so_thanh_phan).FirstOrDefault();
                if (tp == null)
                {
                    var totalrc = db.dm_thanh_phan.Count();
                    ho_so_thanh_phan obj = new ho_so_thanh_phan();
                    obj.ho_so_id = hosoId;
                    obj.ma_thanh_phan = des.ma_thu_tuc_hc + "-" + (totalrc + 1);
                    obj.ten_thanh_phan = tp8.ThanhPhan_8;
                    obj.ban_goc = tp8.BanChinh8;
                    obj.ban_sao = tp8.BanSao8;
                    obj.ban_pho_to = tp8.BanPhoto8;
                    obj.file_dinh_kem = tp8.file_dinh_kem8;
                    obj.Deleted = 0;
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ho_so_thanh_phan> _role = db.ho_so_thanh_phan.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    tp.ten_thanh_phan = tp8.ThanhPhan_8;
                    tp.ban_goc = tp8.BanChinh8;
                    tp.ban_sao = tp8.BanSao8;
                    tp.ban_pho_to = tp8.BanPhoto8;
                    tp.file_dinh_kem = tp8.file_dinh_kem8;
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();
                }
              
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
                return RedirectToAction("HoSoDaTraKetQua", "TraKetQua");
            }
            else if (type_view == StatusAction.Edit.ToString())
            {
                return RedirectToAction("SuaHoSo", new { code = des.Id, type = StatusAction.View.ToString() });
            }
            return RedirectToAction("HoSoDangTiepNhan");
        }

        public async Task<IActionResult> HoSoPaging( int currentPage = 1, string tu_khoa = "", string ma_thu_tuc = "", int tat_ca = 1, int dung_han = 0, int qua_han = 0, int trang_thai = 1, int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { tu_khoa = tu_khoa, CurrentPage = currentPage, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = dung_han, qua_han = qua_han, RecordsPerPage = pageSize, trang_thai_hs = trang_thai, userid = user };
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
            if (trang_thai == 18)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPaging.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 25)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingChoChuyenMotCua.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 23)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingChoKy.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 19)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingDangXuLy.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 20)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingPhoiHop.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 21)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingChuaBoSung.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 22)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingDaChuyenXuLy.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 24)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingDaKy.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 26)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingDaPhoiHop.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 27)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingGanQuaHan.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 28)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingTheoDoiDonDoc.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 29)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingChoChuyenMotCua.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 30)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingXuLyThay.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 31)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoPagingLienThong.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 11)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/Paging/HoSoChoTiepNhanChuaChinhThucPaging.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 2)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/Paging/HoSoChuyenChuaXLPaging.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 8)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/Paging/HoSoChuyenDaXLPaging.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 9)
            {
                HoSoPaging hoSoPaginglt = new HoSoPaging() { tu_khoa = tu_khoa, CurrentPage = currentPage, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = dung_han, qua_han = qua_han, RecordsPerPage = pageSize, trang_thai_hs = 120, userid = user };
                hs = _hoso.HoSoGiaiDoanPaging(hoSoPaginglt, out totalRecored);
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
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/Paging/HoSoLienThongPaging.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 10)
            {
                HoSoPaging hoSoPaginggd2 = new HoSoPaging() { tu_khoa = tu_khoa, CurrentPage = currentPage, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = dung_han, qua_han = qua_han, RecordsPerPage = pageSize, trang_thai_hs = 120, userid = user };
                hs = _hoso.HoSoGiaiDoanPaging(hoSoPaginggd2, out totalRecored);
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
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/Paging/HoSoChoTiepNhanGD2Paging.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 14)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TraKetQua/HoSoPagingChoTraKQ.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 12)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TraKetQua/HoSoPagingDaTraKQ.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 15)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TraKetQua/HoSoPagingCoKQGD1.cshtml", hs);
                return Content(result);
            }
            else if (trang_thai == 1)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/Paging/HoSoDangXLPaging.cshtml", hs);
                return Content(result);
            }
            else
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/HoSoPaging.cshtml", hs);
                return Content(result);
            }
        }
        public async Task<IActionResult> HoSoLienThongPaging(int currentPage = 1, string tu_khoa = "", string ma_thu_tuc = "", int tat_ca = 1, int dung_han = 0, int qua_han = 0, int trang_thai = 1, int pageSize = 25)
        {
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { tu_khoa = tu_khoa, CurrentPage = currentPage, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = dung_han, qua_han = qua_han, RecordsPerPage = pageSize, trang_thai_hs = 120, userid = user };
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
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/HoSoLienThongPaging.cshtml", hs);
            return Content(result);
        }
        public IActionResult HoSoDangTiepNhan(string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var userId = Guid.Parse(userManager.GetUserId(User));
            //hs mới tiếp nhận status = 1
            var currentPage = 1;
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                using (var db = new EAContext())
                {
                    HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 1, userid = userId.ToString() };
                    hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
                    lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
                    totalRecored = hs.Count();
                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 1, userid = userId.ToString() };
                    hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
                    lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
                    totalRecored = hs.Count();
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
            var userId = Guid.Parse(userManager.GetUserId(User));
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1,tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 2, userid = userId.ToString() };
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
            var user = userManager.GetUserId(User);
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoTrucTuyen, userid = user };
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
            var Flag = false;
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _hoso.DanhSachNguoiDung();
            if (type == StatusAction.Send.ToString())
            {
                Flag = true;
                ViewBag.type_view = StatusAction.View.ToString();
                var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
                //Lấy thủ tục bởi mã lv
                ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
                ViewBag.isSend = Flag;
                ViewBag.LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
                return View(hoso);
            }else
            if (type == StatusAction.View.ToString())
            {
                ViewBag.type_view = StatusAction.View.ToString();
                var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
                //Lấy thủ tục bởi mã lv
                ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
                ViewBag.LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
                ViewBag.isSend = Flag;
                return View(hoso);
            }
            else
            {
                ViewBag.type_view = StatusAction.Edit.ToString();
                var obj = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
                ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = obj.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
                ViewBag.LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
                ViewBag.isSend = Flag;
                return View(obj);
            }
        }
        public IActionResult ViewHoSo(int code, string type)
        {
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.type_view = StatusAction.View.ToString();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            ViewBag.LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        } 
        public IActionResult ViewGD2(int code, string type)
        {
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
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
        public IActionResult ViewHoSoDaXL(int code, string type)
        {
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
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
        public IActionResult ViewInPhieu(int code, string type)
        {
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
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
                obj.trang_thai = (int)StatusTiepNhanHoSo.HoSoChuyenChuaXL;
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(Id, "Chuyển đến người xử lý", (int)StatusTiepNhanHoSo.HoSoTrucTuyen, (int)StatusTiepNhanHoSo.HoSoChuyenChuaXL, Guid.Parse(userManager.GetUserId(User)));
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
        public string TiepNhanHoSoGD2(int code)
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            if (obj != null)
            {
                obj.trang_thai = (int)StatusTiepNhanHoSo.HoSoChoTiepNhanGD2;
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                SaveLogHS(code, "Tiếp nhận hồ sơ giai đoạn 2", (int)StatusTiepNhanHoSo.HoSoTrucTuyen, (int)StatusTiepNhanHoSo.HoSoChoTiepNhanGD2, Guid.Parse(userManager.GetUserId(User)));
                return "Tiếp nhận hồ sơ giai đoạn 2 thành công!";
            }
            else
            {
                return "Tiếp nhận hồ sơ giai đoạn 2 lỗi !";
            }
        }
        public IActionResult HoSoChoBoSung(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var user = userManager.GetUserId(User);
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChoBoSung, userid = user };
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
        public IActionResult HoSoChoTiepNhanChuaChinhThuc(int currentPage = 1, string tu_khoa = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var user = userManager.GetUserId(User);
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = tu_khoa, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChoTiepNhanChuaChinhThuc, userid = user };
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
            ViewBag.txtSearch = tu_khoa;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoChuyenDaXL(int currentPage = 1, string tu_khoa = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var user = userManager.GetUserId(User);
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = tu_khoa, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChuyenDaXL, userid = user };
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
            ViewBag.txtSearch = tu_khoa;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }

        public IActionResult HoSoLienThong(int currentPage = 1, string tu_khoa = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var user = userManager.GetUserId(User);
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = tu_khoa, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoLienThong, userid = user };
            hs = _hoso.HoSoLienThongPaging(hoSoPaging, out totalRecored);
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
            ViewBag.txtSearch = tu_khoa;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoChoTiepNhanGD2(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            var user = userManager.GetUserId(User);
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChoTiepNhanGD2, userid = user };
            hs = _hoso.HoSoGiaiDoanPaging(hoSoPaging, out totalRecored);
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
        public IActionResult ViewBoSungHoSo(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
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
        public async Task<IActionResult> TimKiemKhachHangTheoTK(string tukhoa = "",int currentPage = 1)
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var user = userManager.GetUserId(User);
            var totalRecored = 0;
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = tukhoa, ma_thu_tuc = "", tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = 25, trang_thai_hs = (int)StatusTiepNhanHoSo.HoSoChuyenDaXL, userid = user };
            var hs = new List<Ho_So>();
            if (string.IsNullOrEmpty(tukhoa))
            {
                using (var db = new EAContext())
                {
                    var ds = db.Ho_So.Where(n => n.Deleted != 1).ToList();
                    hs = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    var ds = db.Ho_So.Where(n => n.Deleted != 1 && (n.ma_khach_hang.Contains(tukhoa) || (n.ho_ten ?? "").Contains(tukhoa) || (n.so_giay_to ?? "").Contains(tukhoa) || (n.dia_chi ?? "").Contains(tukhoa))).ToList();
                    hs = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                }
            }
           
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TiepNhan/Ajax/TimKiemKhachHangTheoTK.cshtml", hs);
            return Content(result);
        }
    }
}
