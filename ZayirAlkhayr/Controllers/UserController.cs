using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ZayirAlkhayr.Entities.Common;
using System.Collections.Generic;
using ZayirAlkhayr.Interface;
using System.Data;

namespace ZayirAlkhayr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public DataTable GetAllUsers()
        {
            var results = _userService.GetAllUsers();
            return results;
        }

        [HttpPost]
        [Route("AdminLogin")]
        public async Task<ApplicationUserModel> AdminLogin(LoginModel model)
        {
            var results = await _userService.AdminLogin(model);
            return results;
        }

        [HttpGet]
        [Route("AdminLogout")]
        public async Task<bool> AdminLogout(string UserId)
        {
            var results = await _userService.AdminLogout(UserId);
            return results;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<HandleErrorResponseModel> CreateUser(AddUserModel model)
        {
            var results = await _userService.CreateUser(model);
            return results;
        }

        [HttpPost]
        [Route("EditUser")]
        public HandleErrorResponseModel EditUser(AddUserModel model)
        {
            var results = _userService.EditUser(model);
            return results;
        }

        [HttpGet]
        [Route("DeleteUser")]
        public HandleErrorResponseModel DeleteUser(string UserId)
        {
            var results = _userService.DeleteUser(UserId);
            return results;
        }
    }
}
