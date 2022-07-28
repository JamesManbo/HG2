using HG.Entities;
using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.CauHinh;
using HG.WebApp.Data;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HG.WebApp.Controllers.Client
{
    [Authorize]
    public class CauHinhHeThongController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();


        //extend sys identity
        public CauHinhHeThongController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;

        }

        public IActionResult CauHinh(string don_vi = "", string type = "", string Oid = "")
        {
            var id = 0;
            var tab = 1;
            if (Oid != "")
            {
                id = Convert.ToInt32(Oid.Split("-")[0]);
                tab = Convert.ToInt32(Oid.Split("-")[1]);
            }
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var data = new CauHinhModel();
            var phien_lv = new cd_phien_lam_viec();
            var lst_don_vi = new List<dm_don_vi>();
            var lst_linh_vuc = new List<Dm_Linh_Vuc>();
            var lst_chuyen_vien = new List<cd_tt_chuyen_vien>();
            using (var db = new EAContext())
            {
                data.lst_don_vi = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                data.lst_linh_vuc = db.Dm_Linh_Vuc.Where(n => n.Deleted == 0).ToList();
                data.lst_chuyen_vien = db.cd_tt_chuyen_vien.Where(n => n.Deleted == 0).ToList();

                if (String.IsNullOrEmpty(don_vi))
                {
                    don_vi = data.lst_don_vi.FirstOrDefault().ma_don_vi ?? "";
                }

                data.phien_lv = db.cd_phien_lam_viec.OrderByDescending(n => n.id).FirstOrDefault(n => n.don_vi == don_vi) ?? new cd_phien_lam_viec();

            }
            if (id > 0 && type == StatusAction.Edit.ToString() && tab == 2)
            {
                data.chuyen_vien = data.lst_chuyen_vien.FirstOrDefault(n => n.id == id) ?? new cd_tt_chuyen_vien();
                don_vi = data.chuyen_vien.don_vi ?? "";
            }
            else
            {
                data.chuyen_vien = new cd_tt_chuyen_vien();
            }
            ViewBag.type_view = type;
            ViewBag.don_vi = don_vi;
            ViewBag.TotalPage = (data.lst_chuyen_vien.Count() / pageSize) + ((data.lst_chuyen_vien.Count() % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.PageSize = 0;
            ViewBag.TotalRecored = data.lst_chuyen_vien.Count();
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? data.lst_chuyen_vien.Count() : pageSize;
            ViewBag.active = tab;
            return View(data);
        }

        public IActionResult SavePhien(cd_phien_lam_viec item)
        {
            try
            {
                EAContext eAContext = new EAContext();
                var phien = eAContext.cd_phien_lam_viec.FirstOrDefault(n => n.don_vi == item.don_vi);

                var phien_hien_tai = 0;
                if (phien != null)
                {
                    phien_hien_tai = phien.phien_hien_tai ?? 0;
                }
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                item.UidName = User.Identity.Name;
                item.Deleted = 0;
                item.CreatedDateUtc = DateTime.Now;
                item.phien_hien_tai = phien_hien_tai + item.so_phien;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<cd_phien_lam_viec> _role = eAContext.cd_phien_lam_viec.Add(item);
                eAContext.SaveChanges();
                var roleid = _role.Entity.id;
                return RedirectToAction("CauHinh", "CauHinhHeThong", new { don_vi = item.don_vi, Oid = "0-1" });
                //return View(item);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Lỗi dữ liệu";
                return View(item);
            }
        }

        public IActionResult SaveChuyenVien(cd_tt_chuyen_vien item, string type = "")
        {
            try
            {
                EAContext eAContext = new EAContext();
                var obj = eAContext.cd_tt_chuyen_vien.FirstOrDefault(n => n.id == item.id);
                var ten_don_vi = (eAContext.dm_don_vi.FirstOrDefault(n => n.Deleted != 1 && n.ma_don_vi == item.don_vi) ?? new dm_don_vi()).ten_don_vi ?? "";
                if (type == StatusAction.Edit.ToString())
                {
                    obj.UidName = User.Identity.Name;
                    obj.CreatedDateUtc = DateTime.Now;
                    obj.ten_chuyen_vien = item.ten_chuyen_vien;
                    obj.ten_don_vi = ten_don_vi;
                    obj.don_vi = item.don_vi;
                    obj.sdt = item.sdt;
                    obj.ma_linh_vuc = item.ma_linh_vuc;
                    eAContext.Entry(obj).State = EntityState.Modified;
                    eAContext.SaveChanges();
                }
                else
                {

                    item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                    item.CreatedDateUtc = DateTime.Now;
                    item.Deleted = 0;
                    item.ten_don_vi = ten_don_vi;
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<cd_tt_chuyen_vien> _role = eAContext.cd_tt_chuyen_vien.Add(item);
                    eAContext.SaveChanges();
                }
                return RedirectToAction("CauHinh", "CauHinhHeThong", new { don_vi = item.don_vi, Oid = "0-2" });
                //return View(item);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Lỗi dữ liệu";
                return RedirectToAction("CauHinh", "CauHinhHeThong", new { don_vi = item.don_vi, Oid = "0-2" });
            }
        }

        public IActionResult XoaChuyenVien(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));

            EAContext db = new EAContext();
            var obj = db.cd_tt_chuyen_vien.Where(n => n.id == id).FirstOrDefault();
            if (obj != null)
            {
                obj.Deleted = 1;
                obj.UpdatedDateUtc = DateTime.Now;
                obj.UpdatedUid = uid;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("CauHinh", "CauHinhHeThong", new { Oid = "0-2" });
        }

        public async Task<IActionResult> ChuyenVienPaging(int currentPage = 0, int pageSize = 0)
        {
            var totalRecored = 0;
            var result = "";

            using (var db = new EAContext())
            {
                var ds = db.cd_tt_chuyen_vien.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize + 1;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/CauHinhHeThong/ChuyenVienPaging.cshtml", datapage);

            }

            return Content(result);
        }

    }
}
