using HG.WebApp.Models;
using IdentityModel.Client;
using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HG.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> LoginCandidate()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (User.Identity.IsAuthenticated)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                return RedirectToAction("Index", "Home");
            }

            var previousUrl = Request.Headers["Referer"].ToString();
            var isRedirectToNextPage = TempData["isRedirectToNextPage"]?.ToString();


            Response.Cookies.Delete("returnUrl");

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(5);

            Response.Cookies.Append("returnUrl", previousUrl, option);


            return await StartAuthentication();
        }

        private async Task<IActionResult> StartAuthentication()
        {
            string referer = Request.Headers["Referer"].ToString();

            CookieOptions option = new CookieOptions();

            option.Expires = DateTime.Now.AddMilliseconds(50000);


            Response.Cookies.Append("returnUrl", referer, option);

            //var discoClient = new DiscoveryClient(ConfigSettingEnum.JwtTokensIssuer.GetConfig()); 
            var discoClient = new DiscoveryClient("");
            discoClient.Policy.RequireHttps = false;
            // read discovery document to find authorize endpoint
            var disco = await discoClient.GetAsync();
            var authorizeUrl = new RequestUrl(disco.AuthorizeEndpoint).CreateAuthorizeUrl(
                clientId: "cnv-fe",
                responseType: "code id_token token",
                scope: "public-api openid profile offline_access",
                //redirectUri: ConfigSettingEnum.JwtTokensRedirectUri.GetConfig(),     
                redirectUri: "",
                state: "random_state",
                nonce: "random_nonce",
                responseMode: "form_post"
               );

            return Redirect(authorizeUrl);
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = new LoginResponse();

                if (request == null)
                {
                    return Json(result);
                }

                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return Json(result);
                }
                //to do

                return Json(result);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public NotifyAjax Login2([FromBody] LoginRequest request)
        {
            try
            {
                var result = new NotifyAjax();



                return result;
            }
            catch (Exception e)
            {

                throw;
            }
        }


    }
}
