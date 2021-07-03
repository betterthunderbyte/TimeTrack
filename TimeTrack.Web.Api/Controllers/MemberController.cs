using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TimeTrack.Core.DataTransfer;
using TimeTrack.Core.DataTransfer.V1;
using TimeTrack.Core.UseCase;
using TimeTrack.UseCase;
using TimeTrack.Web.Api.Common;
using TimeTrack.Web.Service.Tools;
using TimeTrack.Web.Service.Tools.V1;

namespace TimeTrack.Web.Api.Controllers
{
    [ApiController, Route("v1/api/[controller]")]
    public class MemberController : ControllerBase
    {
        private IMemberUseCase _memberUseCase;
        
        public MemberController(IMemberUseCase memberUseCase)
        {
            _memberUseCase = memberUseCase;
        }
        
        [HttpGet("list")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<MemberDataTransfer>>> GetAll()
        {
            var r = await _memberUseCase.GetAllAsync();

            return r.To<MemberDataTransfer>().ToMultiAction();
        }
        
        [HttpPut]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin")]
        public async Task<ActionResult<MemberDataTransfer>> PutSingle([FromBody]MemberDataTransfer memberDataTransfer)
        {
            if (memberDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            memberDataTransfer.To(out var r);
            var re = await _memberUseCase.PutSingleAsync(r);
            return re.To<MemberDataTransfer>().ToSingleAction();
        }
        
        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin")]
        public async Task<ActionResult<MemberDataTransfer>> PutSingle(int id, [FromBody]MemberDataTransfer memberDataTransfer)
        {
            if (memberDataTransfer == null)
            {
                return new BadRequestResult();
            }
            
            memberDataTransfer.To(out var r);
            var re = await _memberUseCase.UpdateSingleAsync(id, r);
            return re.To<MemberDataTransfer>().ToSingleAction();
        }
        
        [HttpPut("{id}/password")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin")]
        public async Task<ActionResult<MemberDataTransfer>> PatchPassword(int id, [FromBody]ChangePasswordDataTransfer changePasswordDataTransfer)
        {
            var r = await _memberUseCase.SetPassword(id, changePasswordDataTransfer.Password);
            return r.To<MemberDataTransfer>().ToSingleAction();
        }
        
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.Bearer, Roles = "Admin")]
        public async Task<ActionResult<MemberDataTransfer>> DeleteSingle(int id)
        {
            var re = await _memberUseCase.DeleteSingleAsync(id);
            return re.To<MemberDataTransfer>().ToSingleAction();
        }
    }
}
