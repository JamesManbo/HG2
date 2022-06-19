using AutoMapper;
using HG.WebApp.Data;
using HG.WebApp.Dto;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace HG.WebApp.Controllers
{
    public class UploadController : Controller
    {
        private IWebHostEnvironment Environment;
        private EAContext db = new EAContext();
        //
        // GET: /Upload/
        public UploadController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        [HttpPost]
        public JsonResult UploadFiles(string folder)
        {
            var now = DateTime.Now;
            var files = HttpContext.Request.Form.Files;
            if (HttpContext.Request.Form.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var lstFiles = new List<string>();
            var virtualPath = string.Format("\\FrontEnd\\_img_server\\{0}\\{1}\\{2}\\{3}\\", folder, now.Year,
                now.Month, now.Day);
            string webRootPath = Environment.WebRootPath;
            //string contentRootPath = _webHostEnvironment.ContentRootPath;

          
          var path = Path.Combine(webRootPath, virtualPath);
            //var path2 = System.Web.HttpContext.Current.Server.MapPath("UploadedFiles");
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
                }
            }
            return Json(new { status = true, msg = "Upload thành công", files = lstFiles });
        }
        public JsonResult UploadFilesReviews(string folder, string Id)
        {
            var now = DateTime.Now;
            var files = HttpContext.Request.Form.Files;
            if (HttpContext.Request.Form.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var lstFiles = new List<string>();
            var lstFilesName = new List<string>();
            var virtualPath = string.Format("\\FrontEnd\\_img_server\\{0}\\{1}\\{2}\\{3}\\", folder, now.Year,
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
                }
            }
            return Json(new { status = true, msg = "Upload thành công", files = lstFiles, names = lstFilesName });
        }
     
        public async Task<JsonResult> UnzipFileImage()
        {
            try
            {
                var now = DateTime.Now;
                var files = HttpContext.Request.Form.Files;
                if (HttpContext.Request.Form.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
                var lstFiles = new List<string>();
                var virtualPath = string.Format("\\FrontEnd\\_img_server\\{0}\\{1}\\{2}\\{3}\\{4}\\", "listtingZip", now.Year,
                    now.Month, now.Day, now.ToString("yyyyMMddHHmmss"));
                string webRootPath = Environment.WebRootPath;
                var path = Path.Combine(webRootPath, virtualPath);
                if (!Directory.Exists(webRootPath + path))
                {
                    Directory.CreateDirectory(webRootPath + path);
                }
                foreach (var fileItem in files)
                {
                    string filename = fileItem.FileName;
                    using (var fileStream = new FileStream(Path.Combine(webRootPath + path, filename), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        await fileItem.CopyToAsync(fileStream);
                        lstFiles.Add(string.Format("{0}\\{1}", path, filename));
                    }
                }
                //create a place to extra
                var destination = string.Format("\\FrontEnd\\_img_server\\{0}\\{1}\\{2}\\{3}\\", "listting", now.Year, now.Month, now.Day);
                var path2 = Path.Combine(webRootPath, destination);
                if (!Directory.Exists(webRootPath + destination))
                {
                    Directory.CreateDirectory(webRootPath + destination);
                }
                else
                {
                    Directory.Delete(webRootPath + destination, true);
                    Directory.CreateDirectory(webRootPath + destination);
                }
                ZipFile.ExtractToDirectory(webRootPath + path + files[0].FileName, webRootPath + destination);
                return Json(new { status = true, msg = "Upload file thành công!." });
            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = "Upload file thất bại! file quá lớn, vui lòng kiểm tra lại." });
            }
            


        }

    }
}
