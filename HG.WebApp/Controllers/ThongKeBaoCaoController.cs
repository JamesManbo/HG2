using AspNetCore.Reporting;
using HG.Data.Business.ThongKeBaoCao;
using HG.Data.Business.HoSo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HG.WebApp.Controllers
{
    public class ThongKeBaoCaoController : BaseController
    {
        private readonly ThongKeBaoCao _thongKeBC;
        
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ThongKeBaoCaoController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment) : base(configuration, httpContextAccessor)
        {
            _thongKeBC = new ThongKeBaoCao(DbProvider);
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            this._webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        // GET: ThongKeBaoCaoController
        public ActionResult SoLuongHoSo()
        {
            return View();
        }

        // GET: ThongKeBaoCaoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ThongKeBaoCaoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThongKeBaoCaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ThongKeBaoCaoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ThongKeBaoCaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ThongKeBaoCaoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ThongKeBaoCaoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Print()
        {
            var listHS = _thongKeBC.BaoCaoSoLuongHoSo(2021, 2);
            
            string mintype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\BaoCaoSLHS.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            parameters.Add("Date", DateTime.Now.Date.ToString("dd"));
            parameters.Add("Month", DateTime.Now.Month.ToString());
            parameters.Add("Year", DateTime.Now.Year.ToString());
            
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DanhSachSLHS", listHS);

            try
            {
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
                return File(result.MainStream, "application/pdf");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
