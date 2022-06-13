using HG.WebApp.Data;
using HG.WebApp.Entities;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HG.WebApp.Controllers
{
    public class ContentController : Controller
    {
        private EAContext db = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        public ContentController(UserManager<AspNetUsers> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(Content item)
        {
            AspNetUsers applicationUser = await userManager.GetUserAsync(User);
            if (applicationUser == null) { return RedirectToAction("Login", "User"); }
            if (_httpContextAccessor != null) { if (!sercutiry.IsAthentication(applicationUser, _httpContextAccessor.HttpContext.Request.Path.ToString())) { return RedirectToAction("NotFound", "Admin"); } };
            if (item.title_Bold == null)
            {
                ViewBag.Message = "";
                return View(db.Contents.FirstOrDefault());
            }
            else
            {
                try
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Update thành công!";
                    return View(item);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Có lỗi xảy ra!";
                    return View(item);
                }
            }
           
        }

        public async Task<IActionResult> EditTitleListing(ListtingDocumentTitle item)
        {
            AspNetUsers applicationUser = await userManager.GetUserAsync(User);
            if (applicationUser == null) { return RedirectToAction("Login", "User"); }
            if (_httpContextAccessor != null) { if (!sercutiry.IsAthentication(applicationUser, _httpContextAccessor.HttpContext.Request.Path.ToString())) { return RedirectToAction("NotFound", "Admin"); } };
            if (item.TieuDe == null)
            {
                ViewBag.Message = "";
                return View(db.ListtingDocumentTitle.FirstOrDefault());
            }
            else
            {
                try
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Update thành công!";
                    return View(item);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Có lỗi xảy ra!";
                    return View(item);
                }
            }
        }

       
    }
}
