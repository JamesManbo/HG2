using HG.Data.Business.DanhMuc;
using HG.Data.Business.GuiHoSo;
using HG.Entities;
using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.Model;
using HG.WebApp.Data;
using HG.WebApp.Entities;
using HG.WebApp.Helper;
using HG.WebApp.Models.DanhMuc;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                var userId = Guid.Parse(user_id);
                lstpb = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();
                nguoi_dung = db.AspNetUsers.Find(userId);
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
                var userId = Guid.Parse(user_id);
                lstpb = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();
                nguoi_dung = db.AspNetUsers.Find(userId);
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
            return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
        }

        [HttpPost]
        public IActionResult ThemTuyenSinhCapMamNon(TuyenSinhCapMamNonModel item)
        {
            var ds_hs = HG.WebApp.Helper.HelperString.ListThanhPhanHoSoRecords().OrderBy(x=>x.stt).ToList();

            EAContext eAContext = new EAContext();
            var data = _tuyensinhcapmamnonDao.mapdata(item);
            data.CreatedUid = Guid.Parse(userManager.GetUserId(User));
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
                var userId = Guid.Parse(user_id);
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                lst_dantoc = db.Dm_Dan_Toc.Where(n => n.Deleted != 1).ToList();
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();

                nguoi_dung = db.AspNetUsers.Find(userId);
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
            return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
            //return View(item);
           
        }
        public async Task<IActionResult> LayHuyenTheoTinh(string ma_dia_ban_cha, string noi_dung)
        {
            EAContext db = new EAContext();
            var LstDiaBan = new List<dm_dia_ban>();
            LstDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_don_vi_cha == ma_dia_ban_cha).ToList();
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
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/GuiHoSo/TuyenSinhCapMamNon/DiaBanHuyenNoiCuTru.cshtml", LstDiaBan);
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
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();
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
            return View("~/Views/GuiHoSo/TuyenSinhCapTieuHoc/GuiTuyenSinhCapTieuHoc.cshtml", modal);
        }

        [HttpPost]
        public IActionResult GuiTuyenSinhCapTieuHoc(TuyenSinhCapTieuHocModel item)
        {
            var user_id = userManager.GetUserId(User);
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
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();
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
            return View("~/Views/GuiHoSo/TuyenSinhCapTieuHoc/GuiTuyenSinhCapTieuHoc.cshtml", modal);
            //return View(item);

        }
        #endregion -------------------------------------

        #region ------- Gửi hồ sơ cấp THCS -------------
        public IActionResult GuiTuyenSinhCapTHCS(string code = "")
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
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();
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
            return View("~/Views/GuiHoSo/TuyenSinhCapTHCS/GuiTuyenSinhCapTHCS.cshtml", modal);
        }

        [HttpPost]
        public IActionResult GuiTuyenSinhCapTHCS(TuyenSinhCapTHCSModel item)
        {
            var user_id = userManager.GetUserId(User);
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
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();
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
            return View("~/Views/GuiHoSo/TuyenSinhCapTHCS/GuiTuyenSinhCapTHCS.cshtml", modal);
            //return View(item);

        }
        #endregion -------------------------------------

        #region ------- Gửi hồ sơ cấp THPT -------------
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
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();
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
            return View("~/Views/GuiHoSo/TuyenSinhCapTHPT/GuiTuyenSinhCapTHPT.cshtml", modal);
        }

        [HttpPost]
        public IActionResult GuiTuyenSinhCapTHPT(TuyenSinhCapTHPTModel item)
        {
            var user_id = userManager.GetUserId(User);
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
                lst_tinh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_don_vi_cha == null).ToList();
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
            return View("~/Views/GuiHoSo/TuyenSinhCapTHPT/GuiTuyenSinhCapTHPT.cshtml", modal);
            //return View(item);

        }
        #endregion -------------------------------------
    }
}