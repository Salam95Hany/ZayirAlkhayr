using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Auth;
using ZayirAlkhayr.Entities.Common;
using ZayirAlkhayr.Entities.Contracts.DTOs.Auth;
using ZayirAlkhayr.Interface.Auth;

namespace ZayirAlkhayr.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ApiResponseModel<DataTable>> GetAllUsers()
        {
            var results = await _authService.GetAllUsers();
            return results;
        }

        [HttpPost]
        [Route("AdminLogin")]
        public async Task<ApiResponseModel<ApplicationUserRespone>> AdminLogin(LoginModel model)
        {
            var results = await _authService.AdminLogin(model);
            return results;
        }

        [HttpGet]
        [Route("AdminLogout")]
        public async Task<ApiResponseModel<string>> AdminLogout(string UserId)
        {
            var results = await _authService.AdminLogout(UserId);
            return results;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ApiResponseModel<string>> CreateUser(AddUserModel model)
        {
            var results = await _authService.CreateUser(model);
            return results;
        }

        [HttpPost]
        [Route("EditUser")]
        public async Task<ApiResponseModel<string>> EditUser(AddUserModel model)
        {
            var results = await _authService.EditUser(model);
            return results;
        }

        [HttpGet]
        [Route("DeleteUser")]
        public async Task<ApiResponseModel<string>> DeleteUser(string UserId)
        {
            var results = await _authService.DeleteUser(UserId);
            return results;
        }

        [HttpPost]
        [Route("EditUserPassword")]
        public async Task<ApiResponseModel<string>> EditUserPassword(string UserId, string Password)
        {
            var results = await _authService.EditUserPassword(UserId, Password);
            return results;
        }

        [HttpGet]
        [Route("GetStatisticsHome")]
        public async Task<object> GetStatisticsHome()
        {
            var results = await _authService.GetStatisticsHome();
            return results;
        }

        [HttpGet]
        [Route("GetUserInfoById")]
        public async Task<ApiResponseModel<UserWithRolesDto>> GetUserInfoById(string UserId)
        {
            var results = await _authService.GetUserInfoById(UserId);
            return results;
        }

        [HttpPost]
        [Route("EditUserProfile")]
        public async Task<ApiResponseModel<string>> EditUserProfile(AddUserModel model)
        {
            var results = await _authService.EditUserProfile(model);
            return results;
        }

        [HttpPost]
        [Route("ChangeUserPassword")]
        public async Task<ApiResponseModel<string>> ChangeUserPassword(AddUserModel model)
        {
            var results = await _authService.ChangeUserPassword(model);
            return results;
        }
    }
}
