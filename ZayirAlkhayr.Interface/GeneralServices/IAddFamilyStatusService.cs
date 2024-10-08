using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZayirAlkhayr.Entities.Common;

namespace ZayirAlkhayr.Interface.GeneralServices
{
    public interface IAddFamilyStatusService
    {
        HandleErrorResponseModel AddNewFamilyStatus(AddFamilyStatusModel Model);
        HandleErrorResponseModel DeleteFamilyStatus(int FamilyStatusId);
    }
}
