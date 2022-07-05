using HG.Data.Business.DanhMuc;
using HG.Entities;
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
    public class DanhMucController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly DanhMucDao _danhmucDao;

        //extend sys identity
        public DanhMucController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new DanhMucDao(DbProvider);
        }

        #region Phong Ban
        public IActionResult PhongBan()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var phongBan = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                phongBan = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.lst_phong_ban = phongBan;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/Phongban/PhongBan.cshtml", phongBan);

        }

        public async Task<IActionResult> PhongBanPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.ma_phong_ban.Contains(tu_khoa) || n.ten_phong_ban.Contains(tu_khoa)).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/Phongban/PhongBanPaging.cshtml", datapage);

            }
            return Content(result);
        }
        [HttpPost]
        public int CheckMaPb(string code)
        {
            var lstpb = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0 && n.ma_phong_ban.ToUpper() == code.ToUpper()).ToList();
            }
            return lstpb.Count();
        }

        public IActionResult ThemPhongBan(string code = "")
        {
            var modal = new Dm_Phong_Ban();
            var user = _danhmucDao.DanhSachNguoiDung("0");
            var lstpb = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.lst_nguoi_dung = user;
            ViewBag.lst_phong_ban = lstpb;
            ViewBag.code = code;
            return View("~/Views/DanhMuc/Phongban/ThemPhongBan.cshtml", modal);
        }

        [HttpPost]
        public IActionResult ThemPhongBan(Dm_Phong_Ban pb)
        {
            pb.ma_phong_ban = String.Concat(HelperString.RemoveSign4VietnameseString(pb.ma_phong_ban).ToUpper().Where(c => !Char.IsWhiteSpace(c)));
            pb.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            pb.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuPhongBan(pb);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo phòng ban lỗi";
            }
            if (pb.type_view == StatusAction.Add.ToString() || _pb > 0)
            {
                var user = _danhmucDao.DanhSachNguoiDung("0");
                var lstpb = new List<Dm_Phong_Ban>();
                using (var db = new EAContext())
                {
                    lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
                }
                ViewBag.lst_nguoi_dung = user;
                ViewBag.lst_phong_ban = lstpb;
                return View("~/Views/DanhMuc/Phongban/ThemPhongBan.cshtml", pb);
            }
            if (pb.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaPhongBan", "DanhMuc", new { code = pb.ma_phong_ban, type = StatusAction.View.ToString() });
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult SuaPhongBan(string code, string type)
        {
            var user = _danhmucDao.DanhSachNguoiDung("0");
            var lstpb = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
            }
            var pb = lstpb.Where(n => n.ma_phong_ban == code).FirstOrDefault();
            ViewBag.lst_phong_ban = lstpb;
            ViewBag.lst_nguoi_dung = user;
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/Phongban/SuaPhongBan.cshtml", pb);
        }

        [HttpPost]
        public IActionResult SuaPhongBan(string code, string type, Dm_Phong_Ban item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var _pb = _danhmucDao.LuuPhongBan(item);
            if (_pb > 0)
            {
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật phòng ban lỗi";
                var user = _danhmucDao.DanhSachNguoiDung("0");
                var lstpb = new List<Dm_Phong_Ban>();
                using (var db = new EAContext())
                {
                    lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
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
        public IActionResult XoaPhongBan(string code = "")
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

        public async Task<IActionResult> RenderViewPhongBan()
        {
            var lstpb = new List<Dm_Phong_Ban>();
            using (var db = new EAContext())
            {
                lstpb = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/Phongban/ViewPhongBan.cshtml", lstpb);
            return Content(result);
        }

        [HttpPost]
        public async Task<IActionResult> RenderViewNguoiDung(string ma_phong_ban)
        {
            var ds_nguoi_dung = _danhmucDao.DanhSachNguoiDung(ma_phong_ban);
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/ViewNguoiDung.cshtml", ds_nguoi_dung);
            return Content(result);
        }
        #endregion

        #region Lĩnh vực
        public IActionResult LinhVuc()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var linhvuc = new List<Dm_Linh_Vuc>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                linhvuc = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/LinhVuc/LinhVuc.cshtml", linhvuc);
        }

        public async Task<IActionResult> LinhVucPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/LinhVuc/LinhVucPaging.cshtml", datapage);

            }
            return Content(result);
        }
        public IActionResult ThemLinhVuc()
        {
            return View("~/Views/DanhMuc/LinhVuc/ThemLinhVuc.cshtml");
        }

        [HttpPost]
        public IActionResult ThemLinhVuc(Dm_Linh_Vuc item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuLinhVuc(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo phòng ban lỗi";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                return View("~/Views/DanhMuc/LinhVuc/ThemLinhVuc.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaLinhVuc", "DanhMuc", new { code = item.ma_linh_vuc, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaLinhVuc(string code, string type)
        {
            var linhvuc = new Dm_Linh_Vuc();
            using (var db = new EAContext())
            {
                linhvuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc == code).FirstOrDefault();
            }
            //var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0 && n.ma_linh_vuc == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/LinhVuc/SuaLinhVuc.cshtml", linhvuc);
        }

        [HttpPost]
        public IActionResult SuaLinhVuc(string code, string type, Dm_Linh_Vuc item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuLinhVuc(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật phòng ban lỗi";
                return PartialView("~/Views/DanhMuc/LinhVuc/SuaLinhVuc.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemLinhVuc", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaLinhVuc", "DanhMuc", new { code = item.ma_linh_vuc, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaLinhVuc(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaLinhVuc(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/LinhVuc" });
        }

        public async Task<IActionResult> RenderViewLinhVuc()
        {
            var linhvuc = new List<Dm_Linh_Vuc>();
            using (var db = new EAContext())
            {
                linhvuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            }
            //var linhvuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/LinhVuc/ViewLinhVuc.cshtml", linhvuc);
            return Content(result);
        }

        #endregion

        #region Chức vụ
        public IActionResult ChucVu()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var chucvu = new List<Dm_Chuc_Vu>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0).ToList();
                chucvu = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/ChucVu/ChucVu.cshtml", chucvu);

        }
        public async Task<IActionResult> ChucVuPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/ChucVu/ChucVuPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemChucVu()
        {
            return View("~/Views/DanhMuc/ChucVu/ThemChucVu.cshtml");
        }

        [HttpPost]
        public IActionResult ThemChucVu(Dm_Chuc_Vu item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuChucVu(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo chức vụ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                return View("~/Views/DanhMuc/ChucVu/ThemChucVu.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaChucVu", "DanhMuc", new { code = item.ma_chuc_vu, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaChucVu(string code, string type)
        {
            var chucvu = new Dm_Chuc_Vu();
            using (var db = new EAContext())
            {
                chucvu = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0 && n.ma_chuc_vu == code).FirstOrDefault();
            }
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/ChucVu/SuaChucVu.cshtml", chucvu);
        }

        [HttpPost]
        public IActionResult SuaChucVu(string code, string type, Dm_Chuc_Vu item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuChucVu(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật chức vụ!";
                return PartialView("~/Views/DanhMuc/ChucVu/SuaChucVu.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemChucVu", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaChucVu", "DanhMuc", new { code = item.ma_chuc_vu, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaChucVu(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaChucVu(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/ChucVu" });
        }

        public async Task<IActionResult> RenderViewChucVu()
        {
            var chucvu = new List<Dm_Chuc_Vu>();
            using (var db = new EAContext())
            {
                chucvu = db.Dm_Chuc_Vu.Where(n => n.Deleted == 0).ToList();
            }
            // var chucvu = eAContext.Dm_Chuc_Vu.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/ChucVu/ViewChucVu.cshtml", chucvu);
            return Content(result);
        }

        #endregion

        #region Giấy tờ hợp lệ
        public IActionResult GiayTo()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var giayto = new List<Dm_Giay_To_Hop_Le>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).ToList();
                giayto = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/GiayTo/GiayTo.cshtml", giayto);

        }

        public async Task<IActionResult> GiayToPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/GiayTo/GiayToPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemGiayTo()
        {
            return View("~/Views/DanhMuc/GiayTo/ThemGiayTo.cshtml");
        }

        [HttpPost]
        public IActionResult ThemGiayTo(Dm_Giay_To_Hop_Le item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuGiayTo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo giấy tờ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                return View("~/Views/DanhMuc/GiayTo/ThemGiayTo.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaGiayTo", "DanhMuc", new { code = item.ma_giay_to, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaGiayTo(string code, string type)
        {
            var giayto = new Dm_Giay_To_Hop_Le();
            using (var db = new EAContext())
            {
                giayto = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0 && n.ma_giay_to == code).FirstOrDefault();
            }
            //var chucvu = eAContext.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0 && n.ma_giay_to == code).FirstOrDefault();
            ViewBag.type_view = type;
            return View("~/Views/DanhMuc/GiayTo/SuaGiayTo.cshtml", giayto);
        }

        [HttpPost]
        public IActionResult SuaGiayTo(string code, string type, Dm_Giay_To_Hop_Le item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuGiayTo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập giấy tờ lỗi!";
                return PartialView("~/Views/DanhMuc/GiayTo/SuaGiayTo.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemGiayTo", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaGiayTo", "DanhMuc", new { code = item.ma_giay_to, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaGiayTo(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaGiayTo(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/GiayTo" });
        }

        public async Task<IActionResult> RenderViewGiayTo()
        {
            var giayto = new List<Dm_Giay_To_Hop_Le>();
            using (var db = new EAContext())
            {
                giayto = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).ToList();
            }
            //var giayto = eAContext.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/GiayTo/ViewGiayTo.cshtml", giayto);
            return Content(result);
        }

        #endregion

        #region Sổ hồ sơ
        public IActionResult SoHoSo()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var hoso = new List<Dm_So_Ho_So>();
            using (var db = new EAContext())
            {
                var ds = db.Dm_So_Ho_So.Where(n => n.Deleted == 0).ToList();
                hoso = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/DanhMuc/SoHoSo/SoHoSo.cshtml", hoso);

        }
        public async Task<IActionResult> SoHoSoPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Dm_Giay_To_Hop_Le.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/SoHoSo/SoHoSoPaging.cshtml", datapage);

            }
            return Content(result);
        }

        public IActionResult ThemSoHoSo()
        {

            using (var db = new EAContext())
            {
                ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            }
            // ViewBag.LinhVuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            return View("~/Views/DanhMuc/SoHoSo/ThemSoHoSo.cshtml");
        }

        [HttpPost]
        public IActionResult ThemSoHoSo(Dm_So_Ho_So item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuSoHoSo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Tạo sổ hồ sơ lỗi!";
            }
            if (item.type_view == StatusAction.Add.ToString() || response > 0)
            {
                using (var db = new EAContext())
                {
                    ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                }
                // ViewBag.LinhVuc = eAContext.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                return View("~/Views/DanhMuc/SoHoSo/ThemSoHoSo.cshtml", item);
            }
            if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaSoHoSo", "DanhMuc", new { code = item.ma_so, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }

        public IActionResult SuaSoHoSo(string code, string type)
        {
            var hoso = new Dm_So_Ho_So();
            using (var db = new EAContext())
            {
                hoso = db.Dm_So_Ho_So.Where(n => n.Deleted == 0 && n.ma_so == code).FirstOrDefault();
            }
            //var hoso = eAContext.Dm_So_Ho_So.Where(n => n.Deleted == 0 && n.ma_so == code).FirstOrDefault();
            ViewBag.type_view = type;
            using (var db = new EAContext())
            {
                ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
            }
            return View("~/Views/DanhMuc/SoHoSo/SuaSoHoSo.cshtml", hoso);
        }

        [HttpPost]
        public IActionResult SuaSoHoSo(string code, string type, Dm_So_Ho_So item)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuSoHoSo(item);
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập giấy tờ lỗi!";
                using (var db = new EAContext())
                {
                    ViewBag.LinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                }
                return PartialView("~/Views/DanhMuc/SoHoSo/SuaSoHoSo.cshtml", item);
            }
            if (item.type_view == StatusAction.Add.ToString())
            {
                return RedirectToAction("ThemSoHoSo", "DanhMuc");
            }
            else if (item.type_view == StatusAction.View.ToString())
            {
                return RedirectToAction("SuaSoHoSo", "DanhMuc", new { code = item.ma_so, type = StatusAction.View.ToString() });
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult XoaSoHoSo(string code)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _danhmucDao.XoaSoHoSo(code, uid);
            if (_pb > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/DanhMuc/SoHoSo" });
        }

        public async Task<IActionResult> RenderViewSoHoSo()
        {
            var hoso = new List<Dm_So_Ho_So>();
            using (var db = new EAContext())
            {
                hoso = db.Dm_So_Ho_So.Where(n => n.Deleted == 0).ToList();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DanhMuc/SoHoSo/ViewSoHoSo.cshtml", hoso);
            return Content(result);
        }

        #endregion

        #region Ngày nghỉ
        public IActionResult NgayNghi(string type = "View")
        {
            var ngay_nghi = new Dm_Ngay_Nghi();
            using (var db = new EAContext())
            {
                ngay_nghi = db.Dm_Ngay_Nghi.Where(n => n.Deleted == 0).OrderByDescending(n => n.nam).FirstOrDefault();
                ViewBag.type_view = type;
            }
            return View("~/Views/DanhMuc/NgayNghi/NgayNghi.cshtml", ngay_nghi);

        }
        [HttpPost]
        public IActionResult NgayNghi(Dm_Ngay_Nghi item, string type)
        {
            item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
            item.UidName = User.Identity.Name;
            var response = _danhmucDao.LuuNgayNghi(item);
            ViewBag.type_view = item.type_view;
            if (response > 0)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "Cập nhật ngày nghỉ lỗi!";
            }
            return RedirectToAction("NgayNghi", "DanhMuc");
            // return PartialView("~/Views/DanhMuc/NgayNghi/NgayNghi.cshtml", item);
        }
        #endregion
    }
}