using Microsoft.AspNetCore.Mvc.Filters;

namespace HG.WebApp
{
    public class CustomActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Controller = filterContext.ActionDescriptor.FilterDescriptors;
            string referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();
        }
    }
    
    // public class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    //{
    //    /// <summary>
    //    /// Event request không xác thực
    //    /// </summary>
    //    /// <param name="actionContext">request HttpActionContext</param>
    //    protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
    //    {
    //        object auth = HttpContext.Current.Request.Headers["Authorization"];
    //        if (auth != null && !auth.ToString().Contains($"Bearer {ConfigUtil.privateSignalRBearerToken}"))
    //        {
    //            if (!HttpContext.Current.User.Identity.IsAuthenticated)
    //            {
    //                base.HandleUnauthorizedRequest(actionContext);
    //            }
    //            else
    //            {
    //                //Setting error message and status fode 403 for unauthorized user
    //                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden) { Content = new StringContent(JsonConvert.SerializeObject(new { Message = "Authorization failed." })), StatusCode = HttpStatusCode.Forbidden };
    //            }
    //        }
    //    }
    //}
}
