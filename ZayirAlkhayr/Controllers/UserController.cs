using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using ZayirAlkhayr.Entities.Common;
using System.Linq;
using System.Data;
using ZayirAlkhayr.Entities.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;
using System.Globalization;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ZADbContext _context;
        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ZADbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpPost]
        [Route("AdminLogin")]
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
                    Expires = DateTime.UtcNow.AddHours(4),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);


                ApplicationUserModel userModel = new ApplicationUserModel
                {
                    UserName = user.UserName,
                    Role = role.FirstOrDefault(),
                    UserId = user.Id,
                    Token = token,
                    LoginDate = DateTime.Now,
                    LoginDateAr = DateTime.Now.ToString("dddd d MMMM , yyyy hh:m", new CultureInfo("ar-AE")),
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

        [HttpPost]
        [Route("CreateUser")]
        public async Task<HandleErrorResponseModel> CreateUser(AddUserModel model)
        {
            var Response = new HandleErrorResponseModel();
            if (ModelState.IsValid)
            {
                IdentityUser appUser = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
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
            else
            {
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        [HttpPost]
        [Route("EditUser")]
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
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email;

                    _context.SaveChanges();
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

        [HttpGet]
        [Route("DeleteUser")]
        public HandleErrorResponseModel DeleteUser(string UserId)
        {
            var Response = new HandleErrorResponseModel();
            var User = _context.Users.Where(x => x.Id == UserId).FirstOrDefault();

            if (User != null)
            {
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

        [HttpPost]
        [Route("AssignNewRoleToUser")]
        public async Task<HandleErrorResponseModel> AssignNewRoleToUser(string UserId, string RoleId)
        {
            var Response = new HandleErrorResponseModel();
            try
            {
                var UserRoles = _context.UserRoles.Where(a => a.UserId == UserId).ToList();
                _context.UserRoles.RemoveRange(UserRoles);
                _context.SaveChanges();

                var role = await _roleManager.FindByIdAsync(RoleId);
                var user = await _userManager.FindByIdAsync(UserId);
                var user_roles = await _userManager.GetRolesAsync(user);
                _userManager.AddToRoleAsync(user, role.Name).GetAwaiter().GetResult();

                Response.Done = true;
                Response.Message = "تم تعديل صلاحية المستخدم بنجاح";
                return Response;
            }
            catch (Exception ex)
            {
                Response.Done = false;
                Response.Message = "لقد حدث خطا";
                return Response;
            }
        }

        [HttpGet]
        [Route("GetAllUserRoles")]
        public async Task<List<IdentityRole>> GetAllUserRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }


        [HttpGet]
        [Route("GetUserRoleByID")]
        public async Task<List<string>> GetUserRoleByID(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var user_roles = await _userManager.GetRolesAsync(user);
            return user_roles.ToList();
        }
    }
}
