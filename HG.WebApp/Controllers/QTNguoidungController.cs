using HG.Data.Business.NguoiDung;
using HG.Data.Business.Sys;
using HG.Entities;
using HG.Entities.Entities;
using HG.Data.Business.ThuTuc;
using HG.Entities.Entities.Nhom;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using HG.WebApp.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HG.Entities.Entities.Model;
using System.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace HG.WebApp.Controllers
{
    public class QTNguoidungController : BaseController
    {
        private readonly NguoiDungDao _nguoiDungDao;
        private readonly ThuTucDao _thuTucDao;
        private readonly ILogger<QTNguoidungController> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        private readonly SystemDao _sys;
        private readonly HG.Data.Business.HoSo.HoSoDao _hoso;
        public QTNguoidungController(ILogger<QTNguoidungController> logger, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
       : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _nguoiDungDao = new NguoiDungDao(DbProvider);
            _sys = new SystemDao(DbProvider);
            _hoso = new HG.Data.Business.HoSo.HoSoDao(DbProvider);
        }
        #region nguoidung
        public IActionResult ListNguoiDung(string txtSearch = "", string ma_phong_ban = "", int trang_thai = 1, int da_xoa = 0)
          {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ViewBag.ma_phong_ban = ma_phong_ban;
            ViewBag.trang_thai = trang_thai;
            ViewBag.da_xoa = da_xoa;
            ViewBag.txtSearch = txtSearch;
            EAContext eAContext = new EAContext();
            HelperString stringHelper = new HelperString();
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = ma_phong_ban, tu_khoa = txtSearch, trang_thai = 0, da_xoa = 0, RecordsPerPage = pageSize };
            var ds = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            ViewBag.TotalRecords = ds.Pagelist.TotalRecords;
            ViewBag.ListPhongBan = _nguoiDungDao.DanhSachPhongBanByDonVi(UserId);
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
        public async Task<bool> ResetPassword(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, token, "1");
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
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
        public async Task<IActionResult> NguoiDungOnlinePaging(int currentPage = 0, int trang_thai = 0, int da_xoa = 0, int pageSize = 0)
        {
            NguoiDungOnlSearchItem nguoidungOnlSearchItem = new NguoiDungOnlSearchItem() { CurrentPage = currentPage,  trang_thai = trang_thai, da_xoa = da_xoa, RecordsPerPage = pageSize };
            var ds = _nguoiDungDao.LayDsNguoiDungOnlPhanTrang(nguoidungOnlSearchItem);
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
            var UserId = Guid.Parse(userManager.GetUserId(User));
            ViewBag.lst_phong_ban = _nguoiDungDao.DanhSachPhongBanByDonVi(UserId);
            var db = new EAContext();
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);

            return PartialView(new NguoiDungModels() { UserName = UserName});
        }
        [HttpGet]
        public IActionResult ThemNguoiDungOnline(string UserName = "")
        {
            var db = new EAContext();
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            var UserId = Guid.Parse(userManager.GetUserId(User));
            ViewBag.lst_phong_ban = _nguoiDungDao.DanhSachPhongBanByDonVi(UserId);
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            NguoiDungOnlSearchItem nguoidungOnlSearchItem = new NguoiDungOnlSearchItem() { CurrentPage = 1, trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungOnlPhanTrang(nguoidungOnlSearchItem);

            return PartialView(new UserOnlineModels { UserName = UserName });
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
                user.CreatedUid = UserId;
                user.CreatedDateUtc = DateTime.Now;
                user.IsAdministratorPB = item.IsAdministratorPB;
                user.IsAdministratorDV = item.IsAdministratorDV;
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
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var db = new EAContext();
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.lst_phong_ban = _nguoiDungDao.DanhSachPhongBanByDonVi(UserId);
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            var abc = _nguoiDungDao.LayNguoiDungBoiId(Id).asp_nhom;
            if(abc != null)
            {
                var obj = string.Join(",", _nguoiDungDao.LayNguoiDungBoiId(Id).asp_nhom.Select(n => n.ma_nhom).ToArray());
            }
            var _user = _nguoiDungDao.LayNguoiDungBoiId(Id);
            //lấy từ nhóm và vai trò
            var NhomByUser = _nguoiDungDao.LayNhomBoiNguoiDung(Id);
            if (NhomByUser != null)
            {
                _user.asp_nhom.AddRange(NhomByUser);
            };
            return View(_user);
        }
        [HttpGet]
        public IActionResult ViewNguoiDung(Guid Id, string type)
        {
            var db = new EAContext();
            var UserId = Guid.Parse(userManager.GetUserId(User));
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.ListPhongBan = _nguoiDungDao.DanhSachPhongBanByDonVi(UserId);
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);
            var _user = _nguoiDungDao.LayNguoiDungBoiId(Id);
            //lấy từ nhóm và vai trò
            var NhomByUser = _nguoiDungDao.LayNhomBoiNguoiDung(Id);
            if (NhomByUser != null)
            {
                _user.asp_nhom.AddRange(NhomByUser);
            };
            return View(_user);
        }
       
        [HttpPost]
        public IActionResult SuaNguoiDung(NguoiDungModels item, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var ObjId = _nguoiDungDao.SuaNguoiDung(item, UserId);
            try {
                if (ObjId.ErrorCode == 0)
                {
                    //lấy từ nhóm và vai trò
                    var NhomByUser = _nguoiDungDao.LayNhomBoiNguoiDung(item.Id);
                    var NhomByUserString = "";
                    if (NhomByUser != null)
                    {
                        NhomByUserString = string.Join(",", NhomByUser.Select(n => n.ma_nhom).ToArray());
                    };
                    if (item.lstGroup != null)
                    {
                        for (int i = 0; i < item.lstGroup.Split(",").Length; i++)
                        {
                            if (!NhomByUserString.Contains(item.lstGroup.Split(",")[i].ToString()))
                            {
                                _nguoiDungDao.ThemMoi_NguoiDung_Nhom(item.Id, item.lstGroup.Split(",")[i].ToString(), UserId);
                            }
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
        #region uyquyenxuly
        public IActionResult UyQuyenXuLy(string txtSearch = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            // ViewBag.ma_phong_ban = ma_phong_ban;
           
            ViewBag.txtSearch = txtSearch;
            EAContext eAContext = new EAContext();
            HelperString stringHelper = new HelperString();
            NguoiDungOnlSearchItem nguoidungOnlSearchItem = new NguoiDungOnlSearchItem() { tu_khoa = txtSearch, CurrentPage = 1, RecordsPerPage = pageSize };
            var user = userManager.GetUserId(User);
            var ds = _nguoiDungDao.LayDsUyQuyenPhanTrang(nguoidungOnlSearchItem,user);
            ViewBag.TotalRecords = ds.Pagelist.TotalRecords;
            // ViewBag.ListPhongBan = eAContext.Dm_Phong_Ban.ToList();
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + 1;
            ViewBag.CurrentPage = 1;
            return View(ds.listUyQuyen);
        }
        [HttpGet]
        public IActionResult ThemUyQuyenXuLy(string UserName = "")
        {
            var db = new EAContext();
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.UserName = userManager.GetUserName(User);
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, tu_khoa = "", RecordsPerPage = pageSize };
            ViewBag.lst_thu_tuc_hc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);

            return PartialView(new UyQuyenXuLyModel() { Id_nguoi_duoc_uy_quyen = null });
        }
        [HttpGet]
        public IActionResult SuaUyQuyenXuLy(string Id)
        {
            var db = new EAContext();
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.UserName = userManager.GetUserName(User);
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, tu_khoa = "", RecordsPerPage = pageSize };
            ViewBag.lst_thu_tuc_hc = _thuTucDao.DanhSanhThuTuc(nhomSearchItem).lstThuTuc;
            NguoiDungSearchItem nguoidungSearchItem = new NguoiDungSearchItem() { CurrentPage = 1, ma_phong_ban = "", trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungPhanTrang2(nguoidungSearchItem);

            return PartialView(_nguoiDungDao.GetUyQuyenbyId(Id));
        }
        [HttpPost]
        public async Task<IActionResult> ThemUyQuyenXuLy(UyQuyenXuLyModel item)
        {
            var UserId = userManager.GetUserId(User);
            var result = _nguoiDungDao.AddUserUyQuyen(item, UserId);
            //ViewBag.LstNhom = db.Asp_nhom.ToList();
            //ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
            //ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
           
            if (result == "Ok")
            {
                return RedirectToAction("UyQuyenXuLy", "QTnguoidung");
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
        [HttpPost]
        public async Task<IActionResult> SuaUyQuyenXuLy(UyQuyenXuLyModel item)
        {
            
            var result = _nguoiDungDao.UpdateUserUyQuyen(item);
            //ViewBag.LstNhom = db.Asp_nhom.ToList();
            //ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
            //ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();

            if (result == "Ok")
            {
                return RedirectToAction("UyQuyenXuLy", "QTnguoidung");
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
        #endregion
        #region nguoidungonl
        public IActionResult ListNguoiDungOnline(string txtSearch = "",  int trang_thai = 1, int da_xoa = 0)
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
           // ViewBag.ma_phong_ban = ma_phong_ban;
            ViewBag.trang_thai = trang_thai;
            ViewBag.da_xoa = da_xoa;
            ViewBag.txtSearch = txtSearch;
            EAContext eAContext = new EAContext();
            HelperString stringHelper = new HelperString();
            NguoiDungOnlSearchItem nguoidungOnlSearchItem = new NguoiDungOnlSearchItem() { tu_khoa = txtSearch, CurrentPage = 1,  trang_thai = trang_thai, da_xoa = da_xoa, RecordsPerPage = pageSize };
            var ds = _nguoiDungDao.LayDsNguoiDungOnlPhanTrang(nguoidungOnlSearchItem);
            ViewBag.TotalRecords = ds.Pagelist.TotalRecords;
           // ViewBag.ListPhongBan = eAContext.Dm_Phong_Ban.ToList();
            ViewBag.TotalPage = (ds.Pagelist.TotalRecords / pageSize) + 1;
            ViewBag.CurrentPage = 1;
            return View(ds.asp_Nhoms);
        }
        [HttpPost]
        public async Task<IActionResult> ThemNguoiDungOnline(UserOnlineModels item)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            AspNetUsers user = new AspNetUsers();
            user.UserName = item.Email;
            user.PhoneNumber = item.PhoneNumber;
            user.Email = item.Email;
            user.mat_khau = item.mat_khau;
            user.anh_dai_dien = item.anh_dai_dien;
            user.anh_cmt = item.anh_cmt;
            user.ho_khau_tt = item.ho_khau_tt;
            user.ten = item.ten;
            user.Type = 1;
            user.khoa_tai_khoan = item.khoa_tai_khoan;
            var result = await userManager.CreateAsync(user, item.mat_khau);
            var db = new EAContext();
            //ViewBag.LstNhom = db.Asp_nhom.ToList();
            //ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
            //ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            if (result.Succeeded)
            {
                //if (item.lstGroup != null)
                //{
                //    for (int i = 0; i < item.lstGroup.Split(",").Length; i++)
                //    {
                //        _nguoiDungDao.ThemMoi_NguoiDung_Nhom(user.Id, item.lstGroup.Split(",")[i].ToString(), UserId);
                //    }
                //}
            };
            if (result.Succeeded)
            {
                if (item.type_view == StatusAction.Add.ToString())
                {
                    return RedirectToAction("listNguoiDungOnline", "QTnguoidung");
                }
                else if (item.type_view == StatusAction.View.ToString())
                {
                    return RedirectToAction("ViewNguoiDungOnline", "QTnguoidung", new { Id = user.Id, type = StatusAction.View.ToString() });
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
        public IActionResult SuaNguoiDungOnline(Guid Id, string type)
        {
            var db = new EAContext();
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            NguoiDungOnlSearchItem nguoidungOnlSearchItem = new NguoiDungOnlSearchItem() { CurrentPage = 1,  trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungOnlPhanTrang(nguoidungOnlSearchItem);
            //var abc = _nguoiDungDao.LayNguoiDungOnlBoiId(Id).asp_nhom;
            //var obj = string.Join(",", _nguoiDungDao.LayNguoiDungOnlBoiId(Id).asp_nhom.Select(n => n.ma_nhom).ToArray());
            return View(_nguoiDungDao.LayNguoiDungOnlBoiId(Id));
        }
        [HttpGet]
        public IActionResult ViewNguoiDungOnline(Guid Id, string type)
        {
            var db = new EAContext();
            ViewBag.LstNhom = db.Asp_nhom.ToList();
            ViewBag.lst_phong_ban = db.Dm_Phong_Ban.ToList();
            ViewBag.lst_chuc_vu = db.Dm_Chuc_Vu.ToList();
            NguoiDungOnlSearchItem nguoidungOnlSearchItem = new NguoiDungOnlSearchItem() { CurrentPage = 1, trang_thai = 0, da_xoa = 0, RecordsPerPage = 100 };
            ViewBag.ListNguoiDung = _nguoiDungDao.LayDsNguoiDungOnlPhanTrang(nguoidungOnlSearchItem);
            return View(_nguoiDungDao.LayNguoiDungOnlBoiId(Id));
        }
        [HttpPost]
        public IActionResult SuaNguoiDungOnline(UserOnlineModels item, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            var ObjId = _nguoiDungDao.SuaNguoiDungOnline(item, UserId);
            try
            {
                if (ObjId.ErrorCode == 0)
                {

                    if (item.type_view == StatusAction.Add.ToString())
                    {
                        return RedirectToAction("ThemNguoiDungOnline");
                    }
                    if (item.type_view == StatusAction.View.ToString())
                    {
                        return RedirectToAction("ViewNguoiDungOnline", "QTnguoidung", new { Id = item.Id, type = StatusAction.View.ToString() });
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
            catch (Exception e)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Lỗi dữ liệu!!!";
                return PartialView(item);
            }
        }
        public IActionResult XoaNnguoiDungOnline(string Id, string type)
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
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/QTNguoidung/ListNguoiDungOnline" });
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
            int pageSize = 25;
            var currentPage = 1;
            ViewBag.Status = "";
            EAContext eAContext = new EAContext();
            ViewBag.ma_nhom = code;
            ViewBag.lst_nhom = eAContext.Asp_nhom.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_nguoi_dung = _hoso.DanhSachNguoiDung();
            var lst_nhom_nguoi_dung = _nguoiDungDao.GetNhomNguoiDungByMaNhom(code);
            ViewBag.lst_nhom_nguoi_dung = lst_nhom_nguoi_dung;
            ViewBag.ls_vai_tro = eAContext.Asp_dm_vai_tro.ToList();
            ViewBag.ListPhongBan = eAContext.Dm_Phong_Ban.ToList();
            //paging vaitro
            var totalRecored = eAContext.Asp_dm_vai_tro.Count();
            ViewBag.Stt = (currentPage - 1) * pageSize;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            //lấy vai trò theo mã nhóm
            ViewBag.lst_nhom_vaitro = _nguoiDungDao.LayVaitroThemMaNhom(code);
            //Lấy tất cả các nhóm thuộc tất cả các người dùng: đầu vào User đầu ra nhóm - user khi tạo người dùng
            var lstUserGroup = _nguoiDungDao.ListNguoiDungByMaNhom(code);
            var lstFromQTNguoiDung = "";
            if (lstUserGroup != null && lst_nhom_nguoi_dung != null)
            {
                lstFromQTNguoiDung = String.Join(", ", lstUserGroup.Where(n => (lst_nhom_nguoi_dung.lst_ma_nguoi_dung ?? "").Contains(n.ma_nguoi_dung.ToString()) == false).Select(n => n.ma_nguoi_dung).ToArray());
            }
            if (lst_nhom_nguoi_dung != null)
            {
                ViewBag.type_view = StatusAction.View.ToString();
                if (lst_nhom_nguoi_dung.lst_ma_nguoi_dung != "") {
                    lst_nhom_nguoi_dung.lst_ma_nguoi_dung = lst_nhom_nguoi_dung.lst_ma_nguoi_dung + "," + lstFromQTNguoiDung;
                }
                else
                {
                    lst_nhom_nguoi_dung.lst_ma_nguoi_dung = lstFromQTNguoiDung; 
                }
            }
            else
            {
                ViewBag.type_view = StatusAction.Add.ToString();
            }
           
            return View();
        }
        [HttpPost]
        public IActionResult QLNhomVaitroChitiet(phong_ban_nhom_nguoi_dung item)
        {
            int pageSize = 25;
            var currentPage = 1;
            var UserId = Guid.Parse(userManager.GetUserId(User));
            item.CreatedUid = UserId;
            EAContext eAContext = new EAContext();
            ViewBag.lst_nhom = eAContext.Asp_nhom.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_nguoi_dung = _hoso.DanhSachNguoiDung();
            ViewBag.ls_vai_tro = eAContext.Asp_dm_vai_tro.ToList();
            ViewBag.ListPhongBan = eAContext.Dm_Phong_Ban.ToList();
            var totalRecored = eAContext.Asp_dm_vai_tro.Count();
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = 1;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.ma_nhom = item.ma_nhom;
            ViewBag.Stt = (currentPage - 1) * pageSize;
            if (item.type_view == StatusAction.Edit.ToString())
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
                ViewBag.type_view = StatusAction.Add.ToString();
                return View();
            }
            else
            {
                //Asp_user_nhom lấy người dùng thuộc nhóm này, nếu người dùng nào ko có trong list này sẽ bị xóa
                if (item != null)
                {
                    var lstUserGroup = _nguoiDungDao.ListNguoiDungByMaNhom(item.ma_nhom);
                    foreach (var k in lstUserGroup)
                    {
                        if (!item.lstGroup.Contains(k.ma_nguoi_dung.ToString().ToLower()))
                        {
                            _nguoiDungDao.XoaNguoiDungByMaNhom(item.ma_nhom, k.ma_nguoi_dung);
                            //ko tồn tại thì xóa
                        }
                    }
                }
                var obj = _nguoiDungDao.ThemMoiNhomNguoiDung(item);
                //remove nếu người dùng bị xóa nằm trong nhóm user 

                ViewBag.lst_nhom_nguoi_dung = _nguoiDungDao.GetNhomNguoiDungByMaNhom(item.ma_nhom);
                if (obj.ErrorCode == 0)
                {
                    item.type_view = StatusAction.View.ToString();
                    ViewBag.type_view = StatusAction.View.ToString();
                }
                else
                {
                    item.type_view = StatusAction.Edit.ToString();
                    ViewBag.type_view = StatusAction.Edit.ToString();
                }
                return View(item);
            }
        }
        public IActionResult QLNhomVaitroChitietPaging(int currentPage = 1, string code = "", int pageSize = 25)
        {
            EAContext eAContext = new EAContext();
            ViewBag.ma_nhom = code;
            ViewBag.lst_nhom = eAContext.Asp_nhom.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_nguoi_dung = eAContext.AspNetUsers.ToList();
            var lst_nhom_nguoi_dung = _nguoiDungDao.GetNhomNguoiDungByMaNhom(code);
            ViewBag.lst_nhom_nguoi_dung = lst_nhom_nguoi_dung;
            ViewBag.ls_vai_tro = eAContext.Asp_dm_vai_tro.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            //lấy vai trò theo mã nhóm
            ViewBag.lst_nhom_vaitro = _nguoiDungDao.LayVaitroThemMaNhom(code);
            var totalRecored = eAContext.Asp_dm_vai_tro.Count();
            ViewBag.Stt = (currentPage - 1) * pageSize;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.PageSize = pageSize;
            if (lst_nhom_nguoi_dung != null)
            {
                ViewBag.type_view = StatusAction.View.ToString();
            }
            else
            {
                ViewBag.type_view = StatusAction.Add.ToString();
            }
            return PartialView();
        }
        public async Task<IActionResult> QLNhomVaitroChitietPaging2(int currentPage = 1, string code = "", int pageSize = 25)
        {
            EAContext eAContext = new EAContext();
            ViewBag.ma_nhom = code;
            ViewBag.lst_nhom = eAContext.Asp_nhom.Where(n => n.Deleted == 0).ToList();
            ViewBag.lst_nguoi_dung = eAContext.AspNetUsers.ToList();
            var lst_nhom_nguoi_dung = _nguoiDungDao.GetNhomNguoiDungByMaNhom(code);
            ViewBag.lst_nhom_nguoi_dung = lst_nhom_nguoi_dung;
            ViewBag.ls_vai_tro = eAContext.Asp_dm_vai_tro.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            //lấy vai trò theo mã nhóm
            ViewBag.lst_nhom_vaitro = _nguoiDungDao.LayVaitroThemMaNhom(code);
            var totalRecored = eAContext.Asp_dm_vai_tro.Count();
            ViewBag.TotalRecored = totalRecored;
            ViewBag.Stt = (currentPage - 1) * pageSize;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : ((currentPage - 1) * pageSize) + 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
            ViewBag.PageSize = pageSize;
            if (lst_nhom_nguoi_dung != null)
            {
                ViewBag.type_view = StatusAction.View.ToString();
            }
            else
            {
                ViewBag.type_view = StatusAction.Add.ToString();
            }
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/QLNhomVaitroChitietPaging2.cshtml");
            return Content(result);
        }
        public async Task<IActionResult> ThemNhomVaitro(int currentPage = 1,string code="", string lstvaitro = "", int pageSize = 25)
        {
            VaitroModel vaitroModel = new VaitroModel();
            EAContext eAContext = new EAContext();
            ViewBag.ma_nhom = code;
            ViewBag.lst_nhom_vaitro = lstvaitro;
            vaitroModel.asp_Dm_Vai_Tro = eAContext.Asp_dm_vai_tro.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
            vaitroModel.danh_sach_nhom_vai_tro = lstvaitro;
            var res = _nguoiDungDao.ThemNhomVaitro(code, lstvaitro);
            ViewBag.lst_nhom_vaitro = _nguoiDungDao.LayVaitroThemMaNhom(code);
            var totalRecored = eAContext.Asp_dm_vai_tro.Count();
            ViewBag.TotalRecored = totalRecored;
            ViewBag.Stt = (currentPage - 1) * pageSize;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.PageSize = pageSize;
            if (res.ErrorCode == 0)
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
                for (var item  = 0; item < abc.Count(); item++)
                {
                    if (abc[item] != "")
                    {
                        var obj = db.AspNetUsers.Where(n => n.Id == Guid.Parse(abc[item].ToString())).FirstOrDefault();
                        if (obj != null) { result.Add(obj); }
                    }
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
                if (string.IsNullOrEmpty(ma_phong_ban))
                {
                    var lstUsers = db.AspNetUsers.Where(n => (lstUsershaschecked ?? "").Contains(n.Id.ToString()) == false).ToList();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/NguoiDungByPhongBan.cshtml", lstUsers.Distinct().ToList());
                    return Content(result);
                }
                else
                {
                    var lstUsers = db.AspNetUsers.Where(n => n.ma_phong_ban == ma_phong_ban && (lstUsershaschecked ?? "").Contains(n.Id.ToString()) == false).ToList();
                    var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/NguoiDungByPhongBan.cshtml", lstUsers.Distinct().ToList());
                    return Content(result);
                }
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
                        if(item != "")
                        {
                            var obj = db.AspNetUsers.Where(n => n.Id == Guid.Parse(item)).FirstOrDefault();
                            if (obj != null && result.Where(n=>n.Id == obj.Id).Count() == 0) { result.Add(obj); }
                        }
                       
                    }
                }
                var resultC = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/QTNguoidung/NguoiDungDaChon.cshtml", result.Distinct().ToList());
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
