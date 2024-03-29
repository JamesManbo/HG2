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
    public class TraKetQuaController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LuongXuLyDao _danhmucDao;
        private readonly ThuTucDao _thuTucDao;
        private readonly XuLyHoSoDao _xulyhsDao;
        private readonly TraKetQuaDao _traketquaDao;
        private readonly HG.Data.Business.HoSo.HoSoDao _hoso;
        private readonly NguoiDungDao _nguoiDungDao;
        private readonly IWebHostEnvironment _environment;
        public TraKetQuaController(IWebHostEnvironment environment, ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
            _traketquaDao = new TraKetQuaDao(DbProvider);
            _hoso = new HG.Data.Business.HoSo.HoSoDao(DbProvider);
            _nguoiDungDao = new NguoiDungDao(DbProvider);

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HoSoChoTraKetQua(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTraKetQua.HoSoChoTraKS, userid = user };
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
        public IActionResult HoSoDaTraKetQua(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTraKetQua.HoSoDaTraKQ, userid = user };
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
        public IActionResult HoSoDaCoKQGD1(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTraKetQua.HoSoNhanKQGD1, userid = user };
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
        public IActionResult HoSoNhanKQBC(int currentPage = 1, string txtSearch = "", string ma_linh_vuc = "", string ma_thu_tuc = "", int pageSize = 25)
        {
            //hs mới tiếp nhận status = 1
            var totalRecored = 0;
            var hs = new List<Ho_So>();
            var lv = new List<Dm_Linh_Vuc>();
            var user = userManager.GetUserId(User);
            HoSoPaging hoSoPaging = new HoSoPaging() { CurrentPage = 1, tu_khoa = txtSearch, ma_thu_tuc = ma_thu_tuc, tat_ca = 1, dung_han = 0, qua_han = 0, RecordsPerPage = pageSize, trang_thai_hs = (int)StatusTraKetQua.HoSoNhanKQQuaBC, userid = user };
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
        public IActionResult ViewHoSoChoTraKetQua(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 18);
            ViewBag.ThongTinTraKQ = _traketquaDao.LayThongTinTraKetQua(code);
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = hoso.ma_thu_tuc_hc, RecordsPerPage = 25 };
            ViewBag.LstThuTuc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoDaTraKetQua(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 18);
            ViewBag.ThongTinTraKQ = _traketquaDao.LayThongTinTraKetQua(code);
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = hoso.ma_thu_tuc_hc, RecordsPerPage = 25 };
            ViewBag.LstThuTuc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoCoKQGD1(int code, string type)
        {
            var UserId = Guid.Parse(userManager.GetUserId(User));
            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = _nguoiDungDao.DanhSachNguoiDung(UserId);
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 18);
            ViewBag.ThongTinTraKQ = _traketquaDao.LayThongTinTraKetQua(code);
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = hoso.ma_thu_tuc_hc, RecordsPerPage = 25 };
            ViewBag.LstThuTuc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        public IActionResult ViewHoSoNhanKQBC(int code, string type)
        {

            EAContext db = new EAContext();
            ViewBag.LstLinhVuc = db.Dm_Linh_Vuc.Where(n => n.Deleted != 1).ToList();
            ViewBag.LstNguoiDung = db.AspNetUsers.ToList();
            ViewBag.type_view = StatusAction.View.ToString();
            ViewBag.LstQuyTrinhXuLy = _xulyhsDao.DanhSachQuyTrinhXuLyKey();
            ViewBag.PhanCongThuHien = _xulyhsDao.GetPhanCongXuLy(code, 18);
            ViewBag.ThongTinTraKQ = _traketquaDao.LayThongTinTraKetQua(code);
            var hoso = db.Ho_So.Where(n => n.Id == code).FirstOrDefault();
            //Lấy thủ tục bởi mã lv
            ThuTucModels nhomSearchItem = new ThuTucModels() { CurrentPage = 1, ma_pb = "", ma_lv = hoso.ma_linh_vuc, tu_khoa = hoso.ma_thu_tuc_hc, RecordsPerPage = 25 };
            ViewBag.LstThuTuc = _thuTucDao.GetDanhSachThuTuc(hoso.ma_thu_tuc_hc);
            //Lấy biểu mẫu
            ViewBag.LstBieuMau = db.dm_bieu_mau.Where(n => n.Deleted != 1).ToList();

            return View(hoso);
        }
        [HttpPost]
        public async Task<int> LuuThongTinTraKetQua(ThongTinTraKQModels model)
        {
            var user = userManager.GetUserId(User);
            var result = _traketquaDao.LuuThongTinTraKetQua(model, user);
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
        public async Task<IActionResult> LuuThongTinHoSo(string Id, string type, int trangthai, int trangthaitruoc)
        {
            //var lstObj = _danhmucDao.LayLuongThanhPhanByMaTTHC(ma_thu_tuc);
            EAContext db = new EAContext();
            var hoso = db.Ho_So.Where(n => n.Id == Int32.Parse(Id)).FirstOrDefault();

            var title = "Trả kết quả";
            hoso.trang_thai = trangthai;
            //if (trangthai == 14)
            //{

            //}
            db.Update(hoso);
            db.SaveChangesAsync();
            SaveLogHS(int.Parse(Id), title, trangthaitruoc, trangthai, Guid.Parse(userManager.GetUserId(User)));
            ViewBag.view_type = type;
            //ViewBag.lstThanhPhan = lstThanhPhan;
            //ViewBag.ma_luong = ma_luong;

            return RedirectToAction("TraKetQua", "HoSoChoTraKetQua", new { currentPage = 1, txtSearch = "", ma_linh_vuc = "", ma_thu_tuc = "", pageSize = 25 });
            // var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/XuLyHoSo/HoSoChoXuLy.cshtml",hs);
            // return Content(result);
        }
    }
}
