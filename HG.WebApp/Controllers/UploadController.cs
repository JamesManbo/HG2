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
     
        public async Task<JsonResult>  UploadFilesExcels(string folder)
        {
            var now = DateTime.Now;
            var files = HttpContext.Request.Form.Files;
            if (HttpContext.Request.Form.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var lstFiles = new List<string>();
            var virtualPath = string.Format("\\FrontEnd\\_img_server\\{0}\\{1}\\{2}\\{3}\\{4}\\", folder, now.Year,
                now.Month, now.Day, now.ToString("yyyyMMddHHmmss"));
            string webRootPath = Environment.WebRootPath;

            var path = Path.Combine(webRootPath, virtualPath);
            //var path2 = System.Web.HttpContext.Current.Server.MapPath("UploadedFiles");
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
            var pathread = webRootPath + path + files.Single().FileName;
            
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(pathread, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            while (reader.Read()) //Each ROW
                            {
                                if (reader.Depth != 0)
                                {
                                    ListtingDTO listtingDTO = new ListtingDTO();
                                    listtingDTO.customerCeo = new CustomerCeo();
                                    listtingDTO.ListtingImage = new ListtingImage();
                                    listtingDTO.Locations = new Entities.Location();
                                    for (int column = 0; column < reader.FieldCount; column++)
                                    {
                                            switch (column)
                                            {
                                                case 0:
                                                    if (reader[column] != null) { listtingDTO.Title = reader.GetString(column); }
                                                    break;
                                                case 1:
                                                    if (reader[column] != null) { listtingDTO.customerCeo.TaxNumber = reader.GetString(column); }
                                                    break;
                                                case 2:
                                                    if (reader[column] != null) { listtingDTO.CategorysString = reader.GetString(column); }
                                                    break;
                                                case 3:
                                                    if (reader[column] != null) { listtingDTO.customerCeo.WebUri = reader.GetString(column); }
                                                    break;
                                                case 4:
                                                    if (reader[column] != null) { listtingDTO.customerCeo.Gmail = reader[column].ToString(); }
                                                    break;
                                                case 5:
                                                    if (reader[column] != null) { listtingDTO.Phone = reader.GetString(column).ToString(); }
                                                    break;
                                                case 6:
                                                    if (reader[column] != null) { listtingDTO.Description = reader.GetString(column); }
                                                    break;
                                                case 7:
                                                    if (reader[column] != null) { listtingDTO.Locations.Province = reader.GetString(column); }
                                                    break;
                                                case 8:
                                                    if (reader[column] != null) { listtingDTO.NationsID = Convert.ToInt32(reader.GetDouble(column)); }
                                                    break;
                                                case 9:
                                                    if (reader[column] != null) { listtingDTO.Locations.Address = reader.GetString(column); }
                                                    break;
                                                case 10:
                                                    if (reader[column] != null) { listtingDTO.ListtingImage.ImagePath = string.Format("\\FrontEnd\\_img_server\\{0}\\{1}\\{2}\\{3}\\", "Listting", now.Year,now.Month, now.Day) + reader.GetString(column); }
                                                    break;
                                                case 11:
                                                    if (reader[column] != null) { listtingDTO.customerCeo.Facebook = reader.GetString(column); }
                                                    break;
                                                case 12:
                                                    if (reader[column] != null) { listtingDTO.customerCeo.FirstName = reader.GetString(column); }
                                                    break;
                                                case 13:
                                                    if (reader[column] != null) { listtingDTO.customerCeo.Intagram = reader.GetString(column); }
                                                    break;
                                                case 14:
                                                    if (reader[column] != null) { listtingDTO.customerCeo.CapitalNumber = reader.GetDouble(column).ToString(); }
                                                    break;
                                                case 15:
                                                    if (reader[column] != null) { listtingDTO.customerCeo.Tweit = reader.GetString(column); }
                                                    break;

                                                default: break;
                                            }
                                       
                                    }
                                    //insert to db
                                    var a = await InsertDataAsnyc(listtingDTO);

                                };
                                //var abc = reader.RowCount;

                            }
                        } while (reader.NextResult()); //Move to NEXT SHEET

                    }
                }
            } catch (Exception ex)
            {

            }


            
             return Json(new { status = true, msg = "Upload thành công" });
        }
        public async Task<int> InsertDataAsnyc( ListtingDTO listtingDTO)
        {
            try
            {
                var obj = db.Listting.Where(n => n.Title == listtingDTO.Title).FirstOrDefault();
                if(obj == null)
                {
                    listtingDTO.TitleSeo = HG.WebApp.Helper.HelperString.convertToUnSign2(listtingDTO.Title);
                    listtingDTO.Status = 1;
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ListtingDTO, Listting>(); });
                    var des = config.CreateMapper().Map<ListtingDTO, Listting>(listtingDTO);
                    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Listting> created = db.Listting.Add(des);
                    await db.SaveChangesAsync();
                    var imgid = created.Entity.Id;
                    //save image
                    listtingDTO.ListtingImage.ListtingID = imgid;
                    listtingDTO.customerCeo.ListtingID = imgid;
                    listtingDTO.Locations.ListtingID = imgid;
                    db.ListtingImages.Add(listtingDTO.ListtingImage);
                    //save seo
                    db.CustomerCeos.Add(listtingDTO.customerCeo);
                    //add address
                    db.Locations.Add(listtingDTO.Locations);
                    await db.SaveChangesAsync();
                    return imgid;
                }
                else
                {
                    obj.Description = listtingDTO.Description;
                    obj.Title = listtingDTO.Title;
                    obj.Gmail = listtingDTO.Gmail;
                    obj.DateModify = DateTime.Now;
                    obj.CategorysString = listtingDTO.CategorysString;
                    obj.NationsID = listtingDTO.NationsID;
                    obj.Phone = listtingDTO.Phone;
                    db.Entry(obj).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return 0;
                }
               
            }
            catch(Exception e)
            {
                return 0;
            }
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
