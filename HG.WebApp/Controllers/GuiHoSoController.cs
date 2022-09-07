using HG.Data.Business.DanhMuc;
using HG.Data.Business.GuiHoSo;
using HG.Data.Business.ThuTuc;
using HG.Entities;
using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.Model;
using HG.Entities.HoSo;
using HG.WebApp.Data;
using HG.WebApp.Entities;
using HG.WebApp.Helper;
using HG.WebApp.Models.DanhMuc;
using HG.WebApp.Sercurity;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using static HG.Data.Business.GuiHoSo.TuyenSinhCapMamNonDao;
using static HG.Data.Business.GuiHoSo.TuyenSinhCapTHCSDao;
using static HG.Data.Business.GuiHoSo.TuyenSinhCapTHPTDao;
using static HG.Data.Business.GuiHoSo.TuyenSinhCapTieuHocDao;

namespace HG.WebApp.Controllers
{
    //[Authorize]
    public class GuiHoSoController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly DanhMucDao _danhmucDao;
        private readonly TuyenSinhCapMamNonDao _tuyensinhcapmamnonDao;
        private readonly TuyenSinhCapTieuHocDao _tuyensinhcaptieuhocDao;
        private readonly TuyenSinhCapTHCSDao _tuyensinhcapthcsDao;
        private readonly TuyenSinhCapTHPTDao _tuyensinhcapthptDao;
        private readonly ThuTucDao _thuTucDao;
        //extend sys identity
        public GuiHoSoController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new DanhMucDao(DbProvider);
            _tuyensinhcapmamnonDao = new TuyenSinhCapMamNonDao(DbProvider);
            _tuyensinhcaptieuhocDao = new TuyenSinhCapTieuHocDao(DbProvider);
            _tuyensinhcapthcsDao = new TuyenSinhCapTHCSDao(DbProvider);
            _tuyensinhcapthptDao = new TuyenSinhCapTHPTDao(DbProvider);
            _thuTucDao = new ThuTucDao(DbProvider);
        }

        #region Gửi hồ sơ cấp Mầm mon
        public IActionResult TuyenSinhCapMamNon()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var phongBan = new List<Ghs_Tuyen_Sinh_Cap_Mam_Non>();
            using (var db = new EAContext())
            {
                var ds = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                phongBan = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.lst_phong_ban = phongBan;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/TuyenSinhCapMamNon.cshtml", phongBan);

        }

        public async Task<IActionResult> TuyenSinhCapMamNonPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ma_phong_ban.ToUpper().Contains(tu_khoa.ToUpper()) || n.ten_phong_ban.ToUpper().Contains(tu_khoa.ToUpper())).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/GuiHoSo/TuyenSinhCapMamNon/PhongBaTuyenSinhCapMamNonPagingnPaging.cshtml", datapage);

            }
            return Content(result);
        }
        [HttpPost]
        public JsonResult CheckMaTuyenSinhCapMamNon(string code)
        {
            var lstpb = new List<Ghs_Tuyen_Sinh_Cap_Mam_Non>();
            using (var db = new EAContext())
            {
                lstpb = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0 && n.ma_ho_so.ToUpper() == code.ToUpper()).ToList();
            }
            if (lstpb.Count() == 0)
            {
                return Json(new { error = 0, href = "/GuiHoSo/ThemTuyeSinhCapMamNon?code=" + code.ToUpper() });
            }
            return Json(new { error = 1, href = "/GuiHoSo/SuaTuyenSinhCapMamNon?code=" + code.ToUpper() + "&type=" + StatusAction.Edit.ToString() });
        }
        [HttpPost]
        public IActionResult UpdateNguoiDung(AspNetUsers obj_user)
        {
            var user_id = userManager.GetUserId(User);


            var modal = new Ghs_Tuyen_Sinh_Cap_Mam_Non();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstpb = new List<Ghs_Tuyen_Sinh_Cap_Mam_Non>();
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            using (var db = new EAContext())
            {
                if(user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                    nguoi_dung.ten = obj_user.ten;
                    nguoi_dung.PhoneNumber = obj_user.PhoneNumber;
                    nguoi_dung.ngay_sinh = obj_user.ngay_sinh;
                    nguoi_dung.Email = obj_user.Email;
                    nguoi_dung.ma_phong_ban = obj_user.ma_phong_ban;
                    nguoi_dung.ma_chuc_vu = obj_user.ma_chuc_vu;
                    db.AspNetUsers.Update(nguoi_dung);
                    db.SaveChanges();
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }
                lstpb = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
            }

            ViewBag.lst_nguoi_dung = user;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
        }
        public IActionResult ThemTuyenSinhCapMamNon(string code = "")
        {
            var user_id = userManager.GetUserId(User);
            var modal = new Ghs_Tuyen_Sinh_Cap_Mam_Non();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstpb = new List<Ghs_Tuyen_Sinh_Cap_Mam_Non>();
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);

                }
                lstpb = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
            }

            ViewBag.lst_nguoi_dung = user;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.code = code;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();

            return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
        }

        [HttpPost]
        public IActionResult ThemTuyenSinhCapMamNon(TuyenSinhCapMamNonModel item)
        {
            #region ---- Thông tin form ------
            var user_id = userManager.GetUserId(User);
            var modal = new Ghs_Tuyen_Sinh_Cap_Mam_Non();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);

                }
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();

                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
            }
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            #endregion -----------------------
            if (item.chkIsCamKet != "on")
            {
                ViewBag.error = true;
                ViewBag.Message = "Vui lòng cam kết khai đúng thông tin!";
                return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
            }
            if (item.filesName_0 == null || item.filesName_1 == null) 
            {
                ViewBag.error = true;
                ViewBag.Message = "Chưa có file đính kèm hoặc file đính kèm bị lỗi!";
                return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
            }
            var ds_hs = HG.WebApp.Helper.HelperString.ListThanhPhanHoSoRecords().OrderBy(x=>x.stt).ToList();
            EAContext eAContext = new EAContext();
            var data = _tuyensinhcapmamnonDao.mapdata(item);
            if (user_id != null)
            {
                data.CreatedUid = Guid.Parse(user_id);

            }
            ViewBag.UidName = User.Identity.Name;
            data.Deleted = 0;
            data.CreatedDateUtc = DateTime.Now;
            //var obj = eAContext.dm_don_vi.Where(n => n.ma_don_vi == item.ma_don_vi).Count();
            //if (obj > 0)
            //{
            //    ViewBag.ErrorCode = 1;
            //    ViewBag.ErrorMsg = "Mã đã tồn tại trong hệ thống";
            //    return View(item);
            //};
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ghs_Tuyen_Sinh_Cap_Mam_Non> _role = eAContext.Ghs_Tuyen_Sinh_Cap_Mam_Non.Add(data);
            eAContext.SaveChanges();
            var roleid = _role.Entity.Id;

            for (int i = 0; i < ds_hs.Count(); i++)
            {
                var obj_hs = new Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso();
                obj_hs.ten_thanh_phan = ds_hs[i].ten_thanh_phan;
                if (i==0)
                {
                    obj_hs.file_dinh_kem = item.filesName_0;
                }
                if (i==1)
                {
                    obj_hs.file_dinh_kem = item.filesName_1;
                }
                var name = "filesName_" + i;
                obj_hs.bieu_mau = ds_hs[i].bieu_mau;
                obj_hs.ban_chinh = ds_hs[i].ban_chinh;
                obj_hs.ban_sao = ds_hs[i].ban_sao;
                obj_hs.ban_photo = ds_hs[i].ban_photo;
                obj_hs.id_ghs_tuyen_sinh_cap_mam_non = roleid;
                obj_hs.Stt = ds_hs[0].stt;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso> _role_hs = eAContext.Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso.Add(obj_hs);
            }
            eAContext.SaveChanges();

            ViewBag.Success = true;
            ViewBag.Message = "Gửi hồ sơ thành công!";

            
            return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
           
        }
        public async Task<IActionResult> LayHuyenTheoTinh(string ma_dia_ban_cha, string noi_dung)
        {
            EAContext db = new EAContext();
            var LstDiaBan = new List<dm_dia_ban>();
            LstDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_dia_ban_cha == ma_dia_ban_cha).ToList();
            var result = "";
            if (noi_dung == "huyen_noi_sinh")
            {
                 result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/GuiHoSo/TuyenSinhCapMamNon/DiaBanHuyenNoiSinh.cshtml", LstDiaBan);
            }
            if (noi_dung == "huyen_noi_cu_tru")
            {
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/GuiHoSo/TuyenSinhCapMamNon/DiaBanHuyenNoiCuTru.cshtml", LstDiaBan);
            }
            if (noi_dung == "xa_noi_cu_tru")
            {
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/GuiHoSo/TuyenSinhCapMamNon/DiaBanXaNoiCuTru.cshtml", LstDiaBan);
            }
            return Content(result);
            //return LstDiaBan;
        }
        public IActionResult SuaTuyenSinhCapMamNon(string code, string type)
        {
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstpb = new List<Ghs_Tuyen_Sinh_Cap_Mam_Non>();
            var lstdv = new List<dm_don_vi>();
            using (var db = new EAContext())
            {
                lstpb = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
            }
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_phong_ban = lstpb;
            ViewBag.lst_don_vi = lstdv;
            var pb = lstpb.Where(n => n.ma_ho_so == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/SuaTuyenSinhCapMamNon.cshtml", pb);
        }

        [HttpPost]
        public IActionResult SuaTuyenSinhCapMamNon(string code, string type, TuyenSinhCapMamNonModel item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _tuyensinhcapmamnonDao.Luu(item);
            if (_pb.ErrorCode > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = _pb.ErrorMsg;
                var user = _danhmucDao.DanhSachNguoiDung("");
                var lstpb = new List<Ghs_Tuyen_Sinh_Cap_Mam_Non>();
                using (var db = new EAContext())
                {
                    lstpb = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
                ViewBag.lst_phong_ban = lstpb;
                ViewBag.lst_nguoi_dung = user;
                return PartialView("~/Views/GuiHoSo/TuyenSinhCapMamNon/SuaTuyenSinhCapMamNon.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemTuyenSinhCapMamNon", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaTuyenSinhCapMamNon", "GuiHoSo", new { code = item.ma_ho_so, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaTuyenSinhCapMamNon(string code = "")
        {
            if (String.IsNullOrEmpty(code))
            {
                return Json(new { error = 1, msg = "Bạn cần chọn mã để xóa" });
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaPhongBan(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/PhongBan" });
        }

        public async Task<IActionResult> RenderViewTuyenSinhCapMamNon()
        {
            var lstpb = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/Phongban/ViewPhongBan.cshtml", lstpb);
            return Content(result);
        }
        #endregion


        #region ------ Gửi hồ sơ cấp tiểu học ----------
        [HttpPost]
        public IActionResult UpdateNguoiDungCapTieuHoc(AspNetUsers obj_user)
        {
            var user_id = userManager.GetUserId(User);

            var modal = new Ghs_Tuyen_Sinh_Cap_Tieu_Hoc();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstpb = new List<Ghs_Tuyen_Sinh_Cap_Tieu_Hoc>();
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                    nguoi_dung.ten = obj_user.ten;
                    nguoi_dung.PhoneNumber = obj_user.PhoneNumber;
                    nguoi_dung.ngay_sinh = obj_user.ngay_sinh;
                    nguoi_dung.Email = obj_user.Email;
                    nguoi_dung.ma_phong_ban = obj_user.ma_phong_ban;
                    nguoi_dung.ma_chuc_vu = obj_user.ma_chuc_vu;
                    db.AspNetUsers.Update(nguoi_dung);
                    db.SaveChanges();
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }
                lstpb = db.Ghs_Tuyen_Sinh_Cap_Tieu_Hoc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            return View("~/Views/GuiHoSo/TuyenSinhCapTieuHoc/GuiTuyenSinhCapTieuHoc.cshtml", modal);
        }
        public IActionResult GuiTuyenSinhCapTieuHoc(string code = "")
        {
            var user_id = userManager.GetUserId(User);


            var modal = new Ghs_Tuyen_Sinh_Cap_Tieu_Hoc();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstpb = new List<Ghs_Tuyen_Sinh_Cap_Tieu_Hoc>();
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }
                lstpb = db.Ghs_Tuyen_Sinh_Cap_Tieu_Hoc.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.code = code;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            return View("~/Views/GuiHoSo/TuyenSinhCapTieuHoc/GuiTuyenSinhCapTieuHoc.cshtml", modal);
        }

        [HttpPost]
        public IActionResult GuiTuyenSinhCapTieuHoc(TuyenSinhCapTieuHocModel item)
        {
            var user_id = userManager.GetUserId(User);

            var modal = new Ghs_Tuyen_Sinh_Cap_Tieu_Hoc();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }

                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            if (item.chkIsCamKet != "on")
            {
                ViewBag.error = true;
                ViewBag.Message = "Vui lòng cam kết khai đúng thông tin!";
                return View("~/Views/GuiHoSo/TuyenSinhCapTieuHoc/GuiTuyenSinhCapTieuHoc.cshtml", modal);
            }
            if (item.filesName_0 == null || item.filesName_1 == null)
            {
                ViewBag.error = true;
                ViewBag.Message = "Chưa có file đính kèm hoặc file đính kèm bị lỗi!";
                return View("~/Views/GuiHoSo/TuyenSinhCapTieuHoc/GuiTuyenSinhCapTieuHoc.cshtml", modal);
            }

            var ds_hs = HG.WebApp.Helper.HelperString.ListThanhPhanHoSoCapTieuHocRecords().OrderBy(x => x.stt).ToList();
            EAContext eAContext = new EAContext();
            var data = _tuyensinhcaptieuhocDao.mapdata(item);
            if (user_id != null)
            {
                data.CreatedUid = Guid.Parse(user_id);
                ViewBag.UidName = User.Identity.Name;
            }
            data.Deleted = 0;
            data.CreatedDateUtc = DateTime.Now;
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ghs_Tuyen_Sinh_Cap_Tieu_Hoc> _role = eAContext.Ghs_Tuyen_Sinh_Cap_Tieu_Hoc.Add(data);
            eAContext.SaveChanges();
            var roleid = _role.Entity.Id;

            for (int i = 0; i < ds_hs.Count(); i++)
            {
                var obj_hs = new Ghs_Tuyen_Sinh_Cap_Tieu_Hoc_Hoso();
                obj_hs.ten_thanh_phan = ds_hs[i].ten_thanh_phan;
                if (i == 0)
                {
                    obj_hs.file_dinh_kem = item.filesName_0;
                }
                if (i == 1)
                {
                    obj_hs.file_dinh_kem = item.filesName_1;
                }
                var name = "filesName_" + i;
                obj_hs.bieu_mau = ds_hs[i].bieu_mau;
                obj_hs.ban_chinh = ds_hs[i].ban_chinh;
                obj_hs.ban_sao = ds_hs[i].ban_sao;
                obj_hs.ban_photo = ds_hs[i].ban_photo;
                obj_hs.id_ghs_tuyen_sinh_cap_tieu_hoc = roleid;
                obj_hs.Stt = ds_hs[0].stt;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ghs_Tuyen_Sinh_Cap_Tieu_Hoc_Hoso> _role_hs = eAContext.Ghs_Tuyen_Sinh_Cap_Tieu_Hoc_Hoso.Add(obj_hs);
            }
            eAContext.SaveChanges();

            ViewBag.Success = true;
            ViewBag.Message = "Gửi hồ sơ thành công!";
            
            return View("~/Views/GuiHoSo/TuyenSinhCapTieuHoc/GuiTuyenSinhCapTieuHoc.cshtml", modal);
            //return View(item);

        }
        #endregion -------------------------------------

        #region ------- Gửi hồ sơ cấp THCS -------------
        [HttpPost]
        public IActionResult UpdateNguoiDungCapTHCS(AspNetUsers obj_user)
        {

            var user_id = userManager.GetUserId(User);


            var modal = new Ghs_Tuyen_Sinh_Cap_THCS();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                    nguoi_dung.ten = obj_user.ten;
                    nguoi_dung.PhoneNumber = obj_user.PhoneNumber;
                    nguoi_dung.ngay_sinh = obj_user.ngay_sinh;
                    nguoi_dung.Email = obj_user.Email;
                    nguoi_dung.ma_phong_ban = obj_user.ma_phong_ban;
                    nguoi_dung.ma_chuc_vu = obj_user.ma_chuc_vu;
                    db.AspNetUsers.Update(nguoi_dung);
                    db.SaveChanges();
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            return View("~/Views/GuiHoSo/TuyenSinhCapTHCS/GuiTuyenSinhCapTHCS.cshtml", modal);
        }
        public IActionResult GuiTuyenSinhCapTHCS(string code = "")
        {
            var user_id = userManager.GetUserId(User);


            var modal = new Ghs_Tuyen_Sinh_Cap_THCS();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.code = code;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            return View("~/Views/GuiHoSo/TuyenSinhCapTHCS/GuiTuyenSinhCapTHCS.cshtml", modal);
        }

        [HttpPost]
        public IActionResult GuiTuyenSinhCapTHCS(TuyenSinhCapTHCSModel item)
        {

            var user_id = userManager.GetUserId(User);
            var modal = new Ghs_Tuyen_Sinh_Cap_THCS();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }

                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();

            if (item.chkIsCamKet != "on")
            {
                ViewBag.error = true;
                ViewBag.Message = "Vui lòng cam kết khai đúng thông tin!";
                return View("~/Views/GuiHoSo/TuyenSinhCapTHCS/GuiTuyenSinhCapTHCS.cshtml", modal);
            }
            if (item.filesName_0 == null || item.filesName_1 == null)
            {
                ViewBag.error = true;
                ViewBag.Message = "Chưa có file đính kèm hoặc file đính kèm bị lỗi!";
                return View("~/Views/GuiHoSo/TuyenSinhCapTHCS/GuiTuyenSinhCapTHCS.cshtml", modal);
            }

            var ds_hs = HG.WebApp.Helper.HelperString.ListThanhPhanHoSoCapTHCSRecords().OrderBy(x => x.stt).ToList();
            EAContext eAContext = new EAContext();
            var data = _tuyensinhcapthcsDao.mapdata(item);
            if (user_id != null)
            {
                data.CreatedUid = Guid.Parse(user_id);
                ViewBag.UidName = User.Identity.Name;
            }
            data.Deleted = 0;
            data.CreatedDateUtc = DateTime.Now;
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ghs_Tuyen_Sinh_Cap_THCS> _role = eAContext.Ghs_Tuyen_Sinh_Cap_THCS.Add(data);
            eAContext.SaveChanges();
            var roleid = _role.Entity.Id;

            for (int i = 0; i < ds_hs.Count(); i++)
            {
                var obj_hs = new Ghs_Tuyen_Sinh_Cap_THCS_Hoso();
                obj_hs.ten_thanh_phan = ds_hs[i].ten_thanh_phan;
                if (i == 0) { obj_hs.file_dinh_kem = item.filesName_0; }
                if (i == 1) { obj_hs.file_dinh_kem = item.filesName_1; }
                if (i == 2){ obj_hs.file_dinh_kem = item.filesName_2; }
                if (i == 3) { obj_hs.file_dinh_kem = item.filesName_3; }
                var name = "filesName_" + i;
                obj_hs.bieu_mau = ds_hs[i].bieu_mau;
                obj_hs.ban_chinh = ds_hs[i].ban_chinh;
                obj_hs.ban_sao = ds_hs[i].ban_sao;
                obj_hs.ban_photo = ds_hs[i].ban_photo;
                obj_hs.id_ghs_tuyen_sinh_cap_thcs = roleid;
                obj_hs.Stt = ds_hs[0].stt;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ghs_Tuyen_Sinh_Cap_THCS_Hoso> _role_hs = eAContext.Ghs_Tuyen_Sinh_Cap_THCS_Hoso.Add(obj_hs);
            }
            eAContext.SaveChanges();

            ViewBag.Success = true;
            ViewBag.Message = "Gửi hồ sơ thành công!";
            
            return View("~/Views/GuiHoSo/TuyenSinhCapTHCS/GuiTuyenSinhCapTHCS.cshtml", modal);
            //return View(item);

        }
        #endregion -------------------------------------

        #region ------- Gửi hồ sơ cấp THPT -------------
        [HttpPost]
        public IActionResult UpdateNguoiDungCapTHPT(AspNetUsers obj_user)
        {

            var user_id = userManager.GetUserId(User);


            var modal = new Ghs_Tuyen_Sinh_Cap_THPT();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                    nguoi_dung.ten = obj_user.ten;
                    nguoi_dung.PhoneNumber = obj_user.PhoneNumber;
                    nguoi_dung.ngay_sinh = obj_user.ngay_sinh;
                    nguoi_dung.Email = obj_user.Email;
                    nguoi_dung.ma_phong_ban = obj_user.ma_phong_ban;
                    nguoi_dung.ma_chuc_vu = obj_user.ma_chuc_vu;
                    db.AspNetUsers.Update(nguoi_dung);
                    db.SaveChanges();
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            return View("~/Views/GuiHoSo/TuyenSinhCapTHPT/GuiTuyenSinhCapTHPT.cshtml", modal);
        }

        public IActionResult GuiTuyenSinhCapTHPT(string code = "")
        {
            var user_id = userManager.GetUserId(User);


            var modal = new Ghs_Tuyen_Sinh_Cap_THPT();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.code = code;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            return View("~/Views/GuiHoSo/TuyenSinhCapTHPT/GuiTuyenSinhCapTHPT.cshtml", modal);
        }

        [HttpPost]
        public IActionResult GuiTuyenSinhCapTHPT(TuyenSinhCapTHPTModel item)
        {
            var user_id = userManager.GetUserId(User);
            var modal = new Ghs_Tuyen_Sinh_Cap_THPT();
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstdv = new List<dm_don_vi>();
            var lst_dantoc = new List<Dm_Dan_Toc>();
            var lst_tinh = new List<dm_dia_ban>();
            var nguoi_dung = new AspNetUsers();
            var ds_phongban = new List<Dm_Phong_Ban>();
            var ds_chucvu = new List<Dm_Chuc_Vu>();
            var ds_gioitinh = new List<Dm_Gioi_Tinh>();
            using (var db = new EAContext())
            {
                if (user_id != null)
                {
                    var userId = Guid.Parse(user_id);
                    nguoi_dung = db.AspNetUsers.Find(userId);
                }

                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                ds_phongban = db.Dm_Phong_Ban.ToList();
                ds_chucvu = db.Dm_Chuc_Vu.ToList();
                ds_gioitinh = db.Dm_Gioi_Tinh.ToList();
            }
            ViewBag.ds_gioi_tinh = ds_gioitinh;
            ViewBag.nguoi_dung = nguoi_dung;
            ViewBag.lst_phong_ban = ds_phongban;
            ViewBag.lst_chuc_vu = ds_chucvu;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_don_vi = lstdv;
            ViewBag.lst_dan_toc = lst_dantoc;
            ViewBag.lst_tinh = lst_tinh;
            ViewBag.nam_hoc = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();

            if (item.chkIsCamKet != "on")
            {
                ViewBag.error = true;
                ViewBag.Message = "Vui lòng cam kết khai đúng thông tin!";
                return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
            }
            if (item.filesName_0 == null || item.filesName_1 == null)
            {
                ViewBag.error = true;
                ViewBag.Message = "Chưa có file đính kèm hoặc file đính kèm bị lỗi!";
                return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
            }

            var ds_hs = HG.WebApp.Helper.HelperString.ListThanhPhanHoSoCapTHPTRecords().OrderBy(x => x.stt).ToList();
            EAContext eAContext = new EAContext();
            var data = _tuyensinhcapthptDao.mapdata(item);
            if (user_id != null)
            {
                data.CreatedUid = Guid.Parse(user_id);
                ViewBag.UidName = User.Identity.Name;
            }
            data.Deleted = 0;
            data.CreatedDateUtc = DateTime.Now;
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ghs_Tuyen_Sinh_Cap_THPT> _role = eAContext.Ghs_Tuyen_Sinh_Cap_THPT.Add(data);
            eAContext.SaveChanges();
            var roleid = _role.Entity.Id;

            for (int i = 0; i < ds_hs.Count(); i++)
            {
                var obj_hs = new Ghs_Tuyen_Sinh_Cap_THPT_Hoso();
                obj_hs.ten_thanh_phan = ds_hs[i].ten_thanh_phan;
                if (i == 0) { obj_hs.file_dinh_kem = item.filesName_0; }
                if (i == 1) { obj_hs.file_dinh_kem = item.filesName_1; }
                if (i == 2) { obj_hs.file_dinh_kem = item.filesName_2; }
                if (i == 3) { obj_hs.file_dinh_kem = item.filesName_3; }
                if (i == 4) { obj_hs.file_dinh_kem = item.filesName_4; }
                if (i == 5) { obj_hs.file_dinh_kem = item.filesName_5; }
                var name = "filesName_" + i;
                obj_hs.bieu_mau = ds_hs[i].bieu_mau;
                obj_hs.ban_chinh = ds_hs[i].ban_chinh;
                obj_hs.ban_sao = ds_hs[i].ban_sao;
                obj_hs.ban_photo = ds_hs[i].ban_photo;
                obj_hs.id_ghs_tuyen_sinh_cap_thpt = roleid;
                obj_hs.Stt = ds_hs[0].stt;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ghs_Tuyen_Sinh_Cap_THPT_Hoso> _role_hs = eAContext.Ghs_Tuyen_Sinh_Cap_THPT_Hoso.Add(obj_hs);
            }
            eAContext.SaveChanges();

            ViewBag.Success = true;
            ViewBag.Message = "Gửi hồ sơ thành công!";
           
            return View("~/Views/GuiHoSo/TuyenSinhCapTHPT/GuiTuyenSinhCapTHPT.cshtml", modal);
            //return View(item);

        }
        #endregion -------------------------------------

        #region gửi hồ sơ một cửa
        public IActionResult MotCua()
        {
            EAContext db = new EAContext();
            ViewBag.ListDonVi = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
            ViewBag.ListLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstMucDo = db.Dm_Muc_Do_Thuc_Hien.Where(n => n.ma_thuc_hien != "MD1" && n.ma_thuc_hien != "MD2").ToList();
            return View();
        }
        public async Task<IActionResult> CheckSearchDV(string donvi = "", string linhvuc = "", string mucdo = "", string ten_thu_tuc = "", string ma_thu_tuc = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = "", tu_khoa = "", RecordsPerPage = pageSize };
            var LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
            ViewBag.TotalRecords = LstThuTuc.lstThuTuc.Count();
            ViewBag.TotalPage = (LstThuTuc.lstThuTuc.Count() / pageSize) + 1;
            ViewBag.CurrentPage = 1;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/GuiHoSo/CheckSearchDV.cshtml", LstThuTuc.lstThuTuc);
            return Content(result);
        }
        public IActionResult GuiHoSoMotCua(string Ma = "")
        {
            EAContext db = new EAContext();
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = "", tu_khoa = "", RecordsPerPage = pageSize };
            var LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
            var obj = new HG.Entities.Entities.ThuTuc.DmThuTuc();
            var lstThanhPhan = new List<dm_thanh_phan>();
            if (LstThuTuc != null && LstThuTuc.lstThuTuc != null)
            {
                obj = LstThuTuc.lstThuTuc.Where(n => n.ma_thu_tuc == Ma).FirstOrDefault();
                lstThanhPhan = db.dm_thanh_phan.Where(n => n.ma_thu_tuc == Ma).ToList();
            };
            ViewBag.MaKH = "00000" + db.Ho_So.Count() + 1;
            ViewBag.ListThanhPhan = lstThanhPhan;
            ViewBag.MaThuTuc = Ma;
            var user = db.AspNetUsers.Where(n => n.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.sodienthoai = user == null ? "098xx": user.PhoneNumber;
            return View(obj);
        }
        [HttpPost]
        public IActionResult GuiHoSoMotCua(cd_hoso hoso)
        {
            EAContext db = new EAContext();
            var user_obj = db.AspNetUsers.Where(n => n.UserName == User.Identity.Name).FirstOrDefault();
            var user_id = Guid.Parse(userManager.GetUserId(User));
            var hosoId = 0;
            var lstThanhPhan = new List<dm_thanh_phan>();
            //lấy lại thủ tục vừa chọn có thể người dùng tạo hs mới
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = "", tu_khoa = "", RecordsPerPage = pageSize };
            var LstThuTuc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
            var obj = new HG.Entities.Entities.ThuTuc.DmThuTuc();
            if (LstThuTuc != null && LstThuTuc.lstThuTuc != null)
            {
                obj = LstThuTuc.lstThuTuc.Where(n => n.ma_thu_tuc == hoso.ma_thu_tuc_hc).FirstOrDefault();
                lstThanhPhan = db.dm_thanh_phan.Where(n => n.ma_thu_tuc == hoso.ma_thu_tuc_hc).ToList();
            };
            try
            {
                Ho_So des = new Ho_So();
                des.type = (int)TypeHS.CongDan;
                des.trang_thai = (int)StatusTiepNhanHoSo.HoSoTrucTuyen;
                des.nguoi_xu_ly = user_id.ToString();
                des.CreatedUid = user_id;
                des.ho_ten = user_obj == null ? "" : user_obj.UserName;
                des.ma_khach_hang = hoso.ma_khach_hang;
                des.ma_thu_tuc_hc = hoso.ma_thu_tuc_hc;
                des.dia_chi = "";
                des.ma_linh_vuc = hoso.ma_linh_vuc ?? "";
                des.dien_thoai = "";
                des.email = "";
                des.ten_ho_so = "";
                des.ngay_hen_tra = DateTime.Now.AddDays(5);
                des.gio_hen_tra = DateTime.Now;
                des.ma_luong_xu_ly = "0";
                des.le_phi = hoso.chiphidukien;

                des.nhan_kq_qua_buu_chinh = hoso.nhanquabuudien;
                des.nhan_kq_truc_tuyen = hoso.nhanquatnvatkq;
                des.nhan_kq_zalo = hoso.nhanquazalo;
                des.nhan_kq_email = hoso.nhanquaemail;
                if (hoso.dacoketqua == 1)
                {
                    des.trang_thai = (int)StatusTraKetQua.HoSoDaTraKQ;
                }
                if (hoso.datiepnhan == 1)
                {
                    des.trang_thai = (int)StatusTiepNhanHoSo.HoSoDangTiepNhan;
                }
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Ho_So> _hoso = db.Ho_So.Add(des);
                db.SaveChanges();
                hosoId = _hoso.Entity.Id;
                if (hoso.files_thanh_phan_1 != null && hosoId != 0)
                {
                  
                };
                ViewBag.error = false;
                ViewBag.Message = "Cập nhật hồ sơ thành công vui lòng chờ quản trị liên hệ!";

                return View(obj);
            }
            catch(Exception e)
            {
                ViewBag.error = true;
                ViewBag.Message = "Chưa có file đính kèm hoặc file đính kèm bị lỗi!";
                return View(obj);
            }
        }
        #endregion

    }
}