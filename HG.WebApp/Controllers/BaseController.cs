using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using HG.WebApp.Data;
using HG.Data.SqlService;
using HG.Entities.Entities.HoSo;
using Microsoft.EntityFrameworkCore;

namespace HG.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected SqlDbProvider DbProvider { get; private set; }

        /// <summary>
        /// Constructor initial SqlDbProvider
        /// </summary>
        /// <summary>
        /// Dispose SqlDbProvider
        /// </summary>
        public void Dispose()
        {
            DbProvider.Dispose();
        }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IConfiguration Configuration { get; }
        public BaseController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            DbProvider = new SqlDbProvider();
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
       

        #region Add by TuanTA 15-10-2020 - Cookies AddressBip of Mmemonic

        public string CookieAddress
        {
            get { return Configuration["User:Address"]; }
        }

        public string CookieUID
        {
            get { return Configuration["User:CookieUID"]; }
        }

        public string CookiePicture
        {
            get { return Configuration["User:CookiePicture"]; }
        }
        public void SaveLogHS(int ma_ho_so, string noi_dung_xl, int trangthaitruoc, int trangthaisau, Guid manguoiupdate)
        {
            using (var db = new EAContext())
            {
                Ho_So_History ho_So_History = new Ho_So_History();
                ho_So_History.ma_ho_so = ma_ho_so;
                ho_So_History.noi_dung_xu_ly = noi_dung_xl;
                ho_So_History.tra_thai_xu_ly_truoc = trangthaitruoc;
                ho_So_History.tra_thai_xu_ly_sau = trangthaisau;
                ho_So_History.CreatedUid = manguoiupdate;
                ho_So_History.CreatedDateUtc = DateTime.Now;
                db.Entry(ho_So_History).State = EntityState.Modified;
                db.SaveChanges();
            }
         }
        /// <summary>
        /// add by Tuanta
        /// get user ip of customer when them login to website
        /// </summary>
        //public string GetUserIP()
        //{
        //    var userIp = string.Empty;
        //    if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        //    {
        //        userIp = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    }
        //    else if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.UserHostAddress))
        //    {
        //        userIp = System.Web.HttpContext.Current.Request.UserHostAddress;
        //    }
        //    return userIp;
        //}

        /// <summary>
        /// add by Tuanta
        /// get user id of customer when them login to website
        /// </summary>
        protected string Address
        {
            get
            {
                return HttpContext.Session.GetString(Configuration["User:SessionName"] == null ? HttpContext.Session.GetString(Configuration["User:SessionName"]) : "");
            }
        }

        protected void SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }

      

        /// <summary>  
        /// Get the cookie  
        /// </summary>  
        /// <param name="key">Key </param>  
        /// <returns>string value</returns>  
        public string GetCookie(string key)
        {
            return Request.Cookies[key];
        }
        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">Key </param>  
        /// <returns>string value</returns>  
        public void SetCookie(string key, string value)
        {
            //string referer = Request.Headers["Referer"].ToString();
            //CookieOptions option = new CookieOptions();
            //option.Expires = DateTime.Now.AddMilliseconds(50000);
            //option.HttpOnly = true;
            //Response.Cookies.Append(key, value, option);
            CookieOptions option = new CookieOptions();
            Response.Cookies.Append(key, value, option);
            option.Expires = DateTime.Now.AddMilliseconds(50000);
            option.HttpOnly = true;
        }

        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }

        #endregion


    }
    public static class CoinExchangeExtensions
    {
        public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewNamePath, TModel model)
        {
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    else
                        viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

                    if (!viewResult.Success)
                        return $"A view with the name '{viewNamePath}' could not be found";

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception exc)
                {
                    return $"Failed - {exc.Message}";
                }
            }
        }

        public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewNamePath)
        {
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    else
                        viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

                    if (!viewResult.Success)
                        return $"A view with the name '{viewNamePath}' could not be found";

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception exc)
                {
                    return $"Failed - {exc.Message}";
                }
            }
        }


    }

}


