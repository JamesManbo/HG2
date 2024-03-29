﻿using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.DanhMuc.DonVi;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HG.WebApp.Controllers
{
    public class DonViController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //extend sys identity
        public DonViController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;

        }

        public IActionResult DonVi(string txtSearch = "")
        {

            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var dsObj = new List<dm_don_vi>();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                using (var db = new EAContext())
                {
                    var ds = db.dm_don_vi.Where(n => n.Deleted == 0 && (n.ten_don_vi ?? "").Contains(txtSearch)).ToList();
                    dsObj = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    var ds = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                    dsObj = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
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
            return View(dsObj);
        }
        [HttpGet]
        public IActionResult DonViThem()
        {
            EAContext db = new EAContext();
            ViewBag.ListCapDiaBan = db.dm_cap_dia_ban.ToList();
            ViewBag.ListDonVi = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
            //ViewBag.ListDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_dia_ban_cha == null && n.ma_cap == obj.ma_cap).ToList();
            return View(new dm_don_vi());
        }

        [HttpPost]
        public IActionResult DonViThem(dm_don_vi item)
        {
            try
            {
                EAContext eAContext = new EAContext();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                ViewBag.UidName = User.Identity.Name;
                item.Deleted = 0;
                item.la_dich_vu_cong = 0;
                item.CreatedDateUtc = DateTime.Now;
                var obj = eAContext.dm_don_vi.Where(n => n.ma_don_vi == item.ma_don_vi).Count();
                if (obj > 0)
                {
                    ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "Mã đã tồn tại trong hệ thống";
                    return View(item);
                };
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_don_vi> _role = eAContext.dm_don_vi.Add(item);
                eAContext.SaveChanges();
                var roleid = _role.Entity.Id;
                if (!string.IsNullOrEmpty(roleid.ToString()))
                {
                    ViewBag.type_view = StatusAction.View.ToString();
                    return View(item);
                }
                return View(item);
            }
            catch (Exception e)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Lỗi dữ liệu";
                return View(item);
            }
        }
        public async Task<IActionResult> ChinhSuaDonVi(string code, string type)
        {

            if (type == StatusAction.Edit.ToString())
            {
                EAContext db = new EAContext();
                ViewBag.ListCapDiaBan = db.dm_cap_dia_ban.ToList();
                ViewBag.ListDiaBan = db.dm_don_vi.Where(n => n.Deleted == 0 && n.ma_cap_co_quan == "003").ToList();
                ViewBag.type_view = StatusAction.Edit.ToString();
                var obj = db.dm_don_vi.Where(n => n.ma_don_vi == code).FirstOrDefault();
                if (obj != null && obj.CreatedUid != null)
                {
                    ViewBag.CreateUserName = await userManager.GetUserNameAsync(await userManager.FindByIdAsync(obj.CreatedUid.ToString()));
                    //lấy danh sách địa bàn cha (tỉnh thành)
                    ViewBag.ListTinhThanh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                    //lấy danh sách địa bàn con
                    if (obj.ma_dia_ban_con != null)
                    {
                        ViewBag.ListQuanHuyen = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == obj.ma_dia_ban_cha).ToList();
                    };
                    //lấy danh sách địa bàn con c1 (xã phường)
                    if (obj.ma_dia_ban_con_c1 != null)
                    {
                        ViewBag.ListXaPhuong = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_con == obj.ma_dia_ban_con).ToList();
                    };
                }
                ViewBag.ListDonVi = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                return View(obj);
            }
            else
            {
                EAContext db = new EAContext();
                ViewBag.ListCapDiaBan = db.dm_cap_dia_ban.ToList();
                ViewBag.type_view = StatusAction.View.ToString();
                ViewBag.ListDiaBan = db.dm_don_vi.Where(n => n.Deleted == 0 && n.ma_cap_co_quan == "003").ToList();
                var obj = db.dm_don_vi.Where(n => n.ma_don_vi == code).FirstOrDefault();
                //lấy danh sách địa bàn cha (tỉnh thành)
                ViewBag.ListTinhThanh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
                //lấy danh sách địa bàn con
                if (obj.ma_dia_ban_con != null)
                {
                    ViewBag.ListQuanHuyen = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == obj.ma_dia_ban_cha).ToList();
                };
                //lấy danh sách địa bàn con c1 (xã phường)
                if (obj.ma_dia_ban_con_c1 != null)
                {
                    ViewBag.ListXaPhuong = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_con == obj.ma_dia_ban_con).ToList();
                };
                return View(obj);
            }
        }
        [HttpPost]
        public IActionResult ChinhSuaDonVi(dm_don_vi item)
        {
            try
            {
                item.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                item.UpdatedDateUtc = DateTime.Now;
                EAContext db = new EAContext();
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DonVi");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Có lỗi xảy ra!";
                return View(item);
            }
        }
        public IActionResult XoaDonVi(string code)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            foreach (var item in code.Split(""))
            {
                EAContext db = new EAContext();
                var obj = db.dm_don_vi.Where(n => n.ma_don_vi == item).FirstOrDefault();
                if (obj != null)
                {
                    obj.Deleted = 1;
                    obj.UpdatedDateUtc = DateTime.Now;
                    obj.UpdatedUid = uid;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            return RedirectToAction("DonVi");
        }

        public async Task<IActionResult> DonViPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "")
        {
            var totalRecored = 0;
            var result = "";
            if (string.IsNullOrEmpty(tu_khoa))
            {
                using (var db = new EAContext())
                {
                    var ds = db.dm_don_vi.Where(n => n.Deleted != 1).ToList();
                    var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                    ViewBag.Stt = (currentPage - 1) * pageSize;
                    ViewBag.TotalRecored = totalRecored;
                    ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                    ViewBag.CurrentPage = currentPage;
                    ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                    ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                    result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/DonViPaging.cshtml", datapage);

                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    var ds = db.dm_don_vi.Where(n => n.Deleted != 1 && (n.ten_don_vi ?? "").Contains(tu_khoa)).ToList();
                    var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                    ViewBag.TotalRecored = totalRecored;
                    ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                    ViewBag.CurrentPage = currentPage;
                    ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                    ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                    result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/DonViPaging.cshtml", datapage);

                }
            }

            return Content(result);
        }
        public async Task<IActionResult> LayDanhSachDiaBanTheoMa(string ma_cap_co_quan, string matinh)
        {
            EAContext db = new EAContext();
            var LstDiaBan = db.dm_dia_ban.Where(n => n.ma_dia_ban_cha == null || n.ma_dia_ban_cha == "001").ToList();
            ViewBag.CapCoQuan = ma_cap_co_quan;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/LayDanhSachDiaBanTheoMa.cshtml", LstDiaBan);
            return Content(result);
        }

        public async Task<IActionResult> DanhSachDonViCha(string ma_cap_co_quan)
        {
            EAContext db = new EAContext();
            var LstDiaBan = db.dm_don_vi.Where(n => n.Deleted == 0 && n.ma_cap_co_quan == "003").ToList();
            ViewBag.CapCoQuan = ma_cap_co_quan;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/DanhSachDonViCha.cshtml", LstDiaBan);
            return Content(result);
        }
        public async Task<IActionResult> LayHuyenTheoTinh(string ma_dia_ban_cha, string matinh)
        {
            EAContext db = new EAContext();
            var LstDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_dia_ban_cha == ma_dia_ban_cha).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/LayDanhSachDiaBanTheoMaTinh.cshtml", LstDiaBan);
            return Content(result);
        }


        public async Task<IActionResult> LayDanhSachDiaBanTheoMa_new(string ma_cap_co_quan)
        {
            EAContext db = new EAContext();
            var LstDiaBan = db.dm_dia_ban.Where(n => n.ma_dia_ban_cha == null || n.ma_dia_ban_cha == "001").ToList();
            ViewBag.CapCoQuan = ma_cap_co_quan;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/Ajax/GetDiaBanByCode.cshtml", LstDiaBan);
            return Content(result);
        }
        //Lấy Huyện theo tỉnh new
        public async Task<IActionResult> TinhThanhChange(string ma_dia_ban_cha)
        {
            EAContext db = new EAContext();
            var LstDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_dia_ban_cha == ma_dia_ban_cha).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/Ajax/TinhThanhChange.cshtml", LstDiaBan);
            return Content(result);
        }
        public async Task<IActionResult> LayXaTheoHuyen(string ma_dia_ban_con)
        {
            EAContext db = new EAContext();
            var LstDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_dia_ban_con == ma_dia_ban_con).ToList();
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/Ajax/LayXaTheoHuyen.cshtml", LstDiaBan);
            return Content(result);
        }


        #region Cấp địa bàn
        public async Task<IActionResult> DiaBanPaging(int currentPage = 0, int pageSize = 0, string tu_khoa = "", string ma_tinh = "")
        {
            var totalRecored = 0;
            var result = "";
            var ds = new List<dm_dia_ban>();
            if (string.IsNullOrEmpty(tu_khoa))
            {
                using (var db = new EAContext())
                {
                    if (string.IsNullOrEmpty(ma_tinh))
                    {
                        ds = db.dm_dia_ban.Where(n => n.Deleted != 1).ToList();
                    }
                    else
                    {
                        ds = db.dm_dia_ban.Where(n => n.Deleted != 1 &&  (n.ma_dia_ban_cha == ma_tinh || n.ma_dia_ban_con == ma_tinh )).ToList();
                    }
                    var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                    ViewBag.Stt = (currentPage - 1) * pageSize;
                    ViewBag.TotalRecored = totalRecored;
                    ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                    ViewBag.CurrentPage = currentPage;
                    ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                    ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                    result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/DiaBanPaging.cshtml", datapage);

                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    if (string.IsNullOrEmpty(ma_tinh))
                    {
                        ds = db.dm_dia_ban.Where(n => n.Deleted != 1 && (n.ten_dia_ban ?? "").Contains(tu_khoa)).ToList();
                    }
                    else
                    {
                        ds = db.dm_dia_ban.Where(n => n.Deleted != 1 && (n.ten_dia_ban ?? "").Contains(tu_khoa) && (n.ma_dia_ban_cha == ma_tinh || n.ma_dia_ban_con == ma_tinh)).ToList();
                    }
                    var datapage = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                    ViewBag.TotalRecored = totalRecored;
                    ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                    ViewBag.CurrentPage = currentPage;
                    ViewBag.RecoredFrom = (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize;
                    ViewBag.RecoredTo = ViewBag.TotalPage == currentPage ? totalRecored : currentPage * pageSize;
                    result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/DonVi/DiaBanPaging.cshtml", datapage);

                }
            }

            return Content(result);
        }
        public IActionResult DiaBan(string txtSearch= "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            var dsObj = new List<dm_dia_ban>();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                using (var db = new EAContext())
                {
                    var ds = db.dm_dia_ban.Where(n => n.Deleted == 0 && (n.ten_dia_ban ?? "").Contains(txtSearch)).ToList();
                    dsObj = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    totalRecored = ds.Count();
                }
            }
            else
            {
                using (var db = new EAContext())
                {
                    var ds = db.dm_dia_ban.Where(n => n.Deleted == 0).ToList();
                    dsObj = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
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
            return View(dsObj);
        }
        public IActionResult DiaBanThem()
        {
            return View(new dm_dia_ban());
        }

        [HttpPost]
        public IActionResult DiaBanThem(dm_dia_ban item)
        {
            try
            {
                EAContext eAContext = new EAContext();
                item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                ViewBag.UidName = User.Identity.Name;
                item.Deleted = 0;
                item.CreatedDateUtc = DateTime.Now;
                var obj = eAContext.dm_dia_ban.Where(n => n.ma_dia_ban == item.ma_dia_ban).Count();
                if (obj > 0)
                {
                    ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "Tên đã tồn tại trong hệ thống";
                    return View(item);
                };
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<dm_dia_ban> _role = eAContext.dm_dia_ban.Add(item);
                eAContext.SaveChanges();
                var roleid = _role.Entity.Id;
                if (!string.IsNullOrEmpty(roleid.ToString()))
                {
                    ViewBag.type_view = StatusAction.View.ToString();
                    return View(item);
                }
                return View(item);
            }
            catch (Exception e)
            {
                ViewBag.ErrorCode = 1;
                ViewBag.ErrorMsg = "Lỗi dữ liệu";
                return View(item);
            }
        }
        public IActionResult SuaDiaBan(int code, string type)
        {
            EAContext db = new EAContext();
            ViewBag.ListCapDiaBan = db.dm_cap_dia_ban.ToList();
            var obj = db.dm_dia_ban.Where(n => n.Id == code).FirstOrDefault();
            //lấy danh sách địa bàn cha (tỉnh thành)
            ViewBag.ListTinhThanh = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
            //lấy danh sách địa bàn con
            if (obj.ma_dia_ban_cha != null)
            {
                ViewBag.ListQuanHuyen = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == obj.ma_dia_ban_cha).ToList();
            };
            //lấy danh sách địa bàn con c1 (xã phường)
            if (obj.ma_dia_ban_con != null)
            {
                ViewBag.ListXaPhuong = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_con == obj.ma_dia_ban_con).ToList();
            };
            if (type == StatusAction.Edit.ToString())
            {
                ViewBag.type_view = StatusAction.Edit.ToString();
                ViewBag.UserCreate = User.Identity.Name;
                
                ViewBag.ListDiaBan = db.dm_dia_ban.Where(n => n.Deleted == 0 && n.ma_dia_ban_cha == null && n.ma_cap == obj.ma_cap).ToList();
                return View(obj);
            }
            else
            {
                EAContext eAContext = new EAContext();
                ViewBag.type_view = StatusAction.View.ToString();
                return View(obj);
            }
        }
        [HttpPost]
        public IActionResult SuaDiaBan(dm_dia_ban item)
        {
            try
            {
                item.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                item.UpdatedDateUtc = DateTime.Now;
                EAContext db = new EAContext();
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.type_view = StatusAction.View.ToString();
                return View(item);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "Có lỗi xảy ra!";
                ViewBag.ErrorCode = 1;
                return View(item);
            }
        }
        public IActionResult XoaDiaBan(string code)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            var uid = Guid.Parse(userManager.GetUserId(User));
            foreach (var item in code.Split(""))
            {
                EAContext db = new EAContext();
                var obj = db.dm_dia_ban.Where(n => n.Id == Convert.ToInt32(item)).FirstOrDefault();
                if (obj != null)
                {
                    obj.Deleted = 1;
                    obj.UpdatedDateUtc = DateTime.Now;
                    obj.UpdatedUid = uid;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            return RedirectToAction("DiaBan");
        }
        #endregion  
    }
}
