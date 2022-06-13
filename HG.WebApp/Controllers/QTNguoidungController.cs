using HG.Data.Business.NguoiDung;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Nhom;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using HG.WebApp.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
{
    public class QTNguoidungController : BaseController
    {
        private readonly NguoiDungDao _nguoiDungDao;
        private readonly ILogger<QTNguoidungController> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        public QTNguoidungController(ILogger<QTNguoidungController> logger, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
       : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _nguoiDungDao = new NguoiDungDao(DbProvider);
        }

        public IActionResult ListNguoiDung(string ma_phong_ban = "", int trang_thai = 0, int da_xoa = 0)
        {
            ViewBag.ma_phong_ban = ma_phong_ban;
            ViewBag.trang_thai = trang_thai;
            ViewBag.da_xoa = da_xoa;
            var totalRecouds = 0;
            HelperString stringHelper = new HelperString();
            var ds = _nguoiDungDao.LayDsNguoiDungPhanTrang(new NguoiDungSearchItem { ma_phong_ban = ma_phong_ban,  PageIndex = 0, RecordsPerPage = 25, trang_thai = trang_thai, da_xoa = da_xoa }, out totalRecouds);
            ViewBag.pagingdemo = stringHelper.getHtmlPagingNews("localhost",4, 5, 10, 100);
            return View(ds);
        }

        [HttpGet]
        public IActionResult ThemMoi()
        {
            List<Asp_nhom> lstDMNhom = new List<Asp_nhom>();
            using (var db = new EAContext())
            {
                lstDMNhom = db.Asp_nhom.ToList();
            }
            ViewBag.LstNhom = lstDMNhom;
            return PartialView(new NguoiDungModels());
        }
        [HttpPost]
        public IActionResult ThemMoi(NguoiDungModels item)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var ObjId = _nguoiDungDao.ThemMoi(item, UserId);

			if (!string.IsNullOrEmpty(ObjId) && item.lstGroup != null)
			{
				for (int i = 0; i < item.lstGroup.Split(",").Length; i++)
				{
					_nguoiDungDao.ThemMoi_NguoiDung_Nhom(Guid.Parse(ObjId), item.lstGroup.Split(",")[i].ToString(), UserId);
				}
				return RedirectToAction("ThemMoi");
			}
			else
			{
				item.responseErr.ErrorCode = 1;
				item.responseErr.ErrorMsg = "Có lỗi xảy ra";
				return PartialView(item);
			}
        }

        [HttpGet]
        public IActionResult EditNguoiDung(Guid Id)
        {
            
            return View(_nguoiDungDao.LayNguoiDungBoiId(Id));
        }
        //Case 1
        //public Response Xoa(Guid id)
        //{
        //    return _nguoiDungDao.Xoa(id);
        //}

        //Case2
        public IActionResult Xoa(Guid id)
        {
            var obj = _nguoiDungDao.Xoa(id);
            if(obj.ErrorCode == 0)
			{
                return RedirectToAction("ListNguoiDung");
			}
			else
			{
                //Them Errcode day
                return View();
			}
        }
    }
}
