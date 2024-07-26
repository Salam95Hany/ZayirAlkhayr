using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Models;
using ZayirAlkhayr.Interface;
using ZayirAlkhayr.Interface.Common;

namespace ZayirAlkhayr.Service
{
    public class UserService : IUserService
    {
        private UserManager<AdminUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ZADbContext _context;
        private readonly ISQLHelper _sQLHelper;
        public UserService(UserManager<AdminUser> userManager, RoleManager<IdentityRole> roleManager, ZADbContext context, ISQLHelper sQLHelper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _sQLHelper = sQLHelper;
        }

        public DataTable GetAllUsers()
        {
            var Params = new SqlParameter[0];
            var dt = _sQLHelper.ExecuteDataTable("web.SP_GetAllUsersData", Params);
            return dt;
        }

        public async Task<ApplicationUserModel> AdminLogin(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var key = Encoding.UTF8.GetBytes("1234567890123456");
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                     {
                         new Claim("UserID" , user.Id.ToString()),
                         new Claim(_options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
                     }),
                    Expires = DateTime.UtcNow.AddHours(8),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                user.IsActive = true;
                user.LoginDate = DateTime.Now;
                _context.SaveChanges();
                ApplicationUserModel userModel = new ApplicationUserModel
                {
                    UserName = user.UserName,
                    Role = role.FirstOrDefault(),
                    UserId = user.Id,
                    Token = token,
                    LoginDate = DateTime.Now,
                    LoginDateAr = DateTime.Now.ToString("dddd d MMMM , yyyy", new CultureInfo("ar-AE")),
                    LoginTimeAr = DateTime.Now.ToString("hh:mm:ss"),
                    ResponseCode = 200,
                    ResponseMessage = "تم تسجيل الدخول بنجاح",

                };
                return userModel;
            }
            else
            {
                ApplicationUserModel userModel = new ApplicationUserModel
                {
                    ResponseCode = 100,
                    ResponseMessage = "اسم المستخدم او كلمة المرور غير صالح",
                    LoginDate = DateTime.Now
                };
                return userModel;
            }
        }

        public async Task<bool> AdminLogout(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                user.IsActive = false;
                user.LoginDate = null;
                _context.SaveChanges();
            }
            return true;
        }

        public async Task<HandleErrorResponseModel> CreateUser(AddUserModel model)
        {
            var Response = new HandleErrorResponseModel();

            AdminUser appUser = new AdminUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                IsActive = false
            };

            try
            {
                var result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    bool adminRoleExists = await _roleManager.RoleExistsAsync(model.Role);
                    if (!adminRoleExists)
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));

                    await _userManager.AddToRoleAsync(appUser, model.Role);
                    Response.Done = true;
                    Response.Message = "تم اضافة مستخدم جديد بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "لقد حدث خطا";
                    return Response;
                }
            }
            catch (Exception ex)
            {
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel EditUser(AddUserModel model)
        {
            var Response = new HandleErrorResponseModel();
            try
            {
                var user = _context.Users.Where(a => a.Id == model.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.NormalizedUserName = model.UserName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email;
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

                    _context.SaveChanges();
                    AssignNewRoleToUser(model.UserId, model.Role).GetAwaiter().GetResult();
                    Response.Done = true;
                    Response.Message = "تم تعديل المستخدم بنجاح";
                    return Response;
                }
                else
                {
                    Response.Done = false;
                    Response.Message = "لقد حدث خطا";
                    return Response;
                }

            }
            catch (Exception)
            {
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public HandleErrorResponseModel DeleteUser(string UserId)
        {
            var Response = new HandleErrorResponseModel();
            var User = _context.Users.FirstOrDefault(x => x.Id == UserId);

            if (User != null)
            {
                var UserRoles = _context.UserRoles.Where(a => a.UserId == UserId).ToList();
                _context.UserRoles.RemoveRange(UserRoles);
                _context.Users.Remove(User);
                _context.SaveChanges();

                Response.Done = true;
                Response.Message = "تم حذف المستخدم بنجاح";
                return Response;
            }
            else
            {
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        public async Task<HandleErrorResponseModel> AssignNewRoleToUser(string UserId, string RoleName)
        {
            var Response = new HandleErrorResponseModel();
            try
            {
                var UserRoles = _context.UserRoles.Where(a => a.UserId == UserId).ToList();
                _context.UserRoles.RemoveRange(UserRoles);
                _context.SaveChanges();

                var role = await _roleManager.FindByNameAsync(RoleName);
                var user = await _userManager.FindByIdAsync(UserId);
                _userManager.AddToRoleAsync(user, role.Name).GetAwaiter().GetResult();

                Response.Done = true;
                return Response;
            }
            catch (Exception ex)
            {
                Response.Done = false;
                return Response;
            }
        }

        public StatisticsHomeModel GetStatisticsHome()
        {
            var StatisticsModel = new StatisticsHomeModel();
            var VisitorCount = _context.WebSiteVisitors.Count();
            var User = _context.Users.ToList();
            StatisticsModel.VisitorCount = VisitorCount;
            StatisticsModel.ActiveUserCount = User.Where(i => i.IsActive).Count();
            StatisticsModel.InactiveUserCount = User.Where(i => !i.IsActive).Count();
            return StatisticsModel;
        }
    }
}
