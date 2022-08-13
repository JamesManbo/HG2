using HG.WebApp;
using HG.WebApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace HG.WebAppApp
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<EAContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("EAContext")));
            services.AddIdentity<AspNetUsers, AspNetRoles>()
               .AddEntityFrameworkStores<EAContext>()
               .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            #region Public Transient
            //Cache
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserManager<AspNetUsers>, UserManager<AspNetUsers>>();
            services.AddTransient<SignInManager<AspNetUsers>, SignInManager<AspNetUsers>>();
            services.AddTransient<RoleManager<AspNetRoles>, RoleManager<AspNetRoles>>();
            #endregion
            services.AddDistributedMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
            });
            services.AddResponseCompression();
            services.AddControllersWithViews();
            services.AddMvc(option => option.EnableEndpointRouting = false);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void
            Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error");
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/Error/404";
                    await next();
                }
                else
                {
                    if (context.Response.StatusCode == 401)
                    {
                        context.Request.Path = "/Home/Error/401";
                        await next();
                    }
                }
            });
            app.UseSession();
            app.UseAuthentication();
            app.UseStatusCodePages(context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                    response.StatusCode == (int)HttpStatusCode.Forbidden ||
                    response.StatusCode == 404)
                    response.Redirect("/User/Login");
                return Task.CompletedTask;
            });

            app.UseResponseCompression();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseIdentityServer();    

            app.UseMvc(routes =>
                {
                    //var rewrite = new RewriteOptions();
                    //rewrite.AddRewrite(@"(.*)-(\d+).html", "Job/Index?jobId=$2", skipRemainingRules: true);
                    routes.MapRoute(
                        name: "area",
                        template: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                    routes.MapRoute(
                        "Register",
                        "Register", // URL with parameters
                        new { controller = "User", action = "Register" }
                     );
                    routes.MapRoute(
                       "YellowPage",
                       "YellowPage", // URL with parameters
                       new { controller = "EA", action = "Page1" }
                    );
                    routes.MapRoute(
                       "YellowPageFilter",
                       "YellowPageFilter", // URL with parameters
                       new { controller = "EA", action = "Page2" }
                    );
                    routes.MapRoute(
                       "YellowPageDetail",
                       "YellowPageDetail/{slug}", // URL with parameters
                       new { controller = "EA", action = "ListtingDetails" }
                    );
                    routes.MapRoute(
                     "EditListting",
                     "EditListting/{Id}", // URL with parameters
                     new { controller = "Admin", action = "Edit" }
                  );
                    routes.MapRoute(
                     "AddReview",
                     "AddReview/{Id}", // URL with parameters
                     new { controller = "Review", action = "AddReview" }
                  );
                    routes.MapRoute(
                     "AddListting",
                     "AddListting", // URL with parameters
                     new { controller = "Review", action = "AddListting" }
                  );
                    routes.MapRoute(
                     "LogoutClient",
                     "LogoutClient", // URL with parameters
                     new { controller = "LogoutClient", action = "User" }
                  );
                    routes.MapRoute(
                    "DichVuCong",
                    "DichVuCong", // URL with parameters
                    new { controller = "News", action = "News" }
                 );
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=User}/{action=login}/{id?}");
                });
            //    routes.MapRoute(
            //       name: "default",
            //       template: "{controller=Admin}/{action=Index}/{id?}");
            //});
        }
    }
}

