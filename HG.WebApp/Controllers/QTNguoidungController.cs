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
            EAContext eAContext = new EAContext();
            HelperString stringHelper = new HelperString();
            var ds = _nguoiDungDao.LayDsNguoiDungPhanTrang(new NguoiDungSearchItem { ma_phong_ban = ma_phong_ban,  PageIndex = 0, RecordsPerPage = 3, trang_thai = trang_thai, da_xoa = da_xoa }, out totalRecouds);

            ViewBag.ListPhongBan = eAContext.Dm_Phong_Ban.ToList();
            ViewBag.TotalPage = totalRecouds/3;
            ViewBag.CurrentPage = 1;
            return View(ds);
        }
        public async Task<IActionResult> NguoiDungPaging(int currentPage = 0, string ma_phong_ban = "", int trang_thai = 0, int da_xoa = 0)
        {
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = currentPage, ma_phong_ban = ma_phong_ban, trang_thai = trang_thai, da_xoa = da_xoa, RecordsPerPage = 3 };
            var ds = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            ds.Pagelist.CurrentPage = currentPage;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoiDung/NguoiDungPaging.cshtml", ds);
            return Content(result);
        }

        [HttpGet]
        public IActionResult ThemNguoiDung()
        {
            List<Asp_nhom> lstDMNhom = new List<Asp_nhom>();
            using (var db = new EAContext())
            {
                lstDMNhom = db.Asp_nhom.ToList();
            }
            ViewBag.LstNhom = lstDMNhom;
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);

            return PartialView(new NguoiDungModels());
        }
        #region nguoidung
        [HttpPost]
        public IActionResult ThemNguoiDung(NguoiDungModels item)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var ObjId = _nguoiDungDao.ThemMoi(item, UserId);

			if (!string.IsNullOrEmpty(ObjId) && item.lstGroup != null)
			{
				for (int i = 0; i < item.lstGroup.Split(",").Length; i++)
				{
					_nguoiDungDao.ThemMoi_NguoiDung_Nhom(Guid.Parse(ObjId), item.lstGroup.Split(",")[i].ToString(), UserId);
				}
				return RedirectToAction("ThemNguoiDung");
			}
			else
			{
				item.responseErr.ErrorCode = 1;
				item.responseErr.ErrorMsg = "Có lỗi xảy ra";
				return PartialView(item);
			}
        }

        [HttpGet]
        public IActionResult SuaNguoiDung(Guid Id, string type)
        {
            List<Asp_nhom> lstDMNhom = new List<Asp_nhom>();
            using (var db = new EAContext())
            {
                lstDMNhom = db.Asp_nhom.ToList();
            }
            ViewBag.LstNhom = lstDMNhom;
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            return View(_nguoiDungDao.LayNguoiDungBoiId(Id));
        }
        [HttpGet]
        public IActionResult ViewNguoiDung(Guid Id, string type)
        {
            List<Asp_nhom> lstDMNhom = new List<Asp_nhom>();
            using (var db = new EAContext())
            {
                lstDMNhom = db.Asp_nhom.ToList();
            }
            ViewBag.LstNhom = lstDMNhom;
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            return View(_nguoiDungDao.LayNguoiDungBoiId(Id));
        }
        [HttpPost]
        public IActionResult SuaNguoiDung(NguoiDungModels item, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var ObjId = _nguoiDungDao.SuaNguoiDung(item, UserId);
            try {
                if (ObjId.ErrorCode == 0 && item.lstGroup != null)
                {
                    for (int i = 0; i < item.lstGroup.Split(",").Length; i++)
                    {
                        _nguoiDungDao.ThemMoi_NguoiDung_Nhom(item.Id, item.lstGroup.Split(",")[i].ToString(), UserId);
                    }
                    if (item.type_view == StatusAction.Add.ToString())
                    {
                        return RedirectToAction("ThemNguoiDung");
                    }
                    if (item.type_view == StatusAction.View.ToString())
                    {
                        return RedirectToAction("ListNguoiDung");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    item.responseErr.ErrorCode = 1;
                    item.responseErr.ErrorMsg = "Có lỗi xảy ra";
                    return PartialView(item);
                }
            }
            catch(Exception e)
            {
                item.responseErr.ErrorCode = 1;
                item.responseErr.ErrorMsg = "Có lỗi xảy ra";
                return PartialView(item);
            }
            
        }
        //Case 1
        //public Response Xoa(Guid id)
        //{
        //    return _nguoiDungDao.Xoa(id);
        //}

        //Case2
        public IActionResult XoaNnguoiDung(string code, string type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _nguoiDungDao.Xoa(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/SuperAdmin/ViewNhom" });
        }
        #endregion

        #region Nhom vai trò
        public IActionResult QLNhomVaitro()
        {
            return View();
        }
        #endregion
        #region Thống kê truy cập
        public IActionResult ThongKeTruyCap()
        {
            return View();
        }
        #endregion
    }
}
