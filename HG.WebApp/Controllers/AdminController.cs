
using AutoMapper;
using HG.WebApp;
using HG.WebApp.Controllers;
using HG.WebApp.Data;
using HG.WebApp.Dto;
using HG.WebApp.Entities;
using HG.WebApp.Sercurity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using X.PagedList;

namespace HG.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<AspNetUsers> userManager;
        private EAContext db = new EAContext();
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        public AdminController(ILogger<AdminController> logger, UserManager<AspNetUsers> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        {
            this._logger = logger;
            this.userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
        }
     

        public async Task<IActionResult> Index(string TieuDe, string FulName, Guid? UserID, DateTime? fromdate, DateTime? todate, int page = 1)
        {
            ViewBag.CanSign = "True";
            //check login user
            AspNetUsers applicationUser = await userManager.GetUserAsync(User);
            if (applicationUser == null) { return RedirectToAction("Login", "User"); }
            //if (_httpContextAccessor != null) { if (!sercutiry.IsAthentication(applicationUser, _httpContextAccessor.HttpContext.Request.Path.ToString())) { return RedirectToAction("NotFound", "Admin"); } };
            //end check login user
            List<ListtingDTO> entryPoint = new List<ListtingDTO>();
            //check role cong tac vien
            if (sercutiry.IsCollaborators(applicationUser))
            {
                entryPoint = (from ep in db.Listting
                             
                                  where ep.UserId == applicationUser.Id
                                   select new ListtingDTO
                                  {
                                      Title = ep.Title,
                                      Id = ep.Id,
                                      TitleSeo = ep.TitleSeo,
                                      CategorysString = ep.CategorysString,
                                      Date = ep.Date,
                                      DateCreate = ep.DateCreate,
                                      Status = ep.Status,
                                      customerCeo = new CustomerCeo
                                      {
                                          
                                      },
                                      UserID = ep.UserId

                                  }).ToList();

                ViewBag.CanSign = "False";
            }
            else
            {
                entryPoint = (from ep in db.Listting
                                  join e in db.CustomerCeos on ep.Id equals e.ListtingID
                                  where 1 == 1
                                  select new ListtingDTO
                                  {
                                      Title = ep.Title,
                                      Id = ep.Id,
                                      TitleSeo = ep.TitleSeo,
                                      CategorysString = ep.CategorysString,
                                      Date = ep.Date,
                                      DateCreate = ep.DateCreate,
                                      Status = ep.Status,
                                      customerCeo = new CustomerCeo
                                      {
                                          FirstName = e.FirstName
                                      },
                                      UserID = ep.UserId
                                  }).ToList();
            };
            if (TieuDe != null)
            {
                entryPoint = entryPoint.Where(n => n.Title.Contains(TieuDe)).ToList();
            };
            if (FulName != null)
            {
                entryPoint = entryPoint.Where(n => n.customerCeo.FirstName.Contains(FulName)).ToList();
            };
            if (UserID != null)
            {
                entryPoint = entryPoint.Where(n => n.UserID == UserID).ToList();
            };
            if (fromdate.HasValue)
            {
                fromdate = fromdate ?? DateTime.Now;
                entryPoint = entryPoint.Where(n => (DateTime)fromdate.Value >= (DateTime)n.DateCreate.Value).ToList();
            };
            if (fromdate.HasValue)
            {
                fromdate = fromdate ?? DateTime.Now;
                entryPoint = entryPoint.Where(n => (DateTime)todate.Value <= (DateTime)n.DateCreate.Value).ToList();
            };

            ViewBag.TieuDe = TieuDe;
            ViewBag.FulName = FulName;
            ViewBag.UserID = UserID;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.ListCategories = (from ep in db.Categorys
                                      where 1 == 1
                                      select new CategoryDTO
                                      {
                                          Name = ep.Name,
                                          Id = ep.Id,
                                          Description = ep.Description,
                                          Status = ep.Status,
                                          IsShowOnHome = ep.IsShowOnHome,
                                          ImagePath = ep.ImagePath,
                                          ParentId = ep.ParentId,
                                      }).ToList();
            ViewBag.ListUser = db.AspNetUsers.ToList();
            return View(entryPoint.OrderByDescending(n => n.Id).ToPagedList(page, 10));
        }

       
        public IActionResult Add()
        {
            ViewBag.ListCategories = db.Categorys.Take(50).ToList();
            ViewBag.ListNations = db.Nations.Take(50).ToList();
            return View();
        }

        [HttpPost]
        //[ValidateInput(false)]
        public async Task<IActionResult> Add(ListtingDTO item)
        {
            try
            {
                AspNetUsers applicationUser = await userManager.GetUserAsync(User);
                //check data
                if (db.Listting.FirstOrDefault(n=>n.Phone == item.Phone) != null ||
                    db.Listting.FirstOrDefault(n => n.Gmail == item.Gmail) != null ||
                    db.Listting.FirstOrDefault(n => n.TitleSeo == item.TitleSeo) != null ||
                    db.Listting.FirstOrDefault(n => n.Title == item.Title) != null
                    )
                {
                    ViewBag.ListCategories = db.Categorys.Take(50).ToList(); 
                    ViewBag.ListNations = db.Nations.Take(50).ToList();
                    return View(item);
                }

                //save listing
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<ListtingDTO, Listting>(); });
                var des = config.CreateMapper().Map<ListtingDTO, Listting>(item);
                des.UserId = applicationUser.Id;
                des.Status = 0;
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
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult DeleteListting(int id)
        {
            try
            {
                db.Listting.Remove(new Listting() { Id = id });
                db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!db.Listting.Any(i => i.Id == id))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    throw ex;
                }
            }
            //Tags obj = new Tags();
          
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {

            ViewBag.ListCategories = db.Categorys.Take(50).ToList();
            ViewBag.ListNations = db.Nations.Take(50).ToList();
            var item = db.Listting.Find(id);
            if(item == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Listting, ListtingDTO>(); });
                var des = config.CreateMapper().Map<Listting, ListtingDTO>(item);
                var custemerCeo = db.CustomerCeos.Where(n => n.ListtingID == id).FirstOrDefault();
                var location = db.Locations.Where(n => n.ListtingID == id).FirstOrDefault();
                var lstImage = db.ListtingImages.Where(n => n.ListtingID == id).FirstOrDefault();
                des.customerCeo = custemerCeo == null ? new CustomerCeo() : custemerCeo;
                des.Locations = location == null ? new Location() : location;
                des.ListtingImage = lstImage == null ? new ListtingImage() : lstImage;
                return View(des);
            }
           
        }
        [HttpPost]
        public IActionResult Edit(ListtingDTO item)
        {
            //save listing
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<ListtingDTO, Listting>(); });
            var des = config.CreateMapper().Map<ListtingDTO, Listting>(item);
            db.Entry(des).State = EntityState.Modified;
            var objCustomer = db.CustomerCeos.AsNoTracking().FirstOrDefault(n => n.ListtingID == des.Id);
            var objLocation = db.Locations.AsNoTracking().Where(n => n.ListtingID == des.Id).FirstOrDefault();
            item.ListtingImage.Id = db.ListtingImages.AsNoTracking().Where(n => n.ListtingID == des.Id).First().Id;
            if(objCustomer != null)
            {
                item.customerCeo.Id = objCustomer.Id;
                item.customerCeo.ListtingID = objCustomer.ListtingID;
                var configCus = new MapperConfiguration(cfg => { cfg.CreateMap<CustomerCeo, CustomerCeo>(); });
                var desCus = config.CreateMapper().Map<CustomerCeo, CustomerCeo>(item.customerCeo);
                db.Entry(desCus).State = EntityState.Modified;
                db.SaveChanges();
            };
            if(objLocation != null)
            {
                item.Locations.ListtingID = des.Id;
                item.Locations.Id = objLocation.Id;
                db.Entry(item.Locations).State = EntityState.Modified;
            }

            item.ListtingImage.ListtingID = des.Id;
            db.Entry(item.ListtingImage).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index","Admin");
        }
        public IActionResult AddTagName(string tagName, int listtingId)
        {
            try { 
            Tags tags = new Tags();
            tags.TagName = tagName;
            tags.ListtingId = listtingId;
            db.Tags.Add(tags);
            db.SaveChanges();
           
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult DeleteTag(int id)
        {
            //Tags obj = new Tags();
            var obj = db.Tags.Find(id);
            if(obj != null)
            {
                db.Tags.Attach(obj);
                db.Tags.Remove(obj);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult FindTag(string tagName, int page = 1)
        {
            //Tags obj = new Tags();
            var obj = db.Tags.Where(n => n.TagName.Contains(tagName)).Select(n=>n.ListtingId).ToArray();
            var lstListtingDTO = new List<ListtingDTO>();
            if (obj != null)
            {
                lstListtingDTO = (from ep in db.Listting
                                  join e in db.CustomerCeos on ep.Id equals e.ListtingID
                                  where obj.Contains(ep.Id)
                                  select new ListtingDTO
                                  {
                                      Title = ep.Title,
                                      Id = ep.Id,
                                      TitleSeo = ep.TitleSeo,
                                      CategorysString = ep.CategorysString,
                                      Date = ep.Date,
                                      DateCreate = ep.DateCreate,
                                      customerCeo = new CustomerCeo
                                      {
                                          FirstName = e.FirstName
                                      }

                                  }).ToList();
                
            }
            ViewBag.NameTag = tagName;
            return View(lstListtingDTO.ToPagedList(page, 10));

        }
        public IActionResult ReviewListting(int id)
        {
            var objNews = (from ep in db.Listting
                              join e in db.CustomerCeos on ep.Id equals e.ListtingID
                              where ep.Id == id
                              select new ListtingDTO
                              {
                                  Title = ep.Title,
                                  Id = ep.Id,
                                  TitleSeo = ep.TitleSeo,
                                  CategorysString = ep.CategorysString,
                                  Date = ep.Date,
                                  DateCreate = ep.DateCreate,
                                  customerCeo = new CustomerCeo
                                  {
                                      FirstName = e.FirstName
                                  }

                              }).ToList();
       
            return PartialView(objNews);
        }
        public IActionResult CurrentUserPartial()
        {
            if(User.Identity.Name == null)
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.CurrentUser = userManager.GetUserId(User) ?? "";
            return PartialView();
        }

        public IActionResult NotFound()
        {
            return View();
        } 
        public async Task<IActionResult> Home()
        {
            AspNetUsers applicationUser = await userManager.GetUserAsync(User);
            if (applicationUser == null) { return RedirectToAction("Login", "User"); }
            return View();
        }
        [HttpPost]
        public string DeleteAll(string temps)
        {
            if (string.IsNullOrWhiteSpace(temps))
            {
                return "Bạn chưa chọn bản ghi.";
            }
            else
            {
                var lstData = temps.Split(",");
                for(int i = 0; i< lstData.Length; i++)
                {
                    var obj = db.Listting.Find(Convert.ToInt32(lstData[i]));
                    var obj2 = db.CustomerCeos.FirstOrDefault(n=>n.ListtingID == Convert.ToInt32(lstData[i]));
                    var obj3 = db.ListtingImages.FirstOrDefault(n => n.ListtingID == Convert.ToInt32(lstData[i]));
                    if (obj != null)
                    {
                        db.Listting.Attach(obj);
                        db.Listting.Remove(obj);
                        db.SaveChanges();
                    }
                    if (obj2 != null)
                    {
                        db.CustomerCeos.Attach(obj2);
                        db.CustomerCeos.Remove(obj2);
                        db.SaveChanges();
                    }
                    if (obj3 != null)
                    {
                        db.ListtingImages.Attach(obj3);
                        db.ListtingImages.Remove(obj3);
                        db.SaveChanges();
                    }
                }
                return "Xóa thành công." + temps;

            }
        }
        public string Sign(int Id)
        {
            try {
                var obj = db.Listting.FirstOrDefault(n => n.Id == Id);
                if (obj != null)
                {
                    if (obj.Status == 1)
                    {
                        obj.Status = 0;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        return "Gỡ duyệt thành công!";
                    }
                    else
                    {
                        obj.Status = 1;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        return "Duyệt thành công!";
                    };
                }
                else
                {
                    return "Có lỗi xảy ra!";
                }
            }
            catch(Exception ex)
            {
                return "Có lỗi xảy ra!" + ex.Message;
            }
        }

        public IActionResult UploadFile()
        {


            return View();
        }

    }
}
