using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZayirAlkhayr.Interface.GeneralServices;

namespace ZayirAlkhayr.Controllers.GeneralServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyNationalityController : ControllerBase
    {
        private readonly IFamilyNationalityService _familyNationalityService;
        public FamilyNationalityController(IFamilyNationalityService familyNationalityService)
        {
            _familyNationalityService = familyNationalityService;
        }
    }
}
