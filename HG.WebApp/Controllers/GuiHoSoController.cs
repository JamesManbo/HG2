using HG.Data.Business.DanhMuc;
using HG.Data.Business.GuiHoSo;
using HG.Entities;
using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Model;
using HG.WebApp.Data;
using HG.WebApp.Entities;
using HG.WebApp.Helper;
using HG.WebApp.Models.DanhMuc;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
{
    [Authorize]
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
        }

        #region Gửi hồ sơ cấp tiểu học
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

        public IActionResult ThemTuyenSinhCapMamNon(string code = "")
        {
            var modal = new Ghs_Tuyen_Sinh_Cap_Mam_Non();
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
            ViewBag.code = code;
            return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", modal);
        }

        [HttpPost]
        public IActionResult ThemTuyenSinhCapMamNon(Ghs_Tuyen_Sinh_Cap_Mam_Non pb)
        {

            //pb.ma_ho_so = HelperString.CreateCode(pb.ma_ho_so);
            pb.ma_ho_so = "MaHoSo";
            pb.thong_tin_chi_tiet = "Thông tin chi tiết";
            pb.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            pb.UidName = User.Identity.Name;
            var _pb = _tuyensinhcapmamnonDao.Luu(pb);
            if (_pb.ErrorCode > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = _pb.ErrorMsg;
            }
            if (pb.type_view == StatusAction.Add.ToString() || _pb.ErrorCode > 0)
            {
                var user = _tuyensinhcapmamnonDao.DanhSachNguoiDung("");
                var lstpb = new List<Ghs_Tuyen_Sinh_Cap_Mam_Non>();
                using (var db = new EAContext())
                {
                    lstpb = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
                ViewBag.lst_nguoi_dung = user;
                ViewBag.lst_phong_ban = lstpb;
                return View("~/Views/GuiHoSo/TuyenSinhCapMamNon/ThemTuyenSinhCapMamNon.cshtml", pb);
            }
            if (pb.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaTuyenSinhCapMamNon", "GuiHoSo", new { code = pb.ma_ho_so, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult SuaTuyenSinhCapMamNon(string code, string type)
        {
            var user = _danhmucDao.DanhSachNguoiDung("");
            var lstpb = new List<Dm_Phong_Ban>();
            var lstdv = new List<dm_don_vi>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstdv = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
            }
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_phong_ban = lstpb;
            ViewBag.lst_don_vi = lstdv;
            var pb = lstpb.Where(n => n.ma_phong_ban == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/Phongban/SuaPhongBan.cshtml", pb);
        }

        [HttpPost]
        public IActionResult SuaTuyenSinhCapMamNon(string code, string type, Dm_Phong_Ban item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuPhongBan(item);
            if (_pb.ErrorCode > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = _pb.ErrorMsg;
                var user = _danhmucDao.DanhSachNguoiDung("");
                var lstpb = new List<Dm_Phong_Ban>();
                using (var db = new EAContext())
                {
                    lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
                ViewBag.lst_phong_ban = lstpb;
                ViewBag.lst_nguoi_dung = user;
                return PartialView("~/Views/DanhMuc/Phongban/SuaPhongBan.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemPhongBan", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaPhongBan", "DanhMuc", new { code = item.ma_phong_ban, type = StatusAction.View.ToString() });
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

    }
}