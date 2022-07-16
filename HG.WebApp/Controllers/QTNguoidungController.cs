using HG.Data.Business.NguoiDung;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Nhom;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using HG.WebApp.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        #region nguoidung
        public IActionResult ListNguoiDung(string txtSearch = "", string ma_phong_ban = "", int trang_thai = 1, int da_xoa = 0)
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ViewBag.ma_phong_ban = ma_phong_ban;
            ViewBag.trang_thai = trang_thai;
            ViewBag.da_xoa = da_xoa;
            ViewBag.txtSearch = txtSearch;
            EAContext eAContext = new EAContext();
            HelperString stringHelper = new HelperString();
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() {tu_khoa = txtSearch, CurrentPage = 1, ma_phong_ban = ma_phong_ban, trang_thai = trang_thai, da_xoa = da_xoa, RecordsPerPage = pageSize };
            var ds = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            ViewBag.TotalRecords = ds.Pagelist.TotalRecords;
            ViewBag.ListPhongBan = eAContext.Dm_Phong_Ban.ToList();
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + 1;
            ViewBag.CurrentPage = 1;
            return View(ds.asp_Nhoms);
        }
        public string Unlock(string id)
        {
            EAContext db = new EAContext();
            var uid = Guid.Parse(id);
            var obj = db.AspNetUsers.Where(n => n.Id == uid).FirstOrDefault();
            if(obj != null)
            {
                obj.khoa_tai_khoan = 0;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return "Mở khóa thành công!";
            }
            else
            {
                return "Mở khóa thất bại!";
            }

        }
        public string ResetPassword(string id)
        {
            EAContext db = new EAContext();
            var uid = Guid.Parse(id);
            var obj = db.AspNetUsers.Where(n => n.Id == uid).FirstOrDefault();
            if (obj != null)
            {
                obj.mat_khau = "1";
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return "Mở khóa thành công!";
            }
            else
            {
                return "Mở khóa thất bại!";
            }

        }
        public async Task<IActionResult> NguoiDungPaging(int currentPage = 0, string ma_phong_ban = "", int trang_thai = 0, int da_xoa = 0, int pageSize = 0)
        {  
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = currentPage, ma_phong_ban = ma_phong_ban, trang_thai = trang_thai, da_xoa = da_xoa, RecordsPerPage = pageSize };
            var ds = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            ds.Pagelist.CurrentPage = currentPage;
            var totalRecored = ds.Pagelist.TotalRecords;
            ViewBag.TotalRecored = ds.Pagelist.TotalRecords;
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + 1;
            ViewBag.CurrentPage = 1;
            ViewBag.pageSize = pageSize;
            ViewBag.Stt = (currentPage - 1) * pageSize;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : ((currentPage - 1) * pageSize) + 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoiDung/NguoiDungPaging.cshtml", ds);
            return Content(result);
        }
        public IActionResult KiemTraNguoiDung(string code)
        {
            EAContext db = new EAContext();
            var obj = db.AspNetUsers.Where(n => n.UserName == code).FirstOrDefault();
            return obj == null ? Content("") : Content(obj.Id.ToString());
        }
        [HttpGet]
        public IActionResult ThemNguoiDung(string UserName = "")
        {
            var db = new EAContext();
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);

            return PartialView(new NguoiDungModels() { UserName = UserName});
        }

        [HttpPost]
        public async Task<IActionResult> ThemNguoiDung(NguoiDungModels item)
        {
                var UserId = Guid.Parse(userManager.GetUserId(User));
                AspNetUsers user = new AspNetUsers();
                user.UserName = item.UserName;
                user.PhoneNumber = item.PhoneNumber;
                user.Email = item.Email;
                user.mat_khau = "1";
                user.ma_chuc_vu = item.ma_chuc_vu;
                user.ma_phong_ban = item.ma_phong_ban;
                user.ho_dem = item.ho_dem;
                user.ten = item.ten;
                user.ngay_sinh = item.ngay_sinh;
                user.khoa_tai_khoan = item.khoa_tai_khoan;
                var result = await userManager.CreateAsync(user, user.mat_khau);
                var db = new EAContext();
                ViewBag.LstNhom = db.Asp_nhom.ToList();
                ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
                ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
                if (result.Succeeded)
                {
                    if (item.lstGroup != null)
                    {
                        for (int i = 0; i < item.lstGroup.Split(",").Length; i++)
                        {
                            _nguoiDungDao.ThemMoi_NguoiDung_Nhom(user.Id, item.lstGroup.Split(",")[i].ToString(), UserId);
                        }
                    }
                };
                if (result.Succeeded)
                {
                    if (item.type_view == StatusAction.Add.ToString())
                    {
                        return RedirectToAction("ThemNguoiDung", "QTnguoidung");
                    }
                    else if (item.type_view == StatusAction.View.ToString())
                    {
                        return RedirectToAction("ViewNguoiDung", "QTnguoidung", new { Id = user.Id, type = StatusAction.View.ToString() });
                    }
                }
                else
                {
                    ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "Có lỗi xảy ra !!!!";
                    return View(item);
                }
            ViewBag.ErrorCode = 1;
            ViewBag.ErrorMsg = "Có lỗi xảy ra !!!!";
            return View(item);

        }

        [HttpGet]
        public IActionResult SuaNguoiDung(Guid Id, string type)
        {
            var db = new EAContext();
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            var abc = _nguoiDungDao.LayNguoiDungBoiId(Id).asp_nhom;
            var obj = string.Join(",", _nguoiDungDao.LayNguoiDungBoiId(Id).asp_nhom.Select(n => n.ma_nhom).ToArray()) ;
            return View(_nguoiDungDao.LayNguoiDungBoiId(Id));
        }
        [HttpGet]
        public IActionResult ViewNguoiDung(Guid Id, string type)
        {
            var db = new EAContext();
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
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
                if (ObjId.ErrorCode == 0)
                {
                    if(item.lstGroup != null)
                    {
                        for (int i = 0; i < item.lstGroup.Split(",").Length; i++)
                        {
                            _nguoiDungDao.ThemMoi_NguoiDung_Nhom(item.Id, item.lstGroup.Split(",")[i].ToString(), UserId);
                        }
                    };
                    if (item.type_view == StatusAction.Add.ToString())
                    {
                        return RedirectToAction("ThemNguoiDung");
                    }
                    if (item.type_view == StatusAction.View.ToString())
                    {
                        return RedirectToAction("ViewNguoiDung", "QTnguoidung", new { Id = item.Id, type = StatusAction.View.ToString() });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "Lỗi dữ liệu!!!";
                    return PartialView(item);
                }
            }
            catch(Exception e)
            {
                  ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "Lỗi dữ liệu!!!";
                return PartialView(item);
            }
            
        }
        //Case 1
        //public Response Xoa(Guid id)
        //{
        //    return _nguoiDungDao.Xoa(id);
        //}

        //Case2
        public IActionResult XoaNnguoiDung(string Id, string type)
        {
            var _pb = 0;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
                foreach(var item in Id.Split(","))
                {
                    _pb = _nguoiDungDao.Xoa(Guid.Parse(item), uid);
                }
           
            if (_pb > 0)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/QTNguoidung/ListNguoiDung" });
        }
        public IActionResult XoaNnguoiDung2(string Id, string type)
        {
            var _pb = 0;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            foreach (var item in Id.Split(","))
            {
                _pb = _nguoiDungDao.Xoa(Guid.Parse(item), uid);
            }

            if (_pb > 0)
            {
                return RedirectToAction("ListNguoiDung");
            }
            return RedirectToAction("ListNguoiDung");
        }
        #endregion

        #region Nhom vai trò
        public IActionResult QLNhomVaitro(string txtSearch = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ViewBag.txtSearch = txtSearch;
            if (string.IsNullOrEmpty(txtSearch))
            {
                EAContext eAContext = new EAContext();
                ViewBag.TotalRecords = eAContext.Asp_nhom.Where(n => n.Deleted != 1).Count();
                ViewBag.TotalPage = (eAContext.Asp_nhom.Where(n => n.Deleted != 1).Count() / pageSize) + 1;
                ViewBag.CurrentPage = 1;
                return View(eAContext.Asp_nhom.Where(n => n.Deleted != 1).ToList());
            }
            else
            {
                EAContext eAContext = new EAContext();
                ViewBag.TotalRecords = eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).Count();
                ViewBag.TotalPage =  ( eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).Count() / pageSize ) + 1;
                ViewBag.CurrentPage = 1;
                return View(eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).ToList());
            }
           
        }
        public async Task<IActionResult> QLNhomVaitroPaging(int currentPage = 1, int pageSize = 10, string txtSearch = "")
        {

            if (string.IsNullOrEmpty(txtSearch))
            {
                EAContext eAContext = new EAContext();
                var totalRecored = eAContext.Asp_nhom.Where(n => n.Deleted != 1).Count();
                ViewBag.TotalPage = (eAContext.Asp_nhom.Where(n => n.Deleted != 1).Count() / pageSize) + 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.PageSize = pageSize;
                ViewBag.txtSearch = txtSearch;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : ((currentPage - 1) * pageSize) + 1;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/QLNhomVaitroPaging.cshtml", eAContext.Asp_nhom.Where(n => n.Deleted != 1).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList());
                return Content(result);
            }
            else
            {
                EAContext eAContext = new EAContext();
                var totalRecored = eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).Count();
                ViewBag.TotalPage = (eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).Count() / pageSize) + 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.PageSize = pageSize;
                ViewBag.txtSearch = txtSearch;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : ((currentPage - 1) * pageSize) + 1;
                ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/QLNhomVaitroPaging.cshtml", eAContext.Asp_nhom.Where(n => n.Deleted != 1 && (n.ten_nhom ?? "").Contains(txtSearch)).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList());
                return Content(result);
            }
            
        }
        //lay vai tri chitiet boi ma nhom
        public IActionResult QLNhomVaitroChitiet(string code)
         {
            ViewBag.Status = "";
            EAContext eAContext = new EAContext();
            ViewBag.ma_nhom = code;
            ViewBag.lst_nhom = eAContext.Asp_nhom.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_nguoi_dung = eAContext.AspNetUsers.ToList();
            var lst_nhom_nguoi_dung = _nguoiDungDao.GetNhomNguoiDungByMaNhom(code);
            ViewBag.lst_nhom_nguoi_dung = lst_nhom_nguoi_dung;
            ViewBag.ls_vai_tro = eAContext.Asp_dm_vai_tro.ToList();
            ViewBag.ListPhongBan = eAContext.Dm_Phong_Ban.ToList();
            //lấy vai trò theo mã nhóm
            ViewBag.lst_nhom_vaitro = _nguoiDungDao.LayVaitroThemMaNhom(code);
            //Lấy tất cả các nhóm thuộc tất cả các người dùng: đầu vào User đầu ra nhóm - user khi tạo người dùng
            var lstUserGroup = _nguoiDungDao.ListNguoiDungByMaNhom(code);
            var lstFromQTNguoiDung = "";
            if (lstUserGroup != null)
            {
                lstFromQTNguoiDung = String.Join(", ", lstUserGroup.Where(n => (lst_nhom_nguoi_dung.lst_ma_nguoi_dung ?? "").Contains(n.ma_nguoi_dung.ToString()) == false).Select(n => n.ma_nguoi_dung).ToArray());
            }
            if (lst_nhom_nguoi_dung != null)
            {
                if (lst_nhom_nguoi_dung.lst_ma_nguoi_dung != "") {
                    lst_nhom_nguoi_dung.lst_ma_nguoi_dung = lst_nhom_nguoi_dung.lst_ma_nguoi_dung + "," + lstFromQTNguoiDung;
                    ViewBag.Status = "View";
                }
                else
                {
                    lst_nhom_nguoi_dung.lst_ma_nguoi_dung = lstFromQTNguoiDung;
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult QLNhomVaitroChitiet(phong_ban_nhom_nguoi_dung item)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            item.CreatedUid = UserId;
            EAContext eAContext = new EAContext();
            ViewBag.lst_nhom = eAContext.Asp_nhom.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_nguoi_dung = eAContext.AspNetUsers.ToList();
            ViewBag.ls_vai_tro = eAContext.Asp_dm_vai_tro.ToList();
            ViewBag.ListPhongBan = eAContext.Dm_Phong_Ban.ToList();
            if (item.type_view == "Edit")
            {
                //lấy vai trò theo nhóm
                ViewBag.lst_nhom_vaitro = _nguoiDungDao.LayVaitroThemMaNhom(item.ma_nhom);
                var lst_nhom_nguoi_dung = _nguoiDungDao.GetNhomNguoiDungByMaNhom(item.ma_nhom);
                ViewBag.lst_nhom_nguoi_dung = lst_nhom_nguoi_dung;
                ViewBag.Status = "";
                //Lấy tất cả các nhóm thuộc tất cả các người dùng: đầu vào User đầu ra nhóm - user khi tạo người dùng
                var lstUserGroup = _nguoiDungDao.ListNguoiDungByMaNhom(item.ma_nhom);
                var lstFromQTNguoiDung = "";
                ViewBag.ListUsers = null;
                if (lstUserGroup != null)
                {
                    lstFromQTNguoiDung = String.Join(", ", lstUserGroup.Where(n => (lst_nhom_nguoi_dung.lst_ma_nguoi_dung ?? "").Contains(n.ma_nguoi_dung.ToString()) == false).Select(n => n.ma_nguoi_dung).ToArray());
                }
                if (lst_nhom_nguoi_dung != null)
                {
                    if (lst_nhom_nguoi_dung.lst_ma_nguoi_dung != "")
                    {
                        lst_nhom_nguoi_dung.lst_ma_nguoi_dung = lst_nhom_nguoi_dung.lst_ma_nguoi_dung + "," + lstFromQTNguoiDung;
                        //chuyển string lst Users sang List Asp User
                        ViewBag.ListUsers = ListNguoiDungDaDuocAddVaoNhom(lst_nhom_nguoi_dung.lst_ma_nguoi_dung);
                    }
                    else
                    {
                        lst_nhom_nguoi_dung.lst_ma_nguoi_dung = lstFromQTNguoiDung;
                    }
                }
                ViewBag.type_view = StatusAction.Edit.ToString();
                return View();
            }
            else
            {
                var obj = _nguoiDungDao.ThemMoiNhomNguoiDung(item);
                ViewBag.lst_nhom_nguoi_dung = _nguoiDungDao.GetNhomNguoiDungByMaNhom(item.ma_nhom);
                if (obj.ErrorCode == 0)
                {
                    item.type_view = StatusAction.View.ToString();
                }
                else
                {
                    item.type_view = StatusAction.Edit.ToString();
                }
                return View(item);
            }
        }
        public IActionResult QLNhomVaitroChitietPaging(string code)
        {
            EAContext eAContext = new EAContext();
            ViewBag.ma_nhom = code;
            ViewBag.lst_nhom = eAContext.Asp_nhom.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_nguoi_dung = eAContext.AspNetUsers.ToList();
            var lst_nhom_nguoi_dung = _nguoiDungDao.GetNhomNguoiDungByMaNhom(code);
            ViewBag.lst_nhom_nguoi_dung = lst_nhom_nguoi_dung;
            ViewBag.ls_vai_tro = eAContext.Asp_dm_vai_tro.ToList();
            //lấy vai trò theo mã nhóm
            ViewBag.lst_nhom_vaitro = _nguoiDungDao.LayVaitroThemMaNhom(code);
            return PartialView();
        }
        public async Task<IActionResult> ThemNhomVaitro(string code, string lstvaitro = "")
        {
            VaitroModel vaitroModel = new VaitroModel();
            EAContext eAContext = new EAContext();
            ViewBag.ma_nhom = code;
            ViewBag.lst_nhom_vaitro = lstvaitro;
            vaitroModel.asp_Dm_Vai_Tro = eAContext.Asp_dm_vai_tro.ToList();
            vaitroModel.danh_sach_nhom_vai_tro = lstvaitro;
            var res = _nguoiDungDao.ThemNhomVaitro(code, lstvaitro);
            if(res.ErrorCode == 0)
            {
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/ThemNhomVaitro.cshtml", vaitroModel);
                return Content(result);
            }
            else
            {
                return View();
            }

        }

        public List<AspNetUsers> ListNguoiDungDaDuocAddVaoNhom(string lstusersbynhom)
        {
            EAContext db = new EAContext();
            var result = new List<AspNetUsers>();
            if(lstusersbynhom != null && !string.IsNullOrEmpty(lstusersbynhom))
            {
                var abc = lstusersbynhom.Split(",");
                foreach (var item in lstusersbynhom.Split(","))
                {
                    var obj = db.AspNetUsers.Where(n => n.Id == Guid.Parse(item)).FirstOrDefault();
                    if(obj != null) { result.Add(obj); }
                }
                return result;
            }
            return new List<AspNetUsers>();
        }

        public async Task<IActionResult> Nguoidungbyphongban(string ma_phong_ban = "", string lstUsershaschecked = "")
        {
            try
            {
                EAContext db = new EAContext();
                var lstUsers = db.AspNetUsers.Where(n => n.ma_phong_ban == ma_phong_ban && (lstUsershaschecked ?? "").Contains(n.Id.ToString()) == false).ToList();
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/NguoiDungByPhongBan.cshtml", lstUsers);
                return Content(result);
            }
            catch (Exception e)
            {
                return Content("");
            }
           
        } 
        public async Task<IActionResult> FETCHUSERS(string lstUsershaschecked = "")
        {
            try
            {
                EAContext db = new EAContext();
                var result = new List<AspNetUsers>();
                if (lstUsershaschecked != null && !string.IsNullOrEmpty(lstUsershaschecked))
                {
                    var abc = lstUsershaschecked.Split(",");
                    foreach (var item in lstUsershaschecked.Split(","))
                    {
                        var obj = db.AspNetUsers.Where(n => n.Id == Guid.Parse(item)).FirstOrDefault();
                        if (obj != null) { result.Add(obj); }
                    }
                }
                var resultC = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/NguoiDungDaChon.cshtml", result);
                return Content(resultC);
            }
            catch (Exception e)
            {
                return Content("");
            }
           
        }

        public async Task<IActionResult> ThayDoiNhomNguoiDung(string lstUsershaschecked = "")
        {
            try
            {
                EAContext db = new EAContext();
                var lstUsers = db.AspNetUsers.ToList();
                ViewBag.lst_nhom_nguoi_dung = lstUsershaschecked;
                var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/ThayDoiNhomNguoiDung.cshtml", lstUsers);
                return Content(result);
            }
            catch (Exception e)
            {
                return Content("");
            }

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
