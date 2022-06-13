using HG.WebApp.Data;
using HG.WebApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HG.WebApp.Controllers
{
    public class SliderController : Controller
    {
        private IWebHostEnvironment Environment;

        public SliderController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        private readonly EAContext db = new EAContext();
        public IActionResult Index(int page = 1)
        {
            return View(db.Slider.ToPagedList(page, 10));
        }
        public IActionResult Add(string SliderName, string SliderUrl, int Status, string Note, int IsPartner,  List<IFormFile> postedFiles)
        {
            var now = DateTime.Now;
            List<string> uploadedFiles = new List<string>();
            string path = "";
            try
            {
                if (postedFiles.Count > 0)
                {
                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;
                    var virtualPath = string.Format("\\FrontEnd\\_img_server\\{0}\\{1}\\{2}\\{3}\\{4}\\", "Slider", now.Year,
                       now.Month, now.Day, now.ToString("hhmmss"));
                    path = Path.Combine(this.Environment.WebRootPath, virtualPath);
                    if (!Directory.Exists(wwwPath + path))
                    {
                        Directory.CreateDirectory(wwwPath + path);
                    }
                    foreach (IFormFile postedFile in postedFiles)
                    {
                        string fileName = Path.GetFileName(postedFile.FileName);
                        using (FileStream stream = new FileStream(Path.Combine(wwwPath + path, fileName), FileMode.Create))
                        {
                            postedFile.CopyTo(stream);
                            uploadedFiles.Add(fileName);
                        }
                    }
                }
                
                Slider sl = new Slider();
                sl.SliderName = SliderName;
                sl.SliderUrl = path + uploadedFiles[0].ToString();
                sl.Note = Note;
                sl.Status = Status;
                sl.IsPartner = IsPartner;
                db.Slider.Add(sl);
                db.SaveChanges();

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Slider");
        }

        public IActionResult Edit(int SliderId, string ESliderName, string ENote, int EStatus, string ESliderUrl, int IsPartner, List<IFormFile> IconEdit)
        {
            var now = DateTime.Now;
            List<string> uploadedFiles = new List<string>();
            string path = "";
            try
            {
                if(IconEdit.Count > 0)
                {
                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;
                    var virtualPath = string.Format("\\FrontEnd\\_img_server\\{0}\\{1}\\{2}\\{3}\\{4}\\", "Slider", now.Year,
                       now.Month, now.Day, now.ToString("hhmmss"));
                    path = Path.Combine(this.Environment.WebRootPath, virtualPath);
                    if (!Directory.Exists(wwwPath + path))
                    {
                        Directory.CreateDirectory(wwwPath + path);
                    }
                    foreach (IFormFile postedFile in IconEdit)
                    {
                        string fileName = Path.GetFileName(postedFile.FileName);
                        using (FileStream stream = new FileStream(Path.Combine(wwwPath + path, fileName), FileMode.Create))
                        {
                            postedFile.CopyTo(stream);
                            uploadedFiles.Add(fileName);
                        }
                    }
                }
                var entryPoint = db.Slider.AsNoTracking().FirstOrDefault(n => n.Id == SliderId);
                if (entryPoint != null)
                {
                    entryPoint.SliderName = ESliderName;
                    entryPoint.SliderUrl =  (IconEdit.Count > 0 ? path + uploadedFiles[0].ToString(): ESliderUrl);
                    entryPoint.Status = EStatus;
                    entryPoint.Note = ENote;
                    entryPoint.IsPartner = IsPartner;
                    db.Entry(entryPoint).State = EntityState.Modified;
                    db.SaveChanges();
                }


            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Slider");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                db.Slider.Remove(new Slider() { Id = id });
                db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!db.Slider.Any(i => i.Id == id))
                {
                    return RedirectToAction("Index", "Slider");
                }
                else
                {
                    throw ex;
                }
            }
           
            return RedirectToAction("Index", "Slider");
        }



    }
}
