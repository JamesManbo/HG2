using AutoMapper;
using HG.WebApp.Data;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace HG.WebApp.Controllers
{
    public class UploadController : Controller
    {
        private IWebHostEnvironment Environment;
        IConfiguration _iconfiguration;
        private EAContext db = new EAContext();
        //
        // GET: /Upload/
        public UploadController(IWebHostEnvironment _environment, IConfiguration iconfiguration)
        {
            Environment = _environment;
            _iconfiguration = iconfiguration;
        }

        [HttpPost]
        public JsonResult UploadFiles(string folder)
        {
            var now = DateTime.Now;
            var files = HttpContext.Request.Form.Files;
            if (HttpContext.Request.Form.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var lstFiles = new List<string>();
            var lstFilesName = new List<string>();
            var virtualPath = string.Format(_iconfiguration.GetSection("AppSetting").GetSection("TestUrl").Value + "{0}\\{1}\\{2}\\{3}\\", folder, now.Year,
                now.Month, now.Day);
            string webRootPath = Environment.WebRootPath;
          var path = Path.Combine(webRootPath, virtualPath);
            if (!Directory.Exists(webRootPath + path))
            {
                Directory.CreateDirectory(webRootPath+path);
            }

            foreach (var fileItem in files)
            {
                string filename = fileItem.FileName;
                using (var fileStream = new FileStream(Path.Combine(webRootPath + path, filename), FileMode.Create))
                {
                    fileItem.CopyToAsync(fileStream);
                    lstFiles.Add(string.Format("{0}\\{1}", path, filename));
                    lstFilesName.Add(filename);
                }
            }
            return Json(new { status = true, msg = "Upload thành công", files = lstFiles, names = lstFilesName });
        }
        public JsonResult UploadFilesTP(string folder, int Id)
        {
            EAContext db = new EAContext();
            var now = DateTime.Now;
            var files = HttpContext.Request.Form.Files;
            if (HttpContext.Request.Form.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var lstFiles = new List<string>();
            var lstFilesName = new List<string>();
            var virtualPath = string.Format(_iconfiguration.GetSection("AppSetting").GetSection("TestUrl").Value + "{0}\\{1}\\{2}\\{3}\\", folder, now.Year,
                now.Month, now.Day, Id);
            string webRootPath = Environment.WebRootPath;

            var path = Path.Combine(webRootPath, virtualPath);
            if (!Directory.Exists(webRootPath + path))
            {
                Directory.CreateDirectory(webRootPath + path);
            }

            foreach (var fileItem in files)
            {
                string filename = fileItem.FileName;
                using (var fileStream = new FileStream(Path.Combine(webRootPath + path, filename), FileMode.Create))
                {
                    fileItem.CopyToAsync(fileStream);
                    lstFiles.Add(string.Format("{0}\\{1}", path, filename));
                    lstFilesName.Add(filename);
                    var obj = db.dm_thanh_phan.Where(n => n.Id == Id).FirstOrDefault();
                    if(obj != null)
                    {
                        obj.file_dinh_kem = lstFiles[0];
                        obj.UpdatedDateUtc = DateTime.Now; 
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges(); 
                    }
                }
            }
            return Json(new { status = true, msg = "Upload thành công", files = lstFiles, names = lstFilesName });
        } 
     
    }
}
