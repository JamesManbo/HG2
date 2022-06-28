﻿using Microsoft.AspNetCore.Mvc;
using HG.Entities.Entities.Nhom;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using HG.Data.Business.DanhMuc;
using HG.WebApp.Data;
using HG.Entities.SearchModels;
using HG.Data.Business.Nhom;
using HG.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using HG.Entities.Entities.Model;
using HG.WebApp.Models.DanhMuc;
using HG.Entities.Entities.SuperAdmin;
using HG.WebApp.Models;

namespace HG.WebApp.Controllers
{
    public class SuperAdminController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly DanhMucDao _danhmucDao;
        private readonly NhomDao _nhomDao;
        private readonly DanhMucMenuDao _menuDao;
        //extend sys identity
        public SuperAdminController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new DanhMucDao(DbProvider);
            _nhomDao = new NhomDao(DbProvider);
            _menuDao = new DanhMucMenuDao(DbProvider);
        }


        #region Nhom
        [HttpGet]
        public IActionResult ViewNhom(NhomSearchItem item)
        {
            EAContext eAContext = new EAContext();
            ViewBag.TotalRecords = eAContext.Asp_nhom.Where(n => n.Deleted == 0).Count();
            ViewBag.TotalPage = eAContext.Asp_nhom.Where(n => n.Deleted == 0).Count()/10;
            ViewBag.CurrentPage = 1;
            return View(eAContext.Asp_nhom.Where(n=>n.Deleted == 0).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> SuaNhom(string code, string type)
        {
            if(type == StatusAction.Edit.ToString())
            {
                var UserUpdate = "";
                ViewBag.ListNhom = _nhomDao.LayDsNhomPhanTrang(new NhomSearchItem() { RecordsPerPage = 100 }).asp_Nhoms;
                var obj = _nhomDao.LayNhomId(Guid.Parse(code));
                if (obj.UpdatedUid != null)
                {
                    UserUpdate = await userManager.GetUserNameAsync(await userManager.FindByIdAsync(obj.UpdatedUid.ToString()));
                }
                ViewBag.UserUpdate = UserUpdate;
                return View(obj);
            }
            else
            {
                return View();
            }
          
        }
        [HttpPost]
        public IActionResult SuaNhom(NhomModel item)
        {
            ViewBag.ListNhom = _nhomDao.LayDsNhomPhanTrang(new NhomSearchItem() { RecordsPerPage = 100 }).asp_Nhoms;
            item.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
            var ObjId = _nhomDao.ChinhSuaNhom(item);
            if (ObjId.ErrorCode == 0)
            {
                return RedirectToAction("ViewNhom", "SuperAdmin");
            }
            else
            {
                ViewBag.ErrorCode = ObjId.ErrorCode;
                ViewBag.ErrorMsg = ObjId.ReturnMsg;
                return View(item);
            }
        }

        [HttpGet]
        public IActionResult ThemNhom()
        {
            var ds = _nhomDao.LayDsNhomPhanTrang(new NhomSearchItem() { RecordsPerPage = 100});
            ViewBag.ListNhom = ds.asp_Nhoms;
            return View();
        }

        [HttpPost]
        public IActionResult ThemNhom(NhomModel item)
        {
            var uid = Guid.Parse(userManager.GetUserId(User));
            var ObjId = _nhomDao.ThemMoiNhom(item, uid);
            ViewBag.ListNhom = _nhomDao.LayDsNhomPhanTrang(new NhomSearchItem() { RecordsPerPage = 100 }).asp_Nhoms;
            if (ObjId.ErrorCode == 0 )
            {
                if (item.type_view == StatusAction.Add.ToString())
                {
                    return RedirectToAction("ThemNhom", "SuperAdmin");
                }
                else if (item.type_view == StatusAction.View.ToString())
                {
                    return RedirectToAction("SuaNhom", "SuperAdmin", new { code = item.ma_nhom, type = StatusAction.View.ToString() });
                }
            }
            else
            {
                ViewBag.ErrorCode = ObjId.ErrorCode;
                ViewBag.ErrorMsg = ObjId.ReturnMsg;
                return View(item);
            }
            return View();
        }



        public IActionResult XoaNhom(string code, string type, Asp_nhom item)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = _nhomDao.XoaNhom(code, uid);
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/SuperAdmin/ViewNhom" });
        }

        public async Task<IActionResult> NhomPaging(int currentPage = 0, string tu_khoa = "")
        {
            NhomSearchItem nhomSearchItem = new NhomSearchItem() { CurrentPage = currentPage, tu_khoa = tu_khoa, RecordsPerPage = 10 };
            var ds = _nhomDao.LayDsNhomPhanTrang(nhomSearchItem);
            ds.Pagelist.CurrentPage = currentPage;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/nhomPaging.cshtml", ds);
            return Content(result);
        }
        #endregion
        #region Quyen
        public IActionResult QuyenDanhSach(string txtSearch = "")
        {
            EAContext eAContext = new EAContext();
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPage =  Convert.ToInt32(_config.GetSection("AppSetting").GetSection("PageSize").Value);
            if (!string.IsNullOrEmpty(txtSearch))
            {
                return View(eAContext.AspNetRoles.Where(n=>n.ma_quyen.Contains(txtSearch)).ToList());
            }
            return View(eAContext.AspNetRoles.ToList());
        }

        public IActionResult XoaQuyen(string code, string type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var _pb = 0;
            var uid = Guid.Parse(userManager.GetUserId(User));
            _pb = _nhomDao.XoaQuyen(code, uid);

            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/SuperAdmin/QuyenDanhSach" });
        }
        public IActionResult QuyenThemMoi()
        {
            ViewBag.type_view = "";
            return View(new AspNetRoles());
        }
        [HttpPost]
        public IActionResult QuyenThemMoi(AspNetRoles item)
        {
            try
            {
                EAContext eAContext = new EAContext();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                ViewBag.UidName = User.Identity.Name;
                item.Deleted = 0;
                item.CreatedDateUtc = DateTime.Now;
                var obj = eAContext.AspNetRoles.Where(n => n.ma_quyen == item.ma_quyen).Count();
                if(obj > 0)
                {
                    ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "Mã đã tồn tại trong hệ thống";
                    return View(item);
                }; 
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AspNetRoles> _role = eAContext.AspNetRoles.Add(item);
                eAContext.SaveChanges();
                var roleid = _role.Entity.Id;
                if (!string.IsNullOrEmpty(roleid.ToString()))
                {
                    ViewBag.type_view = StatusAction.View.ToString();
                    return View(item);
                }
                return View();
            }catch(Exception e)
            {
                return View(item);
            }
          
        }
        public IActionResult QuyenChinhSua(string code)
        {
            EAContext eAContext = new EAContext();
            ViewBag.type_view = StatusAction.Edit.ToString();
            return View(eAContext.AspNetRoles.Where(n => n.ma_quyen == code).FirstOrDefault());
        }
        [HttpPost]
        public IActionResult QuyenChinhSua(AspNetRoles item)
        {
            try
            {
                item.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                item.UpdatedDateUtc = DateTime.Now;
                EAContext db = new EAContext();
                db.Entry(item).State = EntityState.Added; // added row
                db.AspNetRoles.Add(item);
                db.SaveChanges();
                ViewBag.Message = "Update thành công!";
                return View(item);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra!";
                return View(item);
            }
        }
        #endregion

        #region Phân quyên
        public IActionResult PhanQuyen()
        {
            EAContext eAContext = new EAContext();
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            MenuModel nhomSearchItem = new MenuModel() { CurrentPage = 1, level = 1, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _menuDao.DanhSanhMenu(nhomSearchItem);
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            ViewBag.lstQuyen = eAContext.AspNetRoles.Where(n => n.Deleted == 0).ToList();
            ViewBag.lstVaiTro = eAContext.Asp_dm_vai_tro.Where(n=>n.Deleted == 0).ToList();
            ViewBag.lstVaiTroChucNangQuyen = eAContext.Asp_vaitro_quyen.Where(n=>n.Deleted == 0).ToList();
            return View(ds);
        }

        public async Task<IActionResult> PhanQuyenChinhSua(string ma_vai_tro)
        {
            EAContext eAContext = new EAContext();
            PhanQuyenModel phanQuyenModel = new PhanQuyenModel();
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            MenuModel nhomSearchItem = new MenuModel() { CurrentPage = 1, level = 1, tu_khoa = "", RecordsPerPage = pageSize };
            var ds = _menuDao.DanhSanhMenu(nhomSearchItem);
            phanQuyenModel.dm_Menu_Pagings = ds;
            phanQuyenModel.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + ((ds.Pagelist.TotalRecords % pageSize) > 0 ? 1 : 0);
            phanQuyenModel.CurrentPage = 1;
            phanQuyenModel.RecoredFrom = 1;
            phanQuyenModel.RecoredTo = phanQuyenModel.TotalPage == 1 ? ds.Pagelist.TotalRecords : pageSize;
            phanQuyenModel.AspNetRoles = eAContext.AspNetRoles.Where(n => n.Deleted == 0).ToList();
            phanQuyenModel.Asp_dm_vai_tro = eAContext.Asp_dm_vai_tro.Where(n => n.Deleted == 0).ToList();
            phanQuyenModel.Asp_vaitro_quyen= eAContext.Asp_vaitro_quyen.Where(n => (n.Deleted == 0 || n.Deleted == null) && n.ma_vai_tro == ma_vai_tro).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/PhanQuyenChinhSua.cshtml", phanQuyenModel);
            return Content(result);
        }
        [HttpPost]
        public IActionResult LuuPhanQuyen([FromBody]List<ObjVaitroChucNangQuyen> objVaitroChucNangQuyen)
        {
            EAContext db = new EAContext();
            Asp_vaitro_quyen item = new Asp_vaitro_quyen();
            //var obj = db.Asp_vaitro_quyen.Where(n => n.ma_vai_tro == ma_vai_tro && n.Deleted == 1).FirstOrDefault();
            var obj = db.Asp_vaitro_quyen.Where(n => n.Deleted == 1).FirstOrDefault();
            if (obj != null)
            {
                //Lawpj lstCode
                //obj.ma_chuc_nang = objVaitroChucNangQuyen.;
                //obj.ds_ma_quyen = lstRole;
                db.Attach(obj);
                db.Entry(obj).Property(p => p.Id).IsModified = true;
                db.SaveChanges();
               return RedirectToAction("PhanQuyenChinhSua", "SuperAdmin", new { ma_vai_tro = 1});
            }
            else
            {
                //foreach by chuc nang
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Asp_vaitro_quyen> _role = db.Asp_vaitro_quyen.Add(item);
                db.SaveChanges();
            }
            return View();
            //return RedirectToAction("PhanQuyenChinhSua", "SuperAdmin", new { ma_vai_tro = ma_vai_tro });
        }
        [HttpGet]
        public async Task<IActionResult> ChucNangQuyenPartial(string ma_trang = "", string ma_vai_tro = "")
        {
            EAContext db = new EAContext();
            MenuRoleModel menuRoleModel = new MenuRoleModel();
            menuRoleModel.AspNetRoles = db.AspNetRoles.Where(n => n.Deleted == 0).ToList();
            menuRoleModel.Asp_vaitro_quyen = db.Asp_vaitro_quyen.Where(n => n.Deleted == 0 && n.ma_chuc_nang == ma_trang && n.ma_vai_tro == ma_vai_tro).FirstOrDefault();
            menuRoleModel.ma_vai_tro = ma_vai_tro;
            menuRoleModel.ma_chuc_nang = ma_trang;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/ChucNangQuyenPartial.cshtml", menuRoleModel);
            return Content(result);
        }
        #endregion

    }
}
