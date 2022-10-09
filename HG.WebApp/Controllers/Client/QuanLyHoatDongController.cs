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
    public class QuanLyHoatDongController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();


        //extend sys identity
        public QuanLyHoatDongController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;

        }
        public IActionResult QuanLy(string type = "", string oid = "")
        {
            var id = 0;
            var tab = 1;
            if (oid != "")
            {
                id = Convert.ToInt32(oid.Split("-")[0]);
                tab = Convert.ToInt32(oid.Split("-")[1]);
            }
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var data = new HoatDongModel();
            using (var db = new EAContext())
            {
                data.lst_hoat_dong = db.cd_quan_ly_hoat_dong.Where(n => n.Deleted == 0).OrderBy(n => n.Stt).Skip(0).Take(pageSize).ToList();
                var innerJoin = from e in db.cd_thong_bao
                                join d in db.cd_thong_bao on e.ma_cha equals d.id
                                into a
                                from b in a.DefaultIfEmpty()
                                where e.Deleted == 0
                                select new cd_thong_bao
                                {
                                    id = e.id,
                                    ma_cha = e.ma_cha,
                                    ten_ma = e.ten_ma,
                                    link = e.link,
                                    noi_dung = e.noi_dung,
                                    ten_ma_cha = b.ten_ma
                                };

                data.lst_thong_bao = innerJoin.Skip(0).Take(pageSize).ToList();
                //db.cd_thong_bao.Where(n => n.Deleted == 0).Skip(0).Take(pageSize).ToList();
                if (type == StatusAction.Edit.ToString())
                {
                    data.hoat_dong = data.lst_hoat_dong.FirstOrDefault(n => n.id == id) ?? new cd_quan_ly_hoat_dong();
                }
                else
                {
                    data.hoat_dong = new cd_quan_ly_hoat_dong();
                }
            }
            ViewBag.active = tab;
            ViewBag.type_view = type;
            data.PagelistHD.TotalPage = (data.lst_hoat_dong.Count() / pageSize) + ((data.lst_hoat_dong.Count() % pageSize) > 0 ? 1 : 0);
            data.PagelistHD.CurrentPage = 1;
            data.PagelistHD.RecoredFrom = 1;
            data.PagelistHD.TotalRecords = data.lst_hoat_dong.Count();
            data.PagelistHD.RecoredTo = data.PagelistHD.TotalPage == 1 ? data.lst_hoat_dong.Count() : pageSize;

            data.PagelistTB.TotalPage = (data.lst_thong_bao.Count() / pageSize) + ((data.lst_thong_bao.Count() % pageSize) > 0 ? 1 : 0);
            data.PagelistTB.CurrentPage = 1;
            data.PagelistTB.RecoredFrom = 1;
            data.PagelistTB.TotalRecords = data.lst_thong_bao.Count();
            data.PagelistTB.RecoredTo = data.PagelistTB.TotalPage == 1 ? data.lst_thong_bao.Count() : pageSize;
            return View(data);
        }


        public IActionResult SaveHoatDong(cd_quan_ly_hoat_dong item, string type)
        {
            try
            {
                EAContext eAContext = new EAContext();
                var obj = eAContext.cd_quan_ly_hoat_dong.FirstOrDefault(n => n.id == item.id);

                if (type == StatusAction.Edit.ToString() && obj != null)
                {
                    obj.UidName = User.Identity.Name;
                    obj.CreatedDateUtc = DateTime.Now;
                    obj.Stt = item.Stt;
                    obj.url_file = item.url_file;
                    obj.file_name = item.file_name;
                    obj.noi_dung = item.noi_dung;
                    eAContext.Entry(obj).State = EntityState.Modified;
                    eAContext.SaveChanges();
                }
                else
                {

                    item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                    item.CreatedDateUtc = DateTime.Now;
                    item.Deleted = 0;
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<cd_quan_ly_hoat_dong> _role = eAContext.cd_quan_ly_hoat_dong.Add(item);
                    eAContext.SaveChanges();
                }
                if (item.type_add == StatusAction.Edit.ToString())
                {
                    return RedirectToAction("QuanLy", "QuanLyHoatDong", new { type = StatusAction.Edit.ToString(), oid = item.id.ToString() + "-1" });
                }
                return RedirectToAction("QuanLy", "QuanLyHoatDong", new { type = StatusAction.Add.ToString() });

            }
            catch (Exception ex)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Lỗi dữ liệu";
                return RedirectToAction("QuanLy", "QuanLyHoatDong");
            }
        }

        [HttpPost]
        public IActionResult XoaHoatDong(string code)
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
                    var obj = db.cd_quan_ly_hoat_dong.Where(n => n.id == Convert.ToInt64(item)).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.Deleted = 1;
                        obj.UpdatedDateUtc = DateTime.Now;
                        obj.UpdatedUid = uid;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json(new { error = 0, msg = "Xóa thành công!", href = "/QuanLyHoatDong/QuanLy" });
            }
            catch (Exception ex)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
        }

        public async Task<IActionResult> HoatDongPaging(int currentPage = 0, int pageSize = 0)
        {
            var totalRecored = 0;
            var result = "";

            using (var db = new EAContext())
            {
                var ds = db.cd_quan_ly_hoat_dong.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize + 1;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                ViewBag.PageSize = (currentPage - 1) * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QuanLyHoatDong/HoatDongPaging.cshtml", datapage);

            }

            return Content(result);
        }

        public IActionResult SaveThongBao(cd_thong_bao item, int type_add)
        {
            try
            {
                EAContext eAContext = new EAContext();
                var obj = eAContext.cd_thong_bao.FirstOrDefault(n => n.id == item.id);
                if (type_add == 2 && item.ten_ma_cha == null)
                {
                    ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "Bạn chưa nhập thông báo cha";
                    return RedirectToAction("QuanLy", "QuanLyHoatDong", new { oid = "0-2" });
                }

                if (obj != null)
                {
                    obj.UidName = User.Identity.Name;
                    obj.CreatedDateUtc = DateTime.Now;
                    obj.ten_ma = item.ten_ma;
                    obj.link = item.link;
                    obj.noi_dung = item.noi_dung;
                    if (type_add == 2)
                    {
                        // mã thông báo cha
                        obj.ma_cha = item.ma_cha;
                    }

                    eAContext.Entry(obj).State = EntityState.Modified;
                    eAContext.SaveChanges();
                }
                else
                {

                    item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                    item.CreatedDateUtc = DateTime.Now;
                    item.Deleted = 0;
                    if (type_add == 1)
                    {
                        // mã thông báo cha
                        item.ma_cha = 0;
                    }
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<cd_thong_bao> _role = eAContext.cd_thong_bao.Add(item);
                    eAContext.SaveChanges();
                }

                return RedirectToAction("QuanLy", "QuanLyHoatDong", new { oid = "0-2" });

            }
            catch (Exception ex)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Lỗi dữ liệu";
                return RedirectToAction("QuanLy", "QuanLyHoatDong", new { oid = "0-2" });
            }
        }

        public IActionResult XoaThongBao(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "User");
                }
                var uid = Guid.Parse(userManager.GetUserId(User));

                EAContext db = new EAContext();

                var obj = db.cd_thong_bao.Where(n => n.id == id).FirstOrDefault();
                if (obj != null && obj.ma_cha == 0)
                {
                    // xóa mã cha => check xem có mã con không
                    var check = db.cd_thong_bao.Where(n => n.ma_cha == id).FirstOrDefault();
                    if (check != null && check.ma_cha > 0)
                    {
                        return Json(new { error = 1, msg = "Thông báo " + check.ten_ma + " đang chứa thông báo con" });
                    }
                }
                if (obj != null)
                {
                    obj.Deleted = 1;
                    obj.UpdatedDateUtc = DateTime.Now;
                    obj.UpdatedUid = uid;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(new { error = 0, msg = "Xóa thành công!", href = "/QuanLyHoatDong/QuanLy?oid=0-2" });
            }
            catch (Exception ex)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
        }

        public async Task<IActionResult> ThongBaoPaging(int currentPage = 0, int pageSize = 0)
        {
            var totalRecored = 0;
            var result = "";

            using (var db = new EAContext())
            {
                var ds = db.cd_thong_bao.Where(n => n.Deleted == 0).ToList();
                var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ds.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize + 1;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                ViewBag.PageSize = (currentPage - 1) * pageSize;
                result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QuanLyHoatDong/ThongBaoPaging.cshtml", datapage);

            }

            return Content(result);
        }


    }
}
