using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.UseCase;
using TimeTrack.UseCase;
using TimeTrack.Web.Api.Common;

namespace TimeTrack.Web.Api.Controllers
{
    [ApiController, Route("v1/api/[controller]")]
    public class OtherController : ControllerBase
    {
        private IOtherUseCase _otherUseCase;
        
        public OtherController(IOtherUseCase otherUseCase)
        {
            _otherUseCase = otherUseCase;
        }
        
        [HttpGet("full")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "User,Admin,Moderator")]
        public async Task<ActionResult<FullDataTransfer>> GetFull()
        {
            if (User.ReceiveMemberId(out var userId))
            {
                var r = await _otherUseCase.GetFullFromUserAsync(userId);
                return r.ToSingleAction();
            }
            
            return new BadRequestResult();
        }
    }
}