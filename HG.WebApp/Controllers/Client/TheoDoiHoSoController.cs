using HG.Data.Business.CauHinh;
using HG.Entities;
using HG.Entities.Entities.CauHinh;
using HG.WebApp.Data;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

namespace HG.WebApp.Controllers.Client
{
    public class TheoDoiHoSoController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        //private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly TheoDoiHSDao _hsDao;

        //extend sys identity
        public TheoDoiHoSoController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _hsDao = new TheoDoiHSDao(DbProvider);

        }

        [HttpGet]
        public IActionResult QuanLy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> HSSapHetHanPaging(string from, string to, int page = 0)
        {
            var total = 0;
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var hs_sap_het_han = _hsDao.HoSoSapHetHan(from, to, page, pageSize, out total);
            ViewBag.TotalPage = (total / pageSize) + ((total % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = page;
            var result = await CoinExchangeExtensions.RenderViewToStringAsync(this, "~/Views/TheoDoiHoSo/HSSapHetHanPaging.cshtml", hs_sap_het_han);
            return Content(result);
        }

        public async Task<IActionResult> ExportHSSapHetHan(string from, string to)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();
            var list = _hsDao.HoSoSapHetHanExcel(from, to);
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                var modelTable = workSheet.Cells;
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                workSheet.Cells[1, 1].Value = "STT";
                workSheet.Cells[1, 2].Value = "MÃ HỒ SƠ";
                workSheet.Cells[1, 3].Value = "THỦ TỤC";
                workSheet.Cells[1, 4].Value = "NGÀY TIẾP NHẬN";
                workSheet.Cells[1, 5].Value = "NGÀY DỰ KIẾN TRẢ KQ";
                workSheet.Cells[1, 6].Value = "TRẠNG THÁI";
                workSheet.Cells[1, 7].Value = "ĐƠN VỊ";

                using (var range = workSheet.Cells["A1:G1"])
                {
                    // Set PatternType
                    range.Style.Font.Bold = true;
                }

                // Đỗ dữ liệu từ list vào 
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    workSheet.Cells[i + 2, 1].Value = item.stt;
                    workSheet.Cells[i + 2, 2].Value = item.ten_ho_so;
                    workSheet.Cells[i + 2, 3].Value = item.ten_thu_tuc;
                    workSheet.Cells[i + 2, 4].Value = HG.WebApp.Common.HelperDateTime.DateTimeToYYYYMMDD((DateTime)item.ngay_tiep_nhan);
                    workSheet.Cells[i + 2, 5].Value = HG.WebApp.Common.HelperDateTime.DateTimeToYYYYMMDD((DateTime)item.ngay_hen_tra);
                    workSheet.Cells[i + 2, 6].Value = item.ten_trang_thai;
                    workSheet.Cells[i + 2, 7].Value = item.ten_don_vi;
                }

                // workSheet.Cells.LoadFromCollection(list, true);
                package.Save();

            }

            stream.Position = 0;
            string excelName = "BAOCAO-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
