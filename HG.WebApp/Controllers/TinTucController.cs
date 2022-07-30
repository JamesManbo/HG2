using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
using HG.Data.Business.TinTuc;
using HG.Entities;
using HG.Entities.DanhMuc;
using HG.Entities.Entities.Model;
using HG.Entities.Entities.ThuTuc;
using HG.Entities.Entities.Tin_Tuc;
using HG.WebApp.Data;
using HG.WebApp.Helper;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HG.WebApp.Controllers
{
    //[Authorize]
    public class TinTucController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly TinTucDao _tinTucDao;
        private IWebHostEnvironment Environment;

        //extend sys identity
        public TinTucController(IWebHostEnvironment _environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            Environment = _environment;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _tinTucDao = new TinTucDao(DbProvider);
        }
        //public IActionResult QuanLy(int tabbieumau = 0)
        //{

        //    var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
        //    ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, tu_khoa = "", RecordsPerPage = pageSize };
        //    var ds = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
        //    ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
        //    ViewBag.CurrentPage = 1;
        //    ViewBag.RecoredFrom = 1;
        //    ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
        //    ViewBag.LstPhongBan = ds.lstPhongBan;
        //    ViewBag.LstLinhVuc = ds.lstLinhVuc;
        //    ViewBag.PageSize = 0;
        //    using (var db = new EAContext())
        //    {
        //        ViewBag.lstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();
        //    }
        //    ViewBag.BieuMau = tabbieumau;
        //    return View("~/Views/ThuTuc/QuanLy.cshtml", ds);
        //}

        //public async Task<IActionResult> QuanLyPaging(int currentPage = 0, int pageSize = 0, string ma_pb = "", string ma_lv = "", string tu_khoa = "")
        //{
        //    ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = currentPage, ma_pb = ma_pb, ma_lv = ma_lv, tu_khoa = tu_khoa, RecordsPerPage = pageSize };
        //    var ds = _thuTucDao.DanhSanhThuTuc(nhomSearchItem);
        //    ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
        //    ViewBag.CurrentPage = currentPage;
        //    ViewBag.RecoredFrom = (currentPage - 1) * pageSize + 1;
        //    ViewBag.PageSize = (currentPage - 1) * pageSize;
        //    ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? ds.Pagelist.TotalRecords : currentPage * pageSize;
        //    using (var db = new EAContext())
        //    {
        //        ViewBag.LstPhongBan = db.Dm_Phong_Ban.Where(n => n.Deleted == 0).ToList();
        //        ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
        //    }
        //    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/ThuTuc/QuanLyPaging.cshtml", ds);
        //    return Content(result);
        //}

        #region ----- Danh sách tin tức -----------------
        public IActionResult TinTuc()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var tintuc = new List<Tin_Tuc>();
            using (var db = new EAContext())
            {
                var ds = db.Tin_Tuc.Where(n => n.Deleted == 0).OrderBy(n => n.CreatedDateUtc).ToList();
                tintuc = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
            }
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            return View("~/Views/TinTuc/TinTuc.cshtml", tintuc);
        }

        public async Task<IActionResult> TinTucPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            // var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var totalRecored = 0;
            var result = "";
            using (var db = new EAContext())
            {
                var ds = db.Tin_Tuc.Where(n => n.Deleted == 0).OrderBy(n => n.CreatedDateUtc).ToList();
                if (!String.IsNullOrEmpty(tu_khoa))
                {
                    ds = ds.Where(n => n.tieu_de.ToUpper().Contains(tu_khoa.ToUpper())).ToList();
                }
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TinTuc/TinTucPaging.cshtml", datapage);

            }
            return Content(result);
        }
        #endregion ------------ end danh sách tin tức ----------------

        #region --- Thêm tin tức ----------
        public IActionResult ThemTinTuc(int id = 0)
        {
            var ds_dm_kenh_tin = new List<Dm_Kenh_Tin>();
            using (var db = new EAContext())
            {
                ViewBag.lst_kenh_tin = db.Dm_Kenh_Tin.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.code = id;
            return View("~/Views/TinTuc/ThemTinTuc.cshtml");
        }
        [HttpPost]
        public IActionResult ThemTinTuc(Tin_Tuc item)
        {
            var user_id = userManager.GetUserId(User);
            if (user_id != null)
            {
                item.CreatedUid = Guid.Parse(user_id);

            }
            item.Deleted = 0;
            item.CreatedDateUtc = DateTime.Now;
            EAContext eAContext = new EAContext();
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Tin_Tuc> _role = eAContext.Tin_Tuc.Add(item);

            //eAContext.SaveChanges();


            if (eAContext.SaveChanges() > 0)
            {
                var roleid = _role.Entity.Id;

                return RedirectToAction("SuaTinTuc", "TinTuc", new { code = item.Id, type = StatusAction.View.ToString() });
            }
             else
            {
                ViewBag.error = 1;
                ViewBag.msg = "Thêm mới không thành công";
                return View("~/Views/TinTuc/ThemTinTuc.cshtml", item);
            }

            //var _pb = _thuTucDao.LuuThuTuc(item);
            //if (_pb.ErrorCode > 0)
            //{
            //    ViewBag.error = 1;
            //    ViewBag.msg = _pb.ErrorMsg;
            //}
            //if (_pb.ErrorCode > 0)
            //{
            //    return RedirectToAction("ThemThuTuc", "ThuTuc", new { code = item.ma_thu_tuc });
            //}
            //return RedirectToAction("SuaThuTuc", "ThuTuc", new { code_key = _pb.Code, code = item.ma_thu_tuc, type = StatusAction.View.ToString(), active = ActionThuTuc.ThuTuc.ToString() });
        }
        #endregion ----- End thêm tin tức ------------

        #region ----- Sửa tin tức-----
        public IActionResult SuaTinTuc(int code, string type)
        {
            var tintuc = new Tin_Tuc();
            var ds_dm_kenh_tin = new List<Dm_Kenh_Tin>();
            using (var db = new EAContext())
            {
                ViewBag.lst_kenh_tin = db.Dm_Kenh_Tin.Where(n => n.Deleted == 0).ToList();
                tintuc = db.Tin_Tuc.Where(n => n.Deleted == 0 && n.Id == code).FirstOrDefault();
            }
          
            ViewBag.type_view = type;
            return View("~/Views/TinTuc/SuaTinTuc.cshtml", tintuc);
        }
        [HttpPost]
        public IActionResult SuaTinTuc(int code, string type, Tin_Tuc item)
        {
            var user_id = userManager.GetUserId(User);
            if (user_id != null)
            {
                item.CreatedUid = Guid.Parse(user_id);

            }
            EAContext eAContext = new EAContext();
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Tin_Tuc> _role = eAContext.Tin_Tuc.Update(item);

            if (eAContext.SaveChanges() > 0)
            {
                var roleid = _role.Entity.Id;

                if (item.type_view == StatusAction.Add.ToString())
                {
                    return RedirectToAction("ThemTinTuc", "TinTuc");
                }
                else if (item.type_view == StatusAction.View.ToString())
                {
                    return RedirectToAction("SuaTinTuc", "TinTuc", new { code = item.Id, type = StatusAction.View.ToString() });
                }

                return RedirectToAction("SuaTinTuc", "TinTuc", new { code = item.Id, type = StatusAction.View.ToString() });
            }
            else
            {
                //ViewBag.error = 1;
                //ViewBag.msg = "Thêm mới không thành công";
                //return View("~/Views/TinTuc/ThemTinTuc.cshtml", item);

                //Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật kênh tin lỗi";
                return PartialView("~/Views/TinTuc/SuaTinTuc.cshtml", item);
            }

            return BadRequest();

            //eAContext.SaveChanges();

            //var roleid = _role.Entity.Id;

            //var response = _tinTucDao.LuuKenhTin(item);
            //if (response > 0)
            //{
            //    // Xử lý các thông báo lỗi tương ứng
            //    ViewBag.error = 1;
            //    ViewBag.msg = "cập nhật kênh tin lỗi";
            //    return PartialView("~/Views/DanhMuc/KenhTin/SuaKenhTin.cshtml", item);
            //}
            //if (item.type_view == StatusAction.Add.ToString())
            //{
            //    return RedirectToAction("ThemKenhTin", "DanhMuc");
            //}
            //else if (item.type_view == StatusAction.View.ToString())
            //{
            //    return RedirectToAction("SuaKenhTin", "DanhMuc", new { code = item.ma_kenh_tin, type = StatusAction.View.ToString() });
            //}
            //return BadRequest();

        }
        #endregion ---- end sửa tin tức -------------
    }
}
