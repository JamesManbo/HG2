using AutoMapper;
using HG.WebApp.Data;
using HG.WebApp.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using X.PagedList;
using HG.WebApp.Sercurity;
using HG.Data.Business.User;

namespace HG.WebApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        private EAContext eAContext = new EAContext();
        private readonly UserManager<AspNetUsers> userManager;
        private readonly SignInManager<AspNetUsers> signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        Sercutiry sercutiry = new Sercutiry();
        private readonly UserDao _userDao;

        //extend sys identity
        public UserController(ILogger<UserController> logger, UserManager<AspNetUsers> userManager, SignInManager<AspNetUsers> signInManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        : base(configuration, httpContextAccessor)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
            _userDao = new UserDao(DbProvider);
        }

//        public async Task<IActionResult> Index(int page = 1, int status = 0)
//        {
//            //call stored
//            var calldatasp = _userDao.GetAspNetUsers();


//            AspNetUsers applicationUser = await userManager.GetUserAsync(User);
//            if (applicationUser == null) { return RedirectToAction("Login", "User"); }
//            if (_httpContextAccessor != null) { if (!sercutiry.IsAthentication(applicationUser, _httpContextAccessor.HttpContext.Request.Path.ToString())) { return RedirectToAction("NotFound", "Admin"); } };            //
//#pragma warning disable CS8602 // Dereference of a possibly null reference.
//            var Username = User.Identity.Name;
//#pragma warning restore CS8602 // Dereference of a possibly null reference.
//            var q = await userManager.Users.CountAsync();
//            var query = await userManager.Users.Select(x => new UserDTO()
//            {
//                Email = x.Email,
//                PhoneNumber = x.PhoneNumber,
//                UserName = x.UserName,
//                RoleId = x.RoleId,
//                Id = x.Id.ToString(),
//            }).ToListAsync();
//            ViewBag.ListRole = eAContext.AspNetRoles.ToList();
//            return View(query.ToPagedList(page, 10));
//        }
        public async Task<IActionResult> AddUser(string UserName, string PhoneNumber, string Email, string Password, Guid RoleId)
        {
            try
            {
                AspNetUsers user = new AspNetUsers();
                user.UserName = UserName;
                user.PhoneNumber = PhoneNumber;
                user.Email = Email;
                user.mat_khau = Password;
                user.RoleId = RoleId;
                var userdata = await userManager.FindByNameAsync(user.UserName);
                if (userdata != null)
                {
                    return RedirectToAction("Index", "User");
                }
                if (await userManager.FindByEmailAsync(user.Email) != null)
                {
                    return RedirectToAction("Index", "User");
                }

                var result = await userManager.CreateAsync(user, user.mat_khau);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Login", "User");
        }

        public IActionResult EditUser(Guid UserID, string UserName, string PhoneNumber, string Email, Guid RoleId)
        {
            try
            {
                var obj = eAContext.AspNetUsers.Find(UserID);
                if (obj != null)
                {
                    obj.UserName = UserName;
                    obj.PhoneNumber = PhoneNumber;
                    obj.Email = Email;
                    obj.RoleId = RoleId;
                    eAContext.Entry(obj).State = EntityState.Modified;
                    eAContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var result = await userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "User");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return RedirectToAction("Index", "User");

                }
            }
            catch (Exception ex)
            {
                if (!eAContext.AspNetUsers.Any(i => i.Id == id))
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }
        }
        public IActionResult Login()
        {
            ViewBag.Msg = "";
            TempData["Fale"] = "";
            return View();
        }

        public async Task<string> ResetPassword(string id)
        {

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return "false";
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, token, "MyN3wP@ssw0rd");
            if (result.Succeeded)
            {
                return "true";
            }
            else
            {
                return "false";
            }
            //return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string UserName, string PassWord,bool RememberMe = false)
        {
            EAContext db = new EAContext();
            var obj = db.AspNetUsers.Where(n => n.UserName == UserName && n.mat_khau == PassWord && (n.khoa_tai_khoan == 1 || n.Deleted == 1)).Count();
            if(obj > 0)
            {
                TempData["Fale"] = "Fale";
                ViewBag.Msg = "Tài khoản đã bị khóa hoặc đã xóa!";
                return RedirectToAction("Login", "User");
            }
            ViewBag.Msg = "";
            var q = await userManager.Users.CountAsync();
            var checkUser = await userManager.FindByNameAsync(UserName);
            var user = userManager.Users;
            if (checkUser == null)
            {
                TempData["Fale"] = "Fale";
                ViewBag.Msg = "Tài khoản không tồn tại";
                return RedirectToAction("Login", "User");
            };

            var result = await signInManager.PasswordSignInAsync(checkUser, PassWord, RememberMe, true);
            if (!result.Succeeded)
            {
                return RedirectToAction("Login", "User");
            }else {
                //laays menu va tat ca nhom vai tro o day
                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    //claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds);
                return RedirectToAction("ListNguoiDung", "QTNguoidung");

            }
          
        }
        [HttpPost]
        public async Task<string> LoginClient(string UserName, string PassWord, bool RememberMe = false)
        {
            var q = await userManager.Users.CountAsync();
            var checkUser = await userManager.FindByNameAsync(UserName);
            var user = userManager.Users;
            if (checkUser == null)
            {
                return "false";
            };

            var result = await signInManager.PasswordSignInAsync(checkUser, PassWord, RememberMe, true);
            if (!result.Succeeded)
            {
                return "true";
            }
            else
            {

                var roles = await userManager.GetRolesAsync(checkUser);
                var claims = new[]
                {
                new Claim(ClaimTypes.Email,checkUser.Email),
                new Claim(ClaimTypes.GivenName,UserName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, UserName)
                 };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds);


                return "true";

            }

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
        public async Task<IActionResult> LogoutClient()
        {
            await this.signInManager.SignOutAsync();
            return Redirect("/");
        }
        public async Task<IActionResult> ListRole(int page = 1)
        {
            AspNetUsers applicationUser = await userManager.GetUserAsync(User);
            if (applicationUser == null) { return RedirectToAction("Login", "User"); }
            if (_httpContextAccessor != null) { if (!sercutiry.IsAthentication(applicationUser, _httpContextAccessor.HttpContext.Request.Path.ToString())) { return RedirectToAction("NotFound", "Admin"); } };
            return View(eAContext.AspNetRoles.ToPagedList(page,10));
        }
        public async Task<IActionResult> Nav()
        {
            AspNetUsers applicationUser = await userManager.GetUserAsync(User);
            var lstIdModuleByRole = eAContext.AspNetRoleModules.Where(n => n.RoleId == applicationUser.RoleId).Select(n=>n.ModuleId).ToArray();
            //if(lstIdModuleByRole != null)
            //{
            //    return PartialView(eAContext.AspModules.Where(n => lstIdModuleByRole.Contains(n.Id)).ToList());
            //}
            return PartialView(new AspModule());
        }
        public IActionResult ListFunction(int page = 1)
        {
            //var query = eAContext.AspModules.Where(n => n.isEF == 2).ToList();
            //return PartialView(query.ToPagedList(page, 10));
            return View();
        }
        //public async Task<IActionResult> AddRoleAsync(RoleDTO item)
        //{
        //    AspNetUsers applicationUser = await userManager.GetUserAsync(User);
        //    AspNetRoles _user = new AspNetRoles();
        //    if (eAContext.AspNetRoles.Where(n => n.Name == item.Name).FirstOrDefault() == null)
        //    {
        //        _user.Name = item.Name;
        //        _user.Description = item.Description;
        //        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AspNetRoles> _role = eAContext.AspNetRoles.Add(_user);
        //        eAContext.SaveChanges();
        //        var roleid = _role.Entity.Id;
        //        if(!string.IsNullOrEmpty(item.ListModule))
        //        {
        //            var lstModule = item.ListModule.Split(","); 
        //            for (var j = 0; j < lstModule.Length; j++)
        //            {
        //                AspNetRoleModules k = new AspNetRoleModules();
        //                k.ModuleId = Convert.ToInt32(lstModule[j]) ;
        //                k.RoleId = roleid;
        //                k.IsFull = true;
        //                eAContext.AspNetRoleModules.Add(k);
        //                eAContext.SaveChanges();
        //            }
        //        }
        //    }
        //    //add taikhoan voi role =>>xong quyen
        //    return RedirectToAction("ListRole","User");
        //}


        public async Task<string> RegisterClient(string UserName, string Password, string Gmail)
        {
            try
            {
                AspNetUsers user = new AspNetUsers();
                user.UserName = UserName;
                user.PhoneNumber = "";
                user.Email = Gmail;
                user.mat_khau = Password;
                var roleMem = eAContext.AspNetRoles.FirstOrDefault(n => n.Name == "membership");
                if(roleMem != null)
                {
                    user.RoleId = roleMem.Id;
                };
                var userdata = await userManager.FindByNameAsync(user.UserName);
                if (userdata != null)
                {
                    return "ErrUsername";
                };
                if (await userManager.FindByEmailAsync(user.Email) != null)
                {
                    return "ErrEmail";
                };

                var result = await userManager.CreateAsync(user, user.mat_khau);
                if (result.Succeeded)
                {
                    return "true";
                }
            }
            catch (Exception ex)
            {
                return "false";
            }
            return "false";
        }

        public async Task<IActionResult> Profile(string username)
        {
            var userdata = await userManager.FindByNameAsync(username);
            ViewBag.ObjectUser = userdata;
            return View();
        } 
        public async Task<string> SaveProfile(string viewpath, string FirstName, string PhoneNumber, string MaritalStatus, string Website)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                user.ho_dem = FirstName;
                user.PhoneNumber = PhoneNumber;
                user.tinh_trang_hon_nhan = MaritalStatus;
                user.ten = Website;
                user.anh_dai_dien = viewpath;
                var abc = await userManager.UpdateAsync(user);
                if (abc.Succeeded)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            catch
            {
                return "false";
            }
            
        }
        public IActionResult CurrentUser()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("Login", "User");
            }
            return PartialView();
        }
    }
}
