using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Common;
using TimeTrack.Web.Service.Tools;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Service.Controllers.V1.Api
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ApiMemberController : ControllerBase
    {
        private MemberUseCase _memberUseCase;
        
        public ApiMemberController(MemberUseCase memberUseCase)
        {
            _memberUseCase = memberUseCase;
        }
        
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<MemberDataTransfer>>> GetAll()
        {
            var r = await _memberUseCase.GetAllAsync();

            return r.To<MemberDataTransfer>().ToMultiAction();
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<MemberDataTransfer>> PutSingle(MemberDataTransfer memberDataTransfer)
        {
            if (memberDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            memberDataTransfer.To(out var r);
            var re = await _memberUseCase.PutSingleAsync(r);
            return re.To<MemberDataTransfer>().ToSingleAction();
        }
    }
}
