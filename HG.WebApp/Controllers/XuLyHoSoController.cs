﻿using AutoMapper;
using HG.Data.Business.DanhMuc;
using HG.Data.Business.HoSo;
using HG.Data.Business.ThuTuc;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Model;
using HG.Entities.HoSo;
using HG.Entities.Entities.Luong;
using HG.Entities.SearchModels;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore.Reporting;
using HG.Data.Business.NguoiDung;

namespace HG.WebApp.Controllers
{
    public class XuLyHoSoController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LuongXuLyDao _danhmucDao;
        private readonly ThuTucDao _thuTucDao;
        private readonly XuLyHoSoDao _xulyhsDao;
        private readonly HG.Data.Business.HoSo.HoSoDao _hoso;
        private readonly NguoiDungDao _nguoiDungDao;
        private readonly IWebHostEnvironment _environment;
        public XuLyHoSoController(IWebHostEnvironment environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _environment = environment;
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _danhmucDao = new LuongXuLyDao(DbProvider);
            _thuTucDao = new ThuTucDao(DbProvider);
            _xulyhsDao = new XuLyHoSoDao(DbProvider);
            _hoso = new HG.Data.Business.HoSo.HoSoDao(DbProvider);
            _nguoiDungDao = new NguoiDungDao(DbProvider);

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HoSoChoXuLy(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 18,userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoDangXuLy(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 19, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoPhoiHop(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 20, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
            hs = hs.Where(e => e.nguoi_phoi_hop == userManager.GetUserId(User).ToUpper()).ToList();
            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;           
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoChuaBoSung(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 21, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);
           
            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoChoKy(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 23, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoDaChuyenXuLy(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 22, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoDaKy(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 24, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoChoChuyenMotCua(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 29, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoDaPhoiHop(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1

            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 26, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoGanDenHan(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1

            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 27, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoTheoDoiDonDoc(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1

            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 28, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoXuLyThay(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1

            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 30, userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult HoSoLienThong(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1

            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = 31,userid = user };
            hs = _hoso.HoSoPaging(hoSoPaging, out totalRecored);

            using (var db = new EAContext())
            {
                lv = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            };
            ViewBag.LstLinhVuc = lv;
            ViewBag.CurrentPage = 1;
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.MaLinhVuc = ma_linh_vuc;
            ViewBag.MaThuTuc = ma_thu_tuc;
            return View(hs);
        }
        public IActionResult ViewHoSo(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code,18);
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
           
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = hoso.ma_thu_tuc_hc, RecordsPerPage = 25 };
            var thutuchc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            ViewBag.LstThuTuc = thutuchc;
            var ngaynhan = (DateTime.Now - hoso.CreatedDateUtc.Value).TotalDays;
            var ngayxuly = hoso.CreatedDateUtc.Value.AddDays(thutuchc.FirstOrDefault().so_ngay_xl);
            if (ngaynhan > thutuchc.FirstOrDefault().so_ngay_xl)
            {
                ViewBag.QuaHan = 1;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
                ViewBag.SoNgayQH = Convert.ToInt32(ngaynhan - thutuchc.FirstOrDefault().so_ngay_xl);
            }
            else
            {
                ViewBag.QuaHan = 0;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
            }    
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoDangXuLy(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 19);
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            var thutuchc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            ViewBag.LstThuTuc = thutuchc;
            var ngaynhan = (DateTime.Now - hoso.CreatedDateUtc.Value).TotalDays;
            var ngayxuly = hoso.CreatedDateUtc.Value.AddDays(thutuchc.FirstOrDefault().so_ngay_xl);
            if (ngaynhan > thutuchc.FirstOrDefault().so_ngay_xl)
            {
                ViewBag.QuaHan = 1;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
                ViewBag.SoNgayQH = Convert.ToInt32(ngaynhan - thutuchc.FirstOrDefault().so_ngay_xl);
            }
            else
            {
                ViewBag.QuaHan = 0;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
            }
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoPhoiHop(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 20);
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            ViewBag.Status = hoso.trang_thai;
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            var thutuchc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            ViewBag.LstThuTuc = thutuchc;
            var ngaynhan = (DateTime.Now - hoso.CreatedDateUtc.Value).TotalDays;
            var ngayxuly = hoso.CreatedDateUtc.Value.AddDays(thutuchc.FirstOrDefault().so_ngay_xl);
            if (ngaynhan > thutuchc.FirstOrDefault().so_ngay_xl)
            {
                ViewBag.QuaHan = 1;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
                ViewBag.SoNgayQH = Convert.ToInt32(ngaynhan - thutuchc.FirstOrDefault().so_ngay_xl);
            }
            else
            {
                ViewBag.QuaHan = 0;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
            }
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoChoKy(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 23) ;
            ViewBag.PhanCongThuHien = ViewBag.PhanCongThuHien != null ? ViewBag.PhanCongThuHien : new PhanCongThucHienModels();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            ViewBag.Status = hoso.trang_thai;
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            ViewBag.LstThuTuc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoDaKy(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 24);
            ViewBag.PhanCongThuHien = ViewBag.PhanCongThuHien != null ? ViewBag.PhanCongThuHien : new PhanCongThucHienModels();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            ViewBag.Status = hoso.trang_thai;
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            var thutuchc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            ViewBag.LstThuTuc = thutuchc;
            var ngaynhan = (DateTime.Now - hoso.CreatedDateUtc.Value).TotalDays;
            var ngayxuly = hoso.CreatedDateUtc.Value.AddDays(thutuchc.FirstOrDefault().so_ngay_xl);
            if (ngaynhan > thutuchc.FirstOrDefault().so_ngay_xl)
            {
                ViewBag.QuaHan = 1;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
                ViewBag.SoNgayQH = Convert.ToInt32(ngaynhan - thutuchc.FirstOrDefault().so_ngay_xl);
            }
            else
            {
                ViewBag.QuaHan = 0;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
            }
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoChoChuyenMotCua(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 29);
            ViewBag.PhanCongThuHien = ViewBag.PhanCongThuHien != null ? ViewBag.PhanCongThuHien : new PhanCongThucHienModels();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            ViewBag.Status = hoso.trang_thai;
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            var thutuchc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            ViewBag.LstThuTuc = thutuchc;
            var ngaynhan = (DateTime.Now - hoso.CreatedDateUtc.Value).TotalDays;
            var ngayxuly = hoso.CreatedDateUtc.Value.AddDays(thutuchc.FirstOrDefault().so_ngay_xl);
            if (ngaynhan > thutuchc.FirstOrDefault().so_ngay_xl)
            {
                ViewBag.QuaHan = 1;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
                ViewBag.SoNgayQH = Convert.ToInt32(ngaynhan - thutuchc.FirstOrDefault().so_ngay_xl);
            }
            else
            {
                ViewBag.QuaHan = 0;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
            }
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoGanDenHan(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 27);
            ViewBag.PhanCongThuHien = ViewBag.PhanCongThuHien != null ? ViewBag.PhanCongThuHien : new PhanCongThucHienModels();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            ViewBag.Status = hoso.trang_thai;
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            var thutuchc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            ViewBag.LstThuTuc = thutuchc;
            var ngaynhan = (DateTime.Now - hoso.CreatedDateUtc.Value).TotalDays;
            var ngayxuly = hoso.CreatedDateUtc.Value.AddDays(thutuchc.FirstOrDefault().so_ngay_xl);
            if (ngaynhan > thutuchc.FirstOrDefault().so_ngay_xl)
            {
                ViewBag.QuaHan = 1;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
                ViewBag.SoNgayQH = Convert.ToInt32(ngaynhan - thutuchc.FirstOrDefault().so_ngay_xl);
            }
            else
            {
                ViewBag.QuaHan = 0;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
            }
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoLienThong(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 31);
            ViewBag.PhanCongThuHien = ViewBag.PhanCongThuHien != null ? ViewBag.PhanCongThuHien : new PhanCongThucHienModels();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            ViewBag.Status = hoso.trang_thai;
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            var thutuchc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            ViewBag.LstThuTuc = thutuchc;
            var ngaynhan = (DateTime.Now - hoso.CreatedDateUtc.Value).TotalDays;
            var ngayxuly = hoso.CreatedDateUtc.Value.AddDays(thutuchc.FirstOrDefault().so_ngay_xl);
            if (ngaynhan > thutuchc.FirstOrDefault().so_ngay_xl)
            {
                ViewBag.QuaHan = 1;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
                ViewBag.SoNgayQH = Convert.ToInt32(ngaynhan - thutuchc.FirstOrDefault().so_ngay_xl);
            }
            else
            {
                ViewBag.QuaHan = 0;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
            }
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public async Task<IActionResult> LayThongTinXuLyHoSo(string ma_ho_so, string type)
        {
            //var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            List<QuaTrinhXuLy> quaTrinhXuLyModels = new List<QuaTrinhXuLy>();
            ViewBag.view_type = type;
            quaTrinhXuLyModels = _xulyhsDao.GetQuaTrinhXuLy(Int32.Parse(ma_ho_so));
            //ViewBag.ma_luong = ma_luong;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/ThongTinXuLy.cshtml", quaTrinhXuLyModels);
            return Content(result);
        }
        public IActionResult ViewHoSoXuLyThay(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 30);
            ViewBag.PhanCongThuHien = ViewBag.PhanCongThuHien != null ? ViewBag.PhanCongThuHien : new PhanCongThucHienModels();
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            ViewBag.Status = hoso.trang_thai;
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = "", RecordsPerPage = 25 };
            var thutuchc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            ViewBag.LstThuTuc = thutuchc;
            var ngaynhan = (DateTime.Now - hoso.CreatedDateUtc.Value).TotalDays;
            var ngayxuly = hoso.CreatedDateUtc.Value.AddDays(thutuchc.FirstOrDefault().so_ngay_xl);
            if (ngaynhan > thutuchc.FirstOrDefault().so_ngay_xl)
            {
                ViewBag.QuaHan = 1;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
                ViewBag.SoNgayQH = Convert.ToInt32(ngaynhan - thutuchc.FirstOrDefault().so_ngay_xl);
            }
            else
            {
                ViewBag.QuaHan = 0;
                ViewBag.HanXuLy = thutuchc.FirstOrDefault().so_ngay_xl;
                ViewBag.NgayXuLy = ngayxuly.ToString("dd/MM/yyyy");
            }
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public async Task<IActionResult> Print(string Id)
        {
            string mintype = "";
            int exception = 1;
            var path = @""+this._environment.WebRootPath+"/Reports/PhieuTiepNhan.rdlc";
            ViewBag.Path = path;
            EAContext db = new EAContext();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var hoso = db.Ho_So.Where(n => n.Id == Int32.Parse(Id)).FirstOrDefault();
            var Get_phieu_hen = _xulyhsDao.Getphieuhen(Int32.Parse(Id));
            var dm_thanh_phan = _xulyhsDao.GetThanhPhanHoSo(hoso.ma_thu_tuc_hc);
            LocalReport localReport = new LocalReport(path);
            
            localReport.AddDataSource("Get_phieu_hen", Get_phieu_hen.ToList());
            localReport.AddDataSource("dm_thanh_phan", dm_thanh_phan.ToList());
            var result = localReport.Execute(RenderType.Pdf, exception, parameters, mintype);
           // return View(Get_phieu_hen);
             return File(result.MainStream, "application/pdf");
        }
        public async Task<IActionResult> SaveAndPrint(int Id, string noidung = "")
        {
            EAContext db = new EAContext();
            var obj = db.Ho_So.Where(n => n.Id == Id).FirstOrDefault();
            if (obj != null)
            {
               
                obj.id_file_dinh_kem = noidung; //chưa tiếp nhận
                obj.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                obj.UpdatedDateUtc = DateTime.Now;
                obj.ho_so_chua_du_dk_tiep_nhan_chinh_thuc = 0;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }
            string mintype = "";
            int exception = 1;
            var path = @"" + this._environment.WebRootPath + "/Reports/PhieuTiepNhan.rdlc";
            ViewBag.Path = path;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var hoso = db.Ho_So.Where(n => n.Id == Id).FirstOrDefault();
            var Get_phieu_hen = _xulyhsDao.Getphieuhen(Id);
            var dm_thanh_phan = _xulyhsDao.GetThanhPhanHoSo(hoso.ma_thu_tuc_hc);
            LocalReport localReport = new LocalReport(path);

            localReport.AddDataSource("Get_phieu_hen", Get_phieu_hen.ToList());
            localReport.AddDataSource("dm_thanh_phan", dm_thanh_phan.ToList());
            var result = localReport.Execute(RenderType.Pdf, exception, parameters, mintype);
            // return View(Get_phieu_hen);
            return File(result.MainStream, "application/pdf");
        }
        [HttpPost]
        public async Task<IActionResult> LuuThongTinHoSo(string Id, string type,int trangthai,int trangthaitruoc)
        {
            //var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            EAContext db = new EAContext();
            var hoso = db.Ho_So.Where(n => n.Id == Int32.Parse(Id)).FirstOrDefault();
            
            var title = "";
            hoso.trang_thai = trangthai;
            switch (trangthai)
            {
                case 18:
                    hoso.id_trang_thai_xl = 1;
                    title = "Hồ sơ chờ xử lý";
                    break;
                case 19:
                    hoso.id_trang_thai_xl = 1;
                    title = "Hồ sơ đang xử lý";
                    break;
                case 20:
                    hoso.id_trang_thai_xl = 1;
                    title = "Hồ sơ phối hợp";
                    break;
                case 22:
                    hoso.id_trang_thai_xl = 2;
                    title = "Hồ sơ đã chuyển xử lý";
                    break;
                case 23:
                    hoso.id_trang_thai_xl = 2;
                    title = "Hồ sơ chờ ký";
                    break;
                case 6:
                    hoso.id_trang_thai_xl = 4;
                    break;
                case 15:
                    hoso.id_trang_thai_xl = 6;
                    break;
                case 14:
                    hoso.id_trang_thai_xl = 3;
                    break;
                case 25:
                    hoso.id_trang_thai_xl = 10;
                    break;
                case 24:
                    hoso.id_trang_thai_xl = 7;
                    title = "Hồ sơ đã ký";
                    break;
                case 12:
                    hoso.id_trang_thai_xl = 5;
                    break;               
                case 99:
                    hoso.id_trang_thai_xl = 9;
                    break;
                case 98:
                    hoso.id_trang_thai_xl = 11;
                    break;
            }
            db.Update(hoso);
            db.SaveChangesAsync();
            SaveLogHS(int.Parse( Id), title, trangthaitruoc, trangthai, Guid.Parse(userManager.GetUserId(User)));
            ViewBag.view_type = type;
            //ViewBag.lstThanhPhan = lstThanhPhan;
            //ViewBag.ma_luong = ma_luong;
           
            return RedirectToAction("XuLyHoSo", "HoSoChoXuLy", new {  currentPage = 1,  txtSearch = "",  ma_linh_vuc = "",  ma_thu_tuc = "",  pageSize = 25 });
           // var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoChoXuLy.cshtml",hs);
           // return Content(result);
        }
        [HttpPost]
        public async Task<int> LuuPhanCongXuLy(PhanCongThucHienModels model)
        {
            var user = userManager.GetUserId(User);
          var result =   _xulyhsDao.ThemSuaPhanCongThucHien(model,user);
            if (result == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }    
        }
        [HttpPost]
        public async Task<int> GuiLienThong(string id)
        {
            var user = userManager.GetUserId(User);
            var result = _xulyhsDao.GuiHoSoLienThong(Int32.Parse( id),"1", user);
            if (result == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        [HttpPost]
        public async Task<IActionResult> LayThongTinLienThong(string ma_ho_so, string type)
        {
            //var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            List<ListHoSoLienThong> Listhosolienthong = new List<ListHoSoLienThong>();
            ViewBag.view_type = type;
            Listhosolienthong = _xulyhsDao.GetHoSoLienThong(Int32.Parse(ma_ho_so));
            //ViewBag.ma_luong = ma_luong;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/ThongTinGuiLienThong.cshtml", Listhosolienthong);
            return Content(result);
        }
        [HttpPost]
        public async Task<IActionResult> LayThongTinNhanLienThong(string ma_ho_so, string type)
        {
            //var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            List<ListKetQuaLienThong> Listhosolienthong = new List<ListKetQuaLienThong>();
            ViewBag.view_type = type;
            Listhosolienthong = _xulyhsDao.GetHoSoNhanLienThong(Int32.Parse(ma_ho_so));
            //ViewBag.ma_luong = ma_luong;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/ThongTinNhanLienThong.cshtml", Listhosolienthong);
            return Content(result);
        }
    }
}
