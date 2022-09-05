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
using HG.Entities.BaoCaoThongKe;

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
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetBaoCaoSoLuong(BaoCaoSoLuongRequestModel request)
        {
            //var tuNgay = DateTime.ParseExact(data.TuNgayStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //var denNgay = DateTime.ParseExact(data.DenNgayStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string renderFormat = "PDF";
            request.TuNgay = new DateTime(2022, 08, 01);
            request.DenNgay = new DateTime(2022, 08, 17);
            request.ChonNgay = true;
            var items = _baocaoDao.GetBaoCaoSoLuongHoSo(request.Nam, request.Quy, request.ChonNgay, request.TuNgay, request.DenNgay);
            using var report = new LocalReport();
            var title = request.ChonNgay ? $"SỐ LƯỢNG HỒ SƠ TỪ NGÀY {request.TuNgay.ToString("dd/MM/yyyy")} ĐẾN NGÀY {request.DenNgay.ToString("dd/MM/yyyy")}" : $"SỐ LƯỢNG HỒ SƠ THEO QUÝ {request.Quy} NĂM {request.Nam}";
            ReportHelper.LoadBaoCaoSoLuong(report, items, title);
            var pdf = report.Render(renderFormat);
            // return new FileContentResult(pdf, "application/pdf");
            var baseStr = Convert.ToBase64String(pdf);
            return Content(baseStr);
        }


        [HttpGet]
        public async Task<IActionResult> BaoCaoTheoNguoiXuLy()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetBaoCaoTheoNguoiXuLy(BaoCaoTheoNguoiXuLyRequestModel request)
        {
            var path = "./Reports/BaoCaoTheoNguoiXuLy.rdlc";
            string renderFormat = "PDF";

            var rd = new Random();
            #region MyRegion
            //var items = new[]
            //{
            //    new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn B",
            //        TenPhongBan = "Hành chính"
            //    },
            //    new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn C",
            //        TenPhongBan = "Hành chính"
            //    },
            //    new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn D",
            //        TenPhongBan = "Hành chính"
            //    },
            //    new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn An",
            //        TenPhongBan = "Kế toán"
            //    },
            //    new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn Dương",
            //        TenPhongBan = "Kế toán"
            //    },
            //     new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn Khúc",
            //        TenPhongBan = "Kế toán"
            //    },
            //      new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn Mạnh",
            //        TenPhongBan = "Kế toán"
            //    },
            //       new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn Chính",
            //        TenPhongBan = "Kế toán"
            //    },
            //        new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn Nhỡ",
            //        TenPhongBan = "Kĩ thuật"
            //    },
            //         new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn Thu",
            //        TenPhongBan = "Kĩ thuật"
            //    },
            //          new BaoCaoTheoNguoiXuLyModel()
            //    {
            //        ChuaXuLyDungHan = rd.Next(0, 100),
            //        ChuaXuLyQuaHan = rd.Next(0, 100),
            //        DangXuLyDungHan = rd.Next(0, 100),
            //        DangXuLyQuaHan = rd.Next(0, 100),
            //        DaXuLyDungHan = rd.Next(0, 100),
            //        DaXuLyQuahan = rd.Next(0, 100),
            //        TenNguoiXuLy = "Nguyễn Văn Hành",
            //        TenPhongBan = "Kĩ thuật"
            //    },

            // };
            #endregion

            var items = _baocaoDao.GetBaoCaoTheoNguoiXuLy(request.MaPhongBan, request.MaNguoiXuLy, request.MaLinhVuc, request.MaThuTuc, request.TuNgay, request.DenNgay);
            using var report = new LocalReport();
            ReportHelper.Load(report, items.ToList(), path, "dsBaoCaoTheoNguoiXuLy", new ReportParameter("title", "BÁO CÁO THEO NGƯỜI XỬ LÝ"));
            var file = report.Render(renderFormat);
            var baseStr = Convert.ToBase64String(file);
            return Content(baseStr);
        }



        [HttpGet]
        public async Task<IActionResult> BaoCaoHoSoBiLoai()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetBaoCaoHoSoBiLoai(BaoCaoHoSoBiLoaiRequestModel request)
        {
            var path = "./Reports/BaoCaoHoSoBiLoai.rdlc";
            string renderFormat = "PDF";

            #region Fake data

            var items = new[]
            {
                new EBaoCaoHoSoBiLoai()
                {
                    MaHoSo = "001",
                    MaThuTuc = "TT01",
                    TenThuTuc = "Xử lý ",
                    TenNguoiTiepNhan = "Nguyễn Văn A",
                    TenNguoIXuLy = "Nguyễn Thị Thơm",
                    TenPhongBan = "Kế hoạch",
                    NgayNhan = DateTime.Now,
                    NoiDungXuLy = "Xử lý BC"
                },
                  new EBaoCaoHoSoBiLoai()
                {
                    MaHoSo = "001",
                    MaThuTuc = "TT01",
                    TenThuTuc = "Xử lý ",
                    TenNguoiTiepNhan = "Nguyễn Văn A",
                    TenNguoIXuLy = "Nguyễn Thị Thơm",
                    TenPhongBan = "Kế hoạch",
                    NgayNhan = DateTime.Now,
                    NoiDungXuLy = "Xử lý BC"
                },
                    new EBaoCaoHoSoBiLoai()
                {
                    MaHoSo = "001",
                    MaThuTuc = "TT01",
                    TenThuTuc = "Xử lý ",
                    TenNguoiTiepNhan = "Nguyễn Văn A",
                    TenNguoIXuLy = "Nguyễn Thị Thơm",
                    TenPhongBan = "Kế hoạch",
                    NgayNhan = DateTime.Now,
                    NoiDungXuLy = "Xử lý BC"
                },
                      new EBaoCaoHoSoBiLoai()
                {
                    MaHoSo = "001",
                    MaThuTuc = "TT01",
                    TenThuTuc = "Xử lý ",
                    TenNguoiTiepNhan = "Nguyễn Văn A",
                    TenNguoIXuLy = "Nguyễn Thị Thơm",
                    TenPhongBan = "Kế hoạch",
                    NgayNhan = DateTime.Now,
                    NoiDungXuLy = "Xử lý BC"
                },
                        new EBaoCaoHoSoBiLoai()
                {
                    MaHoSo = "001",
                    MaThuTuc = "TT01",
                    TenThuTuc = "Xử lý ",
                    TenNguoiTiepNhan = "Nguyễn Văn A",
                    TenNguoIXuLy = "Nguyễn Thị Thơm",
                    TenPhongBan = "Kế hoạch",
                    NgayNhan = DateTime.Now,
                    NoiDungXuLy = "Xử lý BC"
                },
            };

            #endregion

            //var items = _baocaoDao.GetBaoCaoHoSoBiLoai(request.MaPhongBan, request.MaNguoiXuLy, request.MaLinhVuc, request.MaThuTuc,request.TinhTrang, request.TuNgay, request.DenNgay);
            using var report = new LocalReport();
            ReportHelper.Load(report, items.ToList(), path, "dsHoSoBiLoai");
            var file = report.Render(renderFormat);
            var baseStr = Convert.ToBase64String(file);
            return Content(baseStr);
        }

        [HttpGet]
        public async Task<IActionResult> BaoCaoTongHop()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetBaoCaoTongHop(BaoCaoTongHopRequestModel request)
        {
            var path = "./Reports/BaoCaoTongHop.rdlc";
            string renderFormat = "PDF";

            #region Fake data

            var items = new[]
            {
               new EBaoCaoTongHop()
               {
                   TenLinhVuc = "Thẩm định Đề cương và dự toán chi tiết",
                   TenDuAn = "Công nghệ thông tin",
                   NguoiNop = "Nguyễn Thị Mai",
                   TenDonViToChuc = "Sở văn hóa thể thao và du lịch",
                   NgayHenTra = DateTime.Now,
                   NgayHoanThanh = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   TenTrangThai = "Chờ xử lý"
               },
               new EBaoCaoTongHop()
               {
                   TenLinhVuc = "Thẩm định Đề cương và dự toán chi tiết",
                   TenDuAn = "Công nghệ thông tin",
                   NguoiNop = "Nguyễn Thị Mai",
                   TenDonViToChuc = "Sở văn hóa thể thao và du lịch",
                   NgayHenTra = DateTime.Now,
                   NgayHoanThanh = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   TenTrangThai = "Chờ xử lý"
               },
               new EBaoCaoTongHop()
               {
                   TenLinhVuc = "Thẩm định Đề cương và dự toán chi tiết",
                   TenDuAn = "Công nghệ thông tin",
                   NguoiNop = "Nguyễn Thị Mai",
                   TenDonViToChuc = "Sở văn hóa thể thao và du lịch",
                   NgayHenTra = DateTime.Now,
                   NgayHoanThanh = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   TenTrangThai = "Chờ xử lý"
               },
               new EBaoCaoTongHop()
               {
                   TenLinhVuc = "Thẩm định Đề cương và dự toán chi tiết",
                   TenDuAn = "Công nghệ thông tin",
                   NguoiNop = "Nguyễn Thị Mai",
                   TenDonViToChuc = "Sở văn hóa thể thao và du lịch",
                   NgayHenTra = DateTime.Now,
                   NgayHoanThanh = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   TenTrangThai = "Chờ xử lý"
               },
               new EBaoCaoTongHop()
               {
                   TenLinhVuc = "Thẩm định Đề cương và dự toán chi tiết",
                   TenDuAn = "Công nghệ thông tin",
                   NguoiNop = "Nguyễn Thị Mai",
                   TenDonViToChuc = "Sở văn hóa thể thao và du lịch",
                   NgayHenTra = DateTime.Now,
                   NgayHoanThanh = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   TenTrangThai = "Chờ xử lý"
               },
               new EBaoCaoTongHop()
               {
                   TenLinhVuc = "Thẩm định Đề cương và dự toán chi tiết",
                   TenDuAn = "Công nghệ thông tin",
                   NguoiNop = "Nguyễn Thị Mai",
                   TenDonViToChuc = "Sở văn hóa thể thao và du lịch",
                   NgayHenTra = DateTime.Now,
                   NgayHoanThanh = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   TenTrangThai = "Chờ xử lý"
               },
               new EBaoCaoTongHop()
               {
                   TenLinhVuc = "Thẩm định Đề cương và dự toán chi tiết",
                   TenDuAn = "Công nghệ thông tin",
                   NguoiNop = "Nguyễn Thị Mai",
                   TenDonViToChuc = "Sở văn hóa thể thao và du lịch",
                   NgayHenTra = DateTime.Now,
                   NgayHoanThanh = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   TenTrangThai = "Chờ xử lý"
               },
            };

            #endregion

            //var items = _baocaoDao.GetBaoCaoHoSoBiLoai(request.MaPhongBan, request.MaNguoiXuLy, request.MaLinhVuc, request.MaThuTuc,request.TinhTrang, request.TuNgay, request.DenNgay);
            using var report = new LocalReport();
            ReportHelper.Load(report, items.ToList(), path, "dsBaoCaoTongHop");
            var file = report.Render(renderFormat);
            var baseStr = Convert.ToBase64String(file);
            return Content(baseStr);
        }

        [HttpGet]
        public async Task<IActionResult> BaoCaoHoSoTrucTuyen()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetBaoCaoHoSoTrucTuyen(BaoCaoTongHopRequestModel request)
        {
            var path = "./Reports/BaoCaoHoSoTrucTuyen.rdlc";
            string renderFormat = "PDF";

            #region Fake data

            var items = new[]
            {
               new EBaoCaoHoSoTrucTuyen()
               {
                 
                   MaHoSo = "H0001-1112-2022-11",
                   TenThuTuc = "Họp báo",
                   TenNguoiNop = "Nguyễn Thị Thu Hương",
                   NguoiNhan = "Nguyễn Mạnh An",
                   TenCoQuan = "Sở thông tin Hà Giang",
                   NgayHenTra = DateTime.Now,
                   NgayTraKetQua = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   MucDo = "MD3"
               },
                new EBaoCaoHoSoTrucTuyen()
               {

                   MaHoSo = "H0001-1112-2022-11",
                   TenThuTuc = "Họp báo",
                   TenNguoiNop = "Nguyễn Thị Thu Hương",
                   NguoiNhan = "Nguyễn Mạnh An",
                   TenCoQuan = "Sở thông tin Hà Giang",
                   NgayHenTra = DateTime.Now,
                   NgayTraKetQua = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   MucDo = "MD3"
               },
                 new EBaoCaoHoSoTrucTuyen()
               {

                   MaHoSo = "H0001-1112-2022-11",
                   TenThuTuc = "Họp báo",
                   TenNguoiNop = "Nguyễn Thị Thu Hương",
                   NguoiNhan = "Nguyễn Mạnh An",
                   TenCoQuan = "Sở thông tin Hà Giang",
                   NgayHenTra = DateTime.Now,
                   NgayTraKetQua = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   MucDo = "MD3"
               },
                  new EBaoCaoHoSoTrucTuyen()
               {

                   MaHoSo = "H0001-1112-2022-11",
                   TenThuTuc = "Họp báo",
                   TenNguoiNop = "Nguyễn Thị Thu Hương",
                   NguoiNhan = "Nguyễn Mạnh An",
                   NgayHenTra = DateTime.Now,
                   TenCoQuan = "Sở thông tin Hà Giang",
                   NgayTraKetQua = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   MucDo = "MD3"
               },
                   new EBaoCaoHoSoTrucTuyen()
               {

                   MaHoSo = "H0001-1112-2022-11",
                   TenThuTuc = "Họp báo",
                   TenCoQuan = "Sở thông tin Hà Giang",
                   TenNguoiNop = "Nguyễn Thị Thu Hương",
                   NguoiNhan = "Nguyễn Mạnh An",
                   NgayHenTra = DateTime.Now,
                   NgayTraKetQua = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   MucDo = "MD3"
               },
                    new EBaoCaoHoSoTrucTuyen()
               {

                   MaHoSo = "H0001-1112-2022-11",
                   TenThuTuc = "Họp báo",
                   TenNguoiNop = "Nguyễn Thị Thu Hương",
                   NguoiNhan = "Nguyễn Mạnh An",
                   TenCoQuan = "Sở thông tin Hà Giang",
                   NgayHenTra = DateTime.Now,
                   NgayTraKetQua = DateTime.Now,
                   NgayNhan = DateTime.Now,
                   MucDo = "MD3"
               },

            };

            #endregion
            //var items = _baocaoDao.GetBaoCaoHoSoBiLoai(request.MaPhongBan, request.MaNguoiXuLy, request.MaLinhVuc, request.MaThuTuc,request.TinhTrang, request.TuNgay, request.DenNgay);
            using var report = new LocalReport();
            ReportHelper.Load(report, items.ToList(), path, "dsHoSoTrucTuyen");
            var file = report.Render(renderFormat);
            var baseStr = Convert.ToBase64String(file);
            return Content(baseStr);
        }
    }
}
