using AutoMapper;
using HG.Data.Business.DanhMuc;
using HG.Data.Business.ThuTuc;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Model;
using HG.Entities.HoSo;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using HG.WebApp.Models.BaoCaoThongKe;
using HG.WebApp.Reports;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;

using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using System.Globalization;

namespace HG.WebApp.Controllers
{

    [Authorize]
    public class BaoCaoThongKeController : BaseController
    {

        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LuongXuLyDao _danhmucDao;
        private readonly ThuTucDao _thuTucDao;

        private readonly BaoCaoThongKeDao _baocaoDao;
        private readonly HG.Data.Business.HoSo.HoSoDao _hoso;

        public BaoCaoThongKeController(IWebHostEnvironment _environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            //_danhmucDao = new LuongXuLyDao(DbProvider);
            //_thuTucDao = new ThuTucDao(DbProvider);
            //_hoso = new HG.Data.Business.HoSo.HoSoDao(DbProvider);
            _baocaoDao = new BaoCaoThongKeDao(DbProvider);
        }

        [HttpGet]
        public async Task<IActionResult> BaoCaoSoLuong()
        {
            //var uid = Guid.Parse(userManager.GetUserId(User));
            //var UserCreated = await userManager.FindByIdAsync(uid.ToString());
            //ViewBag.UserCreated = UserCreated.ho_dem + " " + UserCreated.ten;
            //ViewBag.NgayTiepNhan = DateTime.Now;
            //var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            //EAContext db = new EAContext();
            //ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            //ViewBag.LstThuTuc = _danhmucDao.DanhSachThuTuc();
            //ViewBag.LstLuongXL = db.Dm_Luong_Xu_Ly.Where(n => n.Deleted != 1).ToList();
            //ViewBag.LstNguoiDung = db.AspNetUsers.Where(n => n.Deleted != 1).ToList();
            //return View(new HoSoModels());

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetBaoCaoSoLuong(BaoCaoSoLuongRequestModel data)
        {
            try
            {
                //var tuNgay = DateTime.ParseExact(data.TuNgayStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //var denNgay = DateTime.ParseExact(data.DenNgayStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string renderFormat = "PDF";
                data.TuNgay = new DateTime(2022, 08, 01);
                data.DenNgay = new DateTime(2022, 08, 17);
                data.ChonNgay = true;
                var items = _baocaoDao.BaoCaoSoLuongHoSo(data.Nam, data.Quy, data.ChonNgay, data.TuNgay, data.DenNgay);
                using var report = new LocalReport();
                var title = data.ChonNgay ? $"SỐ LƯỢNG HỒ SƠ TỪ NGÀY {data.TuNgay.ToString("dd/MM/yyyy")} ĐẾN NGÀY {data.DenNgay.ToString("dd/MM/yyyy")}" : $"SỐ LƯỢNG HỒ SƠ THEO QUÝ {data.Quy} NĂM {data.Nam}";
                ReportHelper.LoadBaoCaoSoLuong(report, items, title);
                var pdf = report.Render(renderFormat);
                // return new FileContentResult(pdf, "application/pdf");
                var baseStr = Convert.ToBase64String(pdf);
                return Content(baseStr);
            }catch(Exception e)
            {
                return Content("");
            }
            
        }
    }
}
