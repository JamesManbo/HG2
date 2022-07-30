using HG.Entities.Entities.CauHinh;
using HG.WebApp.Data;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HG.WebApp.Controllers.Client
{
    public class TrucBanController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();


        //extend sys identity
        public TrucBanController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            _config = configuration;
            _httpContextAccessor = httpContextAccessor;

        }
        public IActionResult DanhSachCanBo(string don_vi = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var data = new List<cd_danh_sach_can_bo>();
            using (var db = new EAContext())
            {
                var innerJoin = from e in db.cd_danh_sach_can_bo
                                join d in db.AspNetUsers on e.user_id equals d.Id
                                join f in db.Dm_Phong_Ban on d.ma_phong_ban equals f.ma_phong_ban
                                into a
                                from b in a.DefaultIfEmpty()
                                join g in db.dm_don_vi on b.ma_don_vi equals g.ma_don_vi

                                where e.Deleted == 0 && (g.ma_don_vi == don_vi || don_vi == "")
                                select new cd_danh_sach_can_bo
                                {
                                    id = e.id,
                                    ten_can_bo = d.ho_dem + " " + d.ten,
                                    ten_don_vi = g.ten_don_vi
                                };
                data = innerJoin.Skip(0).Take(pageSize).ToList();
                ViewBag.lst_don_vi = db.dm_don_vi.Where(n => n.Deleted == 0).ToList();
            }
            ViewBag.don_vi = don_vi;
            ViewBag.TotalPage = (data.Count() / pageSize) + ((data.Count() % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.PageSize = 0;
            ViewBag.TotalRecored = data.Count();
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? data.Count() : pageSize;
            return View(data);
        }

        [HttpPost]
        public IActionResult XoaCanBoTruc(string code)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "User");
                }
                var uid = Guid.Parse(userManager.GetUserId(User));

                EAContext db = new EAContext();
                foreach (var item in code.Split(","))
                {
                    var obj = db.cd_danh_sach_can_bo.Where(n => n.id == Convert.ToInt64(item)).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.Deleted = 1;
                        obj.UpdatedDateUtc = DateTime.Now;
                        obj.UpdatedUid = uid;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json(new { error = 0, msg = "Xóa thành công!", href = "/TrucBan/DanhSachCanBo" });
            }
            catch (Exception ex)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
        }

        public async Task<IActionResult> DanhSachCanBoPaging(string don_vi = "", int currentPage = 0, int pageSize = 0)
        {
            var totalRecored = 0;
            var result = "";
            var data = new List<cd_danh_sach_can_bo>();
            using (var db = new EAContext())
            {
                var innerJoin = from e in db.cd_danh_sach_can_bo
                                join d in db.AspNetUsers on e.user_id equals d.Id
                                join f in db.Dm_Phong_Ban on d.ma_phong_ban equals f.ma_phong_ban
                                into a
                                from b in a.DefaultIfEmpty()
                                join g in db.dm_don_vi on b.ma_don_vi equals g.ma_don_vi
                                where e.Deleted == 0 && (g.ma_don_vi == don_vi || String.IsNullOrEmpty(don_vi))
                                select new cd_danh_sach_can_bo
                                {
                                    id = e.id,
                                    ten_can_bo = d.ho_dem + " " + d.ten,
                                    ten_don_vi = g.ten_don_vi
                                };
                data = innerJoin.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            }
            ViewBag.don_vi = don_vi;

            totalRecored = data.Count();
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = (currentPage - 1) * pageSize + 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
            ViewBag.PageSize = (currentPage - 1) * pageSize;
            result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TrucBan/DanhSachCanBoPaging.cshtml", data);
            return Content(result);
        }
    }
}
