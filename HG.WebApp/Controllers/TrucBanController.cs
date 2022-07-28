using HG.Entities.Entities.CauHinh;
using HG.WebApp.Data;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
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
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;

        }
        public IActionResult DanhSachCanBo(string code = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var data = new List<cd_danh_sach_can_bo>();
            using (var db = new EAContext())
            {
                var innerJoin = from e in db.cd_danh_sach_can_bo
                                join d in db.AspNetUsers on e.user_id equals d.Id
                                join f in db.Dm_Phong_Ban on d.ma_phong_ban equals f.ma_phong_ban
                                join g in db.dm_don_vi on f.ma_don_vi equals g.ma_don_vi
                                where e.Deleted == 0 && (g.ma_don_vi == code || code == "")
                                select new cd_danh_sach_can_bo
                                {
                                    id = e.id,
                                    ten_can_bo = d.ho_dem + " " + d.ten,
                                    ten_don_vi = g.ten_don_vi
                                };
                data = innerJoin.Skip(0).Take(pageSize).ToList();
            }
            return View(data);
        }
    }
}
