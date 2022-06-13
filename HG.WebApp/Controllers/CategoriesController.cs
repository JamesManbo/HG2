using HG.WebApp.Data;
using HG.WebApp.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HG.WebApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        EAContext eAContext = new EAContext();
        public CategoriesController(ILogger<CategoriesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int page =1, int status = 0)
        {
            var entryPoint = (from ep in eAContext.Categorys
                              where 1 == 1
                              select new CategoryDTO {      
                                  Name = ep.Name,
                                  NameEn = ep.NameEn,
                                  Id = ep.Id,
                                  Description = ep.Description,
                                  Status = ep.Status,
                                  ParentId = ep.ParentId,
                                  ImagePath = ep.ImagePath
                              }).ToList();



            return View(entryPoint.ToPagedList(page, 10));
        }
        public IActionResult AddCate(string CName, string CNameEn, string CDes, int Status, int IsShowOnHome, string ImagePath)
        {
            try
            {
                Category cate = new Category();
                cate.Name = CName;
                cate.NameEn = CNameEn;
                cate.Description = CDes;
                cate.IsShowOnHome = IsShowOnHome;
                cate.ImagePath = ImagePath;
                cate.ParentId = 0;
                cate.Status = Status;
                eAContext.Categorys.Add(cate);
                eAContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Categories");
        }

        public IActionResult EditCate(int CategoryID, string CName, string CNameEn, string CDes, int Status, int EditIsShowOnHome, string IconEdit)
        {
            try
            {
                var entryPoint = eAContext.Categorys.AsNoTracking().FirstOrDefault(n=>n.Id == CategoryID);
                if (entryPoint != null)
                {
                    entryPoint.Name = CName;
                    entryPoint.NameEn = CNameEn;
                    entryPoint.Description = CDes;
                    entryPoint.Status = Status;
                    entryPoint.IsShowOnHome = EditIsShowOnHome;
                    entryPoint.ImagePath = string.IsNullOrEmpty(IconEdit) ? entryPoint.ImagePath : IconEdit;
                    eAContext.Entry(entryPoint).State = EntityState.Modified;
                    eAContext.SaveChanges();
                }
            

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Categories");
        }

        public IActionResult DeleteCate(int id)
        {
            try
            {
                eAContext.Categorys.Remove(new Category() { Id = id });
                eAContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!eAContext.Categorys.Any(i => i.Id == id))
                {
                    return RedirectToAction("Index", "Categories");
                }
                else
                {
                    throw ex;
                }
            }
            //Tags obj = new Tags();
            //var obj = eAContext.Categorys.ToList().FirstOrDefault(n=>n.Id == id);
            //if (obj != null)
            //{
            //    eAContext.Categorys.Attach(obj);
            //    eAContext.Categorys.Remove(obj);
            //    eAContext.SaveChanges();
            //}
            return RedirectToAction("Index", "Categories");
        }

        public IActionResult ListNations(int page = 1, int status = 0)
        {
            var entryPoint = (from ep in eAContext.Nations
                              where 1 == 1
                              select new NationDTO
                              {
                                  Name = ep.Name,
                                  Id = ep.Id,
                                  NameAbbreviation = ep.NameAbbreviation,
                                  Status = ep.Status,
                                  Icon = ep.Icon
                              }).ToList();



            return View(entryPoint.ToPagedList(page, 10));
        }
        public IActionResult AddNation(string Name, string NameA, int Status, string Icon)
        {
            try
            {
                Nation ntion = new Nation();
                ntion.Name = Name;
                ntion.NameAbbreviation = NameA;
                ntion.Icon = Icon;
                ntion.Status = Status;
                eAContext.Nations.Add(ntion);
                eAContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("ListNations", "Categories");
        }

        public IActionResult EditNation(int EditNationID, string EditName, string EditNameA, int StatusEdit, string IconEdit)
        {
            try
            {
                var entryPoint = (from ep in eAContext.Nations
                                  where ep.Id == EditNationID
                                  select new NationDTO
                                  {
                                      Name = EditName,
                                      Id = ep.Id,
                                      NameAbbreviation = EditNameA,
                                      Status = StatusEdit,
                                      Icon = ep.Icon
                                  }).FirstOrDefault();
                if (entryPoint != null)
                {
                    entryPoint.Icon = string.IsNullOrEmpty(IconEdit) ? entryPoint.Icon : IconEdit;
                    eAContext.Entry(entryPoint).State = EntityState.Modified;
                    eAContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("ListNations", "Categories");
        }

        public IActionResult DeleteNation(int id)
        {
            try
            {
                eAContext.Nations.Remove(new Nation() { Id = id });
                eAContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!eAContext.Categorys.Any(i => i.Id == id))
                {
                    return RedirectToAction("ListNations", "Categories");
                }
                else
                {
                    throw ex;
                }
            }
            //Tags obj = new Tags();
            //var obj = eAContext.Categorys.ToList().FirstOrDefault(n=>n.Id == id);
            //if (obj != null)
            //{
            //    eAContext.Categorys.Attach(obj);
            //    eAContext.Categorys.Remove(obj);
            //    eAContext.SaveChanges();
            //}
            return RedirectToAction("ListNations", "Categories");
        }



    }
}
