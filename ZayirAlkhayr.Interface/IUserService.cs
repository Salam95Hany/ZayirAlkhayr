using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface
{
    public interface IUserService
    {
        Task<ApplicationUserModel> AdminLogin(LoginModel model);
        Task<bool> AdminLogout(string UserId);
        Task<HandleErrorResponseModel> CreateUser(AddUserModel model);
        HandleErrorResponseModel EditUser(AddUserModel model);
        HandleErrorResponseModel DeleteUser(string UserId);
        DataTable GetAllUsers();
        StatisticsHomeModel GetStatisticsHome();
    }
}
