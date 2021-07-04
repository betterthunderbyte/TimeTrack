using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools;

namespace TimeTrack.Web.Service.Controllers.V1.Api
{
    [ApiController, Route("v1/api/[controller]")]
    public class ApiOtherController : ControllerBase
    {
        private OtherUseCase _otherUseCase;
        
        public ApiOtherController(OtherUseCase otherUseCase)
        {
            _otherUseCase = otherUseCase;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "User,Admin,Moderator")]
        [HttpGet("full")]
        public async Task<ActionResult<FullDataTransfer>> GetFull()
        {
            int userId = 0;
            var claim = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (claim != null && int.TryParse(claim.Value, out userId))
            {

                var r = await _otherUseCase.GetFullFromUserAsync(userId);
                return r.ToSingleAction();
            }

            return new BadRequestResult();
        }
    }
}