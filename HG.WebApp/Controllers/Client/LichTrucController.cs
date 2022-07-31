using HG.Entities;
using HG.Entities.Entities.CauHinh;
using HG.WebApp.Data;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HG.WebApp.Controllers.Client
{
    public class LichTrucController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();


        //extend sys identity
        public LichTrucController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;

        }
        public static int GetWeekOrderInYear(DateTime time)
        {
            CultureInfo myCI = CultureInfo.CurrentCulture;
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            return myCal.GetWeekOfYear(time, myCWR, myFirstDOW);
        }
        public IActionResult QuanLy(int nam, string type = "", string oid = "")
        {
            var id = 0;
            var tab = 1;
            if (oid != "")
            {
                id = Convert.ToInt32(oid.Split("-")[0]);
                tab = Convert.ToInt32(oid.Split("-")[1]);
            }
            var day = DateTime.Now;
            var month = day.Month;
            var week = GetWeekOrderInYear(day);
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var data = new lich_truc_model();
            using (var db = new EAContext())
            {
                data.lst_ngay_nghi = db.Dm_Ngay_Nghi.Where(n => n.Deleted == 0).OrderByDescending(n => n.nam).Skip(0).Take(pageSize).ToList();

                data.lich_truc = db.cd_lich_truc.FirstOrDefault(n => n.thang == month && n.tuan == week) ?? new cd_lich_truc();
            }
            if (id > 0 && type == StatusAction.Edit.ToString() && tab == 1)
            {
                data.ngay_nghi = data.lst_ngay_nghi.FirstOrDefault(n => n.nam == nam) ?? new Dm_Ngay_Nghi();
            }
            ViewBag.type_view = type;
            ViewBag.TotalPage = (data.lst_ngay_nghi.Count() / pageSize) + ((data.lst_ngay_nghi.Count() % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.PageSize = 0;
            ViewBag.TotalRecored = data.lst_ngay_nghi.Count();
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? data.lst_ngay_nghi.Count() : pageSize;
            ViewBag.active = tab;
            return View(data);
        }

        public IActionResult SaveNgayNghi(Dm_Ngay_Nghi item, string type = "")
        {
            try
            {
                EAContext eAContext = new EAContext();
                var obj = eAContext.Dm_Ngay_Nghi.FirstOrDefault(n => n.nam == item.nam);

                if (type == StatusAction.Edit.ToString() && obj != null)
                {
                    obj.UidName = User.Identity.Name;
                    obj.CreatedDateUtc = DateTime.Now;
                    obj.ten_ngay_nghi = item.ten_ngay_nghi;
                    obj.mo_ta = item.mo_ta;
                    obj.ngay_nghi_trong_tuan = item.ngay_nghi_trong_tuan;
                    obj.thang1 = item.thang1;
                    obj.thang2 = item.thang2;
                    obj.thang3 = item.thang3;
                    obj.thang4 = item.thang4;
                    obj.thang5 = item.thang5;
                    obj.thang6 = item.thang6;
                    obj.thang7 = item.thang7;
                    obj.thang8 = item.thang8;
                    obj.thang9 = item.thang9;
                    obj.thang10 = item.thang10;
                    obj.thang11 = item.thang11;
                    obj.thang12 = item.thang12;
                    eAContext.Entry(obj).State = EntityState.Modified;
                    eAContext.SaveChanges();
                }
                else
                {
                    item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                    item.CreatedDateUtc = DateTime.Now;
                    item.Deleted = 0;            
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Dm_Ngay_Nghi> _role = eAContext.Dm_Ngay_Nghi.Add(item);
                    eAContext.SaveChanges();
                }
                return RedirectToAction("QuanLy", "LichTruc");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Lỗi dữ liệu";
                return RedirectToAction("QuanLy", "LichTruc");
            }
        }



    }
}
