using Microsoft.AspNetCore.Mvc;
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
using Newtonsoft.Json;

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
        public IActionResult ViewNhom(int currentPage = 1,int pageSize = 25, string txtSearch = "")
        {
            ViewBag.txtSearch = txtSearch;
            if (string.IsNullOrEmpty(txtSearch))
            {
                EAContext eAContext = new EAContext();
                ViewBag.TotalRecords = eAContext.Asp_nhom.Where(n => n.Deleted != 1).Count();
                ViewBag.TotalPage = eAContext.Asp_nhom.Where(n => n.Deleted != 1).Count() / pageSize;
                ViewBag.CurrentPage = 1;
                return View(eAContext.Asp_nhom.Where(n => n.Deleted == 0).ToList());
            }
            else
            {
                EAContext eAContext = new EAContext();
                ViewBag.TotalRecords = eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).Count();
                ViewBag.TotalPage = eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).Count() / pageSize;
                ViewBag.CurrentPage = 1;
                return View(eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).ToList());
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> SuaNhom(string code, string type)
        {
            var obj = _nhomDao.LayNhomId(code);
            ViewBag.ListNhom = _nhomDao.LayDsNhomPhanTrang(new NhomSearchItem() { RecordsPerPage = 100 }).asp_Nhoms;
            if (type == StatusAction.Edit.ToString())
            {
                ViewBag.UserCreated = "";
                ViewBag.UserUpdate = "";
                if (obj != null && obj.CreatedUid != null)
                {
                    var UserCreated = await userManager.FindByIdAsync(obj.CreatedUid.ToString());
                    ViewBag.UserCreated = UserCreated.ho_dem + " " + UserCreated.ten;
                };
                if (obj != null && obj.UpdatedUid != null)
                {
                    var UserUpdate = await userManager.FindByIdAsync(obj.UpdatedUid.ToString());
                    ViewBag.UserUpdate = UserUpdate.ho_dem + " " + UserUpdate.ten;
                };
                ViewBag.type_view = StatusAction.Edit.ToString();
                return View(obj);
            }
            else
            {
                ViewBag.UserCreated = "";
                ViewBag.UserUpdate = "";
                if (obj != null && obj.CreatedUid != null)
                {
                    var UserCreated = await userManager.FindByIdAsync(obj.CreatedUid.ToString());
                    ViewBag.UserCreated = UserCreated.ho_dem + " " + UserCreated.ten;
                };
                if (obj != null && obj.UpdatedUid != null)
                {
                    var UserUpdate = await userManager.FindByIdAsync(obj.UpdatedUid.ToString());
                    ViewBag.UserUpdate = UserUpdate.ho_dem + " " + UserUpdate.ten;
                };
                ViewBag.type_view = StatusAction.View.ToString();
                return View(obj);
            }
          
        }
        [HttpPost]
        public IActionResult SuaNhom(NhomModel item)
        {
            ViewBag.ListNhom = _nhomDao.LayDsNhomPhanTrang(new NhomSearchItem() { RecordsPerPage = 100 }).asp_Nhoms;
            item.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
            if (item.type_view == StatusAction.Add.ToString())
            {
                var ObjId = _nhomDao.ChinhSuaNhom(item);
                if (ObjId.ErrorCode == 0)
                {
                    ViewBag.type_view = StatusAction.Add.ToString();
                    return RedirectToAction("ThemNhom");
                }
                else
                {
                    ViewBag.ErrorCode = ObjId.ErrorCode;
                    ViewBag.ErrorMsg = ObjId.ReturnMsg;
                    return View(item);
                }
               
            }
            else
            {
                var ObjId = _nhomDao.ChinhSuaNhom(item);
                if (ObjId.ErrorCode == 0)
                {
                    ViewBag.type_view = StatusAction.View.ToString();
                    return View(item);
                }
                else
                {
                    ViewBag.ErrorCode = ObjId.ErrorCode;
                    ViewBag.ErrorMsg = ObjId.ReturnMsg;
                    return View(item);
                }
            }
        }
        public IActionResult KiemTraMaNhom(string code)
        {
            EAContext db = new EAContext();
            var obj = db.Asp_nhom.Where(n => n.ma_nhom == code && n.Deleted != 1).FirstOrDefault();
            return obj == null ? Content("") : Content("True");
        }
        [HttpGet]
        public IActionResult ThemNhom(string ma_nhom = "")
        {
            var ds = _nhomDao.LayDsNhomPhanTrang(new NhomSearchItem() { RecordsPerPage = 100});
            ViewBag.ListNhom = ds.asp_Nhoms;
            return View(new Asp_nhom() { ma_nhom = ma_nhom});
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

        public async Task<IActionResult> NhomPaging(int currentPage = 1, int pageSize = 10, string tu_khoa = "")
        {
            NhomSearchItem nhomSearchItem = new NhomSearchItem() { CurrentPage = currentPage, tu_khoa = tu_khoa, RecordsPerPage = pageSize };
            var ds = _nhomDao.LayDsNhomPhanTrang(nhomSearchItem);
            ds.Pagelist.CurrentPage = currentPage;
            ViewBag.TotalRecords = ds.asp_Nhoms.Count();
            ViewBag.PageSize = pageSize;
            ViewBag.TuKhoa = tu_khoa;
            ViewBag.Stt = (currentPage - 1) * pageSize;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/nhomPaging.cshtml", ds);
            return Content(result);
        }
        #endregion
        #region Quyen
        public IActionResult QuyenDanhSach(string txtSearch = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            EAContext eAContext = new EAContext();
            ViewBag.CurrentPage = 1;
            ViewBag.txtSearch = txtSearch;
            ViewBag.TotalPage =  Convert.ToInt32(_config.GetSection("AppSetting").GetSection("PageSize").Value);
            if (!string.IsNullOrEmpty(txtSearch))
            {
                var totalRecouds = eAContext.AspNetRoles.Where(n => n.Name.Contains(txtSearch) && n.Deleted != 1).Count();
                ViewBag.TotalPage = (totalRecouds / pageSize) + 1;
                ViewBag.CurrentPage = 1;
                return View(eAContext.AspNetRoles.Where(n=>n.Name.Contains(txtSearch) && n.Deleted != 1).ToList());
            }
            else
            {
                var totalRecouds = eAContext.AspNetRoles.Where(n => n.Deleted != 1).Count();
                ViewBag.TotalPage = (totalRecouds / pageSize) + 1;
                ViewBag.CurrentPage = 1;
                return View(eAContext.AspNetRoles.Where(n => n.Deleted != 1).ToList());
            }
            
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
        public IActionResult QuyenThemMoi(string ma_quyen = "")
        {
            EAContext db = new EAContext();
            ViewBag.lst_nguoi_dung = db.AspNetRoles.Where(n => n.Deleted != 1).ToList();
            ViewBag.type_view = "";
            return View(new AspNetRoles() { ma_quyen = ma_quyen });
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
                if (obj > 0)
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
        public async Task<IActionResult> QuyenPaging(int currentPage = 1, int pageSize = 10, string tu_khoa = "")
        {
            EAContext eAContext = new EAContext();
            ViewBag.TotalPage = Convert.ToInt32(_config.GetSection("AppSetting").GetSection("PageSize").Value);
            if (!string.IsNullOrEmpty(tu_khoa))
            {
                ViewBag.Stt = (currentPage - 1) * pageSize;
                var totalRecouds = eAContext.AspNetRoles.Where(n => n.Name.Contains(tu_khoa) && n.Deleted != 1).Count();
                ViewBag.TotalPage = (totalRecouds / pageSize) + 1;
                ViewBag.CurrentPage = 1;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/QuyenPaging.cshtml", eAContext.AspNetRoles.Where(n => n.Name.Contains(tu_khoa) && n.Deleted != 1).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList());
                return Content(result);
            }
            else
            {
                ViewBag.Stt = (currentPage - 1) * pageSize;
                var totalRecouds = eAContext.AspNetRoles.Where(n => n.Deleted != 1).Count();
                ViewBag.TotalPage = (totalRecouds / pageSize) + 1;
                ViewBag.CurrentPage = 1;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/QuyenPaging.cshtml", eAContext.AspNetRoles.Where(n => n.Deleted != 1).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList());
                return Content(result);
            }
           
        }
        public IActionResult KiemTraMaQuyen(string code)
        {
            EAContext db = new EAContext();
            var obj = db.AspNetRoles.Where(n => n.ma_quyen == code && n.Deleted != 1).FirstOrDefault();
            return obj == null ? Content("") : Content("True");
        }
        public async Task<IActionResult> QuyenChinhSua(string code, string type)
        {

            if (type == StatusAction.Edit.ToString())
            {
                EAContext eAContext = new EAContext();
                ViewBag.type_view = StatusAction.Edit.ToString();
                var obj = eAContext.AspNetRoles.Where(n => n.ma_quyen == code).FirstOrDefault();
                ViewBag.UserCreated = "";
                ViewBag.UserUpdate = "";
                if (obj != null && obj.CreatedUid != null)
                {
                    var UserCreated = await userManager.FindByIdAsync(obj.CreatedUid.ToString());
                    ViewBag.UserCreated = UserCreated.ho_dem + " " + UserCreated.ten;
                };
                if(obj != null && obj.UpdatedUid != null)
                {
                    var UserUpdate = await userManager.FindByIdAsync(obj.UpdatedUid.ToString());
                    ViewBag.UserUpdate = UserUpdate.ho_dem + " " + UserUpdate.ten;
                };
                ViewBag.lst_nguoi_dung = eAContext.AspNetRoles.Where(n => n.Deleted != 1).ToList();
                return View(obj);
            }
            else
            {
                EAContext eAContext = new EAContext();
                ViewBag.type_view = StatusAction.View.ToString();
                ViewBag.UserCreated = "";
                ViewBag.UserUpdate = "";
                var obj = eAContext.AspNetRoles.Where(n => n.ma_quyen == code).FirstOrDefault();
                ViewBag.UserCreated = "";
                ViewBag.UserUpdate = "";
                if (obj != null && obj.CreatedUid != null)
                {
                    var UserCreated = await userManager.FindByIdAsync(obj.CreatedUid.ToString());
                    ViewBag.UserCreated = UserCreated.ho_dem + " " + UserCreated.ten;
                }
                if (obj != null && obj.UpdatedUid != null)
                {
                    var UserUpdate = await userManager.FindByIdAsync(obj.UpdatedUid.ToString());
                    ViewBag.UserUpdate = UserUpdate.ho_dem + " " + UserUpdate.ten;
                }
                ViewBag.lst_nguoi_dung = eAContext.AspNetRoles.Where(n => n.Deleted != 1).ToList();
                return View(obj);
            }
        }
        [HttpPost]
        public IActionResult QuyenChinhSua(AspNetRoles item, string type_view)
        {
            try
            {
                EAContext db = new EAContext();
                var obj = db.AspNetRoles.AsNoTracking().Where(n => n.ma_quyen == item.ma_quyen).FirstOrDefault();
                if (type_view == StatusAction.Add.ToString() && obj != null )
                {
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    obj.Stt = item.Stt;
                    obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    obj.UpdatedDateUtc = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.type_view = StatusAction.Add.ToString();
                    return RedirectToAction("QuyenThemMoi");
                }
                else
                {
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    obj.Stt = item.Stt;
                    obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    obj.UpdatedDateUtc = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.type_view = StatusAction.View.ToString();
                    return View(item);
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Có lỗi xảy ra!";
                ViewBag.ErrorCode = 1;
                return View(item);
            }
        }
        #endregion

        #region Phân quyên
        public IActionResult PhanQuyen()
        {
            EAContext eAContext = new EAContext();
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            MenuModel nhomSearchItem = new MenuModel() { CurrentPage = 1, level = 1, tu_khoa = "", RecordsPerPage = 0 };
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
        public IActionResult LuuPhanQuyen(string objVaitroChucNangQuyen)
        {
            var result = JsonConvert.DeserializeObject<Root>(objVaitroChucNangQuyen);
            EAContext db = new EAContext();
            if (result != null)
            {
                foreach (var item in result.objVaitroChucNangQuyen)
                {
                    if (item != null) { 
                            var obj = db.Asp_vaitro_quyen.Where(n => n.ma_vai_tro == item.ma_vai_tro && n.Deleted == 0 && n.ma_chuc_nang == item.ma_chuc_nang).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.ds_ma_quyen = item.ds_quyen_da_chon == null ? "" : item.ds_quyen_da_chon;
                                obj.UpdatedDateUtc = DateTime.Now;
                                db.Entry(obj).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                Asp_vaitro_quyen asp_Vaitro_Quyen = new Asp_vaitro_quyen() { ma_vai_tro = item.ma_vai_tro, ma_chuc_nang = item.ma_chuc_nang, ds_ma_quyen = item.ds_quyen_da_chon, CreatedDateUtc = DateTime.Now, Deleted = 0 };
                                 Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Asp_vaitro_quyen> _role = db.Asp_vaitro_quyen.Add(asp_Vaitro_Quyen);
                                 db.SaveChanges();
                            }
                    };
                }

            };
            var ds_ma_vai_tro = result.objVaitroChucNangQuyen.FirstOrDefault();
            return RedirectToAction("PhanQuyenChinhSua", "SuperAdmin", new { ma_vai_tro = (ds_ma_vai_tro == null)? "" : ds_ma_vai_tro.ma_vai_tro });
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
        #region VaiTro
        public IActionResult VaiTro(string txtSearch = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var vaitro = new List<Asp_dm_vai_tro>();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                using (var db = new EAContext())
                {
                    var ds = db.Asp_dm_vai_tro.Where(n => n.Deleted == 0 && (n.ten_vai_tro ?? "").Contains(txtSearch)).ToList();
                    vaitro = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    var ds = db.Asp_dm_vai_tro.Where(n => n.Deleted == 0 && (n.ten_vai_tro ?? "").Contains(txtSearch)).ToList();
                    vaitro = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                }
            }
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            return View(vaitro);
        }

        public async Task<IActionResult> VaiTroPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            var totalRecored = 0;
            var result = "";
            ViewBag.Stt = (currentPage - 1) * pageSize;
            if (string.IsNullOrEmpty(tu_khoa))
            {
                using (var db = new EAContext())
                {
                    var ds = db.Asp_dm_vai_tro.Where(n => n.Deleted == 0).ToList();
                    var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                    ViewBag.TotalRecored = totalRecored;
                    ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                    ViewBag.CurrentPage = currentPage;
                    ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                    ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                    result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/VaiTroPaging.cshtml", datapage);

                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    var ds = db.Asp_dm_vai_tro.Where(n => n.Deleted == 0 && (n.ten_vai_tro ?? "").Contains(tu_khoa)).ToList();
                    var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                    ViewBag.TotalRecored = totalRecored;
                    ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                    ViewBag.CurrentPage = currentPage;
                    ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                    ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                    result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/SuperAdmin/VaiTroPaging.cshtml", datapage);

                }
            }
           
            return Content(result);
        }
        public async Task<IActionResult> VaiTroView(string code, string type)
        {
            var vai_Tro = new Asp_dm_vai_tro();
            using (var db = new EAContext())
            {
                vai_Tro = db.Asp_dm_vai_tro.Where(n => n.Deleted == 0 && n.ma_vai_tro == code).FirstOrDefault();
            }
            if(vai_Tro != null && vai_Tro.UpdatedUid != null)
            {
                ViewBag.UidName = await userManager.GetUserNameAsync(await userManager.FindByIdAsync(vai_Tro.UpdatedUid.ToString()));
            }
            ViewBag.type_view = type;
            return View(vai_Tro);
        }
        #endregion Vaitro
    }
}
