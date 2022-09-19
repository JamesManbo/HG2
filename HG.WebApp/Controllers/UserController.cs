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
using HG.Entities.Entities;
using HG.Entities;

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
                var menu_role = _userDao.GetUnitsMenuRolesOfUser(checkUser.Id);
                //end menu
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
            return RedirectToAction("Login", "User");
        }
        public async Task<IActionResult> ListRole(int page = 1)
        {
            AspNetUsers applicationUser = await userManager.GetUserAsync(User);
            if (applicationUser == null) { return RedirectToAction("Login", "User"); }
            if (_httpContextAccessor != null) { if (!sercutiry.IsAthentication(applicationUser, _httpContextAccessor.HttpContext.Request.Path.ToString())) { return RedirectToAction("NotFound", "Admin"); } };
            return View(eAContext.AspNetRoles.ToPagedList(page,10));
        }
        public IActionResult Nav()
        {
            EAContext db = new EAContext();
            var UserId = Guid.Parse(userManager.GetUserId(User));
            ViewBag.UserId = UserId.ToString();
            ViewBag.MenuChill = _userDao.GetUnitsMenuRolesOfUser(UserId);
            return PartialView();
        }
        public IActionResult ListFunction(int page = 1)
        {
            //var query = eAContext.AspModules.Where(n => n.isEF == 2).ToList();
            //return PartialView(query.ToPagedList(page, 10));
            return View();
        }
      

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

        public async Task<IActionResult> ThongTinCaNhan(string username)
        {
            EAContext db = new EAContext();
            var usermap = new NguoiDungCongDan();
            var userdata = await userManager.FindByNameAsync(username);
            ViewBag.ObjectUser = userdata;
            if (userdata != null)
            {
                usermap.ten = userdata.ten;
                usermap.UserName = userdata.UserName;
                usermap.PhoneNumber = userdata.PhoneNumber;
                usermap.Email = userdata.Email;
                usermap.ngay_sinh = userdata.ngay_sinh;
                var obj = db.Asp_user_client.Where(n => n.ma_nguoi_dung == userdata.Id).FirstOrDefault();
                if (obj != null)
                {
                    usermap.LoaiGiayToHL = obj.loai_giay_to;
                    usermap.TinhThanh = obj.ma_tinh_thanh;
                    usermap.QuanHuyen = obj.ma_quan_huyen;
                    usermap.XaPhuong = obj.ma_xa_phuong;
                    usermap.SoGiayTo = obj.so_giay_to;
                    usermap.TenCoQuan = obj.ten_co_quan;
                    usermap.DiaChiCoQuan = obj.dia_chi_co_quan;
                    usermap.CodeVerify = obj.code_verify;
                }
            }
            ViewBag.Verify = usermap.CodeVerify;
            ViewBag.DanhSachTinhTP = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == null).ToList();
            ViewBag.DanhSachQuanHuyen = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_cha == usermap.TinhThanh).ToList();
            ViewBag.DanhSachXaPhuong = db.dm_dia_ban.Where(n => n.Deleted != 1 && n.ma_dia_ban_con == usermap.QuanHuyen).ToList();
            return View(usermap);
        }
        [HttpPost]
        public async Task<IActionResult> ThongTinCaNhan(NguoiDungCongDan item)
        {
            var db = new EAContext();
            //var UserId = Guid.Parse(userManager.GetUserId(User));
            var user = await userManager.FindByNameAsync(item.UserName);
            if (user != null)
            {
                user.UserName = item.UserName;
                user.PhoneNumber = item.PhoneNumber;
                user.Email = item.Email;
                user.mat_khau = "1";
                user.ma_chuc_vu = item.ma_chuc_vu;
                user.ma_phong_ban = item.ma_phong_ban;
                user.ho_dem = item.ho_dem;
                user.ten = item.ten;
                user.ngay_sinh = item.ngay_sinh;
                user.khoa_tai_khoan = 0;
                user.IsAdministratorDV = 0;
                user.IsAdministratorPB = 0;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                if (item !=null)
                {
                    var obj = db.AspNetUsers.Where(n => n.UserName == item.UserName && n.ten == item.ten).FirstOrDefault();
                    //tiep tuc them vao bang mapping
                    Asp_user_client usermapping = new Asp_user_client();
                    usermapping.ma_nguoi_dung = obj.Id;
                    usermapping.loai_giay_to = item.LoaiGiayToHL;
                    usermapping.ma_tinh_thanh = item.TinhThanh;
                    usermapping.ma_quan_huyen = item.QuanHuyen;
                    usermapping.ma_xa_phuong = item.XaPhuong;
                    usermapping.so_giay_to = item.SoGiayTo;
                    usermapping.ten_co_quan = item.TenCoQuan;
                    usermapping.dia_chi_co_quan = item.DiaChiCoQuan;
                    usermapping.code_verify = item.CodeVerify;
                    db.Entry(usermapping).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("News", "News");
                }
                else
                {
                    ViewBag.Succeeded = false;
                    ViewBag.Message = "Cập nhật không thành công!";
                    return View(item);
                }
            }
            else
            {
                ViewBag.Succeeded = false;
                ViewBag.Message = "Cập nhật không thành công!";
                return View(item);
            }
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
        public IActionResult CurrentUserClient()
        {
            EAContext db = new EAContext();
            var obj = db.AspNetUsers.Where(n => n.UserName == User.Identity.Name).FirstOrDefault();
            var NameUid = "";
            if(obj != null)
            {
                NameUid = (obj.ho_dem == null ? "" : obj.ho_dem) + " " + (obj.ten == null ? "" : obj.ten);
            }
            ViewBag.NameUid = NameUid;
            return PartialView();
        }
        public IActionResult DoiMatKhau()
        {
            ViewBag.old_password = "";
            ViewBag.new_password = "";
            ViewBag.confirm_password = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DoiMatKhau(string username, string old_password, string new_password, string confirm_password)
        {
            EAContext db = new EAContext();
            ViewBag.old_password = old_password;
            ViewBag.new_password = new_password;
            ViewBag.confirm_password = confirm_password;
            if (new_password != confirm_password)
            {
                ViewBag.Message = "Mật khẩu không giống nhau!";
                ViewBag.Succeeded = false;
                return View();
            };
            var obj = db.AspNetUsers.Where(n => n.UserName == username && n.mat_khau == old_password).AsNoTracking().FirstOrDefault();  
            if(obj == null)
            {
                ViewBag.Message = "Mật khẩu không chính xác";
                ViewBag.Succeeded = false;
                return View();
            };
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {

                ViewBag.Message = "Mật khẩu không chính xác";
                ViewBag.Succeeded = false;
                return View();
            };
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, token, new_password);
            if (result.Succeeded)
            {
                ViewBag.Message = "Thay đổi mật khẩu thành công";
                ViewBag.Succeeded = false;
                return View();
            }
            else
            {
                ViewBag.Message = "Thay đổi mật khẩu không thành công";
                ViewBag.Succeeded = false;
                return View();
            }
        }


    }
}
