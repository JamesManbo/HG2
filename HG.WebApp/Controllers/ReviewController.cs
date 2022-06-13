using AutoMapper;
using HG.WebApp.Data;
using HG.WebApp.Dto;
using HG.WebApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using X.PagedList;

namespace HG.WebApp.Controllers
{
    public class ReviewController : Controller
    {
        private EAContext db = new EAContext();
        private readonly ILogger<ReviewController> _logger;
        private EAContext eAContext = new EAContext();
        public ReviewController(ILogger<ReviewController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int page = 1)
        {
            return View(eAContext.Reviews.ToPagedList(page, 10));
        }

        public IActionResult AddReview(int Id)
        {
            ViewBag.IdListting = Id;
            var obj = db.Listting.Find(Id);
            if (obj != null)
            {
                ViewBag.NameListting = obj.Title;
            }
            else
            {
                ViewBag.NameListting = "";
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddReview(Reviews item)
        {
            try
            {
                item.Date = DateTime.Now;
                db.Reviews.Add(item);
                db.SaveChanges();
            }catch(Exception e)
            {

            }
           

            return Redirect("/YellowPageFilter");
        }
        public static bool ReCaptchaPassed(string gRecaptchaResponse)
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=your secret key no quotes&response={gRecaptchaResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5m)
            {
                return false;
            }

            return true;
        }

        public IActionResult AddListting()
        {
            ViewBag.ListCategories = db.Categorys.Take(50).ToList();
            ViewBag.ListNations = db.Nations.Take(50).ToList();
            ViewBag.DocumentListtingTitle = db.ListtingDocumentTitle.FirstOrDefault();
            ViewBag.Notify = "";
            return View();
        }
        [HttpPost ]
        public IActionResult AddListting(HG.WebApp.Dto.ListtingDTO item)
        {
            try
            {
                //check data
                var obj1 = db.Listting.FirstOrDefault(n => n.Phone == item.Phone);
                var obj3 = db.Listting.FirstOrDefault(n => n.TitleSeo == item.TitleSeo);
                var obj4 = db.Listting.FirstOrDefault(n => n.Title == item.Title);

                if (obj1 != null ||
                    obj3 != null ||
                    obj4 != null 
                   )
                {
                    ViewBag.ListCategories = db.Categorys.Take(50).ToList();
                    ViewBag.ListNations = db.Nations.Take(50).ToList();
                    ViewBag.Notify = "Có dữ liệu bị trùng!";
                    return View(item);
                }
                item.Status = 0;
                //save listing
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<ListtingDTO, Listting>(); });
                var des = config.CreateMapper().Map<ListtingDTO, Listting>(item);
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Listting> created = db.Listting.Add(des);
                db.SaveChanges();
                var imgid = created.Entity.Id;
                //save image
                item.ListtingImage.ListtingID = imgid;
                item.customerCeo.ListtingID = imgid;
                item.customerCeo.AppUserID = imgid;
                item.Locations.ListtingID = imgid;
                db.ListtingImages.Add(item.ListtingImage);
                db.SaveChanges();
                //save seo
                db.CustomerCeos.Add(item.customerCeo);
                //add address
                db.Locations.Add(item.Locations);
                db.SaveChanges();
                ViewBag.ListCategories = db.Categorys.Take(50).ToList();
                ViewBag.ListNations = db.Nations.Take(50).ToList();
                ViewBag.Notify = "Thêm mới listting thành công, vui lòng đợi quản trị xem xét!";
                return View();
            }
            catch (Exception ex)
            {
                return View(new ListtingDTO());
            }
            
        }


    }
}
