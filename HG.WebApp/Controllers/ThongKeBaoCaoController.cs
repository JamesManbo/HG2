using HG.Data.Business.ThongKeBaoCao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HG.WebApp.Controllers
{
    public class ThongKeBaoCaoController : BaseController
    {
        private readonly ThongKeBaoCao _thongKeBC;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ThongKeBaoCaoController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _thongKeBC = new ThongKeBaoCao(DbProvider);
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
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
    }
}
